
using System.Windows;
using System.Windows.Controls.Primitives;
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

       
        }
        private void columnHeader_Click(object sender, RoutedEventArgs e)
        {
            var columnHeader = sender as DataGridColumnHeader;
            if (columnHeader != null)
            {
                SortHelp.Text= columnHeader.Column.Header.ToString();
            }
        }

    }
   

}
