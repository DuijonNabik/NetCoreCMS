﻿using NetCoreCMS.Framework.Core.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Lib
{
    public class TranslationFile
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string Content { get; set; }
        public string Culture { get; set; }
        public string Group { get; set; }

        public void Save()
        {
            if (File.Exists(FullPath))
            {
                File.WriteAllText(FullPath, Content);
            }
        }

        public string Load()
        {
            if (File.Exists(FullPath))
            {
                var fi = new FileInfo(FullPath);
                FileName = fi.Name;
                Culture = GetCultureFromFileName(FileName);
                Content = File.ReadAllText(FullPath);
                Id = Cryptography.GetHash(FullPath);
            }
            return Content;
        }

        public static string GetCultureFromFileName(string name)
        {
            var nameParts = name.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length >= 2)
            {
                return nameParts[nameParts.Length - 2];
            }
            return "-";
        }
    }
}
