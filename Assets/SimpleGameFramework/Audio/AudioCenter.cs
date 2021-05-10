using AudioFramework;
using SimpleGameFramework.Core;
using SimpleGameFramework.Resource;
using SimpleGameFramework.Resource.Bundle.Generated;
using UnityEngine;

namespace SimpleGameFramework.Audio
{
    public static class AudioCenter
    {
        /// <summary>
        /// 初始化Criware组件
        /// 可以不主动初始化，那么会在第一次播放声音的时候自动初始化，但可能引起卡顿
        /// </summary>
        public static void InitCriware ()
        {
            var criwareManager = SGFEntry.Instance.GetManager<CriwareManager>();
            if (criwareManager.criAtom != null)
                return;
            var prefab = SGFEntry.Instance.GetManager<AssetManager>().LoadAsset<GameObject>(BundleAssets.audio_AudioComponent);
            criwareManager.criAtom = prefab.transform.Find (ManagerConfig.CriwareManagerConfig.CRIATOM_GAMEOBJECT_NAME).GetComponent<CriAtom> ();
        }

        /// <summary>
        /// 设置配置文件
        /// 可以在组件Prefab里主动设置好
        /// </summary>
        /// <param name="acfPath">配置文件路径</param>
        public static void SetCriwareAcf (string acfPath)
        {
            InitCriware ();
            SGFEntry.Instance.GetManager<CriwareManager>().SetAcfFile (acfPath);
        }

        /// <summary>
        /// 初始化监听器，如果没有监听器，听不到3D声音
        /// </summary>
        public static void InitListener ()
        {
            InitCriware ();
            if (SGFEntry.Instance.GetManager<CriwareManager>().listener != null)
                return;
            SGFEntry.Instance.GetManager<CriwareManager>().listener = SGFEntry.Instance.GetManager<CriwareManager>().criAtom.transform.parent.Find (ManagerConfig.CriwareManagerConfig.LISTENER_GAMEOBJECT_NAME).gameObject.AddComponent<CriAtomListener> ();
        }

        /// <summary>
        /// 设置声音接收者
        /// </summary>
        /// <param name="trans">声音接收者，会被设置为listener的父节点</param>
        public static void SetListener (Transform trans)
        {
            if (SGFEntry.Instance.GetManager<CriwareManager>().listener == null)
                return;
            SGFEntry.Instance.GetManager<CriwareManager>().listener.transform.SetParent (trans, false);
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="audioData">音频信息</param>
        /// <param name="loop">是否循环</param>
        public static void PlayAudio (IAudioData audioData, bool loop = false)
        {
            InitCriware ();
            SGFEntry.Instance.GetManager<CriwareManager>().PlayAudio (audioData, loop);
        }

        /// <summary>
        /// 停止音频
        /// </summary>
        /// <param name="audioData">音频信息</param>
        public static void StopAudio (IAudioData audioData)
        {
            SGFEntry.Instance.GetManager<CriwareManager>().StopAudio (audioData);
        }

        /// <summary>
        /// 停止所有音乐，包括正在播放和正被暂停的
        /// </summary>
        public static void StopAllAudio ()
        {
            SGFEntry.Instance.GetManager<CriwareManager>().StopAllAudio ();
        }

        /// <summary>
        /// 暂停音频
        /// </summary>
        /// <param name="audioData">音频信息</param>
        public static void PauseAudio (IAudioData audioData)
        {
            SGFEntry.Instance.GetManager<CriwareManager>().PauseAudio (audioData);
        }

        /// <summary>
        /// 暂停所有音频
        /// </summary>
        public static void PauseAllAudio ()
        {
            SGFEntry.Instance.GetManager<CriwareManager>().PauseAllAudio ();
        }

        /// <summary>
        /// 恢复音频
        /// </summary>
        /// <param name="audioData">音频信息</param>
        public static void ResumeAudio (IAudioData audioData)
        {
            SGFEntry.Instance.GetManager<CriwareManager>().ResumeAudio (audioData);
        }

        /// <summary>
        /// 恢复所有音频
        /// </summary>
        public static void ResumeAllAudio ()
        {
            SGFEntry.Instance.GetManager<CriwareManager>().ResumuAllAudio ();
        }

        /// <summary>
        /// 控制音乐播放速度
        /// </summary>
        /// <param name="audioData">音频信息</param>
        public static void MusicSpeed (IAudioData audioData)
        {
            // TODO...
        }

        /// <summary>
        /// 控制所有音乐的播放速度
        /// </summary>
        public static void AllMusicSpeed ()
        {
            // TODO...
        }
    }
}