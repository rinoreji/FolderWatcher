
namespace FolderWatcher.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public FileListViewModel FileListVM { get; set; }

        public MainViewModel()
        {
            FileListVM = new FileListViewModel();
            FileListVM.FolderPath = @"D:\Playground\temp";
            FileListVM.IncludeSubdirectories = true;

            FileListVM.RefreshFileList();
        }
    }
}
