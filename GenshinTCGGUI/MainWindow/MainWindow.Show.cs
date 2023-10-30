using Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGBase;

namespace GenshinTCGGUI
{
    /// <summary>
    /// 用来展示将要打出的卡牌、双方要打出的卡牌
    /// </summary>
    public partial class MainWindow
    {

        /// <summary>
        /// 上一个被选中的卡牌，在屏幕上显示说明，点击其他地方取消显示
        /// </summary>
        public SelectableGrid? LastSelected { get; set; }
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
        /// 使用卡牌时的TargetNum提供<br/>
        /// Trival
        /// </summary>
        public List<SelectableGrid> TargetEnumSelected { get; set; }
        /// <summary>
        /// 重投卡牌使用<br/>
        /// RerollCard
        /// </summary>
        public List<SelectableGrid> RerollCardsSelected { get; set; }
        /// <summary>
        /// 提供/重投骰子<br/>
        /// Trival RerollDice
        /// </summary>
        public List<DiceGrid> DiceSelected { get; set; }
        /// <summary>
        /// 添加要选中的卡牌/骰子
        /// </summary>
        public void TrySelect(SelectableGrid selected)
        {
            LastSelected = selected;
            switch (State)
            {
                case ActionType.Trival:
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
                        else if (UseCardSelected != null)
                        {
                            if (selected.Equals(UseCardSelected))
                            {
                                selected.Glow(0);
                                UseCardSelected = null;
                                TargetEnumSelected.ForEach(t => t.Glow(0));
                                TargetEnumSelected.Clear();
                            }
                            else
                            {
                                if (TargetEnumSelected.Contains(selected))
                                {
                                    selected.Glow(0);
                                    TargetEnumSelected.Remove(selected);
                                }
                                else
                                {
                                    selected.Glow(2);
                                    TargetEnumSelected.Add(selected);
                                }
                            }
                        }
                        else if (selected is CharacterCardGrid ccg)
                        {
                            UseSkillSelected?.Glow(0);
                            UseSkillSelected = null;
                            if (CharacterSelected != null)
                            {
                                CharacterSelected.Glow(0);
                                if (CharacterSelected == ccg)
                                {
                                    CharacterSelected = null;
                                }
                                else
                                {
                                    CharacterSelected = ccg;
                                    ccg.Glow(1);
                                }
                            }
                            else
                            {
                                CharacterSelected = ccg;
                                ccg.Glow(1);
                            }
                        }
                        else if (selected is SkillCardGrid scg)
                        {
                            CharacterSelected?.Glow(0);
                            CharacterSelected = null;
                            if (UseSkillSelected != null)
                            {
                                UseSkillSelected.Glow(0);
                                if (UseSkillSelected == scg)
                                {
                                    UseSkillSelected = null;
                                }
                                else
                                {
                                    UseSkillSelected = scg;
                                    scg.Glow(1);
                                }
                            }else
                            {
                                UseSkillSelected = scg;
                                scg.Glow(1);
                            }
                        }
                        else if (selected is ActionCardGrid acg)
                        {
                            UseSkillSelected?.Glow(0);
                            UseSkillSelected = null;
                            CharacterSelected?.Glow(0);
                            CharacterSelected = null;
                            acg.Glow(1);
                            UseCardSelected = acg;
                        }
                        break;
                    }
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
