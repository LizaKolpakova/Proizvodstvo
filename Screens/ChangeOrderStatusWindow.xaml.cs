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
    /// Логика взаимодействия для ChangeOrderStatusWindow.xaml
    /// </summary>
    public partial class ChangeOrderStatusWindow : Window
    {
        public int StageId { get; set; }

        public ChangeOrderStatusWindow(string stageName, int stageId)
        {
            InitializeComponent();
            txtStageName.Text = stageName;
            StageId = stageId;
            LoadStatuses();
        }

        private void LoadStatuses()
        {
            using (var db = new ProdAndLogEntities())
            {
                cmbStatus.ItemsSource = db.ProductionStageStatus.ToList();
                if (cmbStatus.Items.Count > 0)
                    cmbStatus.SelectedIndex = 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус.");
                return;
            }

            try
            {
                using (var db = new ProdAndLogEntities())
                {
                    var stage = db.ProductionStages.FirstOrDefault(s => s.Id == StageId);
                    if (stage != null)
                    {
                        stage.StatusId = ((ProductionStageStatu)cmbStatus.SelectedItem).Id;
                        db.SaveChanges();
                        MessageBox.Show("Статус обновлён.");
                        DialogResult = true;
                    }
                }
                Close();
            }
            catch (System.Exception ex)
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
