using DrinkStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DrinkStoreApp.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DrinkStoreContext context = new DrinkStoreContext();
        public Window1()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {

            CustomerDataGrid.ItemsSource = context.Customers.Select(p => new
            {
                CustomerID = p.CustomerId,
                FullName = p.FullName,
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
            }).ToList();


            var selectUser = context.Users
                             .Where(user => user.Status == 1)
                             .ToList();
            if (selectUser.Any())
            {
                var firstUser = selectUser.FirstOrDefault();
                if (firstUser != null)
                {
                    tbVN.Text = firstUser.Username;
                }
            }
            else
            {
                tbVN.Text = "No user with Status 1";
            }

            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd/MM/yyyy");
            tbDate.Text = formattedDate;


            var latestOrder = context.Orders.OrderByDescending(o => o.OrderDate).FirstOrDefault();
        }

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerDataGrid.SelectedItem != null)
            {
                var selectedCustomer = CustomerDataGrid.SelectedItem as dynamic; 
                if (selectedCustomer != null)
                {
                }
            }


        }



        private void Order_Click(object sender, RoutedEventArgs e)
        {
            var selectUser = context.Users
                 .Where(user => user.Status == 1)
                 .ToList();
            if (selectUser.Any())
            {
                var firstUser = selectUser.FirstOrDefault();
                if (firstUser != null)
                {
                    tbVN.Text = firstUser.Username;
                }
            }
            else
            {
                tbVN.Text = "Guest";
            }
            if (CustomerDataGrid.SelectedItem != null)
            {
                var selectedCustomer = CustomerDataGrid.SelectedItem as dynamic;
                if (selectedCustomer != null)
                {
                    int selectedCustomerId = selectedCustomer.CustomerID;
                    string selectedCustomerName = selectedCustomer.FullName;
                    Order addOrder = new Order
                    {
                        Status = 1,
                        CustomerId = selectedCustomerId,  
                        CreatedBy = 1,
                        OrderDate = DateTime.Now,
                        DeliveryDate = DateTime.Now,
                    };
                    context.Orders.Add(addOrder);
                    context.SaveChanges();

                    MessageBox.Show("Order added successfully.");
                    AddProductWindow addProductWindow = new AddProductWindow();
                    addProductWindow.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Please select a customer.");
            } LoadData();

        }


        private void PrintBill_Click(object sender, RoutedEventArgs e)
        {
            var latestOrder = context.Orders
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();

            BillPrintWindow billPrintWindow = new BillPrintWindow(latestOrder.OrderId.ToString(), null);
            billPrintWindow.ShowDialog();
        }

        private void PhoneNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string phoneNumber = PhoneNumberTextBox.Text;

            var filteredCustomers = context.Customers
                .Where(c => c.PhoneNumber.Contains(phoneNumber))
                .Select(c => new
                {
                    CustomerID = c.CustomerId,
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                })
                .ToList();

            CustomerDataGrid.ItemsSource = filteredCustomers;
        }
        
    }
}