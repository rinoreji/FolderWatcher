using FolderWatcher.Model;
using SharpHelpers.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FolderWatcher.ViewModels
{
    public class FileListViewModel : ViewModelBase
    {
        FileListService _fileListService;
        public FileListViewModel()
        {
            FileList = new ObservableCollection<FileInfoViewModel>();
        }

        private ObservableCollection<FileInfoViewModel> _fileList;
        public ObservableCollection<FileInfoViewModel> FileList
        {
            get { return _fileList; }
            private set { _fileList = value; RaizePropertyChanged("FilesList"); }
        }

        private string _path;
        public string FolderPath
        {
            get { return _path; }
            set { _path = value; }
        }

        private string _filter;
        public string FileFilter
        {
            get
            {
                return _filter.IsNullOrWhiteSpace() ? "*.*" : _filter;
            }
            set { _filter = value; }
        }

        private bool _includeSubdirectories;
        public bool IncludeSubdirectories
        {
            get { return _includeSubdirectories; }
            set { _includeSubdirectories = value; RaizePropertyChanged("IncludeSubdirectories"); }
        }

        public void UpdateFileStatus(string filePath, string changeStatus, bool isRefreshNeeded = false)
        {
            if (isRefreshNeeded)
                RefreshFileList();

            if (!FileList.Any(f => f.FullName == filePath))
                InvokeOnDispatcherThread(() => FileList.Add(new FileInfoViewModel(new FileInfo(filePath))));
            else
            {
                var info = FileList.First(f => f.FullName == filePath);
                info.ChangeStatus = changeStatus;
                info.StatusUpdatedOn = DateTime.Now;
            }
        }

        public void RefreshFileList()
        {
            var _searchOption = IncludeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            _fileListService = new FileListService(FolderPath, FileFilter, _searchOption);
            UpdateFileList(_fileListService.GetFileList());
        }

        private void UpdateFileList(List<FileInfo> newList)
        {
            var filesToRemove = FileList.Where(f => !newList.Any(nf => nf.FullName == f.FullName)).ToList();
            foreach (var file in filesToRemove)
            {
                InvokeOnDispatcherThread(()=> FileList.Remove(file));
            }

            foreach (var file in newList)
            {
                if (!FileList.Any(f => f.FullName == file.FullName))
                    InvokeOnDispatcherThread(() => FileList.Add(new FileInfoViewModel(file)));
            }
        }

        void InvokeOnDispatcherThread(Action action)
        {
            App.Current.Dispatcher.Invoke(action);
        }
    }
}
