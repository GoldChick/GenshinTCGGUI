
using System.Collections.Generic;
using TCGBase;

namespace TCGClient
{
    public class CardCost
    {
        public DiceCostVariable DiceCosts { get; }
        public IEnumerable<TargetEnum> Targets { get; }

        public CardCost(DiceCostVariable diceCosts, IEnumerable<TargetEnum> targets)
        {
            DiceCosts = diceCosts;
            Targets = targets;
        }
    }
    /// <summary>
    /// 发送全部显示出的行为（使用any卡牌、any技能）的花费
    /// </summary>
    public class DiceCostPacket
    {
        public IEnumerable<CardCost> CardCosts { get; }
        public IEnumerable<DiceCostVariable> SkillCosts { get; }
        public IEnumerable<DiceCostVariable> SwitchCosts { get; }
        public DiceCostVariable? BlendCost { get; }
        public DiceCostPacket(IEnumerable<CardCost> cardCosts, IEnumerable<DiceCostVariable> skillCosts, IEnumerable<DiceCostVariable> switchCosts, DiceCostVariable? blendCost)
        {
            CardCosts = cardCosts;
            SkillCosts = skillCosts;
            SwitchCosts = switchCosts;
            BlendCost = blendCost;
        }
    }
}
