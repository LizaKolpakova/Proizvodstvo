using ProdAndLog.Models;
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

namespace ProdAndLog.Screens
{
    /// <summary>
    /// Логика взаимодействия для CreateProductionOrderWindow.xaml
    /// </summary>
    public partial class CreateProductionOrderWindow : Window
    {
        public CreateProductionOrderWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var db = new ProdAndLogEntities())
            {
                cmbProduct.ItemsSource = db.Products.Where(p => p.IsActive).ToList();
                if (cmbProduct.Items.Count > 0)
                    cmbProduct.SelectedIndex = 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string customer = txtCustomer.Text.Trim();
            if (string.IsNullOrEmpty(customer))
            {
                MessageBox.Show("Укажите клиента.");
                return;
            }

            if (cmbProduct.SelectedItem == null)
            {
                MessageBox.Show("Выберите продукцию.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Количество должно быть целым положительным числом.");
                return;
            }

            if (dpDeadline.SelectedDate == null)
            {
                MessageBox.Show("Укажите дедлайн.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    var order = new ProductionOrder
                    {
                        ProductId = ((Product)cmbProduct.SelectedItem).Id,
                        CustomerName = customer,
                        Quantity = qty,
                        PriorityLevel = (int)sliderPriority.Value,
                        Deadline = dpDeadline.SelectedDate.Value,
                        Status = "запланирован",
                        CreatedAt = DateTime.Now
                    };

                    db.ProductionOrders.Add(order);
                    db.SaveChanges();

                    CreateProductionStages(db, order.Id, order.ProductId);

                    db.SaveChanges();
                    MessageBox.Show("Производственный заказ создан и этапы запланированы.");
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void CreateProductionStages(ProdAndLogEntities db, int orderId, int productId)
        {
            var route = db.TechRoutes.FirstOrDefault(tr => tr.ProductId == productId);
            if (route == null)
            {
                MessageBox.Show($"Внимание: для продукции не найден технологический маршрут.", "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var operations = db.TechOperations.Where(op => op.RouteId == route.Id).OrderBy(op => op.SequenceNumber);
            var statusAwaiting = db.ProductionStageStatus.First(s => s.Name == "ожидает");

            foreach (var op in operations)
            {
                db.ProductionStages.Add(new ProductionStage
                {
                    OrderId = orderId,
                    OperationId = op.Id,
                    StatusId = statusAwaiting.Id
                });
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
