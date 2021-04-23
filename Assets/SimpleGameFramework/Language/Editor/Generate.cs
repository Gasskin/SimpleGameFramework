using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace SimpleGameFramework.Language.Editor
{
    public static class Generate
    {
        // LanguageID.csv 所在位置
        public static string filePath = Application.dataPath + "/SimpleGameFramework/Language/CSV/LanguageID.csv";
        // 自动生成的保存路径
        public static string generatePath = Application.dataPath + "/SimpleGameFramework/Language/Generated/LanguageID.cs";
        
        [MenuItem("SGFTools/Language/Generate LanguageID.cs")]
        public static void GenerateLanguageID()
        {
            StringBuilder sb = new StringBuilder();
            
            // 打开csv读取所有字符
            var datas = File.ReadAllLines(filePath, Encoding.UTF8);

            sb.AppendLine("//==========================================");
            sb.AppendLine("// 这个文件是自动生成的...");
            sb.AppendLine($"// 生成日期：{DateTime.Now.Year}年{DateTime.Now.Month}月{DateTime.Now.Day}日{DateTime.Now.Hour}点{DateTime.Now.Minute}分");
            sb.AppendLine("//");
            sb.AppendLine("//");
            sb.AppendLine("//==========================================");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("namespace SimpleGameFramework.Language.Editor");
            sb.AppendLine("{");
            sb.AppendLine("    public enum LanguageID");
            sb.AppendLine("    {");
            sb.AppendLine("        Invalid = 0,");
            for (int i = 0; i < datas.Length; i++)
            {
                if (string.IsNullOrEmpty(datas[i])) 
                    continue;
                
                var temp = datas[i].Split(',');
                if (string.IsNullOrEmpty(temp[0]))
                    continue;
                
                var enumName = temp[0].Substring(0, 1).ToUpper() + temp[0].Substring(1).ToLower();
                sb.AppendLine($"        /// {temp[1]}");
                sb.AppendLine($"        {enumName} = {i + 1},");
                sb.AppendLine();
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(generatePath, sb.ToString());

            AssetDatabase.Refresh();
        }
    }
}
