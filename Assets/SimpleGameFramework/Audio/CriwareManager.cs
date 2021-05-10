using System;
using System.Collections.Generic;
using System.IO;
using AudioFramework;
using SimpleGameFramework.Core;
using UnityEngine;

namespace SimpleGameFramework.Audio
{
    public class CueSheetRefData
    {
        public string cueSheet;
        public int    refCount;
        public float  countDown;

        public CueSheetRefData (string cueSheet, int refCount, float countDown)
        {
            this.cueSheet  = cueSheet;
            this.refCount  = refCount;
            this.countDown = countDown;
        }
    }

    public class CriwareManager : ManagerBase
    {
    #region Public Field
        /// 正在播放的音乐
        public List<CriAtomSource> audioPlaying = new List<CriAtomSource> ();
        /// 正被暂停的音乐
        public List<CriAtomSource> audioPausing = new List<CriAtomSource> ();
        /// 控制CueSheet的加载和配置文件
        public CriAtom criAtom;
        /// 监听器，接收3D声音
        public CriAtomListener listener;
    #endregion


    #region Private Field
        /// CriwareSource组件池
        private Queue<GameObject> audioPool = new Queue<GameObject> ();
        /// CueSheet的引用计数
        private List<CueSheetRefData> cueSheetRef = new List<CueSheetRefData> ();
        /// CueSheet卸载倒计时，默认时间为 CUE_SHEET_SURVIVAL_TIME
        private List<CueSheetRefData> cueSheetCountDown = new List<CueSheetRefData> ();
        /// 清理CriwareSource组件池的间隔
        private float tempTime; // 用于实际计算，实际清理间隔定义在Config中
    #endregion


    #region Delegate & Event
        /// 音乐停止时调用
        public delegate void OnAudioPlayEndHandler (string audioName);

        public event OnAudioPlayEndHandler onOnAuidoPlayEnd;

        /// TODO 事件池
        public Dictionary<string, OnAudioPlayEndHandler> eventPool = new Dictionary<string, OnAudioPlayEndHandler> ();
    #endregion


    #region Implement

    public override int Priority
    {
        get
        {
            return ManagerPriority.CriwareManager.GetHashCode();
        }
    }

    public override void Init()
        {
#if UNITY_EDITOR
            // DEUBG 音乐组件池
            GameObject audioComponentPool = new GameObject ("AudioComponentPool");
            audioComponentPool.transform.SetParent (SGFEntry.Instance.transform.Find("CriwareManager").transform);
            // DEBUG CueSheet引用计数
            GameObject cueSheetRefCount = new GameObject ("CueSheetReferenceCount");
            cueSheetRefCount.transform.SetParent (SGFEntry.Instance.transform.Find("CriwareManager").transform);
            // DEBUG CueSheet卸载倒计时
            GameObject cueSheetUnLoadCountDown = new GameObject ("CueSheetUnLoadCountDown");
            cueSheetUnLoadCountDown.transform.SetParent (SGFEntry.Instance.transform.Find("CriwareManager").transform);
#endif
        }

        public override void Update(float time)
        {
            // 检查音乐的播放状态
            CheckAudioState ();
            // 检查CueSheet引用池
            CheckCueSheetCountDown (Time.deltaTime);
            // 检查CriwareSource组件对象池
            CheckAudioPool (Time.deltaTime);
#if UNITY_EDITOR
            // DEBUG
            DrawDebugMessage ();
#endif
        }

        public override void ShutDown()
        {
            
        }
    #endregion


