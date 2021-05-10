/****************************************************************************
 *
 * Copyright (c) 2019 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
#define CRIWARE_TIMELINE_1_OR_NEWER
#if UNITY_2018_1_OR_NEWER && CRIWARE_TIMELINE_1_OR_NEWER

using System;
using System.IO;
using System.Threading;
using SimpleGameFramework.Audio;
using SimpleGameFramework.Core;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace AudioFramework
{
    public struct NewCriAtomClipPlayConfig
    {
        readonly public string cueSheetName;
        readonly public string cueName;
        readonly public string acbPath;
        readonly public string awbPath;
        readonly public string acfPath;

        readonly public long   startTimeMs;
        readonly public double speedRate;
        readonly public bool   loop;

        public NewCriAtomClipPlayConfig (
            string cueSheetName,
            string cueName,
            string acbPath,
            string awbPath,
            string acfPath,
            long startTimeMs,
            double speedRate,
            bool loop
        )
        {
            this.cueSheetName = cueSheetName;
            this.cueName      = cueName;
            this.acbPath      = acbPath;
            this.awbPath      = awbPath;
            this.startTimeMs  = startTimeMs;
            this.speedRate    = speedRate;
            this.loop         = loop;
            this.acfPath      = acfPath;
        }
    }

    [Serializable]
    public class NewCriAtomBehaviour : PlayableBehaviour
    {
        [Range (0.0f, 1.0f)]
        public float volume = 1f;
        [Range (-1200.0f, 1200.0f)]
        public float pitch = 0f;
        [Range (0.0f, 1.0f)]
        public float AISACValue = 0f;

        static private int cPreviewStopTimeMs = 500;

        private CriAtomExAcb m_acb              = null;
        private string       m_lastCueSheetName = null;

        public  CriAtomExPlayback playback { get; private set; }
        private bool              _IsClipPlaying = false;
        public  bool              IsClipPlaying { get { return _IsClipPlaying; } private set { _IsClipPlaying = value; } }
        private double            _CueLength = 0;
        public  double            CueLength { get { return _CueLength; } private set { _CueLength = value; } }

        public PlayableDirector m_Direct;
        public GameObject       tempObject;
        public string           path;
        public bool             isDetachParent = false;
        public Vector3          offset;

        public override void OnGraphStop (Playable playable)
        {
            base.OnGraphStop (playable);

            this.Stop (true);
        }

        public override void OnBehaviourPlay (Playable playable, FrameData info)
        {
            if (m_Direct == null)
                m_Direct = playable.GetGraph<Playable> ().GetResolver () as PlayableDirector;

            if (tempObject == null)
            {
                // RunTime
                if (Application.isPlaying)
                {
                    AudioCenter.InitCriware ();
                    tempObject                         = SGFEntry.Instance.GetManager<CriwareManager>().Acquire (null, isDetachParent);
                    tempObject.transform.localPosition += offset;
                }
            #if UNITY_EDITOR
                // Editor
                else
                {
                    tempObject = new GameObject ("CriwareComponent");
                    tempObject.AddComponent<CriAtomSource> ();
                }
            #endif
            }

            if (m_Direct == null)
                throw new Exception ("动态添加PlayableDirector错误");

            foreach (var bind in m_Direct.playableAsset.outputs)
            {
                if (bind.streamName.Equals ("New Cri Atom Track"))
                {
                    // 绑定所需组件
                    m_Direct.SetGenericBinding (bind.sourceObject, tempObject);
                }
            }
        }

        public override void OnBehaviourPause (Playable playable, FrameData info)
        {
            if (tempObject != null)
            {
                // Runtime
                if (Application.isPlaying)
                {
                    SGFEntry.Instance.GetManager<CriwareManager>().Release (tempObject);
                    SGFEntry.Instance.GetManager<CriwareManager>().ReduceReferenceCount (m_lastCueSheetName);
                }
            #if UNITY_EDITOR
                // Editor
                else
                {
                    GameObject.DestroyImmediate (tempObject);
                    tempObject = null;
                }
            #endif
            }
        }

        public void Play (CriAtomSource atomSource, NewCriAtomClipPlayConfig config)
        {
            this.IsClipPlaying = true;

            if (atomSource == null)
            {
                return;
            }

            if (config.cueSheetName != m_lastCueSheetName)
            {
                // 检查当前音频所属的sheet是否加载过
                SGFEntry.Instance.GetManager<CriwareManager>().CheckSheet (config.cueSheetName, config.acbPath, config.awbPath);
                // 设置CriAtom的配置文件为当前音频的配置文件
                SGFEntry.Instance.GetManager<CriwareManager>().SetAcfFile (config.acfPath);
                m_acb = CriAtom.GetAcb (config.cueSheetName);
            }
            if (m_acb != null)
            {
            #if UNITY_EDITOR
                tempObject.name = config.cueName;
            #endif
                SGFEntry.Instance.GetManager<CriwareManager>().audioPlaying.Add (atomSource);

                atomSource.player.SetCue (m_acb, config.cueName);
                this.CueLength     = GetCueLengthSec (m_acb, config.cueName);
                m_lastCueSheetName = config.cueSheetName;

                if (this.playback.status != CriAtomExPlayback.Status.Removed)
                {
                    this.playback.Stop ();
                }

                if (this.CueLength > 0)
                {
                    atomSource.player.SetStartTime (config.startTimeMs);
                    atomSource.player.Loop (config.loop);
                    this.playback = atomSource.player.Start ();
                }
            }
        }

        public void PreviewPlay (Guid trackId, bool instantStop, NewCriAtomClipPlayConfig config)
        {
            this.IsClipPlaying = true;

            if (config.cueSheetName != m_lastCueSheetName)
            {
                //m_acb = CriAtomTimelinePreviewer.Instance.GetAcb (config.cueSheetName);
                string acbPath = (string.IsNullOrEmpty (config.acbPath)) ? null : Path.Combine (CriWare.Common.streamingAssetsPath, config.acbPath);
                string awbPath = (string.IsNullOrEmpty (config.awbPath)) ? null : Path.Combine (CriWare.Common.streamingAssetsPath, config.awbPath);
                m_acb = CriAtomExAcb.LoadAcbFile (null, acbPath, awbPath);
            }
            if (m_acb != null)
            {
                CriAtomTimelinePreviewer.Instance.SetCue (trackId, m_acb, config.cueName);
                this.CueLength     = GetCueLengthSec (m_acb, config.cueName);
                m_lastCueSheetName = config.cueSheetName;

                if (this.playback.status != CriAtomExPlayback.Status.Removed)
                {
                    this.playback.Stop ();
                }

                if (this.CueLength > 0)
                {
                    CriAtomTimelinePreviewer.Instance.SetStartTime (trackId, config.startTimeMs);
                    CriAtomTimelinePreviewer.Instance.SetLoop (trackId, config.loop);
                    this.playback = CriAtomTimelinePreviewer.Instance.Play (trackId);
                    if (instantStop)
                    {
                        WaitAndStop ();
                    }
                }
            }
        }

        private void WaitAndStop ()
        {
            var thread = new Thread (() =>
            {
                Thread.Sleep (cPreviewStopTimeMs);
                this.Stop (true);
            });
            thread.Start ();
        }

        public void Stop (bool noReleaseTime = false)
        {
            this.playback.Stop (noReleaseTime);
            this.IsClipPlaying = false;
        }

        private double GetCueLengthSec (CriAtomExAcb acb, string cueName)
        {
            CriAtomEx.WaveformInfo waveInfo;
            if (acb != null && acb.GetWaveFormInfo (cueName, out waveInfo) == true)
            {
                return waveInfo.numSamples / (double)waveInfo.samplingRate;
            }
            else
            {
                return 0;
            }
        }

    }
}

#endif