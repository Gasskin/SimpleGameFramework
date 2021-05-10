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
    [TrackColor (0.3317462f, 0.6611561f, 0.990566f)]
    [TrackClipType (typeof(NewCriAtomClip))]
    [TrackBindingType (typeof(CriAtomSource))]
    public class NewCriAtomTrack : TrackAsset
    {
        public string m_AisacControls;
        public bool   m_StopOnWrapping = true;
    #if UNITY_EDITOR
        public bool m_IsRenderMono = true;
    #endif

        public override Playable CreateTrackMixer (PlayableGraph graph, GameObject owner, int inputCount)
        {
            var mixerPlayable  = ScriptPlayable<NewCriAtomMixerBehaviour>.Create (graph, inputCount);
            var mixerBehaviour = mixerPlayable.GetBehaviour ();
            if (mixerBehaviour != null)
            {
                mixerBehaviour.m_Director = owner.GetComponent<PlayableDirector> ();
                mixerBehaviour.m_Clips    = this.GetClips ();
                mixerBehaviour.m_Bind     = mixerBehaviour.m_Director.GetGenericBinding (this) as CriAtomSource;

                if (mixerBehaviour.m_Bind != null)
                {
                    NewCriAtomClip newCriAtomClip;
                    foreach (var clip in mixerBehaviour.m_Clips)
                    {
                        newCriAtomClip = clip.asset as NewCriAtomClip;

                        if (string.IsNullOrEmpty (newCriAtomClip.cueSheet) == true)
                        {
                            newCriAtomClip.cueSheet = mixerBehaviour.m_Bind.cueSheet;
                        }
                        if (string.IsNullOrEmpty (newCriAtomClip.cueName) == true)
                        {
                            newCriAtomClip.cueName = mixerBehaviour.m_Bind.cueName;
                        }

                        clip.displayName = newCriAtomClip.cueName;
                    }
                }
                mixerBehaviour.m_AisacControls  = this.m_AisacControls;
                mixerBehaviour.m_StopOnWrapping = this.m_StopOnWrapping;
            }
            return mixerPlayable;
        }
    }
}

#endif