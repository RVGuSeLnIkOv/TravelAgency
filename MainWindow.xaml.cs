using System.Linq;
using System.Windows;
using System.Data;
using System.Windows.Media;
using travelAgency2.Models;

namespace travelAgency2
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //описание события при нажатии (мышью или клавишей Enter) на кнопку войти на начальном экране
        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            //получение логина и пароля от пользователей (работников туристического агентства)
            //метод Trim() убирает случайные пробелы до и после текста
            string login = loginTextBox.Text.Trim();
            string pass = passBox.Password.Trim();

            string hashPass = ChangePass.GetHash(pass);

            //обнуление подсказок ячеек ввода логина и пароля (нет подсказок)
            loginTextBox.ToolTip = null;
            passBox.ToolTip = null;

            //отмена окрашивания ячеек ввода логина и пароля
            loginTextBox.Background = Brushes.Transparent;
            passBox.Background = Brushes.Transparent;

            using (var context = new MyContext())
            {
                var employeeData = context.EmployeesData.Where(e => e.Login == login && BCrypt.Net.BCrypt.Verify(pass, e.Password)).FirstOrDefault();

                if (employeeData != null)
                {
                    int idEmployee = employeeData.IdEmployee;
                    var employee = context.Employees.Where(e => e.IdEmployee == idEmployee).FirstOrDefault();

                    if (employee != null && employee.Post != null)
                    {
                        string post = employee.Post;

                        //если в системе авторизован директор
                        if (post == "Директор")
                        {
                            MessageBox.Show("Добро пожаловать, директор!");
                            DirectorAccount directorAccount = new DirectorAccount(idEmployee);
                            directorAccount.Show();
                            Hide();
                        }
                        //если в системе авторизован администратор
                        else if (post == "Администратор")
                        {
                            MessageBox.Show("Добро пожаловать, администратор!");
                        }
                        //непредвиденная ошибка, возникающая в результате неверного ввода должности в таблице сотрудников в БД
                        else { MessageBox.Show("Произошла ошибка. Повторите вход позже или обратитесь к директору"); }
                    }
                    else { MessageBox.Show("Произошла ошибка. Повторите вход позже или обратитесь к директору"); }
                }
                else
                {
                    //дабовление подсказки об ошибке входа в ячейки ввода логина и пароля
                    loginTextBox.ToolTip = "Ошибка в логине или пароле!";
                    passBox.ToolTip = "Ошибка в логине или пароле!";

                    //окрашивание ячеек ввода логина и пароля в красный цвет
                    loginTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                    passBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                }
            }
        }

        private void Button_Changing_Pass_Click(object sender, RoutedEventArgs e)
        {
            ChangePass ChangePass = new ChangePass();
            ChangePass.Show();
            Hide();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}