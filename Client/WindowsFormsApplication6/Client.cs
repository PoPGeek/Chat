using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication6
{
    public class Client//класс клиента
    {
        TcpClient newClient; 
        public string IP { get; set; }
        public int Port { get; set; }
        public Client(string ip, int port)//конструктор класса с параметрами ip и порта
        {
            newClient = new TcpClient();
            IP = ip;
            Port = port;
            OpenConnection();
        }
        public void OpenConnection()//метод отрытия соединения
        {
            // Устанавливаем соединение с IPEndPoint
            IPAddress ipAddr = IPAddress.Parse(IP);
            IPEndPoint endPoint = new IPEndPoint(ipAddr, Port);
            newClient.Connect(endPoint);
        }
        public void CloseConnection()//метод закрытия соединения
        {
            newClient.Close();
        }
        public DataTable GetTable(string query)//метод для получения таблицы из запроса
        {
            NetworkStream tcpStream = newClient.GetStream();//получаем сетевой поток из клиента
            //переконвертируем наш запрос в массив байт и отсылаем на сервер
            byte[] sendBytes = Encoding.UTF8.GetBytes(query);
            tcpStream.Write(sendBytes, 0, sendBytes.Length);
            //получаем ответ от сервера
            byte[] recBytes = new byte[newClient.ReceiveBufferSize];
            int i = tcpStream.Read(recBytes, 0, recBytes.Length);
            tcpStream.Close();
            //создаем объект DataSet и производим для него десериализацию клиентского потока
            DataTable data = ByteArrayToObject(recBytes);
            return data;
        }
        public void SendQuery(string query)//метод для отправления запроса на сервер без получения ответа(для update,delete,insert)
        {
            NetworkStream tcpStream = newClient.GetStream();

            byte[] sendBytes = Encoding.UTF8.GetBytes(query);
            tcpStream.Write(sendBytes, 0, sendBytes.Length);
            tcpStream.Close();

        }
        //метод десериализации массива байт в таблицу DataTable
        private static DataTable ByteArrayToObject(Byte[] Buffer)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(Buffer);
            return (DataTable)formatter.Deserialize(stream);
        }

    }
}

