using DrinkStoreApp.Models;
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
        public BillPrintWindow(string orderId, string delivery)
        {
            InitializeComponent();
            LoadPage(orderId, delivery);
        }

        private void LoadPage(string orderId, string delivery)
        {
            var orderDetails = context.OrderDetails
            .Where(od => od.OrderId == int.Parse(orderId))
            .Select(od => new
            {
                ProductName = od.Product.ProductName,
                Quantity = od.Quantity,
                Unit = od.Product.Unit.UnitName,
                Price = od.Product.Price,
                SumPrice = od.Quantity * od.Product.Price
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

            lvProduct.ItemsSource = orderDetailsWithSTT;

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
