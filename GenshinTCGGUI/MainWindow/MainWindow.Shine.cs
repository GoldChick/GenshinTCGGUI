using Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TCGBase;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace GenshinTCGGUI
{
    /// <summary>
    /// 用来展示将要打出的卡牌、双方要打出的卡牌
    /// </summary>
    public partial class MainWindow
    {
        protected enum TrivalSelectStateMachine
        {
            None,

            Character,
            Card,
            Skill
        }
        private TrivalSelectStateMachine _selectStateMachine;
        protected TrivalSelectStateMachine SelectStateMachine
        {
            get => _selectStateMachine; set
            {
                CostVariable? cost = null;
                switch (value)
                {
                    case TrivalSelectStateMachine.None:
                        {
                            if (_selectStateMachine != value)
                            {
                                BlackBlocker_DiceOnly.Visibility = Visibility.Hidden;
                                TeamMe.BlackBlocker.Visibility = Visibility.Hidden;
                                TeamEnemy.BlackBlocker.Visibility = Visibility.Hidden;
                                if (TargetEnumsToSelect.Count > TargetEnumSelected.Count)
                                {
                                    var r = GetGridRegion()[(int)TargetEnumsToSelect[TargetEnumSelected.Count]];
                                    Panel.SetZIndex(r, 0);
                                    foreach (var g in NotValidTargetStored)
                                    {
                                        g.Visibility = Visibility.Visible;
                                    }
                                }

                                CharacterSelected?.Glow(0);
                                CharacterSelected = null;
                                UseCardSelected?.Glow(0);
                                UseCardSelected = null;
                                UseSkillSelected?.Glow(0);
                                UseSkillSelected = null;
                                TargetEnumSelected.ForEach(c => c.Glow(0));
                                TargetEnumSelected.Clear();
                                TargetEnumsToSelect.Clear();
                                NotValidTargetStored.Clear();
                                RerollCardsSelected.ForEach(c => c.Glow(0));
                                RerollCardsSelected.Clear();
                                cost = new();
                            }
                        }
                        break;
                    case TrivalSelectStateMachine.Character:
                        {
                            if (CharacterSelected != null)
                            {
                                cost = SwitchCosts.ElementAt(CharacterSelected.Index);
                                CharacterSelected.Glow(1);
                            }
                        }
                        break;
                    case TrivalSelectStateMachine.Card:
                        {
                            if (UseCardSelected != null)
                            {
                                var cc = CardCosts.ElementAt(UseCardSelected.Index);
                                cost = cc.Cost;
                                NotValidTargetStored.ForEach(z => Panel.SetZIndex(z, 0));
                                NotValidTargetStored.Clear();
                                TargetEnumSelected.ForEach(c => c.Glow(0));
                                TargetEnumSelected.Clear();
                                UseCardSelected.Glow(1);

                                TargetEnumsToSelect.Clear();
                                TargetEnumsToSelect.AddRange(cc.Targets);
                                if (TargetEnumsToSelect.Count > 0)
                                {
                                    MainClient.GetCardNextValidTargets(UseCardSelected.Index, Array.Empty<int>(), GetCardNextValidTargetsCallBack);
                                }
                                else
                                {
                                    //TODO:满足条件
                                }
                            }
                        }
                        break;
                    case TrivalSelectStateMachine.Skill:
                        {
                            if (UseSkillSelected != null)
                            {
                                cost = SkillCosts.ElementAt(UseSkillSelected.Index);
                                UseSkillSelected.Glow(1);
                            }
                        }
                        break;
                }
                DiceSelected.ForEach(c => c.Glow(0));
                if (cost != null)
                {
                    DiceSelected.Clear();
                    //TODO:auto select dice
                }
                _selectStateMachine = value;
            }
        }
        /// <summary>
        /// 上一个被选中的卡牌，在屏幕上显示说明，点击其他地方取消显示
        /// </summary>
        public GamingSelectableGrid? LastSelected { get; set; }
        /// <summary>
        /// 切人使用<br/>
        /// Trival SwitchForced
        /// </summary>
        public CharacterCardGrid? CharacterSelected { get; set; }
        /// <summary>
        /// 使用卡牌使用<br/>
        /// Trival
        /// </summary>
        public ActionCardGrid? UseCardSelected { get; set; }
        /// <summary>
        /// 使用技能使用<br/>
        /// Trival
        /// </summary>
        public SkillCardGrid? UseSkillSelected { get; set; }

        /// <summary>
        /// 重投卡牌使用<br/>
        /// RerollCard
        /// </summary>
        public List<GamingSelectableGrid> RerollCardsSelected { get; set; }
        /// <summary>
        /// 提供/重投骰子<br/>
        /// Trival RerollDice
        /// </summary>
        public List<DiceGrid> DiceSelected { get; set; }
        public List<TargetEnum> TargetEnumsToSelect { get; set; }
        /// <summary>
        /// 使用卡牌时的TargetNum提供<br/>
        /// Trival
        /// </summary>
        public List<GamingSelectableGrid> TargetEnumSelected { get; set; }
        public List<GamingSelectableGrid> NotValidTargetStored { get; set; }
        public List<UniformGrid> GetGridRegion() => new()
        {
            TeamEnemy._characters,
            TeamMe._characters,
            TeamEnemy._summons,
            TeamMe._summons,
            TeamEnemy._supports,
            TeamMe._supports
        };
        public List<IEnumerable<GamingSelectableGrid>> GetGrids() => new()
        {
            TeamEnemy._characters.Cards,
            TeamMe._characters.Cards,
            TeamEnemy._summons.Cards,
            TeamMe._summons.Cards,
            TeamEnemy._supports.Cards,
            TeamMe._supports.Cards
        };
        public void GetCardNextValidTargetsCallBack(List<int> list)
        {
            var temp = GetGrids()[(int)TargetEnumsToSelect[TargetEnumSelected.Count]];
            temp = temp.Where(g => !list.Contains(g.Index));
            NotValidTargetStored.AddRange(temp);

            var r = GetGridRegion()[(int)TargetEnumsToSelect[TargetEnumSelected.Count]];
            Panel.SetZIndex(r, 1);
            foreach (var g in NotValidTargetStored)
            {
                g.Visibility = Visibility.Hidden;
            }
        }
        public void ShowWhenUsingCard()
        {
            if (TargetEnumsToSelect.Count > TargetEnumSelected.Count)
            {
                foreach (var g in NotValidTargetStored)
                {
                    g.Visibility = Visibility.Visible;
                }
                NotValidTargetStored.Clear();
                //todo: insert?
            }
            else
            {
                foreach (var g in NotValidTargetStored)
                {
                    g.Visibility = Visibility.Visible;
                }
                NotValidTargetStored.Clear();
            }

            if (TargetEnumsToSelect.Count > TargetEnumSelected.Count)
            {
                //todo insert

                //var valids = MainClient.GetCardNextValidTargets(UseCardSelected.Index, Array.Empty<int>());

            }
        }
        private void TryTrivalSelect(GamingSelectableGrid selected)
        {
            //TODO:preview action dice cost
            switch (SelectStateMachine)
            {
                case TrivalSelectStateMachine.None:
                    {
                        if (selected is CharacterCardGrid ccg)
                        {
                            BlackBlocker_DiceOnly.Visibility = Visibility.Visible;
                            CharacterSelected = ccg;
                            SelectStateMachine = TrivalSelectStateMachine.Character;
                        }
                        else if (selected is ActionCardGrid acg)
                        {
                            TeamMe.BlackBlocker.Visibility = Visibility.Visible;
                            TeamEnemy.BlackBlocker.Visibility = Visibility.Visible;
                            UseCardSelected = acg;
                            SelectStateMachine = TrivalSelectStateMachine.Card;
                        }
                        else if (selected is SkillCardGrid scg)
                        {
                            BlackBlocker_DiceOnly.Visibility = Visibility.Visible;
                            UseSkillSelected = scg;
                            SelectStateMachine = TrivalSelectStateMachine.Skill;
                        }
                    }
                    break;
                case TrivalSelectStateMachine.Character:
                    if (selected is CharacterCardGrid ccg1)
                    {
                        CharacterSelected?.Glow(0);
                        CharacterSelected = ccg1;
                        CharacterSelected.Glow(1);
                    }
                    else if (selected is DiceGrid dg)
                    {
                        if (DiceSelected.Contains(dg))
                        {
                            dg.Glow(0);
                            DiceSelected.Remove(dg);
                        }
                        else
                        {
                            dg.Glow(1);
                            DiceSelected.Add(dg);
                        }
                    }
                    break;
                case TrivalSelectStateMachine.Card:
                    if (selected is DiceGrid dg2)
                    {
                        if (DiceSelected.Contains(dg2))
                        {
                            dg2.Glow(0);
                            DiceSelected.Remove(dg2);
                        }
                        else
                        {
                            dg2.Glow(1);
                            DiceSelected.Add(dg2);
                        }
                    }
                    else if (selected is ActionCardGrid acg1)
                    {
                        //TODO:
                    }
                    else
                    {
                        TargetEnumSelected.Add(selected);
                        selected.Glow(2);
                        var r = GetGridRegion()[(int)TargetEnumsToSelect[TargetEnumSelected.Count - 1]];
                        Panel.SetZIndex(r, 0);
                        if (TargetEnumsToSelect.Count > TargetEnumSelected.Count)
                        {
                            foreach (var g in NotValidTargetStored)
                            {
                                g.Visibility = Visibility.Visible;
                            }
                            NotValidTargetStored.Clear();

                            MainClient.GetCardNextValidTargets(UseCardSelected.Index, TargetEnumSelected.Select(g => g.Index).ToArray(), GetCardNextValidTargetsCallBack);
                        }
                        else
                        {
                            foreach (var g in NotValidTargetStored)
                            {
                                g.Visibility = Visibility.Visible;
                            }
                            NotValidTargetStored.Clear();
                        }
                    }
                    break;
                case TrivalSelectStateMachine.Skill:
                    if (selected is SkillCardGrid scg1)
                    {
                        UseSkillSelected?.Glow(0);
                        UseSkillSelected = scg1;
                        SelectStateMachine = TrivalSelectStateMachine.Skill;
                    }
                    else if (selected is DiceGrid dg1)
                    {
                        if (DiceSelected.Contains(dg1))
                        {
                            dg1.Glow(0);
                            DiceSelected.Remove(dg1);
                        }
                        else
                        {
                            dg1.Glow(1);
                            DiceSelected.Add(dg1);
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 添加要选中的卡牌/骰子
        /// </summary>
        public void TrySelect(GamingSelectableGrid selected)
        {
            LastSelected = selected;
            switch (State)
            {
                case ActionType.Trival:
                    TryTrivalSelect(selected);
                    break;
                case ActionType.ReRollDice:
                    {
                        if (selected is DiceGrid dice)
                        {
                            if (DiceSelected.Contains(dice))
                            {
                                dice.Glow(0);
                                DiceSelected.Remove(dice);
                            }
                            else
                            {
                                dice.Glow(1);
                                DiceSelected.Add(dice);
                            }
                        }
                        break;
                    }
                case ActionType.ReRollCard:
                    {
                        if (selected is ActionCardGrid)
                        {
                            if (RerollCardsSelected.Contains(selected))
                            {
                                selected.Glow(0);
                                RerollCardsSelected.Remove(selected);
                            }
                            else
                            {
                                selected.Glow(1);
                                RerollCardsSelected.Add(selected);
                            }
                        }
                        break;
                    }
                case ActionType.SwitchForced:
                    {
                        if (selected is CharacterCardGrid ccg)
                        {
                            CharacterSelected?.Glow(0);
                            CharacterSelected = ccg;
                            ccg.Glow(1);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// 轮到自己行动，清空刚刚选的乱七八糟的
        /// </summary>
        public void InitSelect()
        {
            CharacterSelected?.Glow(0);
            CharacterSelected = null;
            UseCardSelected?.Glow(0);
            UseCardSelected = null;
            UseSkillSelected?.Glow(0);
            UseSkillSelected = null;
            TargetEnumSelected.ForEach(c => c.Glow(0));
            TargetEnumSelected.Clear();
            RerollCardsSelected.ForEach(c => c.Glow(0));
            RerollCardsSelected.Clear();
            DiceSelected.ForEach(c => c.Glow(0));
            DiceSelected.Clear();
        }
    }
}
