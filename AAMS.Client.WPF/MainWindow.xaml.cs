using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AAMS.Client.WPF.Lib;
using AAMS.Client.WPF.ViewModels;
using AAMS.Client.WPF.Views;
using MahApps.Metro.Controls;

namespace AAMS.Client.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            new LoginWindow().ShowDialog();
            if (StaticVariables.AAMSConnect == null) 
                Environment.Exit(0);
            InitializeComponent();
            StaticVariables.ClassManagementView = new();
            StaticVariables.ClassManagementViewModel = StaticVariables.ClassManagementView.DataContext as ClassManagementViewModel;
            StaticVariables.DepartmentManagementView = new();
            StaticVariables.DepartmentManagementViewModel =
                StaticVariables.DepartmentManagementView.DataContext as DepartmentManagementViewModel;
            StaticVariables.StudentManagementView = new();
            StaticVariables.StudentManagementViewModel = StaticVariables.StudentManagementView.DataContext as StudentManagementViewModel;
            StaticVariables.GradeManagementView = new();
            StaticVariables.GradeManagementViewModel = StaticVariables.GradeManagementView.DataContext as GradeManagementViewModel;
            SwitchPage(new WelcomeView());
        }

        private void MainTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            switch (((TreeViewItem)(e.NewValue)).Header as string)
            {
                case "欢迎":
                    SwitchPage(new WelcomeView());
                    break;
                case "院系管理":
                    StaticVariables.DepartmentManagementViewModel.Update();
                    SwitchPage(StaticVariables.DepartmentManagementView);
                    break;
                case "课程管理":
                    StaticVariables.ClassManagementViewModel.Update();
                    SwitchPage(StaticVariables.ClassManagementView);
                    break;
                case "学生管理":
                    StaticVariables.StudentManagementViewModel.Update();
                    SwitchPage(StaticVariables.StudentManagementView);
                    break;
                case "成绩管理":
                    StaticVariables.GradeManagementViewModel.Update();
                    SwitchPage(StaticVariables.GradeManagementView);
                    break;
                default:
                    break;
            }
        }

        private void SwitchPage(object content)
        {
            MainContainer.Child = content as UserControl;
        }

        private void UpdateAllPage()
        {
            StaticVariables.DepartmentManagementViewModel?.Update();
            StaticVariables.ClassManagementViewModel?.Update();
            StaticVariables.StudentManagementViewModel?.Update();
            StaticVariables.GradeManagementViewModel?.Update();
        }
        private void AddStudentMenuitem_Click(object sender, RoutedEventArgs e)
        {
            new AddStudentWindow().ShowDialog();
            UpdateAllPage();
        }

        private void AddDepartmentMenuitem_Click(object sender, RoutedEventArgs e)
        {
            new AddDepartmentWindow().ShowDialog();
            UpdateAllPage();
        }

        private void AddClassMenuitem_Click(object sender, RoutedEventArgs e)
        {
            new AddClassWindow().ShowDialog();
            UpdateAllPage();
        }

        private void AddGradeMenuitem_Click(object sender, RoutedEventArgs e)
        {
            new AddGradeWindow().ShowDialog();
            UpdateAllPage();;
        }

        private void RestartMenuItem_Click(object sender, RoutedEventArgs e)
        {

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void SQLWindowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SQLWindow().Show();
        }

        private void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllPage();
        }
    }
}