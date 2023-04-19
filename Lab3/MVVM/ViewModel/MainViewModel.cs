using Lab3.MVVM.Core;
using Lab3.MVVM.Model;
using Lab3.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand DisonnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public Server _server;
        public string Username { get; set; }
        public string Message { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.connectedEvent +=UserConnected;
            _server.disconnectedEvent +=UserDisonnected;
            _server.msgReceivedEvent += MsgReceived;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServerAsync(Username,Ip,Port), o=>!string.IsNullOrEmpty(Username)&&! _server._client.Connected);
            DisonnectToServerCommand = new RelayCommand(o => _server.DisonnectToServerAsync(),o=>_server._client.Connected);
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o=>!string.IsNullOrEmpty(Message));
        }
        private void UserConnected()
        {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage()
            };
            if (!Users.Any(x=>x.UID==user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
        private void UserDisonnected()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }
        public void MsgReceived()
        {
            var msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }
    }
}
