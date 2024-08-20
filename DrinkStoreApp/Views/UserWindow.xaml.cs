using DrinkStoreApp.Models;
using Microsoft.VisualBasic.ApplicationServices;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Win32;
using DrinkStoreApp.Services;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using static System.Net.WebRequestMethods;

namespace DrinkStoreApp.Views
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : System.Windows.Window
    {
        private readonly ImageUploadService _imageUploadService = new ImageUploadService();
        private readonly SecurityService _securityService = new SecurityService();

        public UserWindow()
        {
            InitializeComponent();
            LoadData();
        }
        DrinkStoreContext context = new DrinkStoreContext();
        List<string> listStatus = new List<string>() {"Inactive", "Active"};
        private void LoadData()
        {
            dgUser.ItemsSource = context.Users.Select(u => new
            {
                UserId = u.UserId,
                DisplayName = u.DisplayName,
                Username = u.Username,
                RoleName = u.Role.RoleName,
                RoleId = u.RoleId,
                Password = u.Password,
                Phone = u.PhoneNumber,
                Email = u.Email,
                Image = u.Image,
                Status = listStatus[u.Status]
            }).ToList();
            cbRole.ItemsSource = context.UserRoles.Select(r => r.RoleName).ToList();
            cbStatus.ItemsSource = listStatus;
            if (cbStatus.SelectedIndex == -1)
            {
                cbStatus.SelectedIndex = 0;
            }
            if (imgProfile.Source == null)
            {
                imgProfile.Source = new BitmapImage(new Uri("/Resources/Images/noimage.jpg", UriKind.Relative));
            }

        }

        private void dgUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRow = dgUser.SelectedItem as dynamic;
            if (selectedRow != null) {
                tbUserID.Text = selectedRow.UserId.ToString();
                tbDisplayName.Text = selectedRow.DisplayName.ToString();
                tbUsername.Text = selectedRow.Username.ToString();
                tbPassword.Password = selectedRow.Password.ToString();
                cbRole.SelectedItem = selectedRow.RoleName.ToString();
                tbPhone.Text = selectedRow.Phone.ToString();
                tbEmail.Text = selectedRow.Email.ToString();
                cbStatus.SelectedIndex = listStatus.IndexOf(selectedRow.Status);
                string imageUrl;
                if (selectedRow.Image == null || string.IsNullOrWhiteSpace(selectedRow.Image.ToString()))
                {
                    imageUrl = "/Resources/Images/noimage.jpg";
                }
                else
                {
                    //Chỉ có thể dùng url, path yêu cầu bảo mật
                    imageUrl = selectedRow.Image.ToString();
                }
                tbURL.Text = imageUrl;
                if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                {
                    imgProfile.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                }
                else
                {
                    imgProfile.Source = new BitmapImage(new Uri(imageUrl, UriKind.Relative));
                }
            }
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    string imageUrl = await _imageUploadService.UploadImageAsync(filePath);
                    MessageBox.Show($"Success", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbURL.Text = imageUrl;
                    imgProfile.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(tbUserID.Text);
            var editUser = context.Users.FirstOrDefault(u => u.UserId == id);
            if (editUser != null) { 
                editUser.Username = tbUsername.Text;
                editUser.DisplayName = tbDisplayName.Text;
                editUser.Password = tbPassword.Password;
                editUser.Email = tbEmail.Text;
                editUser.PhoneNumber = tbPhone.Text;
                editUser.Status = (byte)(cbStatus.Text == "Active" ? 1 : 0);
                if (cbRole.Text != "Admin" || cbRole != null)
                {
                    editUser.RoleId = context.UserRoles.FirstOrDefault(r => r.RoleName == cbRole.Text).RoleId;
                }
                else MessageBox.Show($"Error update user's role", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                editUser.Image = tbURL.Text;

                context.SaveChanges();
                MessageBox.Show($"Success", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show($"This user is not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            dgUser.SelectedItem = null;
            ClearForm();
            LoadData();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var usersToChange = context.Users.Where(u => u.Role.RoleName != "Admin").ToList();

                foreach (var user in usersToChange)
                {
                    user.Password = user.Username;
                }

                context.SaveChanges();
                MessageBox.Show("Passwords have been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                dgUser.SelectedItem = null;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating passwords: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(tbDisplayName.Text) || string.IsNullOrWhiteSpace(tbUsername.Text) ||
                    string.IsNullOrWhiteSpace(tbPassword.Password) || cbRole.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all required fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Check if username already exists
                if (context.Users.Any(u => u.Username == tbUsername.Text))
                {
                    MessageBox.Show("Username already exists. Please choose a different username.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create new user
                var newUser = new Models.User
                {
                    DisplayName = tbDisplayName.Text,
                    Username = tbUsername.Text,
                    Password = tbPassword.Password,
                    PhoneNumber = tbPhone.Text,
                    Email = tbEmail.Text,
                    Status = (byte)(cbStatus.Text == "Active" ? 1 : 0),
                    RoleId = context.UserRoles.FirstOrDefault(r => r.RoleName == cbRole.Text).RoleId,
                    Image = string.IsNullOrWhiteSpace(tbURL.Text) ? "/Resources/Images/noimage.jpg" : tbURL.Text
                };

                context.Users.Add(newUser);
                context.SaveChanges();

                MessageBox.Show("New user has been added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ClearForm()
        {
            tbUserID.Text = string.Empty;
            tbDisplayName.Text = string.Empty;
            tbUsername.Text = string.Empty;
            tbPassword.Password = string.Empty;
            cbRole.SelectedIndex = -1;
            tbPhone.Text = string.Empty;
            tbEmail.Text = string.Empty;
            cbStatus.SelectedIndex = 0;
            tbURL.Text = string.Empty;
            imgProfile.Source = null;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgUser.SelectedItem == null)
            {
                MessageBox.Show("Please select a user to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedUser = dgUser.SelectedItem as dynamic;
            if (selectedUser != null)
            {
                int currentUserRoleId = 0;
                if (_securityService.GetCurrentUser() != null) {
                    currentUserRoleId = _securityService.GetCurrentUser().RoleId;
                }
                int selectedUserRoleId = selectedUser.RoleId;

                // Check if the user is allowed to delete the selected user based on role IDs
                if (currentUserRoleId == 1 && (selectedUserRoleId > 1) ||
                    currentUserRoleId == 2 && (selectedUserRoleId > 2))
                {
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedUser.DisplayName}?",
                                                              "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        int userId = selectedUser.UserId;
                        var userToDelete = context.Users.FirstOrDefault(u => u.UserId == userId);
                        if (userToDelete != null)
                        {
                            context.Users.Remove(userToDelete);
                            context.SaveChanges();
                            MessageBox.Show("User deleted successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You do not have permission to delete this user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
    }
}
