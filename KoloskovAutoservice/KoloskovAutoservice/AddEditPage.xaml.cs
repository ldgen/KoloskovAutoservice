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

namespace KoloskovAutoservice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private service_a_import _currentService = new service_a_import();

        public AddEditPage(service_a_import SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                _currentService = SelectedService;

            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentService.Cost <= 0)
                errors.AppendLine("Укажите стоимость услуги");

            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentService.Discount)) || _currentService.Discount < 0 || _currentService.Discount > 100)
                errors.AppendLine("Укажите скидку");

            if (string.IsNullOrWhiteSpace(Convert.ToString(_currentService.DurationInSeconds)) || _currentService.DurationInSeconds == 0)
                errors.AppendLine("Укажите длительность услуги");

            if (_currentService.DurationInSeconds > 240 || _currentService.DurationInSeconds < 0)
                errors.AppendLine("Длительность не может быть больше 240 минут или меньше 0");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = Koloskov_AutoserviceEntities.GetContext().service_a_import.ToList();
            allServices = allServices.Where(p => p.Title == _currentService.Title).ToList();

            if (allServices.Count == 0 || (_currentService.ID != 0 && allServices.Count <= 1))
            {
                if (_currentService.ID == 0)
                    Koloskov_AutoserviceEntities.GetContext().service_a_import.Add(_currentService);

                try
                {
                    Koloskov_AutoserviceEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }
        }
    }
}
