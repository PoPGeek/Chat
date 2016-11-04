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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        public void LoadData()//грузим все предметы в датагрид
        {
            Client c = new Client("127.0.0.1", 200);
            dataGridView1.DataSource = c.GetTable("select * from Предметы");
            c.CloseConnection();

        }

        public void Remove(string id)//метод удаления выбранного предмета из таблицы в бд
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(@"delete from Предметы where ""ID""=" + id);
            c.CloseConnection();
        }

        public void Insert(string text)//метод добавления нового предмета в таблицу бд
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(String.Format("insert into Предметы(Название) values('{0}')", text));
            c.CloseConnection();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            LoadData();
            if(Program.AccessCode == 0)
            {
                toolStrip1.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Insert(textBox1.Text);
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Insert("");
            LoadData();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Remove(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            LoadData();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            Client c = new Client("127.0.0.1", 200);

            string query = String.Format(@"update Предметы set {0}='{1}' where ""ID""={2}", dataGridView1.Columns[e.ColumnIndex].HeaderText, dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            c.SendQuery(query);
            c.CloseConnection();
            LoadData();
        }
    }
}
