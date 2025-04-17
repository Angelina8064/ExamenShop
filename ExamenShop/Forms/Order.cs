using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamenShop
{
    public partial class Order : Form
    {
        private string connect = "server=127.0.0.1;database=examen;uid=root;pwd=root";
        public Order()
        {
            InitializeComponent();
            Order_Load();
        }

        public class Orders
        {
            public int OrderID { get; set; }
            public string OrderClient { get; set; }
            public string OrderProduct { get; set; }
            public int OrderCount { get; set; }
            public string OrderStatus { get; set; }
        }

        private void Order_Load()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connect))
                {
                    connection.Open();

                    string query = @"
                            SELECT
                            o.OrderID, 
                            CONCAT(c.ClientsName, ' ', c.ClientsSurname) AS OrderClient, 
                            p.ProductsName AS OrderProduct,
                            o.OrderCount, 
                            o.OrderStatus
                        FROM `order` o
                        LEFT JOIN products p ON o.OrderProduct = p.ProductsID
                        LEFT JOIN clients c ON o.OrderClient = c.ClientsID";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                   
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;

                    dataGridView1.Columns["OrderID"].HeaderText = "ID заказа";
                    dataGridView1.Columns["OrderClient"].HeaderText = "Клиент";
                    dataGridView1.Columns["OrderProduct"].HeaderText = "Товар";
                    dataGridView1.Columns["OrderCount"].HeaderText = "Количество";
                    dataGridView1.Columns["OrderStatus"].HeaderText = "Статус";

                    dataGridView1.Columns["OrderID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAddOrder_Click(object sender, EventArgs e)
        {
            AddOrder form = new AddOrder();
            form.ShowDialog();

            Order_Load();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["OrderID"].Value);

            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить выбранный заказ?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connect))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM `order` WHERE OrderID = @id", connection);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Заказ успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Order_Load(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonProducts_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            products.Show();
            Hide();
        }
    }
}
