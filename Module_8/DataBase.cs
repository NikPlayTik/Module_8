﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Module_8
{
    public class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data source=DESKTOP-VM99VUJ;Initial Catalog=ContactDataBase;Integrated Security=True");
        MainWindow window;

        public DataBase(MainWindow window)
        {
            this.window = window;
        }

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public bool InsertContact(string fullName, string numberPhone, string email, string organization)
        {
            try
            {
                openConnection();
                string query = "INSERT INTO ContactDataBase (FullName, NumberPhone, Email, Organization) VALUES (@fullName, @numberPhone, @email, @organization)";
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@NumberPhone", numberPhone);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Organization", organization);

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                closeConnection();
            }
        }
        public void LoadInitialData()
        {
            List<ContactData> contacts = new List<ContactData>();
            try
            {
                openConnection();
                string query = "SELECT * FROM ContactDataBase";
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fullName = reader["FullName"].ToString();
                        string numberPhone = reader["NumberPhone"].ToString();
                        string email = reader["Email"].ToString();
                        string organization = reader["Organization"].ToString();

                        ContactData contact = new ContactData
                        {
                            fullName = fullName,
                            numberPhone = numberPhone,
                            email = email,
                            organization = organization
                        };

                        contacts.Add(contact);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке данных: {ex.Message}");
            }
            finally
            {
                closeConnection();
            }
            // Присвоить значения contacts коллекции contactsCollection
            window.contactsCollection = new ObservableCollection<ContactData>(contacts);
            window.contactListView.ItemsSource = window.contactsCollection;
        }

        public bool DeleteContact(ContactData contact)
        {
            try
            {
                openConnection();
                string query = "DELETE FROM ContactDataBase WHERE FullName = @FullName AND NumberPhone = @NumberPhone";
                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@FullName", contact.fullName);
                    command.Parameters.AddWithValue("@NumberPhone", contact.numberPhone);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return true; // Контакт успешно удален
                    }
                    else
                    {
                        return false; // Контакт не найден
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении контакта: {ex.Message}");
                return false;
            }
            finally
            {
                closeConnection();
            }
        }

        public bool UpdateContact(ContactData existingContact, ContactData updatedContact)
        {
            try
            {
                openConnection();
                string query = "UPDATE ContactDataBase SET FullName = @FullName, NumberPhone = @NumberPhone, Email = @Email, Organization = @Organization WHERE FullName = @ExistingFullName AND NumberPhone = @ExistingNumberPhone";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@FullName", updatedContact.fullName);
                    command.Parameters.AddWithValue("@NumberPhone", updatedContact.numberPhone);
                    command.Parameters.AddWithValue("@Email", updatedContact.email);
                    command.Parameters.AddWithValue("@Organization", updatedContact.organization);

                    // Передайте параметры для условия WHERE
                    command.Parameters.AddWithValue("@ExistingFullName", existingContact.fullName);
                    command.Parameters.AddWithValue("@ExistingNumberPhone", existingContact.numberPhone);

                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении контакта: {ex.Message}");
                return false;
            }
            finally
            {
                closeConnection();
            }
        }
    }
}
