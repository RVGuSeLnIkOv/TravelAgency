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

namespace travelAgency2
{
    public partial class DirectorAccount : Window
    {
        private int idDirector;

        public int IdDirector
        {
            get { return idDirector; }
            set { idDirector = value; }
        }

        public DirectorAccount(int idUser)
        {
            InitializeComponent();
            idDirector = idUser;
        }

        private void ExitMainButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }

        private void ButtonClients_Click(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients(idDirector);
            clients.Show();
            Hide();
        }

        private void ButtonEmployees_Click(object sender, RoutedEventArgs e)
        {
            Employees employees = new Employees(idDirector);
            employees.Show();
            Hide();
        }

        private void ButtonCountriesCitiesResidences_Click(object sender, RoutedEventArgs e)
        {
            CountriesCitiesResidences countriesCitiesResidences = new CountriesCitiesResidences(idDirector);
            countriesCitiesResidences.Show();
            Hide();
        }

        private void ButtonToursOrders_Click(object sender, RoutedEventArgs e)
        {
            ToursOrders toursOrders = new ToursOrders(idDirector);
            toursOrders.Show();
            Hide();
        }
    }
}
