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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        DataTable dt1;
        DataTable dt2;
        private void LoadGroups()//грузим в комбобокс1 всех студентов
        {
            Client c = new Client("127.0.0.1", 200);
             dt1 = c.GetTable("select * from Студенты");
            c.CloseConnection();

            foreach (DataRow row in dt1.Rows)
                toolStripComboBox1.Items.Add(row[1].ToString());
        }

        private void LoadSubjects()//в комбобокс3 все предметы
        {
            Client c = new Client("127.0.0.1", 200);
            dt2 = c.GetTable("select * from Предметы");
            c.CloseConnection();

            foreach (DataRow row in dt2.Rows)
                toolStripComboBox3.Items.Add(row[1].ToString());
        }

        private void LoadData()
        {
            int studentID = 0, subjectID = 0;
            //ищем ид выбранного студента
            foreach(DataRow row in dt1.Rows)
            {
                if(row[1].ToString() == toolStripComboBox1.SelectedItem.ToString())
                {
                    studentID = (int)row[0];
                    break;
                }
            }
            //ищем ид выбранного предмета
            foreach (DataRow row in dt2.Rows)
            {
                if (row[1].ToString() == toolStripComboBox3.SelectedItem.ToString())
                {
                    subjectID = (int)row[0];
                    break;
                }
            }
            Client c = new Client("127.0.0.1", 200);
            //грузим таблицу со столбиками "оценк и дата" из таблицы оценки у выбранного студента по-выбранному предмету
            dataGridView1.DataSource = c.GetTable(@"select ""Оценка"",""Дата"" from ""Оценки"" where ""IDСтудента""=" + studentID + @" and ""IDПредмета""=" + subjectID);
            c.CloseConnection();


        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadGroups();
            LoadSubjects();
         

            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBox3.SelectedIndex = 0;
            LoadData();

            if (Program.AccessCode == 0)
            {
                button1.Enabled = true;
            }

        }

        private void toolStripComboBox3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }
        //при выборе новых студента или предмета перезагружаем оценки
        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (toolStripComboBox3.SelectedItem != null && toolStripComboBox1.SelectedItem != null)
                LoadData();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox3.SelectedItem != null && toolStripComboBox1.SelectedItem != null)
                LoadData();

        }

        private void toolStripComboBox3_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void Insert(int student,int subject,int mark, string date)//метод добавления новой оценки в таблицу
        {
            Client c = new Client("127.0.0.1", 200);
            c.SendQuery(String.Format(@"insert into Оценки(""IDСтудента"",""IDПредмета"",""Оценка"",""Дата"") values('{0}','{1}','{2}','{3}')", student, subject, mark, date));
            c.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int studentID = 0, subjectID = 0;
            //находим ид студента
            foreach (DataRow row in dt1.Rows)
            {
                if (row[1].ToString() == toolStripComboBox1.SelectedItem.ToString())
                {
                    studentID = (int)row[0];
                    break;
                }
            }
            //ид предмета
            foreach (DataRow row in dt2.Rows)
            {
                if (row[1].ToString() == toolStripComboBox3.SelectedItem.ToString())
                {
                    subjectID = (int)row[0];
                    break;
                }
            }
            //вызываем метод Insert
            Insert(studentID,subjectID,Int32.Parse(numericUpDown1.Value.ToString()),dateTimePicker1.Value.Date.ToShortDateString());
            LoadData();
        }
    }
}
