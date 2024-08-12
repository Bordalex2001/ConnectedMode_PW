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
using ConnectedMode.Model;
using ConnectedMode.Providers;

namespace ConnectedMode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlDbProvider db = new SqlDbProvider();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            facultiesCmBox.ItemsSource = await db.GetFacultiesAsync();
            facultiesCmBox.DisplayMemberPath = "Name";
        }

        private async void facultiesCmBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Faculty f = facultiesCmBox.SelectedItem as Faculty;
            if (f != null)
            {
                groupsCmBox.ItemsSource = await db.GetGroupsAsync(f.Id);
                groupsCmBox.DisplayMemberPath = "Name";
            }
            else
            {
                groupsCmBox.ItemsSource = null;
            }
        }

        private async void groupsCmBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group g = groupsCmBox.SelectedItem as Group;
            if (g != null)
            {
                Task<List<Student>> getStudentsTask = db.GetStudentsAsync(g.Id);
                Task<int> countStudentsTask = db.GetCountOfStudentsAsync(g.Id);
                
                studentsListBox.ItemsSource = await getStudentsTask;
                studentsListBox.DisplayMemberPath = "FullName";          
                CountStudentsTxtBlock.Text = (await countStudentsTask).ToString();
            }
            else
            {
                studentsListBox.ItemsSource = null;
                CountStudentsTxtBlock.Text = String.Empty;
            }
        }
    }
}
