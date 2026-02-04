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
    /// Логика взаимодействия для AddEquipmentWindow.xaml
    /// </summary>
    public partial class AddEquipmentWindow : Window
    {
        public AddEquipmentWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string invNum = txtInvNumber.Text.Trim();
            string model = txtModel.Text.Trim();

            if (string.IsNullOrEmpty(invNum) || string.IsNullOrEmpty(model))
            {
                MessageBox.Show("Инвентарный номер и модель обязательны.");
                return;
            }

            if (dpCommissionDate.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату ввода в эксплуатацию.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    if (db.Equipments.Any(eq => eq.InventoryNumber == invNum))
                    {
                        MessageBox.Show("Оборудование с таким инвентарным номером уже существует.");
                        return;
                    }

                    db.Equipments.Add(new Equipment
                    {
                        InventoryNumber = invNum,
                        Model = model,
                        Type = txtType.Text.Trim(),
                        CommissionDate = dpCommissionDate.SelectedDate.Value,
                        Status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        MaintenanceSchedule = txtMaintenance.Text.Trim()
                    });

                    db.SaveChanges();
                    MessageBox.Show("Оборудование добавлено.");
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
