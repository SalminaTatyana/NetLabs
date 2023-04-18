using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IPAddress localAddress;
        public int localPort;
        public int remotePort;
        public MainWindow()
        {
            InitializeComponent();
            Setting();
        }
        public async void Setting()
        {
            try
            {
                using (FileStream fs = new FileStream("appsetting.json", FileMode.OpenOrCreate))
                {
                    Connection connection = await JsonSerializer.DeserializeAsync<Connection>(fs);
                    if (!String.IsNullOrEmpty(connection.ip))
                    {
                        localAddress = IPAddress.Parse(connection.ip);
                        remotePort = connection.remotePort;
                        localPort = connection.localPort;
                        ip.Text = localAddress.ToString();
                        port.Text = remotePort.ToString();
                        portLocal.Text = localPort.ToString();
                    }
                }
            }
            catch (JsonException ex)
            {

                histoty.Items.Add("Ошибка считывания json");
            }
            catch (Exception ex)
            {

                histoty.Items.Add(ex);
            }

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void IPValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            bool isHandled = true;
            Regex regex2 = new Regex("[0-9.]");
            if (regex2.IsMatch(e.Text))
            {
                isHandled = false;
            }
            e.Handled = isHandled;
        }
        private async void SaveUDPData(object sender, RoutedEventArgs e)
        {
            try
            {

                localAddress = IPAddress.Parse(ip.Text);
                remotePort = Int32.Parse(port.Text);
                localPort = Int32.Parse(portLocal.Text);

                if (Int32.Parse(portLocal.Text) > 65535)
                {
                    throw new OverflowException("Слишком большое число в локальном порте. Используется локальный порт не более 65535");
                }

                if (Int32.Parse(port.Text) > 65535)
                {
                    throw new OverflowException("Слишком большое число в удаленном порте. Используется удаленный порт не более 65535");
                }
                saveUDPDataBtn.IsEnabled = false;
                sendUDPDataBtn.IsEnabled = true;
                checkUdp.IsEnabled = true;
                inText.IsEnabled = true;
                ip.IsEnabled = false;
                port.IsEnabled = false;
                histoty.Items.Add("Подключение");
                ReceiveMessageAsync();
            }
            catch (ArgumentNullException ex)
            {
                if (String.IsNullOrEmpty(ip.Text))
                {
                    histoty.Items.Add("Ip адрес не введен, введите Ip адрес.");
                }
                if (String.IsNullOrEmpty(portLocal.Text))
                {
                    histoty.Items.Add("Локальный порт не введен, введите локальный порт");
                }
                if (String.IsNullOrEmpty(port.Text))
                {
                    histoty.Items.Add("Удаленный порт не введен, введите локальный порт");
                }
            }
            catch (FormatException ex)
            {
                if (ex.TargetSite.DeclaringType.Name == "IPAddressParser")
                {
                    histoty.Items.Add("Ip адрес имеет неправильный тип, введите Ip адрес по примеру: 127.0.0.1");

                }
                if (ex.TargetSite.DeclaringType.Name == "Number")
                {
                    histoty.Items.Add("Порты имеют неправильный тип, введите порты по примеру: 55555");

                }

            }
            catch (OverflowException ex)
            {
                histoty.Items.Add(ex.Message);
            }
            catch (Exception ex)
            {
                histoty.Items.Add(ex.Message);
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (checkbox.IsChecked==true)
            {
                portLocal.Visibility = Visibility.Visible;
                localPortLable.Visibility = Visibility.Visible;
            }
            else {
                portLocal.Visibility = Visibility.Hidden;
                localPortLable.Visibility = Visibility.Hidden;
            }
           
        }
        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string str = inText.Text;
                UDPMessage msg = new UDPMessage();
                msg.IsCheck = false;
                msg.Message = ISO.Code(str);
                msg.Length = msg.Message.Length;
                outText.Text = outText.Text + "Вы: " + str + "\n";
                await SendMessageAsync(msg);
            }
        }
        public async Task ReceiveMessageAsync()
        {
            string formattedDate = DateTime.Now.ToString("dd.MM.yyyy_hh-mm-ss");
            string path = formattedDate + ".txt";
            using UdpClient receiver = new UdpClient(localPort);
            histoty.Items.Add("Прослушивание порта " + localPort);
            while (true)
            {
                var result = await receiver.ReceiveAsync();
                var message = result.Buffer;
                UDPMessage msg = new UDPMessage();
                msg = UDPMessageManager.Decoding(message);
                if (msg != null) {
                    if (msg.IsCheck)
                    {
                        UDPMessage msgReturn = new UDPMessage();
                        msgReturn.Message = ISO.Code("Проверка пройдена");
                        msgReturn.IsCheck = false;
                        msgReturn.Length = msgReturn.Message.Length;
                        SendMessageAsync(msgReturn);
                        outText.Text = outText.Text + "Собеседник: " + "Проверка соединения пройдена" + "\n";
                    }
                    else {
                        if (msg.Message!=null)
                        {
                            string msgDecode = ISO.Decode(msg.Message);
                            if (msg.Length == msg.Message.Length)
                            {
                                outText.Text = outText.Text + "Собеседник: " + msgDecode + "\n";

                            }
                            else
                            {
                                outText.Text = outText.Text + "Собеседник: " + "Сообщение повреждено" + "\n";

                            }
                            FileManager.Save(message);
                        }
                        
                    }
                }  
            }
        }
        async Task SendMessageAsync(UDPMessage msg)
        {
            using UdpClient sender = new UdpClient();
            byte[] bytes = UDPMessageManager.CodingAsync(msg);
            if (msg.IsCheck)
            {
                outText.Text = outText.Text + "Вы: " +"Проверка подключения" + "\n";

            }
            
            await sender.SendAsync(bytes, bytes.Length, new IPEndPoint(localAddress, remotePort));
        }
        private async void SendUDPData(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = inText.Text;
                UDPMessage msg= new UDPMessage();
                msg.IsCheck= false;
                msg.Message = ISO.Code(str);
                msg.Length=msg.Message.Length;
                outText.Text = outText.Text + "Вы: " + str + "\n";

                await SendMessageAsync(msg);

            }
            catch (ArgumentOutOfRangeException)
            {
                histoty.Items.Add("Удаленный порт слишком большой, введите порт от 1024 до 65535");
            }
        }

        private async void CheckUDPData(object sender, RoutedEventArgs e)
        {
            string str = inText.Text;
            UDPMessage msg = new UDPMessage();
            msg.IsCheck = true;
            msg.Message = ISO.Code(str);
            msg.Length = msg.Message.Length;
            await SendMessageAsync(msg);
        }
    }
}
