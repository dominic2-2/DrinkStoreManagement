using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DrinkStoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrinkStoreApp.Views
{
    public partial class PaymentWindow : Window
    {
        private List<Payment> payments;
        private List<User> users;
        private List<Customer> customers;

        
        public PaymentWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            payments = GetPayments();
            users = GetUsers();
            customers = GetCustomers();

            PaymentListView.ItemsSource = payments;

            cbSearchBy.ItemsSource = new List<string>
            {
                "Transaction ID"
              
            };
        }

        private void cbSearchBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string searchBy = cbSearchBy.SelectedItem as string;
            string searchText = txtSearch.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                PaymentListView.ItemsSource = payments;
                return;
            }

            IEnumerable<Payment> filteredPayments = payments;

            switch (searchBy)
            {
                case "Transaction ID":
                    filteredPayments = payments.Where(p => p.TransactionId != null && p.TransactionId.Contains(searchText));
                    break;
                case "Payment Date":
                    if (DateTime.TryParse(searchText, out DateTime paymentDate))
                    {
                        filteredPayments = payments.Where(p => p.PaymentDate.Date == paymentDate.Date);
                    }
                    break;
                case "Amount Paid":
                    if (decimal.TryParse(searchText, out decimal amountPaid))
                    {
                        filteredPayments = payments.Where(p => p.AmountPaid == amountPaid);
                    }
                    break;
                case "Total Amount":
                    if (decimal.TryParse(searchText, out decimal totalAmount))
                    {
                        filteredPayments = payments.Where(p => p.TotalAmount == totalAmount);
                    }
                    break;
            }

            PaymentListView.ItemsSource = filteredPayments;
        }

        private void txtSearch_Key(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                cbSearchBy_SelectionChanged(sender, null);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            cbSearchBy.SelectedIndex = -1;
            PaymentListView.ItemsSource = payments;
        }

        private void OnPaymentSelected(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentListView.SelectedItem is Payment selectedPayment)
            {
                var user = users.FirstOrDefault(u => u.UserId == selectedPayment.CreatedBy);
                var customer = customers.FirstOrDefault(c => c.CustomerId == selectedPayment.CustomerId);

                OrderIdTextBlock.Text = selectedPayment.OrderId.ToString();
                OrderDateTextBlock.Text = selectedPayment.PaymentDate.ToShortDateString();
                ProcessedByTextBlock.Text = user?.DisplayName ?? "N/A";
                CustomerNameTextBlock.Text = customer?.FullName ?? "N/A";
            }
            else
            {
                ClearDetails();
            }
        }

        private void ClearDetails()
        {
            OrderIdTextBlock.Text = string.Empty;
            OrderDateTextBlock.Text = string.Empty;
            ProcessedByTextBlock.Text = "N/A";
            CustomerNameTextBlock.Text = "N/A";
        }

        private List<Payment> GetPayments()
        {
            // Replace with actual data retrieval logic
            return new List<Payment>();
        }

        private List<User> GetUsers()
        {
            // Replace with actual data retrieval logic
            return new List<User>();
        }

        private List<Customer> GetCustomers()
        {
            // Replace with actual data retrieval logic
            return new List<Customer>();
        }
    }
}
