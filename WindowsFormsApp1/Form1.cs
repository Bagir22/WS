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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //static string connectionString = "server=c13625814;user=dbadmin;database=yl;password=123456;";
        static string connectionString = "server=localhost;user=root;database=vivod;password=2202;";

        DataTable dt = new DataTable();
        int page = 1;
        int first_item = 0;
        int last_item = 14;
        int max_page;

        private void Form1_Load(object sender, EventArgs e)
        {
            dataTableFill(createQueryString());            

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (filterBox.Items.Contains(dt.Rows[i][1].ToString()) == false)
                {
                    filterBox.Items.Add(dt.Rows[i][1].ToString());
                }
            }
        }
        public string createQueryString()
        {
            string query = "Select * from materials_s_import";
            if (filterBox.SelectedIndex != 0)
                query += $" where (`Тип материала` = '{filterBox.SelectedItem.ToString()}')";
            if (!String.IsNullOrEmpty(textBox1.Text) && filterBox.SelectedIndex != 0)
                query += $" and (`Наименование материала` like concat('%','{textBox1.Text}', '%'))";
            if (!String.IsNullOrEmpty(textBox1.Text) && filterBox.SelectedIndex == 0)
                query += $" where (`Наименование материала` like concat('%','{textBox1.Text}', '%'))";

            query += $" order by `{sortBox.SelectedItem.ToString()}`";
            page = 1;
            return query;
        }

        public void dataTableFill(string query)
        {
            try
            {                
                dt.Clear();
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    conn.Open();
                    adapter.Fill(dt);
                    int pages = dt.Rows.Count / 15;
                    conn.Close();
                }                
                label16.Text = $"Всего записей {dt.Rows.Count}";
                max_page = dt.Rows.Count / 15 + 1;
                vivod();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void vivod()
        {
            try
            {
                label1.Text = $"Страница {page}/{max_page}";
                tableLayoutPanel1.Controls.Clear();            
                for (int i = first_item; i <= last_item; i++)
                {
                    if (i + 2 == dt.Rows.Count)
                        break;
                    Item item = new Item();
                    if (Convert.ToInt32(dt.Rows[i][4]) < Convert.ToInt32(dt.Rows[i][5]))
                        item.BackColor = ColorTranslator.FromHtml("#f19292");
                    else if ((Convert.ToInt32(dt.Rows[i][4])) >= ((Convert.ToInt32(dt.Rows[i][5]))*3))
                        item.BackColor = ColorTranslator.FromHtml("#ffba01");
                    else
                        item.BackColor = Color.White;
                    item.Controls[0].Text = $"{dt.Rows[i][1]} | {dt.Rows[i][0]}\n" +
                        $"Минимальное количество: {dt.Rows[i][5]} шт\n" +
                        $"Поставщики: ";
                    item.Controls[2].Text = $"Остаток: {dt.Rows[i][4]}";
                    item.Controls[1].Text = $"{dt.Rows[i][0]}";
                    try                       
                    {
                        (item.Controls.Find("pictureBox" + 1, true)[0] as PictureBox).ImageLocation = $@"..\..\..\1\{dt.Rows[i][2]}";
                    }
                    catch
                    {
                        (item.Controls.Find("pictureBox" + 1, true)[0] as PictureBox).ImageLocation = $@"..\..\..\1\materials\picture.png";
                    }
                    tableLayoutPanel1.Controls.Add(item);
                }           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void filterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTableFill(createQueryString());
        }

        private void sortBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataTableFill(createQueryString());
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                dataTableFill(createQueryString());
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            if (page != 1)
            {
                --page;
                first_item -= 15;
                last_item -= 15;
                vivod();
            }
            
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            if (page < max_page)
            {
                ++page;
                first_item += 15;
                last_item += 15;
                vivod();
            }            
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            addForm f = new addForm(connectionString);
            if(f.ShowDialog() == DialogResult.OK)
                dataTableFill(createQueryString());        
        }

        private void changeButton_Click(object sender, EventArgs e)
        { 
            List<string> items = new List<string>();
            for (int i = 0; i < 15; i++)
            {
                foreach (object checkBox in this.Controls[0].Controls[i].Controls.Find("checkBox" + 1, true))
                {
                    if ((checkBox as CheckBox).Checked == true)
                    {
                        try
                        {
                            items.Add((this.Controls[0].Controls[i].Controls.Find("label2", true)[0] as Label).Text);                         
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }

            changeForm f = new changeForm(items, connectionString);
            if (f.ShowDialog() == DialogResult.OK)
                dataTableFill(createQueryString());
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 15; i++)
            {
                foreach (object checkBox in this.Controls[0].Controls[i].Controls.Find("checkBox"+1, true))
                {
                    if ((checkBox as CheckBox).Checked == true)
                    {
                        try
                        {
                            string text = (this.Controls[0].Controls[i].Controls.Find("label2", true)[0] as Label).Text;
                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                string query = $"delete from materials_s_import where `Наименование материала` = '{text}'";
                                MySqlCommand command = new MySqlCommand(query, conn);
                                conn.Open();
                                command.ExecuteNonQuery();
                                conn.Close();
                            }                           
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }            
            }
            MessageBox.Show("Данные удалены");           
            dataTableFill(createQueryString());
        }            
    }
}
