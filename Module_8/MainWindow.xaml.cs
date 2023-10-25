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
            // Проверьте, выбран ли контакт для редактирования
            if (contactListView.SelectedItem != null)
            {
                // Получите выбранный контакт
                ContactData selectedContact = (ContactData)contactListView.SelectedItem;

                // Откройте окно редактирования и передайте выбранный контакт
                EditWindow editWindow = new EditWindow(selectedContact, dataBase);
                editWindow.ShowDialog();

                // Обновите отображение списка контактов после редактирования
                contactListView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите контакт для редактирования.");
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
