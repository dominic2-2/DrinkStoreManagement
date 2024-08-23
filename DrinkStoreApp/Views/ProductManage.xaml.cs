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
    /// Interaction logic for ProductManage.xaml
    /// </summary>
    public partial class ProductManage : Window
    {
        DrinkStoreContext context = new DrinkStoreContext();
        public ProductManage()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            TextBoxUnit.ItemsSource = context.Units.ToList();

            if (context.Units.Any())
            {
                TextBoxUnit.SelectedValue = context.Units.First().UnitId;
            }

            var products = context.Products
                                  .Include(p => p.Unit)
                                  .ToList(); 
            DvProduct.ItemsSource = products;
        }


        private Product GetProductById(int productId)
        {
            return context.Products.FirstOrDefault(p => p.ProductId == productId);
        }


        private void DvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DvProduct.SelectedItem is Product product)
            {
                TextBoxProductId.Text = product.ProductId.ToString();
                TextBoxProductName.Text = product.ProductName;
                TextBoxPrice.Text = product.Price.ToString();
                RadioButtonYes.IsChecked = product.Status == 1;
                RadioButtonNo.IsChecked = product.Status == 0;
                TextBoxUnit.SelectedValue = product.Unit?.UnitId;
                TextBlockQuantity.Text = product.Quantity.ToString();
                TextBoxDescription.Text = product.Description;
                string imagePath = product.Image;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    try
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                        bitmap.EndInit();
                        ProductImage.Source = bitmap;
                        TextBlockImagePath.Text = imagePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}");
                        ProductImage.Source = null;
                        TextBlockImagePath.Text = "Error loading image.";
                    }
                }
                else
                {
                    ProductImage.Source = null; // Xóa hình ảnh nếu không có đường dẫn
                    TextBlockImagePath.Text = "No image available.";
                }
            }
        }




        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
                TextBoxProductId.Text = string.Empty;
                TextBoxProductName.Text = string.Empty;
                TextBoxPrice.Text = string.Empty;
                TextBoxDescription.Text = string.Empty;
                TextBlockImagePath.Text = string.Empty;
                RadioButtonYes.IsChecked = false;
                RadioButtonNo.IsChecked = false;
                TextBoxUnit.SelectedIndex = -1; 
                ProductImage.Source = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newProduct = new Product
                {
                    ProductName = TextBoxProductName.Text,
                    Price = decimal.Parse(TextBoxPrice.Text),
                    Status = RadioButtonYes.IsChecked == true ? (byte)1 : (byte)0,
                    Description = TextBoxDescription.Text,
                    Image = TextBlockImagePath.Text,
                    UnitId = GetUnitIdByName(TextBoxUnit.Text),
                    Quantity = decimal.Parse(TextBlockQuantity.Text),
                };
                context.Products.Add(newProduct);
                context.SaveChanges();
                LoadData();
                MessageBox.Show("Product added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product");
            }
        }


        private int GetUnitIdByName(string unitName)
        {
            var unit = context.Units.FirstOrDefault(u => u.UnitName == unitName);
            return unit != null ? unit.UnitId : throw new Exception("Unit not found.");
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(TextBoxProductId.Text, out int productId))
                {
                    var product = context.Products.FirstOrDefault(p => p.ProductId == productId);

                    if (product != null)
                    {
                        product.ProductName = TextBoxProductName.Text;
                        product.Price = decimal.Parse(TextBoxPrice.Text);
                        product.Status = RadioButtonYes.IsChecked == true ? (byte)1 : (byte)0;
                        product.Description = TextBoxDescription.Text;
                        product.Quantity = decimal.Parse(TextBlockQuantity.Text);
                        if (TextBoxUnit.SelectedValue != null)
                        {
                            product.UnitId = (int)TextBoxUnit.SelectedValue;
                        }
                        else
                        {
                            throw new Exception("Invalid Unit ID.");
                        }
                        product.Image = TextBlockImagePath.Text;
                        context.SaveChanges();
                        LoadData(); 
                    }
                    else
                    {
                        MessageBox.Show("Product not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Product ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing product");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(TextBoxProductId.Text, out int productId))
                {
                    var product = context.Products
                                         .Include(p => p.OrderDetails)
                                         .Include(p => p.ProductPromotions)
                                         .Include(p => p.Recipes)
                                         .FirstOrDefault(p => p.ProductId == productId);

                    if (product != null)
                    {
                        // Kiểm tra ràng buộc khóa ngoại
                        bool hasForeignKeyReferences = product.OrderDetails.Any() ||
                                                       product.ProductPromotions.Any() ||
                                                       product.Recipes.Any();

                        if (hasForeignKeyReferences)
                        {
                            MessageBox.Show("Cannot delete this product because it is referenced by other records.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            context.Products.Remove(product);
                            context.SaveChanges();
                            LoadData();
                            MessageBox.Show("Product deleted successfully.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Product not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Product ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}");
            }
        }
    }
}
