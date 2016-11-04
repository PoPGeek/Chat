using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    public partial class Form55 : Form
    {
        public Form55()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            Client c = new Client("127.0.0.1", 200);
            dataGridView1.DataSource = c.GetTable("select * from Пользователи");
            c.CloseConnection();

        }

        public void Remove(string id)
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(@"delete from Пользователи where ""ID""=" + id);
            c.CloseConnection();
        }

        public void Insert(string text, string group,int access)
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(String.Format("insert into Пользователи(Логин,Пароль,Права) values('{0}','{1}',{2})", text, group, access));
            c.CloseConnection();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Insert(textBox1.Text, textBox2.Text, comboBox1.SelectedIndex);
            LoadData();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Insert("", "", 0);
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Remove(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            LoadData();
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
           
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Client c = new Client("127.0.0.1", 200);

            string query = String.Format(@"update Пользователи set {0}='{1}' where ""ID""={2}", dataGridView1.Columns[e.ColumnIndex].HeaderText, dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            c.SendQuery(query);
            c.CloseConnection();
            LoadData();
        }
    }
}
