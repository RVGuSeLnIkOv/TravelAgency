using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using travelAgency2.Models;

namespace travelAgency2
{
    public partial class Clients : Window
    {
        private int idDirector;

        public int IdDirector
        {
            get { return idDirector; }
            set { idDirector = value; }
        }

        bool CheckingForeignPassport(string foreignPassport)
        {
            string patternForeign = @"^\d{2}[\s№]\d{7}$";
            bool isCorrect = Regex.IsMatch(foreignPassport, patternForeign);

            return isCorrect;
        }

        bool CheckingInternalPassport(string internalPassport)
        {
            string patternInternal = @"^\d{4}\s\d{6}$";
            bool isCorrect = Regex.IsMatch(internalPassport, patternInternal);

            return isCorrect;
        }

        bool CheckingPhoneNumber(string phoneNumber)
        {
            string patternPhoneNumber = @"^8\d{10}$";
            bool isCorrect = Regex.IsMatch(phoneNumber, patternPhoneNumber);

            return isCorrect;
        }

        bool CheckingGender(string gender)
        {
            gender = gender.ToLower();
            if (gender == "м" || gender == "ж" || gender == "мужской" || gender == "женский" || gender == "муж" || gender == "жен")
                return true;
            else
                return false;
        }

        string StrConversion(string input)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string output = textInfo.ToLower(input);
            output = textInfo.ToTitleCase(output);

            return output;
        }

        public Clients(int idUser)
        {
            InitializeComponent();
            clientsData.Visibility = Visibility.Collapsed;
            idDirector = idUser;
            using (var context = new MyContext())
            {
                // отображение списка клиентов на экране
                ClientsGrid.ItemsSource = context.Clients.ToList();
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
            string gender = genderComboBox.Text.Trim();
            DateTime? birthday = birthPicker.SelectedDate.GetValueOrDefault();

            using (var context = new MyContext())
            {
                // последовательный поиск по всем непустым полям
                var query = context.Clients.AsQueryable();

                if (!string.IsNullOrEmpty(surname))
                    query = query.Where(c => c.ClientSurname.Contains(surname));

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(c => c.ClientName.Contains(name));

                if (!string.IsNullOrEmpty(patr))
                    query = query.Where(c => c.ClientPatronymic.Contains(patr));

                if (!string.IsNullOrEmpty(gender))
                    query = query.Where(c => c.ClientGender.Contains(gender[0]));

                if (birthday != DateTime.MinValue)
                    query = query.Where(c => DateTime.Parse(c.ClientBirthday.ToString()).Date == birthday);

                List<Client> result = query.ToList();

                if (result.ToArray().Length == 0)
                    MessageBox.Show("Клиенты не найдены!");
                else
                    ClientsGrid.ItemsSource = result;
            }
        }

        // нажатие кнопки "Очистить"
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            surnameTextBox.Clear();
            nameTextBox.Clear();
            patrTextBox.Clear();
            genderComboBox.ClearValue(ComboBox.SelectedItemProperty);
            birthPicker.ClearValue(DatePicker.SelectedDateProperty);
        }

