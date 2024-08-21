using DrinkStoreApp.Services;
using System.Windows;
using DrinkStoreApp.Models;

namespace DrinkStoreApp.Views
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly DrinkStoreContext context;

        public ChangePasswordWindow()
        {
        }

        public ChangePasswordWindow(DrinkStoreContext context)
        {
            InitializeComponent();
            this.context = context;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
          
                var securityService = new SecurityService();
                var currentUser = securityService.GetCurrentUser();

                if (currentUser == null)
                {
                    MessageBox.Show("No user is currently logged in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Retrieve the input values
                string currentPasswordInput = txtCurrentPassword.Password;
                string newPassword = txtNewPassword.Password;
                string confirmNewPassword = txtConfirmNewPassword.Password;

                // Verify the current password
                if (currentPasswordInput != currentUser.Password)
                {
                    MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Check if the new password and confirmation password match
                if (newPassword != confirmNewPassword)
                {
                    MessageBox.Show("New password and confirmation do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Update the password
                var saveUser = context.Users.FirstOrDefault(u => u.Equals(currentUser));
                if (saveUser != null)
                {
                    saveUser.Password = newPassword;
                    saveUser.Status = 0;
                    context.SaveChanges();

                    MessageBox.Show("Password has been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }



                // Optionally, you might want to close the window or clear the fields
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != loginWindow)
                    {
                        window.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the password: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
