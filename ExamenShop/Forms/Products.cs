using MySql.Data.MySqlClient;
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
    public partial class Products : Form
    {
        private string connect = "server=127.0.0.1;database=examen;uid=root;pwd=root";

        public Products()
        {
            InitializeComponent();
            Products_Load();
        }

        public class Product
        {
            public int ProductsID { get; set; }
            public string ProductsName { get; set; }
            public string ProductsCategory { get; set; }
            public decimal ProductsPrice { get; set; }
            public int ProductsCount { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            order.Show();
            Hide();
        }

        private void Products_Load()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connect))
                {
                    connection.Open();

                    string query = @"
                            SELECT
                            p.ProductsID, 
                            p.ProductsName,
                            p.ProductsCategory, 
                            p.ProductsPrice,
                            p.ProductsCount
                        FROM products p";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;

                    dataGridView1.Columns["ProductsID"].HeaderText = "ID";
                    dataGridView1.Columns["ProductsName"].HeaderText = "Наименование";
                    dataGridView1.Columns["ProductsCategory"].HeaderText = "категория";
                    dataGridView1.Columns["ProductsPrice"].HeaderText = "Цена";
                    dataGridView1.Columns["ProductsCount"].HeaderText = "Количество";

                    dataGridView1.Columns["ProductsID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                var product = new Product
                {
                    ProductsID = Convert.ToInt32(selectedRow.Cells["ProductsID"].Value),
                    ProductsName = selectedRow.Cells["ProductsName"].Value.ToString(),
                    ProductsCategory = selectedRow.Cells["ProductsCategory"].Value.ToString(),
                    ProductsPrice = Convert.ToDecimal(selectedRow.Cells["ProductsPrice"].Value),
                    ProductsCount = Convert.ToInt32(selectedRow.Cells["ProductsCount"].Value)
                };

                EditProduct editForm = new EditProduct(product);
                editForm.ShowDialog();
                Products_Load(); // Обновляем данные после редактирования
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования");
            }
        }

    }
}
