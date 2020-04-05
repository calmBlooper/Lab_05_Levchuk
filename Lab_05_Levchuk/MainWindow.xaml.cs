using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_05_Levchuk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var data = new Test { Test1 = "Test1", Test2 = "Test2",Test22=true, Test3 = "Test1", Test4 = "Test2", Test5 = "Test1", Test6 = "Test2", Test7 = "Test1",Test9=DateTime.Now.ToString() };
            //List<Test> lol = new List<Test>();
            //lol.Add(data);
            // DataGridTest.Items.Add(data);
           // DataGridTest.ItemsSource = lol;
       
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var data = new Test { Test1 = "Test1", Test2 = "Test2", Test22 = true, Test3 = "Test1", Test4 = "Test2", Test5 = "Test1", Test6 = "Test2", Test7 = "Test1", Test9 = DateTime.Now.ToString() };

            DataGridTest.Items.Add(data);
        }
    
}
   
public class Test
    {
        public string Test1 { get; set; }
        public string Test2 { get; set; }
        public bool Test22 { get; set; }
        public string Test3 { get; set; }
        public string Test4 { get; set; }
        public string Test5 { get; set; }
        public string Test6 { get; set; }
        public string Test7 { get; set; }

        public string Test9 { get; set; }
    }
}
