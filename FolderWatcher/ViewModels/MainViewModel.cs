

using System;
using System.Timers;
namespace FolderWatcher.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public FileListViewModel FileListVM { get; set; }
        Timer timer = new Timer();

        public MainViewModel()
        {
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            FileListVM = new FileListViewModel();
            FileListVM.FolderPath = @"G:\Share4Team\RRc";
            FileListVM.IncludeSubdirectories = true;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    FileListVM.RefreshFileList();
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            });
        }
    }
}
