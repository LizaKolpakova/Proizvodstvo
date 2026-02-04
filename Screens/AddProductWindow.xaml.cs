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
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArticle.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Артикул и наименование обязательны.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    if (db.Products.Any(p => p.Article == txtArticle.Text))
                    {
                        MessageBox.Show("Артикул уже существует.");
                        return;
                    }

                    db.Products.Add(new Product
                    {
                        Article = txtArticle.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Category = txtCategory.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        Unit = txtUnit.Text.Trim(),
                        IsActive = chkActive.IsChecked == true
                    });

                    db.SaveChanges();
                    MessageBox.Show("Продукция добавлена.");
                    DialogResult = true;
                    Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
