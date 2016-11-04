using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else
                textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client c = new Client("127.0.0.1", 200);//cоздаем новый экземпляр клиента

            DataTable data = c.GetTable(String.Format("select * from Пользователи where Логин='{0}'", textBox1.Text));//получаем таблицу с нашим логином
            try
            {
                if (data.Rows[0]["Пароль"].ToString() == textBox2.Text)//если пароль совпадает с введенным
                {
                    Program.Login = textBox1.Text;//запоминаем логин
                    Program.AccessCode = (int)data.Rows[0]["Права"];//запоминаем уровень доступа
                    Form2 f = new Form2();
                    f.Show();
                    Hide();
                }
                else
                    MessageBox.Show("Неверный пароль!");
            }
            catch
            {
                MessageBox.Show("Пользователь не найден!");
            }
            c.CloseConnection();
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
