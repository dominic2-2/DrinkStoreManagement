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
using System.Collections.ObjectModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;
using System.IO;

namespace DrinkStoreApp.Views
{
    public partial class CustomerManage : System.Windows.Window
    {
        public CustomerManage()
        {
            InitializeComponent();
            LoadCustomers();
            cbSearchBy.ItemsSource = listSearch; 
            if (cbSearchBy.Items.Count > 0)
                cbSearchBy.SelectedIndex = 0; 
        }

        private ObservableCollection<dynamic> Customers = new ObservableCollection<dynamic>();
        private DrinkStoreContext context = new DrinkStoreContext();
        private List<string> listSearch = new List<string> { "PhoneNumber", "Email" };

        private void LoadCustomers()
        {
            lstCustomers.ItemsSource = context.Customers.Select(c => new
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address,
                BirthDate = c.BirthDate
            }).ToList();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbFullName.Text) ||
                string.IsNullOrEmpty(tbPhoneNumber.Text) ||
                dpBirthDate.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateOnly birthDate = DateOnly.FromDateTime(dpBirthDate.SelectedDate.Value);

            var newCustomer = new Models.Customer
            {
                FullName = tbFullName.Text,
                PhoneNumber = tbPhoneNumber.Text,
                Email = tbEmail.Text,
                Address = tbAddress.Text,
                BirthDate = birthDate,
            };

