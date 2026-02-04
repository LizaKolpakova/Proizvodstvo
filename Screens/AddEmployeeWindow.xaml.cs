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
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string empNum = txtEmpNumber.Text.Trim();
            string fullName = txtFullName.Text.Trim();

            if (string.IsNullOrEmpty(empNum) || string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Табельный номер и ФИО обязательны.");
                return;
            }

            if (!decimal.TryParse(txtRateAmount.Text, out decimal rate) || rate < 0)
            {
                MessageBox.Show("Ставка должна быть неотрицательным числом.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    if (db.Employees.Any(emp => emp.EmployeeNumber == empNum))
                    {
                        MessageBox.Show("Сотрудник с таким табельным номером уже существует.");
                        return;
                    }

                    db.Employees.Add(new Employee
                    {
                        EmployeeNumber = empNum,
                        FullName = fullName,
                        Position = txtPosition.Text.Trim(),
                        Department = txtDepartment.Text.Trim(),
                        Qualifications = txtQualifications.Text.Trim(),
                        RateType = (cmbRateType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        RateAmount = rate,
                        IsActive = chkActive.IsChecked == true
                    });

                    db.SaveChanges();
                    MessageBox.Show("Сотрудник добавлен.");
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
