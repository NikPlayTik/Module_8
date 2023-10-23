using System;
using System.Collections.Generic;
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

namespace Module_8
{
    public partial class AddWindow : Window
    {
        public event EventHandler<ContactData> ContactSaved;
        public AddWindow()
        {
            InitializeComponent();
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            ContactData newContact = new ContactData
            {
                fullName = fullNameTextBox.Text,
                numberPhone = phoneTextBox.Text,
                email = emailTextBox.Text,
                organization = organizationTextBox.Text
            };

            ContactSaved?.Invoke(this, newContact);

            Close(); // Закрыть окно после сохранения
        }
    }
}
