using Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using TCGBase;

namespace GenshinTCGGUI
{
    public partial class MainWindow
    {
        private ActionType State;
        private bool During;
        private bool Over;
        private NetEvent? NetEvent;
        public NetEvent RequestEventCallBack(ActionType demand, string txt)
        {
            Dispatcher.Invoke(() =>
            {
                State = demand;
                assist0.Text = "我方行动";
                assist1.Text = $"当前索取行动：{demand}";
                assist2.Text = $"帮助文本：{txt}";
                InitSelect();
            });
            Over = false;

            //if (During)
            //{

            //}
            //else
            {
                During = true;
                var a = Task.Run(() =>
                {
                    while (NetEvent == null)
                    {
                        Thread.Sleep(100);
                    }
                    During = false;
                    var copy = NetEvent;
                    NetEvent = null;
                    return copy;
                });
                return a.Result;
            }
            //TODO:超时
        }
        private void ActionPermitted_Click(object sender, RoutedEventArgs e)
        {
            switch (State)
            {
                case ActionType.ReRollDice:
                    {
                        int[] dices = new int[DiceContainer.Children.Count];
                        DiceSelected.ForEach(c => dices[c.Index] = 1);
                        State = ActionType.Pass;
                        NetEvent = new(new(ActionType.ReRollDice), Array.Empty<int>(), dices);
                    }
                    break;
                case ActionType.ReRollCard:
                    {
                        int[] cards = new int[CardMe.Children.Count];
                        RerollCardsSelected.ForEach(c => cards[c.Index] = 1);
                        State = ActionType.Pass;
                        NetEvent = new(new(ActionType.ReRollCard), Array.Empty<int>(), cards);
                    }
                    break;
                case ActionType.SwitchForced:
                    {
                        if (CharacterSelected != null)
                        {
                            State = ActionType.Pass;
                            NetEvent = new(new(ActionType.SwitchForced, CharacterSelected.Index));
                        }
                    }
                    break;
                default:
                    {
                        if (CharacterSelected != null && CharacterSelected.Index != TeamMe.CurrCharacter)
                        {
                            int[] selects = new int[8];
                            DiceSelected.ForEach(d => selects[(int)d.Element]++);
                            NetAction action = new(ActionType.Switch, CharacterSelected.Index);
                            if (Client0.GetEventFinalDiceRequirement(action).EqualTo(selects))
                            {
                                State = ActionType.Pass;
                                NetEvent = new(action, selects);
                            }
                        }
                        if (UseCardSelected != null)
                        {
                            NetAction action = new(ActionType.UseCard, UseCardSelected.Index);
                            var additionalTargets = Client0.GetTargetEnums(action);
                            //TODO:比较粗浅
                            var a = (TargetEnum e) => e switch
                            {
                                TargetEnum.Character_Me or TargetEnum.Character_Enemy => typeof(CharacterCardGrid),
                                TargetEnum.Card_Me => typeof(ActionCardGrid),
                                _ => null
                            };
                            if (additionalTargets.Count == TargetEnumSelected.Count)
                            {
                                for (int i = 0; i < additionalTargets.Count; i++)
                                {
                                    if (TargetEnumSelected[i].GetType() != a(additionalTargets[i]))
                                    {
                                        break;
                                    }
                                }
                                int[] selects = new int[8];
                                DiceSelected.ForEach(d => selects[(int)d.Element]++);
                                if (Client0.GetEventFinalDiceRequirement(action).EqualTo(selects))
                                {
                                    State = ActionType.Pass;
                                    NetEvent = new(action, selects, TargetEnumSelected.Select(t => t.Index).ToArray());
                                }
                            }
                        }
                        if (UseSkillSelected != null)
                        {
                            NetAction action = new(ActionType.UseSKill, UseSkillSelected.Index);
                            int[] selects = new int[8];
                            DiceSelected.ForEach(d => selects[(int)d.Element]++);
                            if (Client0.GetEventFinalDiceRequirement(action).EqualTo(selects))
                            {
                                State = ActionType.Pass;
                                NetEvent = new(action, selects, TargetEnumSelected.Select(t => t.Index).ToArray());
                            }
                        }
                    }
                    break;
            }
            Thread.Sleep(100);
        }
        private void Blend_Click(object sender, RoutedEventArgs e)
        {
            if (State == ActionType.Trival && UseCardSelected != null && DiceSelected.Count == 1)
            {
                int[] selects = new int[8];
                int d = (int)DiceSelected[0].Element;
                NetAction action = new(ActionType.Blend, UseCardSelected.Index);
                if (d != 0 && Client0.GetEventFinalDiceRequirement(action).Costs[d] == 0)
                {
                    selects[d] = 1;
                    State = ActionType.Pass;
                    NetEvent = new(action, selects);
                }
            }
        }
        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            if (State == ActionType.Trival)
            {
                State= ActionType.Pass;
                NetEvent = new(new(ActionType.Pass));
            }
        }

    }
}
