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
    public partial class AddProductWindow : Window
    {
        DrinkStoreContext context = new DrinkStoreContext();
        public AddProductWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {

            dgProduct.ItemsSource = context.Products.Select(p => new
            {
                ProductID = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                UnitName = p.Unit.UnitName,
                Quantity = p.Quantity,
            }).ToList();


            var latestOrder = context.Orders.OrderByDescending(o => o.OrderDate).FirstOrDefault();
            dgAddProduct.ItemsSource = context.OrderDetails
                .Where(od => od.OrderId == latestOrder.OrderId)
                .Include(od => od.Product)  
                .ThenInclude(p => p.Unit)   
                .Select(od => new
                {
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quantity,
                    Unit = od.Product.Unit.UnitName
                })
                .ToList();


        }

        private void dgProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductTextBox.Text = string.Empty;
            var products = context.Products
                .Select(p => new
                {
                    ProductID = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    UnitName = p.Unit.UnitName,
                    Quantity = p.Quantity,
                })
                .ToList();

            dgProduct.ItemsSource = products;
            if (dgProduct.SelectedItem != null)
            {
                var selectedProduct = dgProduct.SelectedItem as dynamic;
                if (selectedProduct != null)
                {
                    tbProduct.Text = selectedProduct.ProductName;
                }
            }
        }



        


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var inputProductName = tbProduct.Text.Trim();
            if (string.IsNullOrEmpty(inputProductName))
            {
                MessageBox.Show("Please enter a product name.");
                return;
            }
            var selectedProduct = context.Products
                .FirstOrDefault(p => p.ProductName.ToLower() == inputProductName.ToLower());
            if (selectedProduct == null)
            {
                MessageBox.Show("Product not found.");
                return;
            }
            if (!int.TryParse(tbQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }
            if (quantity > selectedProduct.Quantity)
            {
                MessageBox.Show("Quantity exceeds available stock.");
                return;
            }
            var totalPrice = selectedProduct.Price * quantity;
            var latestOrder = context.Orders.OrderByDescending(o => o.OrderDate).FirstOrDefault();

            if (latestOrder == null)
            {
                MessageBox.Show("No recent order found.");
                return;
            }
            var orderDetail = new OrderDetail
            {
                OrderId = latestOrder.OrderId,
                ProductId = selectedProduct.ProductId,
                Quantity = quantity,
                TotalPrice = totalPrice,
                PromotionApplied = 0
            };
            context.OrderDetails.Add(orderDetail);
            context.SaveChanges();
            selectedProduct.Quantity -= quantity;
            context.SaveChanges();
            dgAddProduct.ItemsSource = context.OrderDetails
                .Where(od => od.OrderId == latestOrder.OrderId)
                .Include(od => od.Product)
                .ThenInclude(p => p.Unit)
                .Select(od => new
                {
                ProductName = od.Product.ProductName,
                Quantity = od.Quantity,
                Unit = od.Product.Unit.UnitName
                })
                .ToList();
            ProductTextBox.Clear();
            tbQuantity.Clear();
            LoadData();
        }





        private void PrintBill_Click(object sender, RoutedEventArgs e)
        {


            // Lấy OrderID của đơn hàng gần đây nhất
            var latestOrder = context.Orders
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();

            BillPrintWindow billPrintWindow = new BillPrintWindow(latestOrder.OrderId.ToString(), null);
            billPrintWindow.ShowDialog();
        }

        
        private void ProductTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var inputProductName = ProductTextBox.Text.Trim();

            // Perform case-insensitive comparison
            var filteredProducts = context.Products
                .Where(p => p.ProductName.Contains(inputProductName))
                .Select(p => new
                {
                    ProductID = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    UnitName = p.Unit.UnitName,
                    Quantity = p.Quantity,
                })
                .ToList();

            // Update DataGrid
            dgProduct.ItemsSource = filteredProducts;

            // If no products are found, handle as needed
            if (!filteredProducts.Any())
            {
                // Optionally clear the text box or show a message
                // MessageBox.Show("No product found.");
            }
        }

        private void dgAddProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}