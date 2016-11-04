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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        DataTable dt1;

        private void LoadStudents()
        {
            Client c = new Client("127.0.0.1", 200);
            dt1 = c.GetTable("select * from Студенты");//записываем в локальную переменную таблицы таблицу Студенты
            c.CloseConnection();
            //грузим студентов в комбобокс
            foreach (DataRow row in dt1.Rows)
                comboBox1.Items.Add(row[1].ToString());
        }
        private void Form7_Load(object sender, EventArgs e)
        {
            LoadStudents();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string group = "";
            //ищем в какой группе учится выбранный студент
            foreach (DataRow row in dt1.Rows)
            {
                if (row[1].ToString() == comboBox1.SelectedItem.ToString())
                {
                    group = row[2].ToString();
                    break;
                }
            }
            string lines = comboBox1.SelectedItem.ToString() + " является студентом группы " + group;

            // Записываем строку в файл.название файла "Report[текущая дата и время в формате ддММгггг ччммсс]"
            string path =Application.StartupPath + "\\Report" + DateTime.Now.ToString("ddMMyyyy hhmmss")+".txt";
            //открываем поток для записи
            System.IO.StreamWriter file = new System.IO.StreamWriter(path,true);
            file.WriteLine(lines);

            file.Close();
            MessageBox.Show("Отчет сгенерирован!");


            //открываем созданный файл отчета
            System.Diagnostics.Process.Start(path);
        }
    }
}
