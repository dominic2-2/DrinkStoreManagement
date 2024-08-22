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
            CustomerComboBox.ItemsSource = context.Customers.Select(p => p.FullName).ToList();
            var FirstCustomer = context.Customers.Select(p => p.FullName).ToList()[0];
            CustomerComboBox.SelectedItem = FirstCustomer;

            CustomerDataGrid.ItemsSource = context.Customers.Select(p => new
            {
                CustomerID = p.CustomerId,
                FullName = p.FullName,
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
            }).ToList();

            ProductComboBox.ItemsSource = context.Products.Select(p => p.ProductName).ToList();
            var FirstProduct = context.Products.Select(p => p.ProductName).ToList()[0];
            ProductComboBox.SelectedItem = FirstProduct;

            dgProduct.ItemsSource = context.Products.Select(p => new
            {
                ProductID = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                UnitName = p.Unit.UnitName,
                Quantity = p.Quantity,
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


            var latestOrder = context.Orders
    .OrderByDescending(o => o.OrderDate)
    .FirstOrDefault();
            dgAddProduct.ItemsSource = context.OrderDetails
    .Where(od => od.OrderId == latestOrder.OrderId)
    .ToList();
        }

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerDataGrid.SelectedItem != null)
            {
                var selectedCustomer = CustomerDataGrid.SelectedItem as dynamic; 
                if (selectedCustomer != null)
                {
                    CustomerComboBox.Text = selectedCustomer.FullName;
                }
            }


        }

        private void dgProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProduct.SelectedItem != null)
            {
                var selectedProduct = dgProduct.SelectedItem as dynamic;
                if (selectedProduct != null)
                {
                    ProductComboBox.Text = selectedProduct.ProductName;
                    tbProduct.Text = selectedProduct.ProductName;

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
                        //DeliveryDate = dpDelivery.SelectedDate.HasValue ? dpDelivery.SelectedDate.Value : (DateTime?)null
                    };
                    context.Orders.Add(addOrder);
                    context.SaveChanges();

                    MessageBox.Show("Order added successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a customer.");
            } LoadData();
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var selectedProductName = ProductComboBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedProductName))
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            var selectedProduct = context.Products
                .FirstOrDefault(p => p.ProductName == selectedProductName);

            if (selectedProduct == null)
            {
                MessageBox.Show("Product not found.");
                return;
            }

            if (!int.TryParse(tbQuantity.Text, out int quantity ) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.");

                return;
            }
            int a = int.Parse(tbQuantity.Text);

            if (a > selectedProduct.Quantity)
            {
                MessageBox.Show("Please enter a valid quantity < real");
            }

            // Tính toán tổng giá (giả định rằng sản phẩm có thuộc tính Price)
            var totalPrice = selectedProduct.Price * quantity;

            // Lấy OrderID của đơn hàng gần đây nhất
            var latestOrder = context.Orders
                .OrderByDescending(o => o.OrderDate)

                .FirstOrDefault();

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

            // Thêm OrderDetail vào context và lưu thay đổi
            context.OrderDetails.Add(orderDetail);
            context.SaveChanges();

            // Cập nhật lại DataGrid dgAddProduct
            dgAddProduct.ItemsSource = context.OrderDetails
                .Where(od => od.OrderId == latestOrder.OrderId)
                .ToList();

            // Hiển thị thông báo thành công
            MessageBox.Show("Product added to order successfully.");

            // Reset các trường nhập liệu
            ProductComboBox.SelectedIndex = -1;
            tbQuantity.Clear();
        }


        private void PrintBill_Click(object sender, RoutedEventArgs e)
        {
            // Lấy OrderID của đơn hàng gần đây nhất
            var latestOrder = context.Orders
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();

            if (latestOrder == null)
            {
                MessageBox.Show("No recent order found.");
                return;
            }

            // Lấy tất cả các chi tiết đơn hàng cho OrderID hiện tại
            var orderDetails = context.OrderDetails
                .Where(od => od.OrderId == latestOrder.OrderId)
                .ToList();

            if (orderDetails.Count == 0)
            {
                MessageBox.Show("No items in the order.");
                return;
            }

            // Tính tổng số tiền phải trả
            var totalAmount = orderDetails.Sum(od => od.TotalPrice);

            // Hiển thị thông tin hóa đơn
            var billDetails = new StringBuilder();
            billDetails.AppendLine($"Order ID: {latestOrder.OrderId}");
            billDetails.AppendLine($"Order Date: {latestOrder.OrderDate.ToString("dd/MM/yyyy")}");
            billDetails.AppendLine("Items:");

            foreach (var detail in orderDetails)
            {
                var product = context.Products.FirstOrDefault(p => p.ProductId == detail.ProductId);
                if (product != null)
                {
                    billDetails.AppendLine($"Product: {product.ProductName}, Quantity: {detail.Quantity}, Total Price: {detail.TotalPrice:C}");
                }
            }

            billDetails.AppendLine($"Total Amount: {totalAmount:C}");

            // Hiển thị thông tin hóa đơn trong một MessageBox
            MessageBox.Show(billDetails.ToString(), "Bill Details");

            // Hoặc bạn có thể in hóa đơn, lưu vào file, v.v. tùy theo yêu cầu của bạn.
        }


    }
}