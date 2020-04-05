using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_05_Levchuk.Models
{
    class ProcessInfo
    {
        private string _name, _id, _userName, _fileName, _filePath;
        private bool _running;
        private int _cpuUsage, _ramUsage, _threadsCount;
        private DateTime _launchDateTime;

        public ProcessInfo(string name, string id, string userName, string fileName, string filePath, bool running, int cpuUsage, int ramUsage, int threadsCount, DateTime launchDateTime)
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

    }
}
