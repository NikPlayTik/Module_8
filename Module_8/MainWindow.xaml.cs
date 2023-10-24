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
        DataBase dataBase = new DataBase();
        public MainWindow()
        {
            InitializeComponent();
            contactListView.ItemsSource = contactsCollection; // коллекция контактов как источник данных для ListView
            dataBase.LoadInitialData();
        }

        private void AddContact(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
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
            
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
