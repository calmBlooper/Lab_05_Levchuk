using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private string _sortHeader = "", _selId;
        private PerformanceCounter theCPUCounter =
new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private List<ProcessInfo> _processesList = new List<ProcessInfo>();
        private int RAMSize = GetRAMSize(), _selInd;
        private bool _pNameS = true, _activeS = true, _threadsCountS = true, _filenameS = true, _pathS = true, _usernameS = true, _idS = true, _cpuUsageS = true, _ramUsageS = true, _dateS = true;
        public MainVM()
        {
            UpdateProcesses();

            //MessageBox.Show(RAMSize.ToString());
            // Console.WriteLine(allProcesses[10].MainModule.FileName);

            // MessageBox.Show(allProcesses[10].MainModule.FileName);

            //OpenFolder(new FileInfo(allProcesses[10].MainModule.FileName).DirectoryName);
            Thread thread = new Thread(ConstantUpdate);
            thread.Start();

            //start running your thread


        }

        public List<ProcessInfo> ProcessesList { get => _processesList; set => _processesList = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int SelInd
        {
            set
            {
                _selInd = value;
                _selId = ProcessesList[value].Id;

            }
            get
            {

                return _selInd;
            }
        }
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
                    processListCopy.Add(new ProcessInfo(allProcesses[i].ProcessName, allProcesses[i].Id.ToString(), allProcesses[i].Responding, GetCPUUsage(allProcesses[i]) + "%", RAMUsage + " Mbs –  " + RAMUsage / (float)RAMSize * 100 + "%", allProcesses[i].Threads.Count.ToString(), GetUsername(allProcesses[i]),
                        allProcesses[i].MainModule.ModuleName, allProcesses[i].MainModule.FileName, allProcesses[i].StartTime.ToString()));
                }
                catch (Exception ex)
                {

                }
            }
            _processesList = new List<ProcessInfo>(processListCopy);
            if (_sortHeader.Length == 0) OnPropertyChanged("ProcessesList");
            SortUpdate();
            if (_selInd != -1)
            {
                _selInd = _processesList.IndexOf(_processesList.Where(p => p.Id.Equals(_selId)).FirstOrDefault());
                OnPropertyChanged("SelInd");
            }

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
                UInt64 SizeinMB = SizeinB / 1024 / 1024;
                result += (int)SizeinMB;
            }
            return result;
        }
        private float getRAMUsage(Process subject)
        {
            var counter = new PerformanceCounter("Process", "Working Set - Private", subject.ProcessName);
            return (float)Math.Round(((double)(counter.RawValue) / 1024 / 1024), 1);
        }
        public string SortHeader
        {
            set
            {
                _sortHeader = value;
                SortList();
            }
            get
            {
                return _sortHeader;
            }
        }
        private string GetUsername(Process subject)
        {
            ObjectQuery objQuery = new ObjectQuery("Select * From Win32_Process where ProcessId='" + subject.Id + "'");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(objQuery);
            string processOwner = "";
            foreach (ManagementObject mo in mos.Get())
            {
                string[] s = new string[2];
                mo.InvokeMethod("GetOwner", (object[])s);
                if (s[0] != null) processOwner = s[0].ToString();
                break;
            }
            return processOwner;
        }
        private void SortList()
        {


            if (_sortHeader.Length > 0)
            {
                List<ProcessInfo> SortedList;
                switch (_sortHeader)
                {
                    case "Name":
                        _activeS = true; ; _threadsCountS = true; _filenameS = true; _pathS = true; _usernameS = true; _idS = true; _cpuUsageS = true; _ramUsageS = true; _dateS = true;
                        if (_pNameS) SortedList = _processesList.OrderBy(o => o.Name).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.Name).ToList();
                        _pNameS = !_pNameS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "ID":
                        _activeS = true; ; _threadsCountS = true; _filenameS = true; _pathS = true; _usernameS = true; _pNameS = true; _cpuUsageS = true; _ramUsageS = true; _dateS = true;
                        if (_idS) SortedList = _processesList.OrderBy(o => o.Id).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.Id).ToList();
                        _idS = !_idS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "Active":
                        _pNameS = true; ; _threadsCountS = true; _filenameS = true; _pathS = true; _usernameS = true; _idS = true; _cpuUsageS = true; _ramUsageS = true; _dateS = true;
                        if (_activeS) SortedList = _processesList.OrderBy(o => o.Running).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.Running).ToList();
                        _activeS = !_activeS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "CPU usage":
                        _activeS = true; ; _threadsCountS = true; _filenameS = true; _pathS = true; _usernameS = true; _idS = true; _pNameS = true; _ramUsageS = true; _dateS = true;
                        if (_cpuUsageS) SortedList = _processesList.OrderBy(o => o.CpuUsage).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.CpuUsage).ToList();
                        _cpuUsageS = !_cpuUsageS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "RAM usage":
                        _activeS = true; ; _threadsCountS = true; _filenameS = true; _pathS = true; _usernameS = true; _idS = true; _cpuUsageS = true; _pNameS = true; _dateS = true;
                        if (_ramUsageS) SortedList = _processesList.OrderBy(o => o.RamUsage).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.RamUsage).ToList();
                        _ramUsageS = !_ramUsageS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "Threads number":
                        _activeS = true; _ramUsageS = true; _filenameS = true; _pathS = true; _usernameS = true; _idS = true; _cpuUsageS = true; _pNameS = true; _dateS = true;
                        if (_threadsCountS) SortedList = _processesList.OrderBy(o => o.ThreadsCount).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.ThreadsCount).ToList();
                        _threadsCountS = !_threadsCountS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "User":
                        _activeS = true; ; _threadsCountS = true; _filenameS = true; _pathS = true; _pNameS = true; _idS = true; _cpuUsageS = true; _ramUsageS = true; _dateS = true;
                        if (_usernameS) SortedList = _processesList.OrderBy(o => o.UserName).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.UserName).ToList();
                        _usernameS = !_usernameS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "File name":
                        _activeS = true; ; _threadsCountS = true; _pNameS = true; _pathS = true; _usernameS = true; _idS = true; _cpuUsageS = true; _ramUsageS = true; _dateS = true;
                        if (_filenameS) SortedList = _processesList.OrderBy(o => o.FileName).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.FileName).ToList();
                        _filenameS = !_filenameS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "Full path":
                        _activeS = true; ; _threadsCountS = true; _filenameS = true; _pNameS = true; _usernameS = true; _idS = true; _cpuUsageS = true; _ramUsageS = true; _dateS = true;
                        if (_pathS) SortedList = _processesList.OrderBy(o => o.FilePath).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.FilePath).ToList();
                        _pathS = !_pathS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "Date and time of launch":
                        _activeS = true; ; _threadsCountS = true; _pNameS = true; _pathS = true; _usernameS = true; _idS = true; _cpuUsageS = true; _ramUsageS = true; _pathS = true;
                        if (_dateS) SortedList = _processesList.OrderBy(o => o.LaunchDateTime).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.LaunchDateTime).ToList();
                        _dateS = !_dateS;
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    default:
                        break;
                }
                if (_selInd != -1)
                {
                    _selInd = _processesList.IndexOf(_processesList.Where(p => p.Id.Equals(_selId)).FirstOrDefault());
                    OnPropertyChanged("SelInd");
                }

            }
        }
        private void SortUpdate()
        {
            if (_sortHeader.Length > 0)
            {
                List<ProcessInfo> SortedList;
                switch (_sortHeader)
                {
                    case "Name":
                        if (!_pNameS) SortedList = _processesList.OrderBy(o => o.Name).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.Name).ToList();
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "ID":
                        if (!_idS) SortedList = _processesList.OrderBy(o => o.Id).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.Id).ToList();
                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "Active":
                        if (!_activeS) SortedList = _processesList.OrderBy(o => o.Running).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.Running).ToList();

                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "CPU usage":
                        if (!_cpuUsageS) SortedList = _processesList.OrderBy(o => o.CpuUsage).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.CpuUsage).ToList();

                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "RAM usage":
                        if (!_ramUsageS) SortedList = _processesList.OrderBy(o => o.RamUsage).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.RamUsage).ToList();

                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "Threads number":
                        if (!_threadsCountS) SortedList = _processesList.OrderBy(o => o.ThreadsCount).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.ThreadsCount).ToList();

                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "User":
                        if (!_usernameS) SortedList = _processesList.OrderBy(o => o.UserName).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.UserName).ToList();

                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "File name":
                        if (!_filenameS) SortedList = _processesList.OrderBy(o => o.FileName).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.FileName).ToList();

                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "Full path":
                        if (!_pathS) SortedList = _processesList.OrderBy(o => o.FilePath).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.FilePath).ToList();

                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    case "Date and time of launch":
                        if (!_dateS) SortedList = _processesList.OrderBy(o => o.LaunchDateTime).ToList();
                        else SortedList = _processesList.OrderByDescending(o => o.LaunchDateTime).ToList();

                        _processesList = new List<ProcessInfo>(SortedList);

                        break;
                    default:
                        break;
                }
                OnPropertyChanged("ProcessesList");
                if (_selInd != -1)
                {
                    _selInd = _processesList.IndexOf(_processesList.Where(p => p.Id.Equals(_selId)).FirstOrDefault());
                    OnPropertyChanged("SelInd");
                }
            }
        }
    }
}