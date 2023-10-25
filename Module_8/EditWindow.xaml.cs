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
    public partial class EditWindow : Window
    {
        private ContactData contactToEdit; // Добавлени поля для хранения контакта
        DataBase dataBase;
        public EditWindow(ContactData contact, DataBase dataBase)
        {
            InitializeComponent();

            contactToEdit = contact;
            this.dataBase = dataBase;
            fullNameTextBox.Text = contactToEdit.fullName;
            phoneTextBox.Text = contactToEdit.numberPhone;
            emailTextBox.Text = contactToEdit.email;
            organizationTextBox.Text = contactToEdit.organization;

        }

        public void InitializeForEdit(ContactData contactToEdit)
        {
            fullNameTextBox.Text = contactToEdit.fullName;
            phoneTextBox.Text = contactToEdit.numberPhone;
            emailTextBox.Text = contactToEdit.email;
            organizationTextBox.Text = contactToEdit.organization;
        }

        private bool contactUpdated;
        private ContactData updatedContact;
        public bool ContactUpdated => contactUpdated;
        public ContactData UpdatedContact => updatedContact;

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
                MessageBox.Show("Пожалуйста, введите номер телефона в правильном формате (+1234567891011)");
                return;
            }

            if (!Regex.IsMatch(email, @"[@.]+")) // проверка номера телефона
            {
                MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты");
                return;
            }

            // Создайте экземпляр ContactData с обновленными данными
            ContactData updatedContact = new ContactData
            {
                fullName = fullName,
                numberPhone = numberPhone,
                email = email,
                organization = organization
            };

            // Отправьте обновленный контакт в базу данных для обновления
            bool updated = dataBase.UpdateContact(contactToEdit, updatedContact);

            if (updated)
            {
                MessageBox.Show("Контакт успешно обновлен в базе данных");
                contactUpdated = true;
                this.updatedContact = updatedContact;
            }
            else
            {
                MessageBox.Show("Произошла ошибка при обновлении контакта в базе данных");
            }
            Close();
        }
    }
}
