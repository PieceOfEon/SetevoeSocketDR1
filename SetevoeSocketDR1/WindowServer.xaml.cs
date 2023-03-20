using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace SetevoeSocketDR1
{
    /// <summary>
    /// Interaction logic for WindowServer.xaml
    /// </summary>
    public partial class WindowServer : Window
    {
        private TcpListener serverListener;
        public WindowServer()
        {
            InitializeComponent();

        }
        private void AddMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                // добавляем новое сообщение в текстовое поле
                messagesTextBox2.AppendText(message + "\n");

                // прокручиваем текстовое поле до последнего сообщения
                messagesTextBox2.ScrollToEnd();
            });
        }
        public async Task Start(int port)
        {
            try
            {
                // создаем новый экземпляр TcpListener и запускаем прослушивание порта
                serverListener = new TcpListener(IPAddress.Any, port);
                serverListener.Start();

                Console.WriteLine($"Server started on port {port}");

                while (true)
                {
                    // принимаем входящее подключение
                    TcpClient client = await serverListener.AcceptTcpClientAsync();

                    // обрабатываем клиента в отдельном потоке
                    Task.Run(() => HandleClient(client));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private async void AcceptClients()
        {
            try
            {
                while (true)
                {
                    // принимаем входящее подключение
                    TcpClient clientSocket = await serverListener.AcceptTcpClientAsync();

                    // создаем новый поток для обработки клиента
                    Task.Run(() => HandleClient(clientSocket));
                }
            }
            catch (Exception ex)
            {
                // выводим сообщение об ошибке
                AddMessage($"Ошибка: {ex.Message}");
            }
        }

        private async void HandleClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];

                    while (true)
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                       
                        if (bytesRead == 0)
                        {
                            break;
                        }

                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        AddMessage($"Client: {message}");
                        
                        if (message == "Bye")
                        {
                            // if the client sends "Bye", close the connection and break out of the loop
                            AddMessage("Client disconnected");
                            break;
                        }
                        // generate a response
                        string response = GenerateResponse();
                        AddMessage($"Server: {response}");
                        // send the response back to the client
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                AddMessage($"Error handling client: {ex.Message}");
                
            }
       
            client.Close();
        }

        private string GenerateResponse()
        {
            string[] responses = {
            "Hello!",
            "How are you?",
            "Fine, thanks.",
            "What's your name?",
            "My name is Server.",
            "What time is it?",
            DateTime.Now.ToString("t")
        };

            Random rand = new Random();
            int index = rand.Next(responses.Length);

            return responses[index];
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // получаем порт сервера из текстового поля
                int port = int.Parse(portTextBox.Text);

                // создаем новый экземпляр TcpListener и запускаем прослушивание порта
                serverListener = new TcpListener(IPAddress.Any, port);
                serverListener.Start();

                // отключаем кнопку "Start"
                //startButton.IsEnabled = false;

                // добавляем сообщение об успешном запуске сервера в окно сообщений
                AddMessage("Server started on port " + port);

                // запускаем цикл ожидания подключений от клиентов
                AcceptClients();
            }
            catch (Exception ex)
            {
                // В случае ошибки вывод

            }

        }
    }
}

