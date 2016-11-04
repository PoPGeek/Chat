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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        public void LoadData()
        {
            Client c = new Client("127.0.0.1", 200);
            //грузим в датагрид таблицу студенты
            dataGridView1.DataSource = c.GetTable("select * from Студенты");
            c.CloseConnection();
            
        }

        public void Remove(string id)//метод удаления студента из таблицы в бд
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(@"delete from Студенты where ""ID""=" + id);
            c.CloseConnection();
        }

        public void Insert(string text,string group)//метод добавления нового студента в таблицу
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(String.Format("insert into Студенты(ФИО,Группа) values('{0}','{1}')", text,group));
            c.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Insert(textBox1.Text, comboBox1.SelectedItem.ToString());
            LoadData();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            LoadData();
            if (Program.AccessCode == 0)
            {
                toolStrip1.Enabled = true;
                button1.Enabled = true;
            }
            Client c = new Client("127.0.0.1", 200);
            //выбираем все группы
            DataTable dt = c.GetTable("select * from Группы");
            c.CloseConnection();
            //грузим группы в комбобокс
            foreach (DataRow row in dt.Rows)
                comboBox1.Items.Add(row[1].ToString());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)//добавляем пустую запись в таблицу
        {
            Insert("","");
            LoadData();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)//удаляем выбранную
        {
            Remove(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            LoadData();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)//при редактировании ячейки
        {
            Client c = new Client("127.0.0.1", 200);
            //посылаем на сервер запрос update
            string query = String.Format(@"update Студенты set {0}='{1}' where ""ID""={2}", dataGridView1.Columns[e.ColumnIndex].HeaderText, dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            c.SendQuery(query);
            c.CloseConnection();
            LoadData();
        }
    }
}
