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
    /// Логика взаимодействия для CreatePurchaseOrderWindow.xaml
    /// </summary>
    public partial class CreatePurchaseOrderWindow : Window
    {
        public CreatePurchaseOrderWindow()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            using (var db = new ProdAndLogEntities())
            {
                cmbSupplier.ItemsSource = db.Suppliers.ToList();
                if (cmbSupplier.Items.Count > 0)
                    cmbSupplier.SelectedIndex = 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSupplier.SelectedItem == null)
            {
                MessageBox.Show("Выберите поставщика.");
                return;
            }

            if (dpExpectedDate.SelectedDate == null)
            {
                MessageBox.Show("Укажите ожидаемую дату поставки.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    var statusNew = db.PurchaseOrderStatus.FirstOrDefault(s => s.Name == "новый");
                    if (statusNew == null)
                    {
                        MessageBox.Show("Ошибка: статус 'новый' не найден в справочнике.");
                        return;
                    }

                    db.PurchaseOrders.Add(new PurchaseOrder
                    {
                        SupplierId = ((Supplier)cmbSupplier.SelectedItem).Id,
                        OrderDate = DateTime.Now,
                        ExpectedDeliveryDate = dpExpectedDate.SelectedDate.Value,
                        StatusId = statusNew.Id
                    });

                    db.SaveChanges();
                    MessageBox.Show("Заказ поставщику создан.");
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
