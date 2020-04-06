using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Lab_05_Levchuk.Models;
using Lab_05_Levchuk.Tools;
namespace Lab_05_Levchuk.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        private List<ProcessInfo> _processesList=new List<ProcessInfo>();

        public MainVM()
        {
            
            var allProcesses = Process.GetProcesses();

            // Console.WriteLine("dvsdf");
            for (int i = 0; i < allProcesses.Length; i++)
            {
                try
                {
                    _processesList.Add(new ProcessInfo(allProcesses[i].ProcessName, allProcesses[i].Id.ToString(), allProcesses[i].Responding, allProcesses[i].WorkingSet64.ToString(), allProcesses[i].WorkingSet64.ToString(), allProcesses[i].Threads.Count.ToString(), allProcesses[i].Threads.Count.ToString(),
                        allProcesses[i].MainModule.ModuleName, allProcesses[i].MainModule.FileName, allProcesses[i].StartTime.ToString()));
                }
                catch (Win32Exception ex)
                {

                }
            }
                try
            {
                OnPropertyChanged("ProcessesList");
                //MessageBox.Show(ProcessesList.Count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SortCommand = new RelayCommand(o => SortClick("HeaderButton"));
            // Console.WriteLine(allProcesses[10].MainModule.FileName);

            // MessageBox.Show(allProcesses[10].MainModule.FileName);

            //OpenFolder(new FileInfo(allProcesses[10].MainModule.FileName).DirectoryName);

        }
        public void DataGrid_Sorting(object sender, System.Windows.Controls.DataGridSortingEventArgs e)
        {
            MessageBox.Show("sorted");
        }
        public List<ProcessInfo> ProcessesList { get => _processesList; set =>_processesList = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SortCommand { get; set; }
        public void SortClick(object sender)
        {
            MessageBox.Show("sorted");
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            try { 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


}

        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {Arguments = folderPath, FileName = "explorer.exe"};
            Process.Start(startInfo);
        }
    else MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
}


}
}