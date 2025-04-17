using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamenShop
{
    public partial class AddOrder : Form
    {
        private string connect = "server=127.0.0.1;database=examen;uid=root;pwd=root";

        public AddOrder()
        {
            InitializeComponent();
        }

        private void AddOrder_Load(object sender, EventArgs e)
        {
            LoadClients();
            LoadProducts();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox3.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connect))
                {
                    connection.Open();

                    string[] clientName = comboBox1.Text.Split(' ');
                    MySqlCommand cmdClient = new MySqlCommand(
                        "SELECT ClientsID FROM clients WHERE ClientsName = @name AND ClientsSurname = @surname", connection);
                    cmdClient.Parameters.AddWithValue("@name", clientName[0]);
                    cmdClient.Parameters.AddWithValue("@surname", clientName[1]);
                    int clientId = Convert.ToInt32(cmdClient.ExecuteScalar());

                    MySqlCommand cmdProduct = new MySqlCommand(
                        "SELECT ProductsID FROM products WHERE ProductsName = @name", connection);
                    cmdProduct.Parameters.AddWithValue("@name", comboBox2.Text);
                    int productId = Convert.ToInt32(cmdProduct.ExecuteScalar());

                    MySqlCommand cmdInsert = new MySqlCommand(
                        "INSERT INTO `order` (OrderClient, OrderProduct, OrderCount, OrderStatus) " +
                        "VALUES (@client, @product, @count, @status)", connection);
                    cmdInsert.Parameters.AddWithValue("@client", clientId);
                    cmdInsert.Parameters.AddWithValue("@product", productId);
                    cmdInsert.Parameters.AddWithValue("@count", Convert.ToInt32(textBox1.Text));
                    cmdInsert.Parameters.AddWithValue("@status", comboBox3.Text);

                    cmdInsert.ExecuteNonQuery();

                    MessageBox.Show("Заказ успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadClients()
        {
            using (MySqlConnection connection = new MySqlConnection(connect))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT ClientsName, ClientsSurname FROM clients", connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string fullName = $"{reader["ClientsName"]} {reader["ClientsSurname"]}";
                    comboBox1.Items.Add(fullName);
                }
                reader.Close();
            }
            comboBox1.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            using (MySqlConnection connection = new MySqlConnection(connect))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT ProductsName FROM products", connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["ProductsName"].ToString());
                }
                reader.Close();
            }
            comboBox2.SelectedIndex = 0;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
