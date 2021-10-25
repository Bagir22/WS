using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Vivod
{
    public partial class addForm : Form
    {
        public addForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        string connectionString;

        private void addForm_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"insert into materials_s_import values('{textBox1.Text}','{textBox2.Text}'," +
                        $"'{textBox3.Text}','{textBox4.Text}','{textBox5.Text}','{textBox6.Text}','{textBox7.Text}','{textBox8.Text}');";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                MessageBox.Show("Данные добавлены");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void addForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
