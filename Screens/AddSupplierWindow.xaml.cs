using ProdAndLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddSupplierWindow.xaml
    /// </summary>
    public partial class AddSupplierWindow : Window
    {
        public AddSupplierWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string inn = txtInn.Text.Trim();
            string name = txtName.Text.Trim();

            if (string.IsNullOrEmpty(inn) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("ИНН и наименование обязательны.");
                return;
            }

            if (!Regex.IsMatch(inn, @"^\d{10}|\d{12}$"))
            {
                MessageBox.Show("ИНН должен содержать 10 или 12 цифр.");
                return;
            }

            if (!int.TryParse(txtRating.Text, out int rating) || rating < 1 || rating > 5)
            {
                MessageBox.Show("Рейтинг должен быть числом от 1 до 5.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    if (db.Suppliers.Any(s => s.Inn == inn))
                    {
                        MessageBox.Show("Поставщик с таким ИНН уже существует.");
                        return;
                    }

                    db.Suppliers.Add(new Supplier
                    {
                        Inn = inn,
                        Name = name,
                        ContactInfo = txtContact.Text.Trim(),
                        ReliabilityRating = rating,
                        PaymentTerms = txtPayment.Text.Trim()
                    });

                    db.SaveChanges();
                    MessageBox.Show("Поставщик добавлен.");
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
