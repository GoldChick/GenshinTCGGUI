﻿using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using TCGBase;
using System.Threading.Tasks;
using System.Text.Json;
using System.Reflection;
using GenshinTCGGUI;
using System.Threading;
using System.Windows.Threading;
using Prefab;

namespace TCGClient
{
    public class SocketClientClient
    {
        private readonly string _ip = string.Empty;
        private readonly int _port = 0;
        private readonly Socket _socket;
        private readonly byte[] _buffer = new byte[1024 * 1024 * 2];

        private readonly Action<ReadonlyGame> _update;

        private ReadonlyGame Game;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">连接服务器的IP</param>
        /// <param name="port">连接服务器的端口</param>
        public SocketClientClient(string ip, int port, Action<ReadonlyGame> updatefull)
        {
            _ip = ip;
            _port = port;
            _update = updatefull;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        /// <summary>
        /// 开启服务,连接服务端
        /// </summary>
        public async void StartClient()
        {
            try
            {
                IPAddress address = IPAddress.Parse(_ip);
                IPEndPoint endPoint = new(address, _port);
                MainWindow.Instance.UpdateHelpText("客户端开启成功");
                try
                {
                    _socket.Connect(endPoint);
                    MainWindow.Instance.UpdateHelpText("客户端连接成功");
                    await Task.Run(Receive);
                }
                catch (Exception ex)
                {
                    MainWindow.Instance.UpdateHelpText($"客户端收到错误 {ex.Message}");
                    return;
                }
            }
            catch (Exception)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                throw;
            }
        }
        public void Receive()
        {
            int rec = 1;
            while (rec > 0)
            {
                rec = _socket.Receive(_buffer, 8, SocketFlags.None);
                string str = Encoding.UTF8.GetString(_buffer, 0, rec);
                if (int.TryParse(str, out int length))
                {
                    int offset = 0;
                    const int seperate = 1024;
                    for (int i = 0; i < length / seperate; i++)
                    {
                        _socket.Receive(_buffer, offset, seperate, SocketFlags.None);
                        offset += seperate;
                    }
                    _socket.Receive(_buffer, offset, length % seperate, SocketFlags.None);
                    str = Encoding.UTF8.GetString(_buffer, 0, length);
                    MessageProcess(str);
                }
            }
        }
        public void Send(string code, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes($"{code}|{message}");
            _socket.Send(Encoding.UTF8.GetBytes(bytes.Length.ToString().PadLeft(8, '0')));
            _socket.Send(bytes);

            //var sendMessage = $"{code}|{message}";
            //_socket.Send(Encoding.UTF8.GetBytes(sendMessage));
        }
        private void MessageProcess(string str)
        {
            string[] strs = str.Split('|');
            switch (strs[0])
            {
                case "GAME":
                    Game = JsonSerializer.Deserialize<ReadonlyGame>(strs[1]);
                    _update.Invoke(Game);
                    DateTime dt = DateTime.Now;
                    MainWindow.Instance.UpdateHelpText($"curr time: {dt}");
                    break;
                case "PACKET":
                    var p = JsonSerializer.Deserialize<ClientUpdatePacket>(strs[1]);
                    MainWindow.Instance.UpdatePacketRender(1, p);
                    break;
                case "COST":
                    DiceCostPacket packet = JsonSerializer.Deserialize<DiceCostPacket>(strs[1]);
                    MainWindow.Instance.ClientUpdateCosts(packet);
                    break;
                case "NETEVENT":
                    ActionType demand = JsonSerializer.Deserialize<ActionType>(strs[1]);
                    var t = Task.Run(() => MainWindow.Instance.RequestEventCallBack(demand, "no txt"));
                    Send("NETEVENT", JsonSerializer.Serialize(t.Result));
                    break;
                case "ENEMY":
                    demand = JsonSerializer.Deserialize<ActionType>(strs[1]);
                    MainWindow.Instance.RequestEnemyEventCallBack(demand);
                    break;
            }
        }
    }
}