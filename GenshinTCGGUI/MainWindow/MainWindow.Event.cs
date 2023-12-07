using Prefab;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TCGBase;
using TCGClient;

namespace GenshinTCGGUI
{
    public partial class MainWindow
    {
        private ActionType State;
        private NetEvent? NetEvent;
        private bool _token;
        private bool _iscurrteam;
        public NetEvent RequestEventCallBack(ActionType demand)
        {
            Dispatcher.Invoke(() =>
            {
                State = demand;
                InitSelect();
                switch (State)
                {
                    case ActionType.ReRollDice:
                        BlackBlocker_DiceOnly.Visibility = Visibility.Visible;
                        BlackBlocker_DiceAndCard.Visibility = Visibility.Hidden;
                        break;
                    case ActionType.ReRollCard:
                        BlackBlocker_DiceAndCard.Visibility = Visibility.Visible;
                        BlackBlocker_DiceOnly.Visibility = Visibility.Hidden;
                        break;
                    default:
                        if (!_iscurrteam)
                        {
                            Task.Run(() =>
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    Tip.Background = new SolidColorBrush(Colors.Gold);
                                    Tip.Visibility = Visibility.Visible;
                                    (Tip.Children[0] as TextBlock).Text = "我方行动";
                                });
                                Thread.Sleep(666);
                                Dispatcher.Invoke(() => Tip.Visibility = Visibility.Hidden);
                            });
                        }
                        _iscurrteam = true;
                        BlackBlocker_DiceAndCard.Visibility = Visibility.Hidden;
                        BlackBlocker_DiceOnly.Visibility = Visibility.Hidden;
                        break;
                }
            });
            _token = true;
            Thread.Sleep(200);
            return Task.Run(() =>
            {
                _token = false;
                while (NetEvent == null)
                {
                    Thread.Sleep(100);
                    if (_token)
                    {
                        return new(new(ActionType.Pass));
                    }
                }
                var copy = NetEvent;
                NetEvent = null;
                return copy;
            }).Result;
        }
        public void RequestEnemyEventCallBack(ActionType demand)
        {
            switch (demand)
            {
                case ActionType.ReRollDice:
                    break;
                case ActionType.ReRollCard:
                    break;
                case ActionType.SwitchForced:
                    break;
                default:
                    if (_iscurrteam)
                    {
                        Task.Run(() =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                Tip.Background = new SolidColorBrush(Colors.Purple);
                                Tip.Visibility = Visibility.Visible;
                                (Tip.Children[0] as TextBlock).Text = "对方行动";
                            });
                            Thread.Sleep(666);
                            Dispatcher.Invoke(() => Tip.Visibility = Visibility.Hidden);
                        });
                    }
                    _iscurrteam = false;
                    break;
            }
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
                            if (SwitchCosts.ElementAt(CharacterSelected.Index).DiceEqualTo(selects))
                            {
                                State = ActionType.Pass;
                                NetEvent = new(new(ActionType.Switch, CharacterSelected.Index), selects);
                                SelectStateMachine = TrivalSelectStateMachine.None;
                            }
                        }
                        if (UseCardSelected != null)
                        {
                            CardCost cost = CardCosts.ElementAt(UseCardSelected.Index);
                            var additionalTargets = cost.Targets;
                            var a = (TargetEnum e) => e switch
                            {
                                TargetEnum.Character_Me or TargetEnum.Character_Enemy => typeof(CharacterCardGrid),
                                _ => null
                            };
                            if (additionalTargets.Count() == TargetEnumSelected.Count)
                            {
                                for (int i = 0; i < additionalTargets.Count(); i++)
                                {
                                    if (TargetEnumSelected[i].GetType() != a(additionalTargets.ElementAt(i)))
                                    {
                                        break;
                                    }
                                }
                                int[] selects = new int[8];
                                DiceSelected.ForEach(d => selects[(int)d.Element]++);
                                if (cost.Cost.DiceEqualTo(selects))
                                {
                                    State = ActionType.Pass;
                                    NetEvent = new(new(ActionType.UseCard, UseCardSelected.Index), selects, TargetEnumSelected.Select(t => t.Index).ToArray());
                                    SelectStateMachine = TrivalSelectStateMachine.None;
                                }
                            }
                        }
                        if (UseSkillSelected != null)
                        {
                            int[] selects = new int[8];
                            DiceSelected.ForEach(d => selects[(int)d.Element]++);
                            if (SkillCosts.ElementAt(UseSkillSelected.Index).DiceEqualTo(selects))
                            {
                                State = ActionType.Pass;
                                NetEvent = new(new(ActionType.UseSKill, UseSkillSelected.Index), selects, TargetEnumSelected.Select(t => t.Index).ToArray());
                                SelectStateMachine = TrivalSelectStateMachine.None;
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
                if (d != 0 && BlendCost != null && BlendCost.DiceCost[0] == 0 && BlendCost.DiceCost[d] == 0 )
                {
                    selects[d] = 1;
                    State = ActionType.Pass;
                    NetEvent = new(new(ActionType.Blend, UseCardSelected.Index), selects);
                    SelectStateMachine = TrivalSelectStateMachine.None;
                }
            }
        }
        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            if (State == ActionType.Trival)
            {
                State = ActionType.Pass;
                NetEvent = new(new(ActionType.Pass));
            }
        }

    }
}
