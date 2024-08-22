using DrinkStoreApp.Models;
using DrinkStoreApp.Services;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
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
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        //private DrinkStoreContext currentUser;
        private readonly SecurityService _securityService = new SecurityService();
        public DashboardWindow()
        {
            InitializeComponent();
            LoadData();
        }
        DrinkStoreContext context = new DrinkStoreContext();
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedItem = (ListBoxItem)((ListBox)sender).SelectedItem;
                var textBlock = ((StackPanel)selectedItem.Content).Children.OfType<TextBlock>().FirstOrDefault();

                if (textBlock != null)
                {
                    string selectedText = textBlock.Text;
                    switch (selectedText)
                    {
                        case "Home":
                            // Navigate to Home Page
                            NavigateToHomePage();
                            break;
                        case "Order":
                            // Navigate to Order Page
                            NavigateToOrderPage();
                            NavigateToHomePage();
                            break;
                        case "Import":
                            // Navigate to Import Page
                            NavigateToImportPage();
                            NavigateToHomePage();
                            break;
                        case "Payment":
                            // Navigate to Payment Page
                            NavigateToPaymentPage();
                            NavigateToHomePage();
                            break;
                        case "Product":
                            // Navigate to Product Page
                            NavigateToProductPage();
                            NavigateToHomePage();
                            break;
                        case "Service":
                            // Navigate to Service Page
                            NavigateToServicePage();
                            NavigateToHomePage();
                            break;
                        case "Recipe":
                            // Navigate to Recipe Page
                            NavigateToRecipePage();
                            NavigateToHomePage();
                            break;
                        case "Ingredient":
                            // Navigate to Ingredient Page
                            NavigateToIngredientPage();
                            NavigateToHomePage();
                            break;
                        case "Unit":
                            // Navigate to Unit Page
                            NavigateToUnitPage();
                            NavigateToHomePage();
                            break;
                        case "Customer":
                            // Navigate to Customer Page
                            NavigateToCustomerPage();
                            NavigateToHomePage();
                            break;
                        case "User":
                            // Navigate to User Page
                            NavigateToUserPage();
                            NavigateToHomePage();
                            break;
                    }
                }
            }
        }

        private void NavigateToHomePage()
        {
            var dashboard = new DashboardWindow();
            dashboard.Show();
            foreach (System.Windows.Window window in Application.Current.Windows)
            {
                if (window != dashboard)
                {
                    window.Close();
                }
            }
        }

        private void NavigateToOrderPage()
        {
            // Logic to navigate to the Order page
        }

        private void NavigateToImportPage()
        {
            // Logic to navigate to the Import page
        }

        private void NavigateToPaymentPage()
        {
            Order order = new Order();
            order.ShowDialog();
        }

        private void NavigateToProductPage()
        {
            ProductManage unitManage = new ProductManage();
            unitManage.ShowDialog();
        }

        private void NavigateToServicePage()
        {
            // Logic to navigate to the Service page
        }

        private void NavigateToRecipePage()
        {
            // Logic to navigate to the Recipe page
        }

        private void NavigateToIngredientPage()
        {
            // Logic to navigate to the Ingredient page
        }

        private void NavigateToUnitPage()
        {
            UnitManage unitManage = new UnitManage();
            unitManage.ShowDialog();
        }

        private void NavigateToCustomerPage()
        {
            CustomerManage customerManage = new CustomerManage();
            customerManage.ShowDialog();
        }

        private void NavigateToUserPage()
        {
            UserWindow userWindow = new UserWindow();
            userWindow.ShowDialog();
        }
        private void LoadData()

        {
            var user = _securityService.GetCurrentUser();
            txtUserName.Text = user.Username;
           txtUserRole.Text = context.UserRoles.FirstOrDefault(c => c.RoleId == user.RoleId).RoleName;
            if (imgAvatar.ImageSource == null)
            {
                imgAvatar.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/meocon.jpg", UriKind.Absolute));

            }


        }


        private void HamburgerButton_Checked(object sender, RoutedEventArgs e)
        {
            drawerHost.IsLeftDrawerOpen = !drawerHost.IsLeftDrawerOpen;
        }
        private void Avatar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AvatarContextMenu.PlacementTarget = (UIElement)sender;
            AvatarContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            AvatarContextMenu.IsOpen = true;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var securityService = new SecurityService();
            var currentUser = context.Users.FirstOrDefault(u => u.Equals(securityService.GetCurrentUser()));
            if (currentUser != null)
            {
                currentUser.Status = 0;
                context.SaveChanges();
            }
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var changePasswordWindow = new ChangePasswordWindow(context);

            
            changePasswordWindow.Owner = this;

            
            changePasswordWindow.ShowDialog();

        }

       
    }
}

