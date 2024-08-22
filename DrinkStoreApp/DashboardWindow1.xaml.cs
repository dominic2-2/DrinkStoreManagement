using DrinkStoreApp.Models;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DrinkStoreApp.Views
{
    public partial class DashboardWindow1 : Window
    {
        DrinkStoreContext context = new DrinkStoreContext();
        public DashboardWindow1()
        {
            InitializeComponent();
            LoadChartData();
        }

        private void LoadChartData()
        {
            using (var context = new DrinkStoreContext())
            {
                // Tạo dữ liệu mẫu doanh thu theo tháng (thay đổi theo yêu cầu)
                var revenueData = context.Orders
                    .Include(o => o.OrderDetails) // Bao gồm OrderDetails để tránh null
                    .AsEnumerable()               // Chuyển sang client-side để xử lý tiếp
                    .GroupBy(o => o.OrderDate.Month)
                    .Select(g => new
                    {
                        Month = g.Key,
                        TotalRevenue = g.Sum(o => o.OrderDetails?.Sum(od => od.TotalPrice) ?? 0) // Tính tổng TotalPrice trong OrderDetails
                    })
                    .OrderBy(r => r.Month)
                    .ToList();



                var monthLabels = revenueData.Select(r => "Month " + r.Month.ToString()).ToArray();
                var revenueValues = new ChartValues<double>(revenueData.Select(r => (double)r.TotalRevenue));

                // Binding dữ liệu vào biểu đồ
                revenueChart.Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Revenue",
                        Values = revenueValues
                    }
                };

                revenueChart.AxisX[0].Labels = monthLabels;
            }
        }
    }
}
