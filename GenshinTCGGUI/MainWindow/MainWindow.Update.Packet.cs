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
                var Teams = new TeamRegion[] { TeamMe, TeamEnemy };
                int teamid = packet.Category / 10;
                int category = packet.Category % 10;
                var packetteam = Teams[int.Abs(teamid - meid)];
                switch (packet.Type)
                {
                    case ClientUpdateType.None:
                        break;
                    case ClientUpdateType.CurrTeam:
                        break;
                    case ClientUpdateType.WaitingTime:
                        break;
                    case ClientUpdateType.Character:
                        var cha_category = (ClientUpdateCreate.CharacterUpdateCategory)category;
                        var cha = packetteam._characters.Cards[packet.Ints[0]];
                        switch (cha_category)
                        {
                            case ClientUpdateCreate.CharacterUpdateCategory.Hurt:
                                cha.HP -= packet.Ints[2];
                                //TODO: int[1]表示伤害的元素，做显示的时候再用
                                break;
                            case ClientUpdateCreate.CharacterUpdateCategory.Heal:
                                cha.HP += packet.Ints[1];
                                break;
                            case ClientUpdateCreate.CharacterUpdateCategory.Element:
                                cha.Element = packet.Ints[1];
                                break;
                            case ClientUpdateCreate.CharacterUpdateCategory.MP:
                                cha.MP = packet.Ints[1];
                                break;
                            case ClientUpdateCreate.CharacterUpdateCategory.Die:
                                break;
                            case ClientUpdateCreate.CharacterUpdateCategory.UseSkill:
                                //TODO:显示
                                break;
                            case ClientUpdateCreate.CharacterUpdateCategory.Switch:
                                packetteam.CurrCharacter = packet.Ints[0];
                                break;
                        }
                        break;
                    case ClientUpdateType.Persistent:
                        IPersistentManager manager = packet.Ints[0] switch
                        {
                            -1 => packetteam,
                            11 => packetteam._summons,
                            12 => packetteam._supports,
                            _ => packetteam._characters.Cards[packet.Ints[0]]
                        };
                        switch (category)
                        {
                            case 0://obtain
                                manager.Add(packet.Strings[0], packet.Strings[1], packet.Ints[1], packet.Ints[2]);
                                break;
                            case 1://trigger
                                manager.Trigger(packet.Ints[1], packet.Ints[2]);
                                break;
                            case 2://lost
                                manager.RemoveAt(packet.Ints[1]);
                                break;
                        }
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
                                    card.Children.Add(teamid == meid ? new ActionCardGrid(packet.Strings[0], packet.Strings[1], CardMe.Children.Count) : new GamingUnselectableActionCardGrid());
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
                }
            });
        }

    }
}
