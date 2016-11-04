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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        DataTable dt;
        private void Form3_Load(object sender, EventArgs e)
        {
            LoadData();
            if (Program.AccessCode == 0)//если учитель,то делаем активными кнопки
            {
                toolStrip1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;

            }
            Client c = new Client("127.0.0.1", 200);//создаем новый экземпляр клиента
            dt = c.GetTable("select * from Предметы");//загружаем в локальную переменную таблицы всю таблицу предметов
            c.CloseConnection();//закрываем соединение

            foreach (DataRow row in dt.Rows)//грузим предметы в комбобокс
                comboBox1.Items.Add(row[1].ToString());
        }
        public void LoadData()//метод загрузки данных в датагрид
        {
            Client c = new Client("127.0.0.1", 200);
            dataGridView1.DataSource = c.GetTable("select * from Группы");//грузим таблицу Группы
            c.CloseConnection();
        }

        public void LoadSubjects()//метод загрузки предметов группы во-второй датагрид
        {
            if (dataGridView1.SelectedRows.Count!=0)//проверяем выбран ли хоть один рядок
            {
                Client c = new Client("127.0.0.1", 200);
                //запрос:выбираем названия предметов из таблицы предметы,где id совпадает с выбранным в датагриде1
                string sql = @"select ""Название"" from Предметы where ""ID"" in (select ""IDПредмета"" from ГруппыПредметы where ""IDГруппы""=" + (int)dataGridView1.SelectedRows[0].Cells[0].Value + ")";
                dataGridView2.DataSource = c.GetTable(sql);//грузим в датагрид2
                c.CloseConnection();
            }
        }

        public void Remove(string id)//метод удаления выбранной в датагриде строки из таблицы в бд
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(@"delete from Группы where ""ID""=" + id);//отправляем запрос на сервер
            c.CloseConnection();
        }
        public void Insert(string text)//метод вставки строки в таблицу
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(String.Format("insert into Группы(Название) values('{0}')",text));
            c.CloseConnection();
        }
      
        
        private void button1_Click(object sender, EventArgs e)
        {
            Insert(textBox1.Text);//добавляем новую группу
            LoadData();//перезагружаем датагрид
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Insert("");//добавляем новую пустую группу,если нажата кнопка добавить на toolstrip
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)//удаляем выбранную группу
        {
            Remove(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            LoadData();
        }

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)//при изменении значения ячейки в таблице
        {
            Client c = new Client("127.0.0.1", 200);
            //запрос upate 
            string query = String.Format(@"update Группы set {0}='{1}' where ""ID""={2}", dataGridView1.Columns[e.ColumnIndex].HeaderText, dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            c.SendQuery(query);
            c.CloseConnection();
            LoadData();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)//метод добавления нового предмета в группу
        {
            int subjid = 0;
            //находим айди выбранного предмета
            foreach(DataRow row in dt.Rows)
            {
                if(row[1].ToString()==comboBox1.SelectedItem.ToString())
                {
                    subjid = (int)row[0];
                    break;
                }
            }
            Client c = new Client("127.0.0.1", 200);
            //отправлем запрос
            c.SendQuery(String.Format(@"insert into ГруппыПредметы(""IDГруппы"",""IDПредмета"") values({0},{1})", (int)dataGridView1.SelectedRows[0].Cells[0].Value, subjid));
            c.CloseConnection();
            //применяем изменения
            LoadSubjects();

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            LoadSubjects();
        }

      
    }
}
