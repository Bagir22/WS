using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace Vivod
{
    public partial class changeForm : Form
    {
        public changeForm(List<string> items, string connectionString)
        {
            InitializeComponent();
            this.items = items;
            this.connectionString = connectionString;
        }
        string connectionString;
        List<string> items;
        DataTable dt = new DataTable();
        private void changeForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"select * from materials_s_import where `Наименование материала`='{items[0]}'";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.Fill(dt);                    
                    conn.Close();
                    textBox1.Text = dt.Rows[0][0].ToString();
                    textBox2.Text = dt.Rows[0][1].ToString();
                    textBox3.Text = dt.Rows[0][2].ToString();
                    textBox4.Text = dt.Rows[0][3].ToString();
                    textBox5.Text = dt.Rows[0][4].ToString();
                    textBox6.Text = dt.Rows[0][5].ToString();
                    textBox7.Text = dt.Rows[0][6].ToString();
                    textBox8.Text = dt.Rows[0][7].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {                    
                    string query = $"update materials_s_import set `Наименование материала`='{textBox1.Text}',`Тип материала`='{textBox2.Text}', " +
                        $"`Изображение`='{textBox3.Text}',`Цена`='{textBox4.Text}', `Количество на складе`='{textBox5.Text}',`Минимальное количество`='{textBox6.Text}', " +
                        $"`Количество в упаковке`='{textBox7.Text}',`Единица измерения`='{textBox8.Text}' " +
                        $"where `Наименование материала`='{items[0]}'";
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные изменены");
                    conn.Close();                    
                }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
