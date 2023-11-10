using Prefab;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TCGBase;
using TCGClient;

namespace GenshinTCGGUI
{
    /// <summary>
    /// 用来更新双方手牌、骰子
    /// </summary>
    public partial class MainWindow
    {
        public void UpdatePacketRender(int meid, ClientUpdatePacket packet)
        {
            Dispatcher.Invoke(() =>
            {
                int teamid = packet.Category / 10;
                int category = packet.Category % 10;
                switch (packet.Type)
                {
                    case ClientUpdateType.None:
                        break;
                    case ClientUpdateType.CurrTeam:
                        break;
                    case ClientUpdateType.WaitingTime:
                        break;
                    case ClientUpdateType.Character:
                        break;
                    case ClientUpdateType.Persistent:
                        break;
                    case ClientUpdateType.Dice:
                        {
                            if (teamid == meid)
                            {
                                DiceContainer.Children.Clear();
                                for (int i = 0; i < packet.Ints.Length; i++)
                                {
                                    DiceContainer.Children.Add(new DiceGrid(packet.Ints[i], i));
                                }
                                DiceCount.Text = DiceContainer.Children.Count.ToString();
                                if (packet.Ints.Length<8)
                                {
                                    var a = 1;
                                }
                            }
                            else
                            {
                                //对方的
                            }
                        }
                        break;
                    case ClientUpdateType.Card:
                        {
                            int index = int.Abs(teamid - meid);
                            UniformGrid card = new UniformGrid[] { CardMe, CardEnemy }[index];
                            TextBlock cardnum = new TextBlock[] { CardNumMe, CardNumEnemy }[index];
                            switch (category)
                            {
                                case 0://Use
                                case 1://Blend
                                    card.Children.RemoveAt(packet.Ints[0]);
                                    break;
                                case 2://Obtain
                                    card.Children.Add(teamid == meid ? new ActionCardGrid(packet.Strings[0], CardMe.Children.Count) : new GamingUnselectableActionCardGrid());
                                    break;
                                case 3://Push
                                    foreach (var i in packet.Ints.Reverse())
                                    {
                                        card.Children.RemoveAt(i);
                                    }
                                    cardnum.Text = (int.Parse(cardnum.Text) + packet.Ints.Length).ToString();
                                    break;
                                case 4://Pop
                                    cardnum.Text = (int.Parse(cardnum.Text) - 1).ToString();
                                    break;
                                case 5://Broke
                                    break;
                            }
                            int cardcount = card.Children.Count;
                            card.Columns = 10 + cardcount % 2;
                            card.FirstColumn = (card.Columns - cardcount) / 2;
                            for (int i = 0; i < cardcount; i++)
                            {
                                if (card.Children[i] is ActionCardGrid acg)
                                {
                                    acg.Index = i;
                                }
                            }

                        }
                        break;
                    default:
                        break;
                }
            });
        }

    }
}
