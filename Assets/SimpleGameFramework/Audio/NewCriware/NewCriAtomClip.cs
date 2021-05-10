/****************************************************************************
 *
 * Copyright (c) 2019 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/
#define CRIWARE_TIMELINE_1_OR_NEWER
#if UNITY_2018_1_OR_NEWER && CRIWARE_TIMELINE_1_OR_NEWER

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace AudioFramework
{
    public class NewCriAtomClip : PlayableAsset, ITimelineClipAsset
    {
        [HideInInspector]
        public string acfName;
        [HideInInspector]
        public string acfPath;
        [HideInInspector]
        public string cueSheet;
        [HideInInspector]
        public string acbPath;
        [HideInInspector]
        public string awbPath;
        [HideInInspector]
        public string cueName;
        

        public bool stopWithoutRelease = false;
        public bool muted              = false;
        public bool ignoreBlend        = false;
        public bool loopWithinClip     = false;

        public NewCriAtomBehaviour templateBehaviour = new NewCriAtomBehaviour ();

        [SerializeField, HideInInspector] private double clipDuration = 0.0;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.Looping | ClipCaps.SpeedMultiplier | ClipCaps.Blending; }
        }

        public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<NewCriAtomBehaviour>.Create (graph, templateBehaviour);
            return playable;
        }

        public void SetClipDuration (double clipDuration)
        {
            this.clipDuration = clipDuration;
        }

        public override double duration
        {
            get
            {
                return clipDuration > 0.0 ? clipDuration : 2.0;
            }
        }
    }
}

#endif