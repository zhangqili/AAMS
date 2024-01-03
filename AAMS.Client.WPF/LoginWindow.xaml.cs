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
using System.Windows.Shapes;
using AAMS.Client.WPF.Lib;
using AAMS.Client.WPF.Properties;
using MahApps.Metro.Controls;

namespace AAMS.Client.WPF
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.Text = Settings.Default.Login;
            PasswordBox.Password = Settings.Default.Password;
            ServerTextBox.Text = Settings.Default.Server;
            PortTextBox.Text = Settings.Default.Port.ToString();
            DatabaseNameBox.Text = Settings.Default.Database;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StaticVariables.AAMSConnect = new AAMSConnect(UsernameTextBox.Text, PasswordBox.Password,ServerTextBox.Text, int.Parse(PortTextBox.Text), DatabaseNameBox.Text);
                Settings.Default.Login = UsernameTextBox.Text;
                Settings.Default.Password = PasswordBox.Password;
                Settings.Default.Server = ServerTextBox.Text;
                Settings.Default.Port = int.Parse(PortTextBox.Text);
                Settings.Default.Database = DatabaseNameBox.Text;
                Settings.Default.Save();
                //new MainWindow().Show();
            }
            catch (Exception exception)
            {
                return;
            }
            Close();
        }
    }
}
