using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class GenerateUIPath
{
    [MenuItem("SGFTools/UI/Generate UID.cs")]
    public static void Generate()
    {
        // Application.dataPath是Assets的路径
        var path = Application.dataPath + "/SimpleGameFramework/UI/Generated";
        // 用来给txt文件填充字符串
        StringBuilder sb = new StringBuilder();
        // 获取目前注册过的UI的名字
        var names = Enum.GetNames(typeof(UIRegister));
        // 如果不存在目录，新建一个
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        sb.AppendLine("//==========================================");
        sb.AppendLine("// 这个文件是自动生成的...");
        sb.AppendLine($"// 生成日期：{DateTime.Now.Year}年{DateTime.Now.Month}月{DateTime.Now.Day}日{DateTime.Now.Hour}点{DateTime.Now.Minute}分");
        sb.AppendLine("//");
        sb.AppendLine("//");
        sb.AppendLine("//==========================================");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("public struct UIStruct");
        sb.AppendLine("{");
        sb.AppendLine("    public string name;");
        sb.AppendLine("    public string path;");
        sb.AppendLine("    public UIStruct(string name,string path)");
        sb.AppendLine("    {");
        sb.AppendLine("        this.name = name;");
        sb.AppendLine("        this.path = path;");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("public static class UIs");
        sb.AppendLine("{");
        for (int i = 0, imax = names.Length; i < imax; i++)
        {
            if (names[i].Equals("None")) 
            {
                Debug.Log(12321);
                continue;
            }
            sb.AppendLine($"    public static UIStruct {names[i]} = new UIStruct(\"{names[i]}\",\"UI/{names[i]}\");");
        }
        sb.AppendLine("}");

        var wirtePath = path + "/UID.cs";
        File.WriteAllText(wirtePath,sb.ToString(),Encoding.UTF8);
        
        AssetDatabase.Refresh();
    }
}
