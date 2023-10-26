using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
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
using MaterialDesignThemes.Wpf;
using System.Windows.Media.Effects;

namespace Module_8
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ContactData> contactsCollection = new ObservableCollection<ContactData>();
        DataBase dataBase;
        public MainWindow()
        {
            InitializeComponent();
            dataBase = new DataBase(this);
            contactListView.ItemsSource = contactsCollection; // коллекция контактов как источник данных для ListView
            dataBase.LoadInitialData();
        }

        private void AddContact(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(this);
            addWindow.ContactSaved += AddWindow_ContactSaved;
            addWindow.ShowDialog();
        }
        private void AddWindow_ContactSaved(object sender, ContactData contact)
        {
            contactsCollection.Add(contact);
            contactListView.Items.Refresh();
        }
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (contactListView.SelectedItem != null)
            {
                ContactData selectedContact = (ContactData)contactListView.SelectedItem;
                EditWindow editWindow = new EditWindow(selectedContact, dataBase);
                editWindow.ShowDialog();

                // Обновление данных в contactsCollection после редактирования
                if (editWindow.ContactUpdated)
                {
                    // Найдите индекс выбранного контакта в contactsCollection
                    int index = contactsCollection.IndexOf(selectedContact);
                    if (index != -1)
                    {
                        // Замените выбранный контакт обновленным контактом
                        contactsCollection[index] = editWindow.UpdatedContact;
                        // Уведомьте ListView об изменениях
                        contactListView.Items.Refresh();
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите контакт для редактирования.");
            }
        }
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (contactListView.SelectedItem != null)
            {
                ContactData selectedContact = (ContactData)contactListView.SelectedItem;

                // Отображение диалогового окна подтверждения удаления
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранный контакт?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Удаление контакта из базы данных
                    bool deleted = dataBase.DeleteContact(selectedContact);

                    if (deleted)
                    {
                        // Удаление контакта из коллекции и обновление отображения
                        contactsCollection.Remove(selectedContact);
                        contactListView.Items.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка при удалении контакта из базы данных.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите контакт для удаления.");
            }
        }
    }
}
