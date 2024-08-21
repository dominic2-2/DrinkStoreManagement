using DrinkStoreApp.Models;
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

namespace DrinkStoreApp.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly DrinkStoreContext _dbContext;

        public LoginWindow()
        {
            InitializeComponent();
            _dbContext = new DrinkStoreContext();
        }

        private bool CheckLogin(string username, string password)
        {
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);
            return user != null;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (CheckLogin(username, password))
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
                Application.Current.Properties["CurrentUser"] = user;
                var dashboard = new DashboardWindow();
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