        // нажатие кнопки "Сохранить" в столбце редактирования
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyContext())
            {
                // получение выбранного пользователем клиента
                var selectedClient = ClientsGrid.SelectedItem as Client;
                
                // проверка на то, что выбранная строка клиента не пустая
                if (selectedClient == null)
                {
                    return;
                }

                // получение выбранного пользователем клиента из базы данных
                var clientFromDB = context.Clients.Find(selectedClient.IdClient);

                // считывание новых данных клиента, которые нужно изменить
                string newSurname = selectedClient.ClientSurname.Trim();
                string newName = selectedClient.ClientName.Trim();
                string newPatr = selectedClient.ClientPatronymic.Trim();
                DateTime newBirthday = selectedClient.ClientBirthday;
                string newGender = selectedClient.ClientGender.Trim();
                string newAddress = selectedClient.ClientAddress.Trim();
                string newPhoneNumber = selectedClient.ClientPhoneNumber.Trim();

                // проверка на корректность изменяемых данных
                if (!string.IsNullOrEmpty(newSurname)
                    && !string.IsNullOrEmpty(newName)
                    && !string.IsNullOrEmpty(newGender)
                    && !string.IsNullOrEmpty(newAddress)
                    && !string.IsNullOrEmpty(newPhoneNumber)
                    && CheckingGender(newGender)
                    && newBirthday != DateTime.MinValue
                    && CheckingPhoneNumber(newPhoneNumber))
                {
                    if (StrConversion(newSurname) == clientFromDB.ClientSurname
                        && StrConversion(newName) == clientFromDB.ClientName
                        && StrConversion(newPatr) == clientFromDB.ClientPatronymic
                        && newGender[0].ToString().ToUpper() == clientFromDB.ClientGender
                        && StrConversion(newAddress) == clientFromDB.ClientAddress
                        && newPhoneNumber == clientFromDB.ClientPhoneNumber
                        && DateTime.Parse(newBirthday.ToString("yyyy-MM-dd")) == clientFromDB.ClientBirthday)
                    {
                        MessageBox.Show("Нечего изменять!");
                        return;
                    }

                    clientFromDB.ClientSurname = StrConversion(newSurname);
                    clientFromDB.ClientName = StrConversion(newName);
                    clientFromDB.ClientPatronymic = StrConversion(newPatr);
                    clientFromDB.ClientBirthday = DateTime.Parse(newBirthday.ToString("yyyy-MM-dd"));
                    clientFromDB.ClientGender = newGender[0].ToString().ToUpper();
                    clientFromDB.ClientAddress = StrConversion(newAddress);
                    clientFromDB.ClientPhoneNumber = newPhoneNumber;

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
                        "\n- фамилия, имя, дата рождения, пол, адрес, номер телефона заполняются обязательно" +
                        "\n- номер телефона должен иметь 11 цифр (начинается с 8)");
                }

                ClientsGrid.ItemsSource = context.Clients.ToList();
            }
        }

        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            Client selectedRow = ClientsGrid.SelectedItem as Client;

            if (selectedRow == null)
            {
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить клиента?", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                using (var context = new MyContext())
                {
                    context.Clients.Remove(selectedRow);

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Клиент и его данные успешно удалены!");
                        ClientsGrid.ItemsSource = context.Clients.ToList();
                        clientsData.Visibility = Visibility.Hidden;
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client selectedRow = ClientsGrid.SelectedItem as Client;

            if (selectedRow == null)
            {
                return;
            }

            clientsData.Visibility = Visibility.Visible;

            SNPLabel.Content = selectedRow.ClientSurname + " " + selectedRow.ClientName + " " + selectedRow.ClientPatronymic;
            using (var context = new MyContext())
            {
                var numInternalPassport = context.ClientsData.Where(e => e.IdClient == selectedRow.IdClient).FirstOrDefault().InternalPassportNumber;
                if (numInternalPassport != null)
                    NumInternalTextBox.Text = numInternalPassport;
                else
                    NumInternalTextBox.Text = "отсутствует";

                var numForeignPassport = context.ClientsData.Where(e => e.IdClient == selectedRow.IdClient).FirstOrDefault().ForeignPassportNumber;
                if (numForeignPassport != null)
                    NumForeignTextBox.Text = numForeignPassport;
                else
                    NumForeignTextBox.Text = "отсутствует";
            }
        }

        private void saveDataButton_Click(object sender, RoutedEventArgs e)
        {
            var newInternalNum = NumInternalTextBox.Text.Trim();
            var newForeignNum = NumForeignTextBox.Text.Trim();
            bool isCorrectToEdit = true;

            NumInternalTextBox.ToolTip = null;
            NumForeignTextBox.ToolTip = null;

            NumInternalTextBox.Background = Brushes.Transparent;
            NumForeignTextBox.Background = Brushes.Transparent;

            if (string.IsNullOrEmpty(newInternalNum) || newInternalNum == "отсутствует")
            {
                isCorrectToEdit = false;

                NumInternalTextBox.ToolTip = "Паспорт РФ должен указываться обязательно!";
                NumForeignTextBox.ToolTip = null;

                NumInternalTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
                NumForeignTextBox.Background = Brushes.Transparent;
            }
            else if (!CheckingInternalPassport(newInternalNum))
            {
                isCorrectToEdit = false;

                NumInternalTextBox.ToolTip = "Номер введен неверно! Пример: 1111 111111";
                NumInternalTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (string.IsNullOrEmpty(newForeignNum) || newForeignNum == "отсутствует")
            {
                newForeignNum = null;
            }
            else if (!CheckingForeignPassport(newForeignNum))
            {
                isCorrectToEdit = false;

                NumForeignTextBox.ToolTip = "Номер введен неверно! Пример: 11 1111111 или 11№1111111";
                NumForeignTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            var selectedClient = ClientsGrid.SelectedItem as Client;

            if (isCorrectToEdit && selectedClient != null)
            {
                NumInternalTextBox.ToolTip = null;
                NumForeignTextBox.ToolTip = null;

                NumInternalTextBox.Background = Brushes.Transparent;
                NumForeignTextBox.Background = Brushes.Transparent;

                using (var context = new MyContext())
                {
                    var clientDataFromDB = context.ClientsData.Find(selectedClient.IdClient);

                    if (clientDataFromDB.InternalPassportNumber == newInternalNum
                        && clientDataFromDB.ForeignPassportNumber == newForeignNum)
                    {
                        MessageBox.Show("Нечего изменять!");
                    }
                    else
                    {
                        clientDataFromDB.InternalPassportNumber = newInternalNum;
                        clientDataFromDB.ForeignPassportNumber = newForeignNum;

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
                }
            }
        }

        private void addClientButton_Click(object sender, RoutedEventArgs e)
        {
            addSurnameTextBox.ToolTip = null;
            addNameTextBox.ToolTip = null;
            addPatrTextBox.ToolTip = null;
            addGenderComboBox.ToolTip = null;
            addBirthPicker.ToolTip = null;
            addAddressTextBox.ToolTip = null;
            addPhoneNumberTextBox.ToolTip = null;
            addInternalPassportTextBox.ToolTip = null;
            addForeignPassportTextBox.ToolTip = null;

            addSurnameTextBox.Background = Brushes.Transparent;
            addNameTextBox.Background = Brushes.Transparent;
            addPatrTextBox.Background = Brushes.Transparent;
            addGenderComboBox.Background = Brushes.Transparent;
            addBirthPicker.Background = Brushes.Transparent;
            addAddressTextBox.Background = Brushes.Transparent;
            addPhoneNumberTextBox.Background = Brushes.Transparent;
            addInternalPassportTextBox.Background = Brushes.Transparent;
            addForeignPassportTextBox.Background = Brushes.Transparent;

            // получение данных для добавления
            string surname = addSurnameTextBox.Text.Trim();
            string name = addNameTextBox.Text.Trim();
            string patr = addPatrTextBox.Text.Trim();
            string gender = addGenderComboBox.Text.Trim();
            DateTime birthday = addBirthPicker.SelectedDate.GetValueOrDefault();
            string address = addAddressTextBox.Text.Trim();
            string phoneNumber = addPhoneNumberTextBox.Text.Trim();
            string internalPassportNum = addInternalPassportTextBox.Text.Trim();
            string foreignPassportNum = addForeignPassportTextBox.Text.Trim();

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

            if (string.IsNullOrEmpty(gender))
            {
                isCorrectToAdd = false;

                addGenderComboBox.ToolTip = "Пол должен указываться обязательно!";
                addGenderComboBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (birthday == DateTime.MinValue)
            {
                isCorrectToAdd = false;

                addBirthPicker.ToolTip = "Дата рождения должна указываться обязательно!";
                addBirthPicker.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (string.IsNullOrEmpty(address))
            {
                isCorrectToAdd = false;

                addAddressTextBox.ToolTip = "Город проживания должен указываться обязательно!";
                addAddressTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
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

            if (string.IsNullOrEmpty(internalPassportNum))
            {
                isCorrectToAdd = false;

                addInternalPassportTextBox.ToolTip = "Паспорт РФ должен указываться обязательно!";
                addInternalPassportTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }
            else if (!CheckingInternalPassport(internalPassportNum))
            {
                isCorrectToAdd = false;

                addInternalPassportTextBox.ToolTip = "Паспорт РФ введен неверно! Пример: 1234 567890";
                addInternalPassportTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (string.IsNullOrEmpty(foreignPassportNum))
                foreignPassportNum = null;
            else if (!CheckingForeignPassport(foreignPassportNum))
            {
                isCorrectToAdd = false;

                addForeignPassportTextBox.ToolTip = "Загран. паспорт введен неверно! Пример: 12 3456789";
                addForeignPassportTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (isCorrectToAdd)
            {
                addSurnameTextBox.Clear();
                addNameTextBox.Clear();
                addPatrTextBox.Clear();
                addGenderComboBox.ClearValue(ComboBox.SelectedItemProperty);
                addBirthPicker.ClearValue(DatePicker.SelectedDateProperty);
                addAddressTextBox.Clear();
                addPhoneNumberTextBox.Clear();
                addInternalPassportTextBox.Clear();
                addForeignPassportTextBox.Clear();

                using (var context = new MyContext())
                {
                    var newClient = new Client
                    {
                        ClientSurname = StrConversion(surname),
                        ClientName = StrConversion(name),
                        ClientPatronymic = StrConversion(patr),
                        ClientGender = gender[0].ToString().ToUpper(),
                        ClientBirthday = birthday,
                        ClientAddress = StrConversion(address),
                        ClientPhoneNumber = phoneNumber
                    };

                    context.Clients.Add(newClient);
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Клиент успешно добавлен!");
                        ClientsGrid.ItemsSource = context.Clients.ToList();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                        return;
                    }

                    int id = newClient.IdClient;

                    var newClientData = new ClientData
                    {
                        IdClient = id,
                        InternalPassportNumber = internalPassportNum,
                        ForeignPassportNumber = foreignPassportNum
                    };

                    context.ClientsData.Add(newClientData);
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Данные клиента успешно добавлены!");
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
