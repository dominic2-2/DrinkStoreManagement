using DrinkStoreApp.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrinkStoreApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DrinkStoreContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new DrinkStoreContext();
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            bool canConnect = CanConnectToDatabase();
            MessageBox.Show(canConnect ? "Kết nối cơ sở dữ liệu thành công!" : "Không thể kết nối cơ sở dữ liệu!", "Kết quả kiểm tra kết nối", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CanConnectToDatabase()
        {
            try
            {
                return _context.Database.CanConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kết nối cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }

}