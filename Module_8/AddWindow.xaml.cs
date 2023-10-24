using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Module_8
{
    public partial class AddWindow : Window
    {
        public event EventHandler<ContactData> ContactSaved;
        MainWindow window;
        public AddWindow(MainWindow window)
        {
            this.window = window;
            InitializeComponent();
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            string fullName = fullNameTextBox.Text;
            string numberPhone = phoneTextBox.Text;
            string email = emailTextBox.Text;
            string organization = organizationTextBox.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(numberPhone) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(organization))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
                return;
            }

            if (!Regex.IsMatch(numberPhone, @"^\+\d{12}$")) // проверка номера телефона
            {
                MessageBox.Show("Пожалуйста, введите номер телефона в правильном формате (10 цифр)");
                return;
            }

            if (!Regex.IsMatch(email, @"[@.]+")) // проверка номера телефона
            {
                MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты");
                return;
            }

            DataBase database = new DataBase(window);
            bool inserted = database.InsertContact(fullName, numberPhone, email, organization);

            if (inserted)
            {
                MessageBox.Show("Контакт успешно добавлен в базу данных");
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении контакта в базу данных");
            }

            ContactData newContact = new ContactData
            {
                fullName = fullName,
                numberPhone = numberPhone,
                email = email,
                organization = organization
            };

            // Далее можно добавить проверки для других полей

            MessageBox.Show($"ФИО: {newContact.fullName}\nТелефон: {newContact.numberPhone}\n" +
                $"Электронная почта: {newContact.email}\nОрганизация: {newContact.organization}");

            ContactSaved?.Invoke(this, newContact);

            Close(); // Закрыть окно после сохранения
        }
    }
}
