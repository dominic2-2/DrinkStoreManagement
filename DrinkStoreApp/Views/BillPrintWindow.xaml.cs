using DrinkStoreApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for BillPrintWindow.xaml
    /// </summary>
    public partial class BillPrintWindow : Window
    {
        DrinkStoreContext context = new DrinkStoreContext();
        public BillPrintWindow(string orderId, string? delivery)
        {
            InitializeComponent();
            LoadPage(orderId, delivery);
        }

        private void LoadPage(string orderId, string? delivery)
        {
            int oID = int.Parse(orderId);
            var orderDetails = context.OrderDetails
            .Where(od => od.OrderId == oID)
            .Select(od => new
            {
                ProductName = od.Product.ProductName,
                Quantity = od.Quantity,
                Unit = od.Product.Unit.UnitName,
                Price = od.Product.Price,
                SumPrice = od.TotalPrice
            }).ToList();

            var orderDetailsWithSTT = orderDetails
            .Select((item, index) => new
            {
                STT = index + 1,
                item.ProductName,
                item.Quantity,
                item.Unit,
                item.Price,
                item.SumPrice
            })
            .ToList();

            //Thêm payment
            decimal totalAmount = 0;
            foreach (var item in orderDetails) {
                totalAmount += item.SumPrice;
            }

            var qrcode = "";

            var payment = new Payment
            {
                OrderId = oID,
                PaymentDate = DateTime.Now,
                PaymentType = "QRCode",
                AmountPaid = totalAmount,
                TotalAmount = totalAmount,
                ChangeDue = 0,
                PaymentStatus = 1,
                CouponApplied = 0,
                TransactionId = qrcode
            };
            context.Payments.Add( payment );
            context.SaveChanges();
            lvProduct.ItemsSource = orderDetailsWithSTT;
            var order = context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .Include(o => o.Payment)
                .FirstOrDefault(o => o.OrderId == oID);

            tbTotalAmount.Text = order.Payment.TotalAmount.ToString();
            tbOrderDate.Text = order.DeliveryDate.ToString();
            tbCustomer.Text = order.Customer.FullName;
            tbDelivery.Text = delivery?? "152 - Bắc Từ Liêm - Hà Nội";
            tbTransactionId.Text = order.Payment.TransactionId;
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }
    }
}
