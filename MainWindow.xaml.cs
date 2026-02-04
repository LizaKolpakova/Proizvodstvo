using ProdAndLog.Models;
using ProdAndLog.Screens;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProdAndLog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _currentDictionary = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnDictProducts_Click(object sender, RoutedEventArgs e)
        {
            _currentDictionary = "Product";
            BtnAddRecord.Visibility = Visibility.Visible;

            using (var db = new ProdAndLogEntities())
            {
                var products = db.Products.Select(p => new
                {
                    ID = p.Id,
                    Артикул = p.Article,
                    Наименование = p.Name,
                    Категория = p.Category,
                    Описание = p.Description,
                    ЕдИзм = p.Unit,
                    Активен = p.IsActive ? "Да" : "Нет"
                }).ToList();

                DataGridDict.ItemsSource = products;

                DataGridDict.AutoGenerateColumns = false;
                DataGridDict.Columns.Clear();
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Артикул", Binding = new Binding("Артикул") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Наименование", Binding = new Binding("Наименование"), Width = new DataGridLength(200) });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("Категория") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Ед. изм.", Binding = new Binding("ЕдИзм") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Активен", Binding = new Binding("Активен") });
            }
        }

        private void BtnDictMaterials_Click(object sender, RoutedEventArgs e)
        {
            _currentDictionary = "Material";
            BtnAddRecord.Visibility = Visibility.Visible;

            using (var db = new ProdAndLogEntities())
            {
                var materials = from m in db.Materials
                                join mt in db.MaterialTypes on m.TypeId equals mt.Id
                                select new
                                {
                                    ID = m.Id,
                                    Код = m.Code,
                                    Наименование = m.Name,
                                    Тип = mt.Name,
                                    ЕдИзм = m.Unit,
                                    МинОстаток = m.MinStock,
                                    Цена = m.MarketPrice
                                };

                DataGridDict.ItemsSource = materials.ToList();

                DataGridDict.AutoGenerateColumns = false;
                DataGridDict.Columns.Clear();
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Код", Binding = new Binding("Код") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Наименование", Binding = new Binding("Наименование"), Width = new DataGridLength(200) });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Тип", Binding = new Binding("Тип") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Ед. изм.", Binding = new Binding("ЕдИзм") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Мин. остаток", Binding = new Binding("МинОстаток") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Цена", Binding = new Binding("Цена") });
            }
        }

        private void BtnDictSuppliers_Click(object sender, RoutedEventArgs e)
        {
            _currentDictionary = "Supplier";
            BtnAddRecord.Visibility = Visibility.Visible;

            using (var db = new ProdAndLogEntities())
            {
                var suppliers = db.Suppliers.Select(s => new
                {
                    ID = s.Id,
                    ИНН = s.Inn,
                    Наименование = s.Name,
                    Контакты = s.ContactInfo,
                    Рейтинг = s.ReliabilityRating,
                    УсловияОплаты = s.PaymentTerms
                }).ToList();

                DataGridDict.ItemsSource = suppliers;

                DataGridDict.AutoGenerateColumns = false;
                DataGridDict.Columns.Clear();
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "ИНН", Binding = new Binding("ИНН") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Наименование", Binding = new Binding("Наименование"), Width = new DataGridLength(200) });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Контакты", Binding = new Binding("Контакты"), Width = new DataGridLength(150) });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Рейтинг", Binding = new Binding("Рейтинг") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Условия оплаты", Binding = new Binding("УсловияОплаты") });
            }
        }

        private void BtnDictEmployees_Click(object sender, RoutedEventArgs e)
        {
            _currentDictionary = "Employee";
            BtnAddRecord.Visibility = Visibility.Visible;

            using (var db = new ProdAndLogEntities())
            {
                var employees = db.Employees.Select(emp => new
                {
                    ID = emp.Id,
                    ТабельныйНомер = emp.EmployeeNumber,
                    ФИО = emp.FullName,
                    Должность = emp.Position,
                    Отдел = emp.Department,
                    Квалификации = emp.Qualifications,
                    Ставка = emp.RateAmount,
                    ТипСтавки = emp.RateType,
                    Статус = emp.IsActive ? "Работает" : "Уволен"
                }).ToList();

                DataGridDict.ItemsSource = employees;

                DataGridDict.AutoGenerateColumns = false;
                DataGridDict.Columns.Clear();
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Таб. №", Binding = new Binding("ТабельныйНомер") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "ФИО", Binding = new Binding("ФИО"), Width = new DataGridLength(180) });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Должность", Binding = new Binding("Должность") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Отдел", Binding = new Binding("Отдел") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Ставка", Binding = new Binding("Ставка") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Тип", Binding = new Binding("ТипСтавки") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new Binding("Статус") });
            }
        }

        private void BtnDictEquipment_Click(object sender, RoutedEventArgs e)
        {
            _currentDictionary = "Equipment";
            BtnAddRecord.Visibility = Visibility.Visible;

            using (var db = new ProdAndLogEntities())
            {
                var equipment = db.Equipments.Select(eq => new
                {
                    ID = eq.Id,
                    ИнвНомер = eq.InventoryNumber,
                    Модель = eq.Model,
                    Тип = eq.Type,
                    ДатаВвода = eq.CommissionDate,
                    Статус = eq.Status,
                    ГрафикТО = eq.MaintenanceSchedule
                }).ToList();

                DataGridDict.ItemsSource = equipment;

                DataGridDict.AutoGenerateColumns = false;
                DataGridDict.Columns.Clear();
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Инв. №", Binding = new Binding("ИнвНомер") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Модель", Binding = new Binding("Модель"), Width = new DataGridLength(180) });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Тип", Binding = new Binding("Тип") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Дата ввода", Binding = new Binding("ДатаВвода") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new Binding("Статус") });
                DataGridDict.Columns.Add(new DataGridTextColumn { Header = "График ТО", Binding = new Binding("ГрафикТО") });
            }
        }

        private void BtnDictAdd_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentDictionary)
            {
                case "Product":
                    if (new AddProductWindow().ShowDialog() == true)
                        BtnDictProducts_Click(null, null); 
                    break;
                case "Material":
                    if (new AddMaterialWindow().ShowDialog() == true)
                        BtnDictMaterials_Click(null, null);
                    break;
                case "Supplier":
                    if (new AddSupplierWindow().ShowDialog() == true)
                        BtnDictSuppliers_Click(null, null);
                    break;
                case "Employee":
                    if (new AddEmployeeWindow().ShowDialog() == true)
                        BtnDictEmployees_Click(null, null);
                    break;
                case "Equipment":
                    if (new AddEquipmentWindow().ShowDialog() == true)
                        BtnDictEquipment_Click(null, null);
                    break;
            }
        }

        private void BtnPurchasesOrders_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProdAndLogEntities())
            {
                var orders = from po in db.PurchaseOrders
                             join s in db.Suppliers on po.SupplierId equals s.Id
                             join st in db.PurchaseOrderStatus on po.StatusId equals st.Id
                             select new
                             {
                                 Номер = po.Id,
                                 Поставщик = s.Name,
                                 ДатаЗаказа = po.OrderDate,
                                 ОжидаемаяПоставка = po.ExpectedDeliveryDate,
                                 Статус = st.Name
                             };

                DataGridPurchases.ItemsSource = orders.ToList();

                DataGridPurchases.AutoGenerateColumns = false;
                DataGridPurchases.Columns.Clear();
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "№", Binding = new Binding("Номер") });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Поставщик", Binding = new Binding("Поставщик"), Width = new DataGridLength(180) });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Дата заказа", Binding = new Binding("ДатаЗаказа") });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Ожидаемая поставка", Binding = new Binding("ОжидаемаяПоставка") });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new Binding("Статус") });
            }
        }

        private void BtnPurchasesReceipts_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProdAndLogEntities())
            {
                var receipts = from r in db.PurchaseReceipts
                               join m in db.Materials on r.MaterialId equals m.Id
                               join w in db.Warehouses on r.WarehouseId equals w.Id
                               join po in db.PurchaseOrders on r.OrderId equals po.Id
                               select new
                               {
                                   r.ReceiptDate,
                                   MaterialName = m.Name,
                                   r.ReceivedQuantity,
                                   r.DefectQuantity,
                                   WarehouseName = w.Name,
                                   OrderId = po.Id
                               };

                var displayList = receipts.ToList().Select(x => new
                {
                    Дата = x.ReceiptDate,
                    Материал = x.MaterialName,
                    Получено = x.ReceivedQuantity,
                    Брак = x.DefectQuantity,
                    Склад = x.WarehouseName,
                    Заказ = $"№{x.OrderId}"  
                }).ToList();

                DataGridPurchases.ItemsSource = displayList;

                DataGridPurchases.AutoGenerateColumns = false;
                DataGridPurchases.Columns.Clear();
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Дата", Binding = new Binding("Дата") });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Материал", Binding = new Binding("Материал"), Width = new DataGridLength(180) });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Получено", Binding = new Binding("Получено") });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Брак", Binding = new Binding("Брак") });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Склад", Binding = new Binding("Склад") });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Заказ", Binding = new Binding("Заказ") });
            }
        }

        private void BtnStockBalances_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProdAndLogEntities())
            {
                var balances = from sb in db.StockBalances
                               join m in db.Materials on sb.MaterialId equals m.Id
                               join w in db.Warehouses on sb.WarehouseId equals w.Id
                               where sb.Quantity > 0
                               select new
                               {
                                   Материал = m.Name,
                                   Склад = w.Name,
                                   Остаток = sb.Quantity
                               };

                DataGridPurchases.ItemsSource = balances.ToList();

                DataGridPurchases.AutoGenerateColumns = false;
                DataGridPurchases.Columns.Clear();
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Материал", Binding = new Binding("Материал"), Width = new DataGridLength(200) });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Склад", Binding = new Binding("Склад") });
                DataGridPurchases.Columns.Add(new DataGridTextColumn { Header = "Остаток", Binding = new Binding("Остаток") });
            }
        }

        private void BtnCreatePurchaseOrder_Click(object sender, RoutedEventArgs e)
        {
            if (new CreatePurchaseOrderWindow().ShowDialog() == true)
                BtnPurchasesOrders_Click(null, null);
        }

        private void BtnCreateReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (new ReceiveMaterialsWindow().ShowDialog() == true)
                BtnPurchasesReceipts_Click(null, null); 
        }

        private void BtnProdOrders_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProdAndLogEntities())
            {
                var orders = from po in db.ProductionOrders
                             join p in db.Products on po.ProductId equals p.Id
                             select new
                             {
                                 Номер = po.Id,
                                 Клиент = po.CustomerName,
                                 Продукт = p.Name,
                                 Количество = po.Quantity,
                                 Приоритет = po.PriorityLevel,
                                 Дедлайн = po.Deadline,
                                 Статус = po.Status
                             };

                var list = orders.ToList(); 
                DataGridProduction.ItemsSource = list;

                DataGridProduction.AutoGenerateColumns = false;
                DataGridProduction.Columns.Clear();
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "№", Binding = new Binding("Номер") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Клиент", Binding = new Binding("Клиент"), Width = new DataGridLength(150) });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Продукт", Binding = new Binding("Продукт"), Width = new DataGridLength(180) });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Кол-во", Binding = new Binding("Количество") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Приоритет", Binding = new Binding("Приоритет") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Дедлайн", Binding = new Binding("Дедлайн") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new Binding("Статус") });
            }
        }

        private int? _selectedOrderId = null;
        private void BtnProdStages_Click(object sender, RoutedEventArgs e)
        {
            List<dynamic> orderList;

            using (var db = new ProdAndLogEntities())
            {
                var rawOrders = from o in db.ProductionOrders
                                join p in db.Products on o.ProductId equals p.Id
                                select new
                                {
                                    o.Id,
                                    ProductName = p.Name,
                                    CustomerName = o.CustomerName
                                };
                orderList = rawOrders.ToList().Select(x => new
                {
                    Id = x.Id,
                    Info = $"{x.Id}: {x.ProductName} для {x.CustomerName}"
                }).Cast<dynamic>().ToList();
            }

            if (!orderList.Any())
            {
                MessageBox.Show("Нет производственных заказов.");
                return;
            }

            var win = new Window { Title = "Выберите заказ", Width = 400, Height = 300 };
            var lb = new ListBox { Margin = new Thickness(10) };
            foreach (var item in orderList)
                lb.Items.Add(new { Id = item.Id, Info = item.Info });
            lb.DisplayMemberPath = "Info";

            var btnOk = new Button
            {
                Content = "Выбрать",
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10)
            };

            btnOk.Click += (s, ev) =>
            {
                if (lb.SelectedItem != null)
                {
                    dynamic selected = lb.SelectedItem;
                    _selectedOrderId = selected.Id;
                    win.DialogResult = true;
                }
                else
                    MessageBox.Show("Выберите заказ.");
            };

            win.Content = new StackPanel { Children = { lb, btnOk } };
            if (win.ShowDialog() != true || !_selectedOrderId.HasValue)
                return;

            using (var db = new ProdAndLogEntities())
            {
                var rawStages = from ps in db.ProductionStages
                                join op in db.TechOperations on ps.OperationId equals op.Id
                                join emp in db.Employees on ps.AssignedEmployeeId equals emp.Id into empGroup
                                from emp in empGroup.DefaultIfEmpty()
                                join eq in db.Equipments on ps.AssignedEquipmentId equals eq.Id into eqGroup
                                from eq in eqGroup.DefaultIfEmpty()
                                join st in db.ProductionStageStatus on ps.StatusId equals st.Id
                                where ps.OrderId == _selectedOrderId.Value
                                select new
                                {
                                    OperationName = op.OperationName,
                                    StatusName = st.Name,
                                    EmployeeFullName = emp != null ? emp.FullName : "(не назначен)",
                                    EquipmentModel = eq != null ? eq.Model : "(не назначено)",
                                    ps.StartDate,
                                    ps.EndDate,
                                    ps.Id
                                };

                var stageList = rawStages.ToList().Select(x => new
                {
                    Этап = x.OperationName,
                    Статус = x.StatusName,
                    Сотрудник = x.EmployeeFullName,
                    Оборудование = x.EquipmentModel,
                    Начало = x.StartDate,
                    Окончание = x.EndDate,
                    StageId = x.Id
                }).ToList();

                DataGridProduction.ItemsSource = stageList;

                DataGridProduction.AutoGenerateColumns = false;
                DataGridProduction.Columns.Clear();
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Этап", Binding = new Binding("Этап") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new Binding("Статус") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Сотрудник", Binding = new Binding("Сотрудник") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Оборудование", Binding = new Binding("Оборудование") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Начало", Binding = new Binding("Начало") });
                DataGridProduction.Columns.Add(new DataGridTextColumn { Header = "Окончание", Binding = new Binding("Окончание") });
            }
        }

        private void BtnCreateProdOrder_Click(object sender, RoutedEventArgs e)
        {
            if (new CreateProductionOrderWindow().ShowDialog() == true)
                BtnProdOrders_Click(null, null);
        }

        private void DataGridProduction_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataGridProduction.SelectedItem == null) return;

            var item = DataGridProduction.SelectedItem as System.Dynamic.DynamicObject;

            var row = DataGridProduction.SelectedItem;
            var stageIdProp = row.GetType().GetProperty("StageId");
            if (stageIdProp == null) return;

            int stageId = (int)stageIdProp.GetValue(row);
            string stageName = row.GetType().GetProperty("Этап").GetValue(row).ToString();

            var win = new ChangeOrderStatusWindow(stageName, stageId);
            if (win.ShowDialog() == true)
            {
                if (_selectedOrderId.HasValue)
                {
                    BtnProdStages_Click(null, null);
                }
            }
        }
    }
}
