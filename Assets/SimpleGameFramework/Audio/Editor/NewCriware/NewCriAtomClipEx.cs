using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace AudioFramework
{
    [CustomEditor (typeof(NewCriAtomClip))]
    public class NewCriAtomClipEx : UnityEditor.Editor
    {
        public string[] acfName;
        public string[] acfPath;
        public string[] cueSheet;
        public string[] acbPath;
        public string[] awbPath;
        public string[] cueName;

        public int acfNameSelected;
        public int cueSheetSelected;
        public int cueNameSelected;

        public CriAtomWindowInfo projInfo;

        public NewCriAtomClip inst => target as NewCriAtomClip;

        public string searchPath = Application.streamingAssetsPath;

        public bool ShowPath = false;

        public Transform transform;

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();

            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            
            if (projInfo == null)
            {
                InitData ();
                InitAcfInfo ();
                InitCueSheets ();
            }

            ShowPath = EditorGUILayout.Toggle ("Show Path", ShowPath);

            // ACF
            acfNameSelected = EditorGUILayout.Popup ("ACF File", acfNameSelected, acfName);
            inst.acfName    = acfName[acfNameSelected];
            inst.acfPath    = acfPath[acfNameSelected];
            if (ShowPath)
            {
                EditorGUILayout.LabelField (inst.acfPath);
            }

            // CueSheet
            cueSheetSelected = EditorGUILayout.Popup ("Cue Sheet", cueSheetSelected, cueSheet);
            inst.cueSheet    = cueSheet[cueSheetSelected];
            inst.acbPath     = acbPath[cueSheetSelected];
            inst.awbPath     = awbPath[cueSheetSelected];
            if (ShowPath)
            {
                EditorGUILayout.LabelField (inst.acbPath);
                EditorGUILayout.LabelField (inst.awbPath);
            }

            // CueName
            InitCueNames ();
            cueNameSelected = EditorGUILayout.Popup ("Cue Name", cueNameSelected, cueName);
            if (cueNameSelected > cueName.Length)
                cueNameSelected = 0;
            inst.cueName                            = cueName[cueNameSelected];
            TimelineEditor.selectedClip.displayName = cueName[cueNameSelected];
            
            if (GUI.changed)
                EditorUtility.SetDirty (target);
        }

        private void InitData ()
        {
            CriAtomPlugin.InitializeLibrary ();
            projInfo = new CriAtomWindowInfo ();
        }

        private void InitAcfInfo ()
        {
            var acfInfos = projInfo.GetAcfInfoList (false, searchPath);
            acfName = new string[acfInfos.Count];
            acfPath = new string[acfInfos.Count];
            for (int i = 0, len = acfInfos.Count; i < len; i++)
            {
                acfName[i] = acfInfos[i].name;
                acfPath[i] = acfInfos[i].filePath;
                if (acfName[i].Equals (inst.acfName))
                    acfNameSelected = i;
            }
        }

        private void InitCueSheets ()
        {
            var acbInfos = projInfo.GetAcbInfoList (false, searchPath);
            cueSheet = new string[acbInfos.Count];
            acbPath  = new string[acbInfos.Count];
            awbPath  = new string[acbInfos.Count];
            for (int i = 0, len = acbInfos.Count; i < len; i++)
            {
                cueSheet[i] = acbInfos[i].name;
                acbPath[i]  = acbInfos[i].acbPath;
                awbPath[i]  = acbInfos[i].awbPath;
                if (cueSheet[i].Equals (inst.cueSheet))
                    cueSheetSelected = i;
            }
        }

        private void InitCueNames ()
        {
            var acbInfos = projInfo.GetAcbInfoList (false, searchPath);
            for (int i = 0, imax = acbInfos.Count; i < imax; i++)
            {
                if (acbInfos[i].name.Equals (inst.cueSheet))
                {
                    var cueNames = acbInfos[i].cueInfoList;
                    cueName = new string[cueNames.Count];
                    for (int j = 0, jmax = cueNames.Count; j < jmax; j++)
                    {
                        cueName[j] = cueNames[j].name;
                        if (cueName[j].Equals (inst.cueName))
                            cueNameSelected = j;
                    }
                    break;
                }
            }
        }

        /*private void DrawTransformConfig ()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField ("Transform Config", EditorStyles.boldLabel);

            var instConfig = inst.instConfig;
            
            // Root Type
            rootType = (ActorType)EditorGUILayout.EnumPopup ("Transform Root Type", instConfig.hierarchyRootType);
            if (rootType != instConfig.hierarchyRootType)
            {
                instConfig.hierarchyRootType           = rootType;
                instConfig.hierarchyPathRelativeToRoot = string.Empty;
                transform                              = null;
            }
            if (instConfig.hierarchyRootType == ActorType.Binding)
                instConfig.hierarchyRootType = ActorType.None;
            
            // Detach Parent
            instConfig.hierarchyDetachParent = EditorGUILayout.Toggle ("Detach Parent", instConfig.hierarchyDetachParent);

            if (rootType == ActorType.Player || rootType == ActorType.DirectorSkillTarget) 
                return;
            
            EditorGUILayout.BeginHorizontal ();

            instConfig.hierarchyPathRelativeToRoot = EditorGUILayout.TextField ("Path Relative To TransformRoot", instConfig.hierarchyPathRelativeToRoot);

            if (GUILayout.Button ("Clear", GUILayout.Width (50)))
            {
                instConfig.hierarchyPathRelativeToRoot = string.Empty;
                transform                              = null;
            }

            EditorGUILayout.EndHorizontal ();
            

            if (TimelineEditor.inspectedDirector != null)
            {

                if (instConfig.hierarchyRootType == ActorType.Binding || instConfig.hierarchyRootType == ActorType.None || instConfig.hierarchyRootType == ActorType.PlayableDirector) 
                {
                    transform = (Transform)EditorGUILayout.ObjectField ("BindTarget", transform, typeof(Transform));

                    if (transform != null)
                    {
                        switch (instConfig.hierarchyRootType)
                        {
                            case ActorType.Binding:
                            case ActorType.None:
                                instConfig.hierarchyPathRelativeToRoot = transform.GetFullPath ();
                                break;
                            case ActorType.PlayableDirector:
                                var instParent = TimelineEditor.inspectedDirector.transform;
                                if (transform.HierarchyNameRelativeToTarget (instParent, out var name))
                                    instConfig.hierarchyPathRelativeToRoot = name;
                                else
                                    EditorGUILayout.HelpBox ($"{transform.name} 不是 {TimelineEditor.inspectedDirector.name} 的孩子", MessageType.Error);
                                break;
                        }
                    }
                }
            }
            
            EditorGUILayout.LabelField("Position Offset");
            var prop         = serializedObject.FindProperty (nameof(inst.instConfig));
            var propPosition = prop.FindPropertyRelative ("localPosition");
            var labelWidth   = EditorGUIUtility.labelWidth;
            
            EditorGUIUtility.labelWidth = 30;

            TransformInspectorOverride.DrawPosition (propPosition);
            TransformInspectorOverride.DrawAllOperation (propPosition,null,null);
            
            EditorGUIUtility.labelWidth = labelWidth;
        }*/
    }
}