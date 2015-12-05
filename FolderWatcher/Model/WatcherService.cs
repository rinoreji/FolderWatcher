using SharpHelpers.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderWatcher
{
    public class WatcherService
    {
        FileSystemWatcher _watcher;

        public Action<string> Changed { get; set; }
        public Action<string> Created { get; set; }
        public Action<string> Deleted { get; set; }
        public Action<string,string> Renamed { get; set; }

        public WatcherService(string path, string filter, bool includeSubdirectory)
        {
            _watcher = new FileSystemWatcher();
            _watcher.Path = path; _watcher.Filter = filter;
            _watcher.IncludeSubdirectories = includeSubdirectory;

            SetupNotifications();
            HookEvents();
        }

        void HookEvents()
        {
            // Add event handlers.
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Created += new FileSystemEventHandler(OnChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnChanged);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);
            //_watcher.SynchronizingObject = this;
        }

        void SetupNotifications()
        {
            _watcher.NotifyFilter = NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.FileName |
                NotifyFilters.DirectoryName;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.All:
                    break;
                case WatcherChangeTypes.Changed:
                    if (Changed.IsNotNull())
                        Changed(e.FullPath);
                    break;
                case WatcherChangeTypes.Created:
                    if (Created.IsNotNull())
                        Created(e.FullPath);
                    break;
                case WatcherChangeTypes.Deleted:
                    if (Deleted.IsNotNull())
                        Deleted(e.FullPath);
                    break;
                case WatcherChangeTypes.Renamed:
                    break;
                default:
                    break;
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            if (Renamed.IsNotNull())
                Renamed(e.OldFullPath, e.FullPath);
        }

        public void Run()
        {
            // Begin watching.
            _watcher.EnableRaisingEvents = true;
        }
    }
}
