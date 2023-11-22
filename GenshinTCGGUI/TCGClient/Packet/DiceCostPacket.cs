
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TCGBase;

namespace TCGClient
{
    public class SkillCost
    {
        public DiceCostVariable DiceCosts { get; }
        public int MP { get; }
        public SkillCost((DiceCostVariable cost, int mp) cost)
        {
            DiceCosts = cost.cost;
            MP = cost.mp;
        }
        [JsonConstructor]
        public SkillCost(DiceCostVariable diceCosts, int mP)
        {
            DiceCosts = diceCosts;
            MP = mP;
        }
    }
    public class CardCost : SkillCost
    {
        public IEnumerable<TargetEnum> Targets { get; }

        public CardCost((DiceCostVariable cost, int mp) cost, IEnumerable<TargetEnum> targets) : base(cost)
        {
            Targets = targets;
        }
        [JsonConstructor]
        public CardCost(DiceCostVariable diceCosts, int mp, IEnumerable<TargetEnum> targets) : base(diceCosts, mp)
        {
            Targets = targets;
        }
    }
    /// <summary>
    /// 发送全部显示出的行为（使用any卡牌、any技能）的花费
    /// </summary>
    public class DiceCostPacket
    {
        public IEnumerable<CardCost> CardCosts { get; }
        public IEnumerable<SkillCost> SkillCosts { get; }
        public IEnumerable<DiceCostVariable> SwitchCosts { get; }
        public DiceCostVariable? BlendCost { get; }
        public DiceCostPacket(IEnumerable<CardCost> cardCosts, IEnumerable<SkillCost> skillCosts, IEnumerable<DiceCostVariable> switchCosts, DiceCostVariable? blendCost)
        {
            CardCosts = cardCosts;
            SkillCosts = skillCosts;
            SwitchCosts = switchCosts;
            BlendCost = blendCost;
        }
    }
}
