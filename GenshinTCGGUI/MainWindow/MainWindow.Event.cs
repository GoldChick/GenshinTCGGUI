using Prefab;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TCGBase;
using TCGClient;

namespace GenshinTCGGUI
{
    public partial class MainWindow
    {
        private ActionType State;
        private NetEvent? NetEvent;
        public NetEvent RequestEventCallBack(ActionType demand, string txt)
        {
            Dispatcher.Invoke(() =>
            {
                State = demand;
                assist1.Text = $"当前索取行动：{demand}";
                assist2.Text = $"帮助文本：{txt}";
                InitSelect();
            });
            var a = Task.Run(() =>
            {
                while (NetEvent == null)
                {
                    Thread.Sleep(100);
                }
                var copy = NetEvent;
                NetEvent = null;
                return copy;
            });
            return a.Result;
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
                            if (SwitchCosts.ElementAt(CharacterSelected.Index).EqualTo(selects))
                            {
                                State = ActionType.Pass;
                                NetEvent = new(new(ActionType.Switch, CharacterSelected.Index), selects);
                            }
                        }
                        if (UseCardSelected != null)
                        {
                            CardCost cost = CardCosts.ElementAt(UseCardSelected.Index);
                            var additionalTargets = cost.Targets;
                            var a = (TargetEnum e) => e switch
                            {
                                TargetEnum.Character_Me or TargetEnum.Character_Enemy => typeof(CharacterCardGrid),
                                TargetEnum.Card_Me => typeof(ActionCardGrid),
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
                                if (cost.DiceCosts.EqualTo(selects))
                                {
                                    State = ActionType.Pass;
                                    NetEvent = new(new(ActionType.UseCard, UseCardSelected.Index), selects, TargetEnumSelected.Select(t => t.Index).ToArray());
                                }
                            }
                        }
                        if (UseSkillSelected != null)
                        {
                            int[] selects = new int[8];
                            DiceSelected.ForEach(d => selects[(int)d.Element]++);
                            if (SkillCosts.ElementAt(UseSkillSelected.Index).EqualTo(selects))
                            {
                                State = ActionType.Pass;
                                NetEvent = new(new(ActionType.UseSKill, UseSkillSelected.Index), selects, TargetEnumSelected.Select(t => t.Index).ToArray());
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
                if (d != 0 && BlendCost != null && BlendCost.Costs[d] == 0)
                {
                    selects[d] = 1;
                    State = ActionType.Pass;
                    NetEvent = new(new(ActionType.Blend, UseCardSelected.Index), selects);
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
