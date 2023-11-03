using Prefab;
using System.Collections.Generic;
using System.Linq;
using TCGBase;
using TCGClient;

namespace GenshinTCGGUI
{
    /// <summary>
    /// 用来更新自己手牌和技能的费用
    /// </summary>
    public partial class MainWindow
    {
        public IEnumerable<CardCost> CardCosts { get; private set; }
        public IEnumerable<DiceCostVariable> SkillCosts { get; private set; }
        public IEnumerable<DiceCostVariable> SwitchCosts { get; private set; }
        public DiceCostVariable BlendCost { get; private set; }
        /// <summary>
        /// 理想的费用更新时间：每次行动开始前
        /// </summary>
        public void ClientUpdateCosts(DiceCostPacket packet)
        {
            ClientUpdateCardCosts(packet.CardCosts);
            ClientUpdateSkillCosts(packet.SkillCosts);
            SwitchCosts = packet.SwitchCosts;
            BlendCost = packet.BlendCost;
        }
        public void ClientUpdateCardCosts(IEnumerable<CardCost> cardcosts)
        {
            Dispatcher.Invoke(() =>
            {
                CardCosts = cardcosts;
                for (int i = 0; i < CardMe.Children.Count; i++)
                {
                    ActionCardGrid acg = CardMe.Children[i] as ActionCardGrid;
                    acg.UpdateCost(cardcosts.ElementAt(i).DiceCosts);
                }
            });
        }
        public void ClientUpdateSkillCosts(IEnumerable<DiceCostVariable> skillcosts)
        {
            Dispatcher.Invoke(() =>
            {
                SkillCosts = skillcosts;
                int cnt = SkillMe.Children.Count;
                for (int i = 0; i < cnt; i++)
                {
                    SkillCardGrid scg = SkillMe.Children[i] as SkillCardGrid;
                    scg.UpdateCost(skillcosts.ElementAt(cnt - i - 1));
                }
            });
        }
    }
}
