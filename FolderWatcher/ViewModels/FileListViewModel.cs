using FolderWatcher.Model;
using SharpHelpers.ExtensionMethods;
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
            FileList = new ObservableCollection<FileInfo>();
        }

        private ObservableCollection<FileInfo> _fileList;
        public ObservableCollection<FileInfo> FileList
        {
            get { return _fileList; }
            private set { _fileList = value; RaizePropertyChanged("Files"); }
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

        public void RefreshFileList()
        {
            var _searchOption = IncludeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            _fileListService = new FileListService(FolderPath, FileFilter, _searchOption);
            UpdateFileList(_fileListService.GetFileList());
        }

        private void UpdateFileList(List<FileInfo> newList)
        {
            var filesToRemove = FileList.Where(f => !newList.Any(nf => nf.FullName == f.FullName));
            foreach (var file in filesToRemove)
            {
                FileList.Remove(file);
            }

            foreach (var file in newList)
            {
                if (!FileList.Any(f => f.FullName == file.FullName))
                    FileList.Add(file);
            }
        }
    }
}
