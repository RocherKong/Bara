using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bara.Common
{
    public class FileWatcherLoader
    {
        private FileWatcherLoader() { }
        private IList<FileSystemWatcher> _watchFileList = new List<FileSystemWatcher>();
        public static FileWatcherLoader Instance = new FileWatcherLoader();

        public void Watch(FileInfo fileInfo, Action OnFileChanged)
        {
            if (OnFileChanged != null)
            {
                WatchFileChange(fileInfo, OnFileChanged);
            }
        }

        private void WatchFileChange(FileInfo fileInfo, Action OnFileChanged)
        {
            FileSystemWatcher watcher = new FileSystemWatcher()
            {
                Path = fileInfo.DirectoryName,
                Filter = fileInfo.Name,
                NotifyFilter = NotifyFilters.LastWrite,
                EnableRaisingEvents = true,
            };

            #region Change Event
            DateTime lastChangeTime = DateTime.Now;
            watcher.Changed += (sender, e) =>
            {
                var timeWaste = (DateTime.Now - lastChangeTime).TotalMilliseconds;
                if (timeWaste <= 1000) { return; }
                OnFileChanged?.Invoke();
                lastChangeTime = DateTime.Now;
            };
            #endregion
            _watchFileList.Add(watcher);
        }

        public void Clear()
        {
            foreach (var watch in _watchFileList)
            {
                watch.EnableRaisingEvents = false;
                watch.Dispose();
            }
        }



    }
}
