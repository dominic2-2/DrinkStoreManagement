using DrinkStoreApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrinkStoreApp.Services
{
    public class SecurityService
    {
        public Models.User? GetCurrentUser()
        {
            var user = Application.Current.Properties["CurrentUser"] as Models.User;
            if (user == null)
            {
                var login = new LoginWindow();
                login.Show();
                foreach (System.Windows.Window window in Application.Current.Windows)
                {
                    if (window != login)
                    {
                        window.Close();
                    }
                }
            }
            return user;
        }
    }
}
