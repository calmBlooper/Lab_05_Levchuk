using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_05_Levchuk.Models
{
     class ProcessInfo
    {
        private string _name, _id, _userName, _fileName, _filePath, _cpuUsage, _ramUsage;
        private bool _running;
        private string  _threadsCount;
        private string _launchDateTime;

        public ProcessInfo(string name, string id, bool running, string cpuUsage, string ramUsage, string threadsCount, string userName, string fileName, string filePath, string launchDateTime)
        {
            _name = name;
            _id = id;
            _userName = userName;
            _fileName = fileName;
            _filePath = filePath;
            _running = running;
            _cpuUsage = cpuUsage;
            _ramUsage = ramUsage;
            _threadsCount = threadsCount;
            _launchDateTime = launchDateTime;
        }

        public string Name { get => _name; set => _name = value; }
        public string Id { get => _id; set => _id = value; }
        public bool Running { get => _running; set => _running = value; }
        public string CpuUsage { get => _cpuUsage; set => _cpuUsage = value; }
        public string RamUsage { get => _ramUsage; set => _ramUsage = value; }
        public string ThreadsCount { get => _threadsCount; set => _threadsCount = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string FileName { get => _fileName; set => _fileName = value; }
        public string FilePath { get => _filePath; set => _filePath = value; }

      
      
        public string LaunchDateTime { get => _launchDateTime; set => _launchDateTime = value; }
    }
}
