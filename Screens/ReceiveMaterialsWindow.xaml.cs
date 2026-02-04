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
    /// Логика взаимодействия для ReceiveMaterialsWindow.xaml
    /// </summary>
    public partial class ReceiveMaterialsWindow : Window
    {
        public ReceiveMaterialsWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new ProdAndLogEntities())
            {
                var allowedStatuses = new[] { "новый", "в пути", "частично получен" };
                var orders = db.PurchaseOrders
                    .Where(po => allowedStatuses.Contains(po.PurchaseOrderStatu.Name))
                    .ToList();

                cmbOrder.ItemsSource = orders;
                if (cmbOrder.Items.Count > 0)
                    cmbOrder.SelectedIndex = 0;

                cmbMaterial.ItemsSource = db.Materials.ToList();
                if (cmbMaterial.Items.Count > 0)
                    cmbMaterial.SelectedIndex = 0;

                cmbWarehouse.ItemsSource = db.Warehouses.ToList();
                if (cmbWarehouse.Items.Count > 0)
                    cmbWarehouse.SelectedIndex = 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOrder.SelectedItem == null || cmbMaterial.SelectedItem == null || cmbWarehouse.SelectedItem == null)
            {
                MessageBox.Show("Выберите заказ, материал и склад.");
                return;
            }

            if (!decimal.TryParse(txtReceived.Text, out decimal received) || received < 0)
            {
                MessageBox.Show("Получено должно быть неотрицательным числом.");
                return;
            }

            if (!decimal.TryParse(txtDefect.Text, out decimal defect) || defect < 0 || defect > received)
            {
                MessageBox.Show("Брак должен быть от 0 до количества полученного.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    int orderId = ((PurchaseOrder)cmbOrder.SelectedItem).Id;
                    int materialId = ((Material)cmbMaterial.SelectedItem).Id;
                    int warehouseId = ((Warehouse)cmbWarehouse.SelectedItem).Id;

                    db.PurchaseReceipts.Add(new PurchaseReceipt
                    {
                        OrderId = orderId,
                        MaterialId = materialId,
                        ReceivedQuantity = received,
                        DefectQuantity = defect,
                        WarehouseId = warehouseId,
                        ReceiptDate = DateTime.Now
                    });

                    var balance = db.StockBalances
                        .FirstOrDefault(sb => sb.MaterialId == materialId && sb.WarehouseId == warehouseId);

                    if (balance == null)
                    {
                        balance = new StockBalance
                        {
                            MaterialId = materialId,
                            WarehouseId = warehouseId,
                            Quantity = 0
                        };
                        db.StockBalances.Add(balance);
                    }

                    balance.Quantity += (received - defect);

                    db.SaveChanges();
                    MessageBox.Show("Приёмка выполнена. Остатки обновлены.");
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
