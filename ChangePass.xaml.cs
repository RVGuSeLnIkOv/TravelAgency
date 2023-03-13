using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data;
using System.Windows.Media;
using System.Security.Cryptography;
using BCrypt.Net;
using travelAgency2.Models;

namespace travelAgency2
{
    public partial class ChangePass
    {
        public static string GetHash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public ChangePass()
        {
            InitializeComponent();
        }

        private void Button_Change_Pass_Click(object sender, RoutedEventArgs e)
        {
            //получение логина и пароля от пользователей (работников туристического агентства)
            //метод Trim() убирает случайные пробелы до и после текста
            string login = loginTextBox1.Text.Trim();
            string pass = newPassBox.Password.Trim();
            string passRep = newPassRepBox.Password.Trim();

            if (login == "" || pass == "" || passRep == "")
            {
                loginTextBox1.ToolTip = "Данные введены не полностью или вообще не введены!";
                newPassBox.ToolTip = "Данные введены не полностью или вообще не введены!";
                newPassRepBox.ToolTip = "Данные введены не полностью или вообще не введены!";

                loginTextBox1.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                newPassBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                newPassRepBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }
            else
            {
                using (var context = new MyContext())
                {
                    var employeeData = context.EmployeesData.Where(e => e.Login == login).FirstOrDefault();
                  
                    //если в результате выполнения запроса нашелся подходящий сотрудник (логин подошел)
                    if (employeeData != null)
                    {
                        if (pass != passRep)
                        {
                            loginTextBox1.ToolTip = null;
                            newPassBox.ToolTip = "Пароли не совпадают!";
                            newPassRepBox.ToolTip = "Пароли не совпадают!";

                            loginTextBox1.Background = Brushes.Transparent;
                            newPassBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                            newPassRepBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                        }
                        else
                        {
                            if (pass.Length < 9)
                            {
                                loginTextBox1.ToolTip = null;
                                newPassBox.ToolTip = "Пароль не может быть меньше 9 символов!";
                                newPassRepBox.ToolTip = "Пароль не может быть меньше 9 символов!";

                                loginTextBox1.Background = Brushes.Transparent;
                                newPassBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                                newPassRepBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                            }
                            else
                            {
                                string hashPassword = GetHash(pass);

                                employeeData.Password = hashPassword;
                                context.SaveChanges();

                                loginTextBox1.ToolTip = null;
                                newPassBox.ToolTip = null;
                                newPassRepBox.ToolTip = null;

                                loginTextBox1.Background = Brushes.Transparent;
                                newPassBox.Background = Brushes.Transparent;
                                newPassRepBox.Background = Brushes.Transparent;

                                MessageBox.Show($"Пароль успешно изменен!");

                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                Hide();
                            }
                        }
                    }
                    else
                    {
                        loginTextBox1.ToolTip = "Ошибка в логине!";
                        newPassBox.ToolTip = null;
                        newPassRepBox.ToolTip = null;

                        loginTextBox1.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                        newPassBox.Background = Brushes.Transparent;
                        newPassRepBox.Background = Brushes.Transparent;
                    }
                }
            }
        }

        private void Button_Authing_Pass_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
