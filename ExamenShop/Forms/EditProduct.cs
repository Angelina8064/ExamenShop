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
using static ExamenShop.Products;

namespace ExamenShop
{
    public partial class EditProduct : Form
    {
        private string connect = "server=127.0.0.1;database=examen;uid=root;pwd=root";
        private Product selectedProduct;

        public EditProduct(Product product)
        {
            InitializeComponent();

            selectedProduct = product;

            textBoxName.Text = selectedProduct.ProductsName;
            comboBoxCategory.Text = selectedProduct.ProductsCategory;
            textBoxPrice.Text = selectedProduct.ProductsPrice.ToString();
            textBoxCount.Text = selectedProduct.ProductsCount.ToString();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connect))
                {
                    connection.Open();

                    string query = @"UPDATE products SET 
                                ProductsName = @name, 
                                ProductsCategory = @category, 
                                ProductsPrice = @price, 
                                ProductsCount = @count 
                                WHERE ProductsID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                    cmd.Parameters.AddWithValue("@category", comboBoxCategory.Text);
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(textBoxPrice.Text));
                    cmd.Parameters.AddWithValue("@count", int.Parse(textBoxCount.Text));
                    cmd.Parameters.AddWithValue("@id", selectedProduct.ProductsID);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Товар успешно обновлен");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении товара: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