    #region 接口
        /// <summary>
        /// 设置CriAtom的配置文件，这个方法不应该被主动调用，应该调用AudioCenter.SetCriwareACF()
        /// </summary>
        /// <param name="acfPath">acf文件的路径</param>
        public void SetAcfFile (string acfPath)
        {
            if (criAtom == null)
                throw new Exception ("CriAtom 还未被初始化，不能设置ACF");

            if (criAtom.acfFile.Equals (acfPath))
                return;

            criAtom.acfFile = acfPath;
            if (!String.IsNullOrEmpty (criAtom.acfFile))
            {
                string path = Path.Combine (CriWare.Common.streamingAssetsPath, criAtom.acfFile);
                CriAtomEx.RegisterAcf (null, path);
            }
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="audioData">音频信息</param>
        /// <param name="loop">是否循环</param>
        public void PlayAudio (IAudioData audioData, bool loop)
        {
            var data = audioData as CriwareData;
            if (data == null)
                return;

            CheckSheet (data.cueSheet, data.acbPath, data.awbPath);

            var go = Acquire ();

        #if UNITY_EDITOR
            go.name = data.cueName;
        #endif

            var source = go.GetComponent<CriAtomSource> ();
            source.cueSheet = data.cueSheet;
            source.cueName  = data.cueName;
            source.loop     = loop;
            source.Play ();

            audioPlaying.Add (source);
        }

        /// <summary>
        /// 停止音乐，会减少CueSheet的引用计数，并直接释放音乐组件
        /// 播放中或者暂停中的，所有匹配的音乐，都会被停止
        /// </summary>
        /// <param name="audioData">音频信息</param>
        public void StopAudio (IAudioData audioData)
        {
            var data = audioData as CriwareData;
            if (data == null)
                return;

            // 检查正在播放的音乐
            for (int i = 0, len = audioPlaying.Count; i < len; i++)
            {
                if (audioPlaying[i].cueName.Equals (data.cueName))
                    audioPlaying[i].Stop ();
            }

            // 检查正被暂停的音乐
            for (int i = 0, len = audioPausing.Count; i < len; i++)
            {
                if (audioPausing[i].cueName.Equals (data.cueName))
                {
                    audioPausing[i].Pause (false);
                    audioPausing[i].Stop ();
                }
            }
        }

        /// <summary>
        /// 停止所有音乐，包括暂停中和播放中的
        /// </summary>
        public void StopAllAudio ()
        {
            // 停止所有正在播放的音乐
            for (int i = 0, len = audioPlaying.Count; i < len; i++)
            {
                audioPlaying[i].Stop ();
            }

            // 停止所有正被暂停的音乐
            for (int i = 0, len = audioPausing.Count; i < len; i++)
            {
                audioPausing[i].Pause (false);
                audioPausing[i].Stop ();
            }
        }

        /// <summary>
        /// 暂停音乐，只是暂停，不会减少CueSheet的引用计数，且保持占有一个音乐组件
        /// 会暂停所有匹配的音乐
        /// </summary>
        /// <param name="audioData">音频信息</param>
        public void PauseAudio (IAudioData audioData)
        {
            var data = audioData as CriwareData;
            if (data == null)
                return;

            for (int i = audioPlaying.Count - 1; i >= 0; i--)
            {
                if (audioPlaying[i].cueName.Equals (data.cueName))
                {
                #if UNITY_EDITOR
                    var go = SGFEntry.Instance.transform.Find("CriwareManager").transform.Find ("AudioComponentPool").Find (audioPlaying[i].cueName).gameObject;
                    go.name = $"{go.name}(Pausing)";
                #endif
                    audioPlaying[i].Pause (true);
                    audioPausing.Add (audioPlaying[i]);
                    audioPlaying.RemoveAt (i);
                }
            }
        }

        /// <summary>
        /// 恢复播放音乐
        /// </summary>
        /// <param name="audioData">音频信息</param>
        public void ResumeAudio (IAudioData audioData)
        {
            var data = audioData as CriwareData;
            if (data == null)
                return;

            for (int i = audioPausing.Count - 1; i >= 0; i--)
            {
                if (audioPausing[i].cueName.Equals (data.cueName))
                {
                #if UNITY_EDITOR
                    var go = SGFEntry.Instance.transform.Find("CriwareManager").transform.Find ("AudioComponentPool").Find ($"{data.cueName}(Pausing)").gameObject;
                    go.name = audioPausing[i].cueName;
                #endif
                    audioPausing[i].Pause (false);
                    audioPlaying.Add (audioPausing[i]);
                    audioPausing.RemoveAt (i);
                }
            }
        }

        /// <summary>
        /// 暂停所有正在播放的音乐
        /// </summary>
        public void PauseAllAudio ()
        {
            // 暂停并放入musicPausing
            for (int i = audioPlaying.Count - 1; i >= 0; i--)
            {
                audioPlaying[i].Pause (true);
                audioPausing.Add (audioPlaying[i]);
                audioPlaying.RemoveAt (i);
            }
            audioPlaying.Clear ();

        #if UNITY_EDITOR
            var trans = SGFEntry.Instance.transform.Find("CriwareManager").transform.Find ("AudioComponentPool");
            for (int i = 0, len = trans.childCount; i < len; i++)
            {
                var child = trans.GetChild (i).gameObject;
                if (!child.activeSelf)
                    continue;
                child.name = $"{child.name}(pausing)";
            }
        #endif
        }

        /// <summary>
        /// 恢复所有暂停的音乐
        /// </summary>
        public void ResumuAllAudio ()
        {
            // 恢复并放入musicPlaying
            for (int i = audioPausing.Count - 1; i >= 0; i--)
            {
                audioPausing[i].Pause (false);
                audioPlaying.Add (audioPausing[i]);
                audioPausing.RemoveAt (i);
            }
            audioPausing.Clear ();

        #if UNITY_EDITOR
            var trans = SGFEntry.Instance.transform.Find("CriwareManager").transform.transform.Find ("AudioComponentPool");
            for (int i = 0, len = trans.childCount; i < len; i++)
            {
                var child = trans.GetChild (i).gameObject;
                if (!child.activeSelf)
                    continue;
                var names = child.name.Split ('(');
                child.name = names[0];
            }
        #endif
        }

        /// <summary>
        /// 设置音频速度，会从正在播放和正在暂停的音乐中寻找目标音乐
        /// </summary>
        /// <param name="audioData">音频信息</param>
        /// <param name="speed">想要加的速度，默认是0即不加速，最小为-1，最大为1</param>
        public void SetMusicSpeed (IAudioData audioData, float speed) { }

        /// <summary>
        /// 设置所有音频的速度，包括暂停中的
        /// </summary>
        /// <param name="speed">想要加的速度，默认是0即不加速，最小为-1，最大为1</param>
        public void SetAllMusicSpeed (float speed) { }
    #endregion


    #region 工具
        /// <summary>
        /// 尝试从组件池里获取一个播放组件
        /// </summary>
        /// <param name="trans">父节点，如果不是null，会设为获取到的组件对象的父节点</param>
        /// <returns></returns>
        public GameObject Acquire (Transform trans = null, bool detach = false)
        {
            // 尝试从对象池里获取一个播放组件
            if (audioPool.Count >= 1)
            {
                var go = audioPool.Dequeue ();
                go.transform.position = Vector3.zero;
                if (trans != null)
                {
                    if (detach)
                    {
                        go.transform.SetParent (trans,false);
                    }
                    else
                    {
                        go.transform.position = trans.position;
                    }
                }
                go.SetActive (true);
                return go;
            }
            // 没有就生成一个
            else
            {
                GameObject go = new GameObject ();
                go.transform.position = Vector3.zero;
                go.AddComponent<CriAtomSource> ();
                if (trans != null)
                {
                    if (detach)
                    {
                        go.transform.SetParent (trans,false);
                    }
                    else
                    {
                        go.transform.position = trans.position;
                    }
                }
            #if UNITY_EDITOR
                else
                {
                    go.transform.SetParent (SGFEntry.Instance.transform.Find("CriwareManager").transform.Find ("AudioComponentPool"));
                }
            #endif
                return go;
            }
        }

        /// <summary>
        /// 释放一个播放组件，释放的对象必须带有CriAtomSource组件
        /// </summary>
        public void Release (GameObject go)
        {
            var temp = go.GetComponent<CriAtomSource> ();
            if (temp == null)
                return;
            temp.Stop ();
            temp.cueName  = string.Empty;
            temp.cueSheet = string.Empty;
            temp.gameObject.SetActive (false);
            audioPool.Enqueue (go);
        #if UNITY_EDITOR
            if (Application.isPlaying)
            {
                var trans = SGFEntry.Instance.transform.Find("CriwareManager").transform.Find ("AudioComponentPool");
                go.transform.SetParent (trans);
            }
        #endif
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void ClearAudioPool ()
        {
            while (audioPool.Count >= 1)
            {
                GameObject.Destroy (audioPool.Dequeue ());
            }
        }

        /// <summary>
        /// 检查当前表是否加载过，并计算相应的引用计数 
        /// </summary>
        public void CheckSheet (string cueSheet, string acbPath, string awbPath)
        {
            // 检查是否加载过cueSheet
            var sheet = CriAtom.GetCueSheet (cueSheet);

            // 如果没有加载过，加载
            if (sheet == null)
                sheet = CriAtom.AddCueSheet (cueSheet, acbPath, awbPath);

            // 把sheet从倒计时数组中移除（如果有的话
            var nodeExisit = cueSheetCountDown.Find (temp => temp.cueSheet.Equals (sheet.name));
            if (nodeExisit != null)
            {
                cueSheetCountDown.Remove (nodeExisit);
            }

            // 增加引用计数
            var node = cueSheetRef.Find (temp => temp.cueSheet.Equals (sheet.name));
            if (node != null)
            {
                node.refCount++;
            }
            else
            {
                cueSheetRef.Add (new CueSheetRefData (sheet.name, 1, ManagerConfig.CriwareManagerConfig.CUE_SHEET_SURVIVAL_TIME));
            }
        }

        /// <summary>
        /// 检查当前音乐的播放状态，如果停止了，那么移除
        /// </summary>
        private void CheckAudioState ()
        {
            // 检查正在播放的音乐，是否结束或停止
            for (int i = audioPlaying.Count - 1; i >= 0; i--)
            {
                if (audioPlaying[i].status == CriAtomSource.Status.Stop || audioPlaying[i].status == CriAtomSource.Status.PlayEnd)
                {
                    // 计算引用计数
                    ReduceReferenceCount (audioPlaying[i].cueSheet);
                    // 事件通知
                    onOnAuidoPlayEnd?.Invoke (audioPlaying[i].cueName);
                    // 归还引用
                    Release (audioPlaying[i].gameObject);
                    // 从正在播放链表中删除
                    audioPlaying.RemoveAt (i);
                }
            }

            // 检查正被暂停的音乐，是否被停止
            for (int j = audioPausing.Count - 1; j >= 0; j--)
            {
                if (audioPausing[j].status != CriAtomSource.Status.Stop)
                    continue;
                // 计算引用计数
                ReduceReferenceCount (audioPausing[j].cueSheet);
                // 归还引用
                Release (audioPausing[j].gameObject);
                // 从正在暂停链表中删除
                audioPausing.RemoveAt (j);
            }
        }

        /// <summary>
        /// 减少引用引用计数
        /// </summary>
        public void ReduceReferenceCount (string cueSheet)
        {
            int index = -1;
            for (int i = 0; i < cueSheetRef.Count; i++)
            {
                if (cueSheetRef[i].cueSheet.Equals (cueSheet))
                    index = i;
            }

            if (index == -1)
                return;

            cueSheetRef[index].refCount--;
            if (cueSheetRef[index].refCount <= 0)
            {
                cueSheetRef[index].countDown = ManagerConfig.CriwareManagerConfig.CUE_SHEET_SURVIVAL_TIME;
                cueSheetCountDown.Add (cueSheetRef[index]);
                cueSheetRef.RemoveAt (index);
            }
        }

        /// <summary>
        /// 检查音乐组件池，超过一定时间后会清空
        /// </summary>
        private void CheckAudioPool (float deltaTime)
        {
            tempTime += deltaTime;
            if (tempTime > ManagerConfig.CriwareManagerConfig.AUDIO_POOL_CLEAR_INTERVAL)
            {
                tempTime = 0;
                ClearAudioPool ();
            }
        }

        /// <summary>
        /// 检查CueSheet存活时间，如果过时了，那就卸载
        /// </summary>
        private void CheckCueSheetCountDown (float deltaTime)
        {
            // List倒序删除
            for (int i = cueSheetCountDown.Count - 1; i >= 0; i--)
            {
                cueSheetCountDown[i].countDown -= deltaTime;
                if (cueSheetCountDown[i].countDown <= 0)
                {
                    CriAtom.RemoveCueSheet (cueSheetCountDown[i].cueSheet);
                    cueSheetCountDown.RemoveAt (i);
                }
            }
        }

        /// <summary>
        /// Debug信息
        /// </summary>
        private void DrawDebugMessage ()
        {
            // ref
            var refNode = SGFEntry.Instance.transform.Find("CriwareManager").transform.transform.Find ("CueSheetReferenceCount").transform;

            for (int i = 0, len = refNode.childCount; i < len; i++)
            {
                var child = refNode.GetChild (i);
                GameObject.Destroy (child.gameObject);
            }

            foreach (var sheet in cueSheetRef)
            {
                GameObject go = new GameObject ($"{sheet.cueSheet}_{sheet.refCount}");
                go.transform.SetParent (refNode);
            }

            // countdown
            var countDownNode = SGFEntry.Instance.transform.Find("CriwareManager").transform.Find ("CueSheetUnLoadCountDown").transform;

            for (int i = 0, len = countDownNode.childCount; i < len; i++)
            {
                var child = countDownNode.GetChild (i);
                GameObject.Destroy (child.gameObject);
            }

            foreach (var sheet in cueSheetCountDown)
            {
                GameObject go = new GameObject ($"{sheet.cueSheet}_{sheet.countDown}");
                go.transform.SetParent (countDownNode);
            }
        }
    #endregion
    
    }
}