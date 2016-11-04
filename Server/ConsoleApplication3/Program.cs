using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {


            string conn_param = "Server=127.0.0.1;Port=5434;User Id=postgres;Password=1111;Database=postgres;";

            //; //Например: "Server=127.0.0.1;Port=5432;User Id=postgres;Password=mypass;Database=mybase;"
            NpgsqlConnection conn = new NpgsqlConnection(conn_param);

            //ставим прослушку по умолчанию на 200 порт   
            int port = 200;
            //устанавливаем IP-адресс сервера с БД
            IPAddress ip_adress = IPAddress.Parse("127.0.0.1");
            //создали TcpListener
            TcpClient client;
            TcpListener server = new TcpListener(ip_adress, port);
            //запустили прослушивание на подключения клиентов
            server.Start();
            Console.WriteLine("Server started on port: "+port+" ["+DateTime.Now.ToShortDateString()+" "+ DateTime.Now.ToShortTimeString()+"]");
            Console.WriteLine("Waiting for connections...");
            while (true)
            {
                //поймали клиента TcpClient                
                client = server.AcceptTcpClient();
                string clientIPAddress = IPAddress.Parse(((
    IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).ToString();
                Console.WriteLine("Client connected! " + clientIPAddress + " [" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "]");
                //получили поток клиента
                NetworkStream client_stream = client.GetStream();
                Console.WriteLine("Receiving data from client...");
                byte[] bytes = new byte[8192];

                int i;
                string data = "";
                // Принимаем данные от клиента в цикле пока не дойдём до конца.
                i = client_stream.Read(bytes, 0, bytes.Length);
                
                    // Преобразуем данные в ASCII string.
                    data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);


                    conn.Open(); //Открываем соединение.

                    //Показываем данные на консоли
                    DataTable dt = new DataTable();
                    if (data.Remove(6) == "select")
                    {
                        Console.WriteLine("Sending reply to client...");
                        DataSet ds = new DataSet();
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(data, conn);
                        da.Fill(ds);
                        dt = ds.Tables[0];
                        byte[] bbb = ObjectToByteArray(dt);
                        client_stream.Write(bbb, 0, bbb.Length);
                    }
                    else
                    {
                        NpgsqlCommand comm = new NpgsqlCommand(data, conn);
                        comm.ExecuteNonQuery();
                    }

              
                // Закрываем соединение.
                client.Close();

                conn.Close();
            }

            //client_streem.Write(ObjectToByteArray(), 0, ObjectToByteArray().Length);

        }

                




        //            // Устанавливаем для сокета локальную конечную точку
        //            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
        //            IPAddress ipAddr = ipHost.AddressList[0];
        //            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

        //            // Создаем сокет Tcp/Ip
        //            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
        //            try
        //            {
        //                sListener.Bind(ipEndPoint);
        //                sListener.Listen(10);

        //                // Начинаем слушать соединения
        //                while (true)
        //                {
        //                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

        //                    // Программа приостанавливается, ожидая входящее соединение
        //                    Socket handler = sListener.Accept();
        //                    string data = null;

        //                    // Мы дождались клиента, пытающегося с нами соединиться

        //                    byte[] bytes = new byte[1024];
        //                    int bytesRec = handler.Receive(bytes);

        //                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
        //                    


        //                    // Отправляем ответ клиенту\


        //                    byte[] msg = ObjectToByteArray(dt);
        //                    handler.Send(msg);

        //                    if (data.IndexOf("<TheEnd>") > -1)
        //                    {
        //                        Console.WriteLine("Сервер завершил соединение с клиентом.");
        //                        break;
        //                    }

        //                    handler.Shutdown(SocketShutdown.Both);
        //                    handler.Close();
        //                    conn.Close(); //Закрываем соединение.

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.ToString());
        //            }
        //            finally
        //            {
        //                Console.ReadLine();
        //                conn.Close(); //Закрываем соединение.

        //            }


        private static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
}
