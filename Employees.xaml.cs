using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using travelAgency2.Models;
using Unidecode.NET;

namespace travelAgency2
{
    public class PostToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string post = value as string;

            if (post == "Администратор")
            {
                return Visibility.Visible;
            }
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Employees : Window
    {
        private int idDirector;

        public int IdDirector
        {
            get { return idDirector; }
            set { idDirector = value; }
        }

        bool CheckingPhoneNumber(string phoneNumber)
        {
            string patternPhoneNumber = @"^8\d{10}$";
            bool isCorrect = Regex.IsMatch(phoneNumber, patternPhoneNumber);

            return isCorrect;
        }

        string StrConversion(string input)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string output = textInfo.ToLower(input);
            output = textInfo.ToTitleCase(output);

            return output;
        }

        int CheckingSalary(string input)
        {
            try
            {
                input = input.Trim();
                int salary = int.Parse(input);
                if (salary < 0)
                    return -1;
                return salary;
            }
            catch
            {
                return -1;
            }
        }

        string GetLogin(string input)
        {
            var login = input.Unidecode();
            int count = 0;

            using (var context = new MyContext())
            {
                var allEmployeesData = context.EmployeesData.ToList();
                var allLogins = allEmployeesData.Select(e => e.Login).ToList();
                count = allLogins.Count(l => l == login);
            }

            if (count > 0)
                login += $"{count}";

            return login;
        }

        public Employees(int idUser)
        {
            InitializeComponent();
            DataContext = new MyContext();
            idDirector = idUser;
            using (var context = new MyContext())
            {
                // отображение списка клиентов на экране
                EmployeesGrid.ItemsSource = context.Employees.ToList();
            }
        }

        // нажатие кнопки "Вернуться"
        private void ExitDirectorAccount_Click(object sender, RoutedEventArgs e)
        {
            DirectorAccount directorAccount = new DirectorAccount(idDirector);
            directorAccount.Show();
            Hide();
        }

        // нажатие кнопки "Поиск"
        private void findButton_Click(object sender, RoutedEventArgs e)
        {
            // получение данных для поиска
            string surname = surnameTextBox.Text.Trim();
            string name = nameTextBox.Text.Trim();
            string patr = patrTextBox.Text.Trim();
            DateTime? birthday = birthPicker.SelectedDate.GetValueOrDefault();
            string post = postComboBox.Text.Trim();

            using (var context = new MyContext())
            {
                // последовательный поиск по всем непустым полям
                var query = context.Employees.AsQueryable();

                if (!string.IsNullOrEmpty(surname))
                    query = query.Where(c => c.EmployeeSurname.Contains(surname));

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(c => c.EmployeeName.Contains(name));

                if (!string.IsNullOrEmpty(patr))
                    query = query.Where(c => c.EmployeePatronymic.Contains(patr));

                if (birthday != DateTime.MinValue)
                    query = query.Where(c => DateTime.Parse(c.EmployeeBirthday.ToString()).Date == birthday);

                if (!string.IsNullOrEmpty(post))
                    query = query.Where(c => c.Post.Contains(post));

                List<Employee> result = query.ToList();

                if (result.ToArray().Length == 0)
                    MessageBox.Show("Сотрудники не найдены!");
                else
                    EmployeesGrid.ItemsSource = result;
            }
        }

