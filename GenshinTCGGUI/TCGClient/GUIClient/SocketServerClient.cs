﻿using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;
using System.Threading.Tasks;
using TCGBase;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GenshinTCGGUI;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace TCGClient
{
    public class SocketServerClient : AbstractMainClient
    {
        private string _ip = string.Empty;
        private int _port = 0;
        private Socket _socket = null;
        private byte[] _buffer = new byte[1024 * 1024 * 2];

        private Socket _clientSocket;

        private NetEvent? NetEvent;
        public SocketServerClient(string ip, int port)
        {
            _ip = ip; _port = port;
        }
        public async void StartListen()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse(_ip);
            IPEndPoint endPoint = new(address, _port);
            _socket.Bind(endPoint);
            _socket.Listen(1);

            MainWindow.Instance.UpdateHelpText("服务端开启成功");
            await Task.Run(()=> _clientSocket = _socket.Accept());

            MainWindow.Instance.UpdateHelpText("服务端连接客户端成功");
            await Task.Run(ReceiveMessage);
            CloseClientSocket();
        }
        public void SendToClient(string code, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes($"{code}|{message}");
            _clientSocket.Send(Encoding.UTF8.GetBytes(bytes.Length.ToString().PadLeft(8, '0')));
            _clientSocket.Send(bytes);
        }
        /// <summary>
        /// 接收客户端消息
        /// </summary>
        private void ReceiveMessage()
        {
            int rec = 1;
            while (rec > 0)
            {
                rec = _clientSocket.Receive(_buffer, 8, SocketFlags.None);
                string str = Encoding.UTF8.GetString(_buffer, 0, rec);
                if (int.TryParse(str, out int length))
                {
                    int offset = 0;
                    const int seperate = 1024;
                    for (int i = 0; i < length / seperate; i++)
                    {
                        _clientSocket.Receive(_buffer, offset, seperate, SocketFlags.None);
                        offset += seperate;
                    }
                    _clientSocket.Receive(_buffer, offset, length % seperate, SocketFlags.None);
                    str = Encoding.UTF8.GetString(_buffer, 0, length);
                    MessageProcess(str);
                }
            }
            CloseClientSocket();
        }
        /// <summary>
        /// 关闭与某个客户端的连接
        /// </summary>
        private void CloseClientSocket()
        {
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Close();
        }
        /// <summary>
        /// 可以在里面暴力throw从而关闭与某客户端的连接
        /// </summary>
        private void MessageProcess(string message)
        {
            string[] strs = message.Split('|');
            switch (strs[0])
            {
                case "NETEVENT":
                    var evt = JsonSerializer.Deserialize<NetEvent>(strs[1]);
                    if (IsEventValid(evt))
                    {
                        NetEvent = evt;
                    }
                    break;
                case "COST":
                    NetAction action = JsonSerializer.Deserialize<NetAction>(strs[1]);
                    SendToClient("COST", JsonSerializer.Serialize(GetEventFinalDiceRequirement(action)));
                    break;
            }
        }

        public override void InitServerSetting(ServerSetting setting)
        {
        }

        public override ServerPlayerCardSet RequestCardSet()
        {
            return new(new PlayerNetCardSet()
            {
                Characters = new[] { "genshin3_3:mona", "genshin3_3:xiangling", "genshin3_3:noel" },
                ActionCards = new[] { "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie" ,
                                                      "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie", "genshin3_3:partner_timmie" ,
                                                      "genshin3_3:赌徒的耳环", "genshin3_3:partner_timmie", "genshin3_3:赌徒的耳环", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth" ,
                                                      "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth", "genshin3_3:partner_changtheninth" ,
                                                      "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu", "genshin3_3:partner_liusu" ,
                                                      "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery", "genshin3_3:location_dawnwinery" },
            });
        }
        public override NetEvent RequestEvent(ActionType demand, string help_txt = "Null")
        {
            SendToClient("COST", JsonSerializer.Serialize(GetAllDiceCost()));
            SendToClient("NETEVENT", JsonSerializer.Serialize(demand));

            return Task.Run(() =>
            {
                while (NetEvent == null)
                {
                    Thread.Sleep(100);
                }
                var copy = NetEvent;
                NetEvent = null;
                return copy;
            }).Result;
        }
        public override void RequestEnemyEvent(ActionType demand)
        {
            base.RequestEnemyEvent(demand);
            SendToClient("ENEMY",JsonSerializer.Serialize(demand));
        }
        public override void Update(ClientUpdatePacket packet)
        {
            base.Update(packet);
            SendToClient("PACKET", JsonSerializer.Serialize(packet));
            MainWindow.Instance.UpdateHelpText("服务端发送Update信息");
        }
        public override void UpdateRegion()
        {
            base.UpdateRegion();
            SendToClient("GAME", JsonSerializer.Serialize(Game));
        }
        public override void BindInit(ReadonlyGame game)
        {
            base.BindInit(game);
            SendToClient("GAME", JsonSerializer.Serialize(Game));
        }
    }
}