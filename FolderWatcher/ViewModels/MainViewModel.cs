
using SharpHelpers.ExtensionMethods;
using System;
using System.Timers;

namespace FolderWatcher.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public FileListViewModel FileListVM { get; set; }
        WatcherService _watcher;

        public MainViewModel()
        {
            FileListVM = new FileListViewModel();
            FileListVM.FolderPath = "DefaultPath".FromAppSettings("%temp%").ExpandPath();
            FileListVM.FileFilter = "DefaultFilter".FromAppSettings("*.*");
            FileListVM.IncludeSubdirectories = "IncludeSubdirectorySearch".FromAppSettings(false);

            _watcher = new WatcherService(FileListVM.FolderPath, FileListVM.FileFilter, FileListVM.IncludeSubdirectories);
         
            _watcher.Changed = (fPath) => FileListVM.UpdateFileStatus(fPath, "Changed");
            _watcher.Created = (fPath) => FileListVM.UpdateFileStatus(fPath, "Created");
            _watcher.Deleted = (fPath) => FileListVM.UpdateFileStatus(fPath, "Deleted", isRefreshNeeded:true);
            _watcher.Renamed = (oPath,nPath) => FileListVM.UpdateFileStatus(nPath, "Renamed", isRefreshNeeded:true);

            FileListVM.RefreshFileList();
            _watcher.Run();
        }
    }
}
