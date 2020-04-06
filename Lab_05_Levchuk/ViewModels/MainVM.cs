using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Lab_05_Levchuk.Models;
using Lab_05_Levchuk.Tools;

namespace Lab_05_Levchuk.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        private PerformanceCounter theCPUCounter =
   new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private List<ProcessInfo> _processesList = new List<ProcessInfo>();
        private int RAMSize = GetRAMSize();
        public MainVM()
        {
            UpdateProcesses();
    
            MessageBox.Show(RAMSize.ToString());
            SortCommand = new RelayCommand(o => SortClick("HeaderButton"));
            // Console.WriteLine(allProcesses[10].MainModule.FileName);

            // MessageBox.Show(allProcesses[10].MainModule.FileName);

            //OpenFolder(new FileInfo(allProcesses[10].MainModule.FileName).DirectoryName);
            Thread thread = new Thread(ConstantUpdate);
            thread.Start();
            
            //start running your thread


        }
        public void DataGrid_Sorting(object sender, System.Windows.Controls.DataGridSortingEventArgs e)
        {
            MessageBox.Show("sorted");
        }
        public List<ProcessInfo> ProcessesList { get => _processesList; set => _processesList = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SortCommand { get; set; }
        public void SortClick(object sender)
        {
            MessageBox.Show("sorted");
        }
        private System.Timers.Timer timer1;

         private void ConstantUpdate()
         {
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        UpdateProcesses();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    Thread.Sleep(2000);
                }
            });
            task.Start();
        }
        private void UpdateProcesses()
        {

            var allProcesses = Process.GetProcesses();
            List<ProcessInfo> processListCopy = new List<ProcessInfo>();
            for (int i = 0; i < allProcesses.Length; i++)
            {
                try
                {
                    float RAMUsage = getRAMUsage(allProcesses[i]);
                    processListCopy.Add(new ProcessInfo(allProcesses[i].ProcessName, allProcesses[i].Id.ToString(), allProcesses[i].Responding, GetCPUUsage(allProcesses[i]) + "%", RAMUsage + " Mbs –  " + RAMUsage / (float)RAMSize * 100 + "%", allProcesses[i].Threads.Count.ToString(), allProcesses[i].Threads.Count.ToString(),
                        allProcesses[i].MainModule.ModuleName, allProcesses[i].MainModule.FileName, allProcesses[i].StartTime.ToString()));
                }
                catch (Win32Exception ex)
                {

                }
            }
            _processesList = new List<ProcessInfo>(processListCopy);
            OnPropertyChanged("ProcessesList");
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            try
            {
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
                { Arguments = folderPath, FileName = "explorer.exe" };
                Process.Start(startInfo);
            }
            else MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
        }
        private float GetCPUUsage(Process subject)
        {
          /*  PerformanceCounter theCPUCounter =
   new PerformanceCounter("Process", "% Processor Time", subject.ProcessName, true);
            return theCPUCounter.NextValue() / Environment.ProcessorCount;*/
            return 0;
        }


        private static int GetRAMSize()
        {
            string Query = "SELECT Capacity FROM Win32_PhysicalMemory";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query);
            int result = 0;
            foreach (ManagementObject WniPART in searcher.Get())
            {
                UInt64 SizeinB = Convert.ToUInt64(WniPART.Properties["Capacity"].Value);
                UInt64 SizeinMB = SizeinB / 1024/1024;
                result += (int)SizeinMB;
            }
            return result;
        }
        private float getRAMUsage(Process subject)
        {
            var counter = new PerformanceCounter("Process", "Working Set - Private", subject.ProcessName);
            return (float)Math.Round(((double)(counter.RawValue) / 1024 / 1024), 1);
        }
    }
}