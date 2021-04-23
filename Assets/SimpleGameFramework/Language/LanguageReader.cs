using System;
using System.Collections.Generic;
using System.IO;
using SimpleGameFramework.Language.Editor;
using UnityEngine;

namespace SimpleGameFramework.Language
{
    public class LanguageReader
    {
        /// <summary>
        /// 读取对应语种的字典，不存在时，读取 LanguageManager.DEFAULT_LANGUAGE 的语种字典
        /// </summary>
        public void Read (Dictionary<LanguageID, string> dic, string language)
        {
            // 目标语种
            var filePath = Application.dataPath + $"/SimpleGameFramework/Language/CSV/Languages/{language}.csv";

            // 如果目标语种不存在，那读取默认语种，注意，我们的CSV表的表名，一定要和SystemLanguage的枚举类型名称一致
            if (!File.Exists(filePath))
                filePath = Application.dataPath + $"/SimpleGameFramework/Language/CSV/Languages/{LanguageManager.DEFAULT_LANGUAGE.ToString ()}.csv";
            if (!File.Exists(filePath))
                throw new Exception($"不存在对应语种{language}的CSV表，也不存在对应默认语种{LanguageManager.DEFAULT_LANGUAGE.ToString()}的CSV表...");

            dic.Clear ();
            dic.Add(LanguageID.Invalid, "#Invalid#");

            StreamReader sr = new StreamReader(filePath);

            int i = -1;
            var data = sr.ReadLine();
            while (data != null) 
            {
                i++;
                if (!string.IsNullOrEmpty(data))
                {
                    dic.Add((LanguageID) (i + 1), data);
                }
                data = sr.ReadLine();
            }
        }
    }
}