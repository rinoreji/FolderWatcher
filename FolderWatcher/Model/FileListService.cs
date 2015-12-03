using SharpHelpers.ExtensionMethods;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FolderWatcher.Model
{
    public class FileListService
    {
        string _folderPath;
        string _fileFilter;
        SearchOption _searchOptions;

        public FileListService(string folderPath, string fileFilter, SearchOption searchOptions)
        {
            _folderPath = folderPath; _fileFilter = fileFilter; _searchOptions = searchOptions;
        }

        internal List<FileInfo> GetFileList()
        {
            if (_folderPath.IsNullOrWhiteSpace())
                throw new DirectoryNotFoundException("Path not set");
            var dirInfo = new DirectoryInfo(_folderPath);
            if (!dirInfo.Exists)
                throw new DirectoryNotFoundException("Invalid directory");

            return dirInfo.GetFiles(_fileFilter, _searchOptions).ToList();
        }
    }
}
