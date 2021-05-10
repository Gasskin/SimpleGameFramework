using System;
using System.IO;
using System.Text;
using SimpleGameFramework.Core;
using UnityEditor;

namespace SimpleGameFramework.Audio.Editor
{
    public class GenerateAudioData
    {
        [MenuItem ("SGFTools/Audio/Generate CriwareAudios.cs")]
        public static void GenerateCriwareAudioData ()
        {
            // 获取音频文件信息
            CriAtomPlugin.InitializeLibrary ();
            CriAtomWindowInfo projInfo   = new CriAtomWindowInfo ();
            var               acfDatas   = projInfo.GetAcfInfoList (true, ManagerConfig.CriwareManagerConfig.SEARCH_PATH);
            var               audioDatas = projInfo.GetAcbInfoList (true, ManagerConfig.CriwareManagerConfig.SEARCH_PATH);

            StringBuilder sb = new StringBuilder ();

            sb.AppendLine ("//==========================================");
            sb.AppendLine ("// 这个文件是自动生成的...");
            sb.AppendLine ($"// 生成日期：{DateTime.Now.Year}年{DateTime.Now.Month}月{DateTime.Now.Day}日{DateTime.Now.Hour}点{DateTime.Now.Minute}分");
            sb.AppendLine ("//");
            sb.AppendLine ("//");
            sb.AppendLine ("//==========================================");
            sb.AppendLine ();
            sb.AppendLine ();
            sb.AppendLine ("namespace AudioFramework");
            sb.AppendLine ("{");
            sb.AppendLine ("    public static class CriwareAudios");
            sb.AppendLine ("    {");
            sb.AppendLine ("        public static class Acf");
            sb.AppendLine ("        {");
            for (int i = 0, imax = acfDatas.Count; i < imax; i++)
            {
                sb.AppendLine ($"            public const string {acfDatas[i].name.ToLower()} = \"{acfDatas[i].filePath}\";");
            }
            sb.AppendLine ("        }");
            sb.AppendLine ();
            for (int i = 0, imax = audioDatas.Count; i < imax; i++)
            {
                var cueSheet = audioDatas[i];
                sb.AppendLine ($"        public static class {cueSheet.name.ToLower()}");
                sb.AppendLine ("        {");
                for (int j = 0, jmax = cueSheet.cueInfoList.Count; j < jmax; j++)
                {
                    var cue = cueSheet.cueInfoList[j];
                    sb.AppendLine ($"            public static CriwareData {cue.name.ToLower()} = new CriwareData(\"{cueSheet.name}\",\"{cue.name}\",\"{cueSheet.acbPath}\",\"{cueSheet.awbPath}\");");
                }
                sb.AppendLine ("        }");
                sb.AppendLine ();
            }
            sb.AppendLine ("    }");
            sb.AppendLine ("}");
            File.WriteAllText (ManagerConfig.CriwareManagerConfig.SAVE_PATH, sb.ToString (), Encoding.UTF8);

            AssetDatabase.Refresh ();
        }
    }
}