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
    /// Interaction logic for UnitManage.xaml
    /// </summary>
    public partial class UnitManage : Window
    {
        DrinkStoreContext context = new DrinkStoreContext();
        public UnitManage()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            dgUnit.ItemsSource = context.Units.Select(p => new
            {
                UnitID = p.UnitId,
                UnitName = p.UnitName,
            }).ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Unit newUnit = new Unit
            {
                UnitName = tbUnit.Text
            };
            context.Units.Add(newUnit);
            context.SaveChanges();
            LoadData();
            tbUnit.Clear();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgUnit.SelectedItem != null)
            {
                var selectedUnit = dgUnit.SelectedItem as dynamic;
                int selectedUnitId = selectedUnit.UnitID;
                Unit unitToUpdate = context.Units.FirstOrDefault(u => u.UnitId == selectedUnitId);

                if (unitToUpdate != null)
                {
                    unitToUpdate.UnitName = tbUnit.Text;
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgUnit.SelectedItem != null)
            {
                var selectedUnit = dgUnit.SelectedItem as dynamic;
                int selectedUnitId = selectedUnit.UnitID;

                // Tìm đối tượng Unit trong context với Eager Loading
                Unit unitToDelete = context.Units
                    .Include(u => u.ImportDetails)
                    .Include(u => u.Ingredients)
                    .Include(u => u.Products)
                    .Include(u => u.RecipeDetails)
                    .FirstOrDefault(u => u.UnitId == selectedUnitId);

                if (unitToDelete != null)
                {
                    // Kiểm tra ràng buộc khóa ngoại
                    bool hasForeignKeyReferences = unitToDelete.ImportDetails.Any() ||
                                                   unitToDelete.Ingredients.Any() ||
                                                   unitToDelete.Products.Any() ||
                                                   unitToDelete.RecipeDetails.Any();

                    if (hasForeignKeyReferences)
                    {
                        // Thông báo cho người dùng không thể xóa do có khóa ngoại
                        MessageBox.Show("Cannot delete this unit because it is referenced by other records.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Xóa đối tượng Unit khỏi context và lưu thay đổi
                        context.Units.Remove(unitToDelete);
                        context.SaveChanges();

                        // Tải lại dữ liệu vào DataGrid
                        LoadData();
                    }
                }
            }
        }



        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            tbUnit.Text = "";
            LoadData();
        }

        private void dgUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUnit.SelectedItem != null)
            {
                var selectedUnit = dgUnit.SelectedItem as dynamic;
                if (selectedUnit != null)
                {
                    tbUnit.Text = selectedUnit.UnitName;
                }
            }
        }

    }
}
