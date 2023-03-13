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
using travelAgency2.Models;

namespace travelAgency2
{
    public partial class ToursOrders : Window
    {
        private int idDirector;

        public int IdDirector
        {
            get { return idDirector; }
            set { idDirector = value; }
        }

        public ToursOrders(int idUser)
        {
            InitializeComponent();
            idDirector = idUser;

            ordersGridBorder.Visibility = Visibility.Collapsed;

            using (var context = new MyContext())
            {
                // отображение списка стран на экране
                ToursGrid.ItemsSource = context.Tours.ToList();
            }
        }

        // нажатие кнопки "Вернуться"
        private void ExitDirectorAccount_Click(object sender, RoutedEventArgs e)
        {
            DirectorAccount directorAccount = new DirectorAccount(idDirector);
            directorAccount.Show();
            Hide();
        }
    }
}
