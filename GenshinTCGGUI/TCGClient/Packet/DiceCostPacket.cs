
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TCGBase;

namespace TCGClient
{
    public class CardCost 
    {
        public CostVariable Cost { get; }
        public IEnumerable<TargetEnum> Targets { get; }

        [JsonConstructor]
        public CardCost(CostVariable cost, IEnumerable<TargetEnum> targets) 
        {
            Cost = cost;
            Targets = targets;
        }
    }
    /// <summary>
    /// 发送全部显示出的行为（使用any卡牌、any技能）的花费
    /// </summary>
    public class DiceCostPacket
    {
        public IEnumerable<CardCost> CardCosts { get; }
        public IEnumerable<CostVariable> SkillCosts { get; }
        public IEnumerable<CostVariable> SwitchCosts { get; }
        public CostVariable? BlendCost { get; }
        public DiceCostPacket(IEnumerable<CardCost> cardCosts, IEnumerable<CostVariable> skillCosts, IEnumerable<CostVariable> switchCosts, CostVariable? blendCost)
        {
            CardCosts = cardCosts;
            SkillCosts = skillCosts;
            SwitchCosts = switchCosts;
            BlendCost = blendCost;
        }
    }
}
