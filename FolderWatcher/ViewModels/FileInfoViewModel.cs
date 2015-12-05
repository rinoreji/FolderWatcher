using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpHelpers.ExtensionMethods;

namespace FolderWatcher.ViewModels
{
    public class FileInfoViewModel : ViewModelBase
    {
        public FileInfoViewModel() { }

        public FileInfoViewModel(FileInfo fInfo)
        {
            if (fInfo.IsNotNull())
            {
                Name = fInfo.Name;
                FullName = fInfo.FullName;
                ChangeStatus = "";
                StatusUpdatedOn = DateTime.Now;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaizePropertyChanged("Name"); }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; RaizePropertyChanged("FullName"); }
        }

        private string _changeStatus;
        public string ChangeStatus
        {
            get { return _changeStatus; }
            set { _changeStatus = value; RaizePropertyChanged("ChangeStatus"); }
        }

        private DateTime _updatedOn;
        public DateTime StatusUpdatedOn
        {
            get { return _updatedOn; }
            set { _updatedOn = value; RaizePropertyChanged("StatusUpdatedOn"); }
        }

    }
}