            try
            {
                context.Customers.Add(newCustomer);
                context.SaveChanges();
                LoadCustomers();
                MessageBox.Show("Customer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearCustomerFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = string.IsNullOrEmpty(tbCustomerID.Text) ? 0 : int.Parse(tbCustomerID.Text);
                var editCustomer = context.Customers.FirstOrDefault(c => c.CustomerId == id);

                if (editCustomer != null)
                {
                    editCustomer.FullName = tbFullName.Text;
                    editCustomer.PhoneNumber = tbPhoneNumber.Text;
                    editCustomer.Email = tbEmail.Text;
                    editCustomer.Address = tbAddress.Text;

                    if (dpBirthDate.SelectedDate.HasValue)
                    {
                        editCustomer.BirthDate = DateOnly.FromDateTime(dpBirthDate.SelectedDate.Value);
                    }
                    else
                    {
                        MessageBox.Show("Please select a birth date.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    context.Customers.Update(editCustomer);
                    context.SaveChanges();
                    MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCustomers();
                }
                else
                {
                    MessageBox.Show("This customer does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lstCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCustomers.SelectedItem is not null)
            {
                var selectedCustomer = lstCustomers.SelectedItem as dynamic;

                if (selectedCustomer != null)
                {
                    tbCustomerID.Text = selectedCustomer.CustomerId.ToString();
                    tbFullName.Text = selectedCustomer.FullName;
                    tbPhoneNumber.Text = selectedCustomer.PhoneNumber;
                    tbEmail.Text = selectedCustomer.Email;
                    tbAddress.Text = selectedCustomer.Address;
                    dpBirthDate.SelectedDate = selectedCustomer.BirthDate.ToDateTime(new TimeOnly(0, 0));
                }
                else
                {
                    ClearCustomerFields();
                }
            }
            else
            {
                ClearCustomerFields();
            }
        }

        private void ClearCustomerFields()
        {
            tbCustomerID.Clear();
            tbFullName.Clear();
            tbPhoneNumber.Clear();
            tbEmail.Clear();
            tbAddress.Clear();
            dpBirthDate.SelectedDate = null;
        }

        private void btnExportCsv_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save Customer Data As CSV"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;

                try
                {
                    var customers = context.Customers.ToList();
                    var csvContent = new StringBuilder();

                    csvContent.AppendLine("CustomerId,FullName,PhoneNumber,Email,Address,BirthDate");

                    foreach (var customer in customers)
                    {
                        var birthDate = customer.BirthDate;
                        csvContent.AppendLine($"{customer.CustomerId},{customer.FullName},{customer.PhoneNumber},{customer.Email},{customer.Address},{birthDate}");
                    }

                    System.IO.File.WriteAllText(filePath, csvContent.ToString());
                    MessageBox.Show("Customer data exported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

       private void btnSaveToDatabase_Click(object sender, RoutedEventArgs e)
{
    var openFileDialog = new OpenFileDialog
    {
        Filter = "CSV files (*.csv)|*.csv",
        Title = "Select a CSV File"
    };

    if (openFileDialog.ShowDialog() == true)
    {
        var filePath = openFileDialog.FileName;

        try
        {
            var csvLines = System.IO.File.ReadAllLines(filePath);
            var header = csvLines.First().Split(',');
            if (header.Length != 6 ||
                header[0] != "CustomerId" ||
                header[1] != "FullName" ||
                header[2] != "PhoneNumber" ||
                header[3] != "Email" ||
                header[4] != "Address" ||
                header[5] != "BirthDate")
            {
                MessageBox.Show("CSV file has an invalid format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 1; i < csvLines.Length; i++)
                    {
                        var line = csvLines[i];
                        var values = line.Split(',');

                        if (values.Length != 6)
                        {
                            MessageBox.Show($"Invalid data format on line {i + 1}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            continue;
                        }

                        try
                        {
                            var customer = new Customer
                            {
                                CustomerId = int.Parse(values[0]),
                                FullName = values[1],
                                PhoneNumber = values[2],
                                Email = values[3],
                                Address = values[4],
                                BirthDate = DateOnly.Parse(values[5])
                            };

                            var existingCustomer = context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
                            if (existingCustomer != null)
                            {
                                context.Entry(existingCustomer).CurrentValues.SetValues(customer);
                            }
                            else
                            {
                                context.Customers.Add(customer);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log parsing errors
                            Console.WriteLine($"Error parsing line {i + 1}: {ex.Message}");
                        }
                    }

                    context.SaveChanges();
                    transaction.Commit();
                    MessageBox.Show("Customer data saved to database successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while reading the file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}


        ObservableCollection<Customer> lst = new ObservableCollection<Customer>();
        private void btnImportCsv_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Select a CSV File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        string? line;
                        while ((line = sr.ReadLine()) != null)
                        {
                           
                            if (line.StartsWith("CustomerId")) continue;

                            string[] items = line.Split(',');

                            if (items.Length < 6)
                            {
                             
                                continue;
                            }

                            try
                            {
                                Customer cAdd = new Customer
                                {
                                    CustomerId = int.Parse(items[0]),
                                    FullName = items[1],
                                    PhoneNumber = items[2],
                                    Email = items[3],
                                    Address = items[4],
                                    BirthDate = string.IsNullOrWhiteSpace(items[5]) ? (DateOnly?)null : DateOnly.Parse(items[5]),
                                    
                                };

                                lst.Add(cAdd);
                            }
                            catch (Exception ex)
                            {
                             
                                Console.WriteLine($"Error parsing line: {line}. Exception: {ex.Message}");
                            }
                        }
                    }

                 
                    lstCustomers.ItemsSource = lst;
                }
                catch (Exception ex)
                {
                
                    Console.WriteLine($"Error reading file: {ex.Message}");
                }
            }
        }


        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = string.IsNullOrEmpty(tbCustomerID.Text) ? 0 : int.Parse(tbCustomerID.Text);
                var customerToDelete = context.Customers.FirstOrDefault(c => c.CustomerId == id);

                if (customerToDelete != null)
                {
                    context.Customers.Remove(customerToDelete);
                    context.SaveChanges();
                    LoadCustomers();
                    MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearCustomerFields();
                }
                else
                {
                    MessageBox.Show("This customer does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbSearchBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PerformSearch();
        }

        private void txtSearch_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            string searchText = txtSearch.Text.ToLower();
            string searchBy = cbSearchBy.SelectedValue?.ToString();

            if (string.IsNullOrWhiteSpace(searchText) || string.IsNullOrEmpty(searchBy))
            {
                LoadCustomers();
                return;
            }

            var filteredCustomers = context.Customers.AsQueryable();

            if (!listSearch.Contains(searchBy))
            {
                LoadCustomers();
                return;
            }

            switch (searchBy)
            {
                case "PhoneNumber":
                    filteredCustomers = filteredCustomers.Where(c => c.PhoneNumber.ToLower().Contains(searchText));
                    break;
                case "Email":
                    filteredCustomers = filteredCustomers.Where(c => c.Email.ToLower().Contains(searchText));
                    break;
                default:
                    LoadCustomers();
                    return;
            }

            lstCustomers.ItemsSource = filteredCustomers.Select(c => new
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address,
                BirthDate = c.BirthDate
            }).ToList();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearCustomerFields();
            txtSearch.Clear();
            cbSearchBy.SelectedIndex = -1;
            lstCustomers.SelectedItem = null;
            LoadCustomers();
        }
    }
}
