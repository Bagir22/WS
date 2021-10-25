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
    public partial class Form2 : Form
    {
        public Form2(string text, string connectionString)
        {
            InitializeComponent();
            this.text = text;
            this.connectionString = connectionString;
        }
        string text, connectionString;

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {         
            try
            {
                int count = Convert.ToInt32(textBox1.Text);
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"update yl.materials_s_import set `Минимальное количество`={count} where `Наименование материала`='{text}'";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Минимальное количество\n материала изменено");
                    conn.Close();
                    DialogResult = DialogResult.OK;
                }
            }
            catch
            {
                MessageBox.Show("Минимальное количество\n материала не изменено");
            }
        }
    }
}
