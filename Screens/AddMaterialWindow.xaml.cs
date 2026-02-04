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
    /// Логика взаимодействия для AddMaterialWindow.xaml
    /// </summary>
    public partial class AddMaterialWindow : Window
    {
        public AddMaterialWindow()
        {
            InitializeComponent();
            LoadMaterialTypes();
        }

        private void LoadMaterialTypes()
        {
            using (var db = new ProdAndLogEntities())
            {
                cmbType.ItemsSource = db.MaterialTypes.ToList();
                if (cmbType.Items.Count > 0)
                    cmbType.SelectedIndex = 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Код и наименование обязательны.");
                return;
            }

            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип материала.");
                return;
            }

            if (!int.TryParse(txtMinStock.Text, out int minStock) || minStock < 0)
            {
                MessageBox.Show("Минимальный остаток должен быть целым неотрицательным числом.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Цена должна быть неотрицательным числом.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    if (db.Materials.Any(m => m.Code == txtCode.Text))
                    {
                        MessageBox.Show("Код материала уже существует.");
                        return;
                    }

                    db.Materials.Add(new Material
                    {
                        Code = txtCode.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        TypeId = ((MaterialType)cmbType.SelectedItem).Id,
                        Unit = txtUnit.Text.Trim(),
                        MinStock = minStock,
                        MarketPrice = price
                    });

                    db.SaveChanges();
                    MessageBox.Show("Материал добавлен.");
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
