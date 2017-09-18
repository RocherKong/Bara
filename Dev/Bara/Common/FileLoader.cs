using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bara.Common
{
    public class FileLoader
    {
        public static FileInfo GetFileInfo(string filePath)
        {
            bool IsAbsolutePath = filePath.IndexOf(":") > 0;
            if (IsAbsolutePath)
            {
                filePath = Path.Combine(AppContext.BaseDirectory, filePath);
            }
            return new FileInfo(filePath);
        }

        public static Stream Load(String filePath)
        {
            var fileInfo = GetFileInfo(filePath);
            return Load(fileInfo);
        }

        public static Stream Load(FileInfo fileInfo)
        {
            return fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}