        // нажатие кнопки "Очистить"
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            surnameTextBox.Clear();
            nameTextBox.Clear();
            patrTextBox.Clear();
            birthPicker.ClearValue(DatePicker.SelectedDateProperty);
            postComboBox.ClearValue(ComboBox.SelectedItemProperty);
        }

        // нажатие кнопки "Сохранить" в столбце редактирования
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyContext())
            {
                // получение выбранного пользователем клиента
                var selectedEmployee = EmployeesGrid.SelectedItem as Employee;

                // проверка на то, что выбранная строка клиента не пустая
                if (selectedEmployee == null)
                {
                    return;
                }

                // получение выбранного пользователем клиента из базы данных
                var employeeFromDB = context.Employees.Find(selectedEmployee.IdEmployee);

                // считывание новых данных клиента, которые нужно изменить
                string newSurname = selectedEmployee.EmployeeSurname.Trim();
                string newName = selectedEmployee.EmployeeName.Trim();
                string newPatr = selectedEmployee.EmployeePatronymic.Trim();
                DateTime newBirthday = selectedEmployee.EmployeeBirthday;
                int newSalary = CheckingSalary(selectedEmployee.Salary.ToString());
                string newPhoneNumber = selectedEmployee.EmployeePhoneNumber.Trim();

                // проверка на корректность изменяемых данных
                if (!string.IsNullOrEmpty(newSurname)
                    && !string.IsNullOrEmpty(newName)
                    && !string.IsNullOrEmpty(newPhoneNumber)
                    && newBirthday != DateTime.MinValue
                    && CheckingPhoneNumber(newPhoneNumber)
                    && newSalary != -1)
                {
                    if (StrConversion(newSurname) == employeeFromDB.EmployeeSurname
                        && StrConversion(newName) == employeeFromDB.EmployeeName
                        && StrConversion(newPatr) == employeeFromDB.EmployeePatronymic
                        && employeeFromDB.Salary == newSalary
                        && newPhoneNumber == employeeFromDB.EmployeePhoneNumber
                        && employeeFromDB.EmployeeBirthday == DateTime.Parse(newBirthday.ToString("yyyy-MM-dd")))
                    {
                        MessageBox.Show("Нечего изменять!");
                        return;
                    }

                    employeeFromDB.EmployeeSurname = StrConversion(newSurname);
                    employeeFromDB.EmployeeName = StrConversion(newName);
                    employeeFromDB.EmployeePatronymic = StrConversion(newPatr);
                    employeeFromDB.EmployeeBirthday = DateTime.Parse(newBirthday.ToString("yyyy-MM-dd"));
                    employeeFromDB.Salary = newSalary;
                    employeeFromDB.EmployeePhoneNumber = newPhoneNumber;

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Изменения успешно сохранены!");
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
                else
                {
                    MessageBox.Show("Данные введены некорректно!" +
                        "\n- фамилия, имя, дата рождения, номер телефона, зарплата и должность заполняются обязательно" +
                        "\n- номер телефона должен иметь 11 цифр (начинается с 8)" +
                        "\n- зарплата должна быть целым числом");
                }

                EmployeesGrid.ItemsSource = context.Employees.ToList();
            }
        }

        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            Employee selectedRow = EmployeesGrid.SelectedItem as Employee;

            if (selectedRow == null)
            {
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить сотрудника?", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                using (var context = new MyContext())
                {
                    //var selectedData = context.EmployeesData.Where(d => d.IdEmployeeData == selectedRow.IdEmployeeData);
                    //context.EmployeesData.RemoveRange(selectedData);

                    //context.Employees.Attach(selectedRow);
                    context.Employees.Remove(selectedRow);

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Сотрудник и его данные успешно удалены!");
                        EmployeesGrid.ItemsSource = context.Employees.ToList();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void addEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            addSurnameTextBox.ToolTip = null;
            addNameTextBox.ToolTip = null;
            addPatrTextBox.ToolTip = null;
            addBirthPicker.ToolTip = null;
            addSalaryTextBox.ToolTip = null;
            addPhoneNumberTextBox.ToolTip = null;

            addSurnameTextBox.Background = Brushes.Transparent;
            addNameTextBox.Background = Brushes.Transparent;
            addPatrTextBox.Background = Brushes.Transparent;
            addBirthPicker.Background = Brushes.Transparent;
            addSalaryTextBox.Background = Brushes.Transparent;
            addPhoneNumberTextBox.Background = Brushes.Transparent;

            // получение данных для добавления
            string surname = addSurnameTextBox.Text.Trim();
            string name = addNameTextBox.Text.Trim();
            string patr = addPatrTextBox.Text.Trim();
            DateTime birthday = addBirthPicker.SelectedDate.GetValueOrDefault();
            int salary = CheckingSalary(addSalaryTextBox.Text);
            string phoneNumber = addPhoneNumberTextBox.Text.Trim();

            bool isCorrectToAdd = true;

            if (string.IsNullOrEmpty(surname))
            {
                isCorrectToAdd = false;

                addSurnameTextBox.ToolTip = "Фамилия должна указываться обязательно!";
                addSurnameTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (string.IsNullOrEmpty(name))
            {
                isCorrectToAdd = false;

                addNameTextBox.ToolTip = "Имя должно указываться обязательно!";
                addNameTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (string.IsNullOrEmpty(patr))
            {
                isCorrectToAdd = false;

                addPatrTextBox.ToolTip = "Отчество должно указываться обязательно!";
                addPatrTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (birthday == DateTime.MinValue)
            {
                isCorrectToAdd = false;

                addBirthPicker.ToolTip = "Дата рождения должна указываться обязательно!";
                addBirthPicker.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (salary == -1)
            {
                isCorrectToAdd = false;

                addSalaryTextBox.ToolTip = "Зарплата должна указываться обязательно и быть целым числом!";
                addSalaryTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                isCorrectToAdd = false;

                addPhoneNumberTextBox.ToolTip = "Номер телефона должен указываться обязательно!";
                addPhoneNumberTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }
            else if (!CheckingPhoneNumber(phoneNumber))
            {
                isCorrectToAdd = false;

                addPhoneNumberTextBox.ToolTip = "Номер телефона введен неверно! Пример: 89021234567";
                addPhoneNumberTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (isCorrectToAdd)
            {
                addSurnameTextBox.Clear();
                addNameTextBox.Clear();
                addPatrTextBox.Clear();
                addBirthPicker.ClearValue(DatePicker.SelectedDateProperty);
                addSalaryTextBox.Clear();
                addPhoneNumberTextBox.Clear();

                string fullName = surname + name[0] + patr[0];


                using (var context = new MyContext())
                {
                    var newEmployee = new Employee
                    {
                        EmployeeSurname = StrConversion(surname),
                        EmployeeName = StrConversion(name),
                        EmployeePatronymic = StrConversion(patr),
                        EmployeeBirthday = birthday,
                        Salary = salary,
                        Post = "Администратор",
                        EmployeePhoneNumber = phoneNumber
                    };

                    context.Employees.Add(newEmployee);
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Сотрудник успешно добавлен!");
                        EmployeesGrid.ItemsSource = context.Employees.ToList();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                        return;
                    }

                    int id = newEmployee.IdEmployee;

                    var newEmployeeData = new EmployeeData
                    {
                        IdEmployee = id,
                        Login = GetLogin(fullName),
                        Password = ChangePass.GetHash("1234567890")
                    };

                    context.EmployeesData.Add(newEmployeeData);
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Данные сотрудника успешно сгенерированы!" +
                            $"\nЛогин: {newEmployeeData.Login}" +
                            "\nПароль: 1234567890" +
                            "\nПередайте эти данные сотруднику и попросите сменить пароль перед первым входом в систему");
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }
    }
}
