using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class CountriesCitiesResidences : Window
    {
        private int idDirector;

        public int IdDirector
        {
            get { return idDirector; }
            set { idDirector = value; }
        }

        string StrConversion(string input)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string output = textInfo.ToLower(input);
            output = textInfo.ToTitleCase(output);

            return output;
        }

        void ShowResidences()
        {
            City selectedRowCity = CitiesGrid.SelectedItem as City;
            TypeResidence selectedRowTypeResidence = TypeResidenceGrid.SelectedItem as TypeResidence;

            using (var context = new MyContext())
            {
                ResidencesGrid.ItemsSource = context.Residences
                    .Where(r => r.IdCity == selectedRowCity.IdCity
                    && r.IdTypeResidence == selectedRowTypeResidence.IdTypeResidence)
                    .ToList();
            }
        }

        int CheckingStars(string input)
        {
            try
            {
                input = input.Trim();
                int stars = int.Parse(input);
                if (stars < 0 || stars > 5)
                    return -1;
                return stars;
            }
            catch
            {
                return -1;
            }
        }

        public CountriesCitiesResidences(int idUser)
        {
            InitializeComponent();
            idDirector = idUser;

            addCity.Visibility = Visibility.Collapsed;
            citiesGridBorder.Visibility = Visibility.Collapsed;

            addTypeResidence.Visibility = Visibility.Collapsed;
            typeResidenceGridBorder.Visibility = Visibility.Collapsed;

            addResidence.Visibility = Visibility.Collapsed;
            residencesGridBorder.Visibility = Visibility.Collapsed;

            using (var context = new MyContext())
            {
                // отображение списка стран на экране
                CountriesGrid.ItemsSource = context.Countries.ToList();
            }
        }

        // нажатие кнопки "Вернуться"
        private void ExitDirectorAccount_Click(object sender, RoutedEventArgs e)
        {
            DirectorAccount directorAccount = new DirectorAccount(idDirector);
            directorAccount.Show();
            Hide();
        }

        private void editCountryButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyContext())
            {
                var selectedCountry = CountriesGrid.SelectedItem as Country;

                if (selectedCountry == null)
                {
                    return;
                }

                var countryFromDB = context.Countries.Find(selectedCountry.IdCountry);

                string newCountryName = selectedCountry.CountryName.Trim();

                // проверка на корректность изменяемых данных
                if (!string.IsNullOrEmpty(newCountryName))
                {
                    if (StrConversion(newCountryName) == countryFromDB.CountryName)
                    {
                        MessageBox.Show("Нечего изменять!");
                        return;
                    }

                    countryFromDB.CountryName = StrConversion(newCountryName);

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
                        "\n- название страны заполняется обязательно");
                }

                CountriesGrid.ItemsSource = context.Countries.ToList();
            }
        }

        private void delCountryButton_Click(object sender, RoutedEventArgs e)
        {
            Country selectedRow = CountriesGrid.SelectedItem as Country;

            if (selectedRow == null)
            {
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить страну (связанная со страной информация также удалится)?", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                using (var context = new MyContext())
                {
                    context.Countries.Remove(selectedRow);

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Страна успешно удалена!");

                        addCity.Visibility = Visibility.Collapsed;
                        citiesGridBorder.Visibility = Visibility.Collapsed;

                        addTypeResidence.Visibility = Visibility.Collapsed;
                        typeResidenceGridBorder.Visibility = Visibility.Collapsed;

                        addResidence.Visibility = Visibility.Collapsed;
                        residencesGridBorder.Visibility = Visibility.Collapsed;

                        CountriesGrid.ItemsSource = context.Countries.ToList();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void addCountryButton_Click(object sender, RoutedEventArgs e)
        {
            addCountryNameTextBox.ToolTip = null;
            addCountryNameTextBox.Background = Brushes.Transparent;

            // получение данных для добавления
            string countryName = addCountryNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(countryName))
            {
                addCountryNameTextBox.ToolTip = "Название страны должно указываться обязательно!";
                addCountryNameTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }
            else
            {
                addCountryNameTextBox.Clear();

                using (var context = new MyContext())
                {
                    var newCountry = new Country
                    {
                        CountryName = StrConversion(countryName)
                    };

                    context.Countries.Add(newCountry);
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Страна успешно добавлена!");
                        CountriesGrid.ItemsSource = context.Countries.ToList();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void CountriesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Country selectedRow = CountriesGrid.SelectedItem as Country;

            if (selectedRow == null)
            {
                return;
            }

            countryLabel.Content = selectedRow.CountryName;
            using (var context = new MyContext())
            {
                var cities = context.Cities.Where(c => c.IdCountry == selectedRow.IdCountry).ToList();
                CitiesGrid.ItemsSource = cities;
            }

            addCity.Visibility = Visibility.Visible;
            citiesGridBorder.Visibility = Visibility.Visible;
        }

        private void editCityButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyContext())
            {
                var selectedCity = CitiesGrid.SelectedItem as City;

                if (selectedCity == null)
                {
                    return;
                }

                var cityFromDB = context.Cities.Find(selectedCity.IdCity);

                string newCityName = selectedCity.CityName.Trim();

                // проверка на корректность изменяемых данных
                if (!string.IsNullOrEmpty(newCityName))
                {
                    if (StrConversion(newCityName) == cityFromDB.CityName)
                    {
                        MessageBox.Show("Нечего изменять!");
                        return;
                    }

                    cityFromDB.CityName = StrConversion(newCityName);

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
                        "\n- название страны заполняется обязательно");
                }

                CitiesGrid.ItemsSource = context.Cities.Where(c => c.IdCountry == selectedCity.IdCountry).ToList();
            }
        }

        private void delCityButton_Click(object sender, RoutedEventArgs e)
        {
            City selectedRow = CitiesGrid.SelectedItem as City;

            if (selectedRow == null)
            {
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить город (связанная с городом информация также удалится)?", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                using (var context = new MyContext())
                {
                    context.Cities.Remove(selectedRow);

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Город успешно удален!");
                        CitiesGrid.ItemsSource = context.Cities.Where(c => c.IdCountry == selectedRow.IdCountry).ToList();

                        addTypeResidence.Visibility = Visibility.Collapsed;
                        typeResidenceGridBorder.Visibility = Visibility.Collapsed;

                        addResidence.Visibility = Visibility.Collapsed;
                        residencesGridBorder.Visibility = Visibility.Collapsed;
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void addCityButton_Click(object sender, RoutedEventArgs e)
        {
            addCityNameTextBox.ToolTip = null;
            addCityNameTextBox.Background = Brushes.Transparent;

            // получение данных для добавления
            string cityName = addCityNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(cityName))
            {
                addCityNameTextBox.ToolTip = "Название города должно указываться обязательно!";
                addCityNameTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }
            else
            {
                addCityNameTextBox.Clear();

                using (var context = new MyContext())
                {
                    Country selectedRow = CountriesGrid.SelectedItem as Country;

                    var newCity = new City
                    {
                        IdCountry = selectedRow.IdCountry,
                        CityName = StrConversion(cityName)
                    };

                    context.Cities.Add(newCity);
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Город успешно добавлен!");
                        CitiesGrid.ItemsSource = context.Cities.Where(c => c.IdCountry == selectedRow.IdCountry).ToList();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void CitiesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            City selectedRow = CitiesGrid.SelectedItem as City;

            if (selectedRow == null)
            {
                return;
            }

            cityLabel.Content = selectedRow.CityName;
            using (var context = new MyContext())
            {
                TypeResidenceGrid.ItemsSource = context.TypesResidence
                    .Where(tr => tr.TypeResidenceName != "Без Проживания").ToList();
            }

            addTypeResidence.Visibility = Visibility.Visible;
            typeResidenceGridBorder.Visibility = Visibility.Visible;
        }

        private void editTypeResidenceButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyContext())
            {
                var selectedTypeResidence = TypeResidenceGrid.SelectedItem as TypeResidence;

                if (selectedTypeResidence == null)
                {
                    return;
                }

                var typeResidenceFromDB = context.TypesResidence.Find(selectedTypeResidence.IdTypeResidence);

                string newTypeResidenceName = selectedTypeResidence.TypeResidenceName.Trim();

                // проверка на корректность изменяемых данных
                if (!string.IsNullOrEmpty(newTypeResidenceName))
                {
                    if (StrConversion(newTypeResidenceName) == typeResidenceFromDB.TypeResidenceName)
                    {
                        MessageBox.Show("Нечего изменять!");
                        return;
                    }

                    typeResidenceFromDB.TypeResidenceName = StrConversion(newTypeResidenceName);

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
                        "\n- название типа проживания заполняется обязательно");
                }

                TypeResidenceGrid.ItemsSource = context.TypesResidence.ToList();
            }
        }

        private void delTypeResidenceButton_Click(object sender, RoutedEventArgs e)
        {
            TypeResidence selectedRow = TypeResidenceGrid.SelectedItem as TypeResidence;

            if (selectedRow == null)
            {
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить тип проживания (связанная с типом проживания информация также удалится)?", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                using (var context = new MyContext())
                {
                    context.TypesResidence.Remove(selectedRow);

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Тип проживания успешно удален!");

                        addResidence.Visibility = Visibility.Collapsed;
                        residencesGridBorder.Visibility = Visibility.Collapsed;

                        TypeResidenceGrid.ItemsSource = context.TypesResidence.ToList();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void addTypeResidenceButton_Click(object sender, RoutedEventArgs e)
        {
            addTypeResidenceNameTextBox.ToolTip = null;
            addTypeResidenceNameTextBox.Background = Brushes.Transparent;

            // получение данных для добавления
            string typeResidenceName = addTypeResidenceNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(typeResidenceName))
            {
                addTypeResidenceNameTextBox.ToolTip = "Название типа проживания должно указываться обязательно!";
                addTypeResidenceNameTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }
            else
            {
                addTypeResidenceNameTextBox.Clear();

                using (var context = new MyContext())
                {
                    var newTypeResidence = new TypeResidence
                    {
                        TypeResidenceName = StrConversion(typeResidenceName)
                    };

                    context.TypesResidence.Add(newTypeResidence);
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Тип проживания успешно добавлен!");
                        TypeResidenceGrid.ItemsSource = context.TypesResidence.ToList();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void TypeResidenceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            City selectedRowCity = CitiesGrid.SelectedItem as City;
            TypeResidence selectedRowTypeResidence = TypeResidenceGrid.SelectedItem as TypeResidence;

            if (selectedRowCity == null || selectedRowTypeResidence == null)
            {
                return;
            }

            cityLabel.Content = selectedRowCity.CityName;
            typeResidenceLabel.Content = selectedRowTypeResidence.TypeResidenceName;

            ShowResidences();

            addResidence.Visibility = Visibility.Visible;
            residencesGridBorder.Visibility = Visibility.Visible;
        }

        private void editResidenceButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyContext())
            {
                var selectedResidence = ResidencesGrid.SelectedItem as Residence;

                if (selectedResidence == null)
                {
                    return;
                }

                var residenceFromDB = context.Residences.Find(selectedResidence.IdResidence);

                string newResidenceName = selectedResidence.ResidenceName.Trim();
                int newStars = CheckingStars(selectedResidence.Stars.ToString());

                // проверка на корректность изменяемых данных
                if (!string.IsNullOrEmpty(newResidenceName)
                    && newStars != -1)
                {
                    if (StrConversion(newResidenceName) == residenceFromDB.ResidenceName
                        && newStars == residenceFromDB.Stars)
                    {
                        MessageBox.Show("Нечего изменять!");
                        return;
                    }

                    residenceFromDB.ResidenceName = StrConversion(newResidenceName);
                    residenceFromDB.Stars = newStars;

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
                        "\n- название места проживания заполняется обязательно" +
                        "\n- кол-во звезд находится в диапазоне от 0 до 5");
                }

                ShowResidences();
            }
        }

        private void delResidenceButton_Click(object sender, RoutedEventArgs e)
        {
            Residence selectedRow = ResidencesGrid.SelectedItem as Residence;

            if (selectedRow == null)
            {
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить место проживания (связанная с местом проживания информация также удалится)?", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                using (var context = new MyContext())
                {
                    context.Residences.Remove(selectedRow);

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Мксто проживания успешно удалено!");
                        ShowResidences();
                    }
                    catch
                    {
                        MessageBox.Show("При изменении произошла ошибка! Повторите запрос");
                    }
                }
            }
        }

        private void addResidenceButton_Click(object sender, RoutedEventArgs e)
        {
            addResidenceNameTextBox.ToolTip = null;
            addStarsTextBox.ToolTip = null;

            addResidenceNameTextBox.Background = Brushes.Transparent;
            addStarsTextBox.Background = Brushes.Transparent;

            // получение данных для добавления
            string residenceName = addResidenceNameTextBox.Text.Trim();
            int stars = CheckingStars(addStarsTextBox.Text);

            bool isCorrectToAdd = true;

            if (string.IsNullOrEmpty(residenceName))
            {
                isCorrectToAdd = false;

                addTypeResidenceNameTextBox.ToolTip = "Название места проживания должно указываться обязательно!";
                addTypeResidenceNameTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }
            if (stars == -1)
            {
                isCorrectToAdd = false;

                addStarsTextBox.ToolTip = "Кол-во звезд должно находиться в диапазоне от 0 до 5!";
                addStarsTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 92, 118));
            }

            if (isCorrectToAdd)
            {
                addResidenceNameTextBox.Clear();
                addStarsTextBox.Clear();

                using (var context = new MyContext())
                {
                    City selectedRowCity = CitiesGrid.SelectedItem as City;
                    TypeResidence selectedRowTypeResidence = TypeResidenceGrid.SelectedItem as TypeResidence;

                    var newResidence = new Residence
                    {
                        IdCity = selectedRowCity.IdCity,
                        IdTypeResidence = selectedRowTypeResidence.IdTypeResidence,
                        ResidenceName = StrConversion(residenceName),
                        Stars = stars
                    };

                    context.Residences.Add(newResidence);
                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Место проживания успешно добавлено!");
                        ShowResidences();
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
