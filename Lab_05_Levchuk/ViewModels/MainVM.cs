using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace Lab_05_Levchuk.ViewModels
{
    class MainVM
    {

        public MainVM()
        {
            /*
            var allProcesses = Process.GetProcesses();
            string kek = "4";
            // Console.WriteLine("dvsdf");
    
                for (int i = 0; i < allProcesses.Length / 2; i++) {
                try
                {
                    kek += allProcesses[i].MainModule.FileName + "#########";
                }
                catch (Win32Exception ex)
                {

                }
            }
         
            MessageBox.Show(kek);
            // Console.WriteLine(allProcesses[10].MainModule.FileName);
       
                MessageBox.Show(allProcesses[10].MainModule.FileName);
      
            OpenFolder(new FileInfo(allProcesses[10].MainModule.FileName).DirectoryName);
            */
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