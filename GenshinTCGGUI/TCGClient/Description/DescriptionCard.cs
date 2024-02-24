using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using TCGBase;

namespace TCGClient
{
    public abstract class AbstractDescriptionCard
    {
        public string CardName { get; }
        public string Description { get; }

        public AbstractDescriptionCard(string cardName, string description)
        {
            CardName = cardName;
            Description = description;
        }
    }
    public abstract class AbstractDescriptionUsable : AbstractDescriptionCard
    {
        public string CostText { get; init; }
        public List<SingleCostVariable> Cost { get; }//for json
        /// <summary>
        /// persistent namespace+nameid
        /// </summary>
        public List<string> RelatedPersistents { get; }

        public AbstractDescriptionUsable(List<SingleCostVariable>? cost, List<string>? relatedPersistents, string cardName, string description) : base(cardName, description)
        {
            Cost = cost ?? new();
            CostText = cost == null ? "无" : string.Join("", cost.Select(scv => $"{scv.Count}{scv.Type}"));
            RelatedPersistents = relatedPersistents ?? new();
        }
    }
    public class DescriptionActionCard : AbstractDescriptionUsable
    {
        public List<string> Tags { get; }
        public DescriptionActionCard(string cardName, string description, List<string>? tags, List<SingleCostVariable>? cost, List<string>? relatedPersistents) : base(cost, relatedPersistents, cardName, description)
        {
            Tags = tags ?? new();
        }
    }
    public class DescriptionSkill : AbstractDescriptionUsable
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SkillCategory SkillCategory { get; }
        public string Type { get; }//for json
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Element { get; }//for json
        public DescriptionSkill(string cardName, string description, string type, ElementCategory element = ElementCategory.Trival, List<SingleCostVariable>? cost = null, List<string>? relatedPersistents = null, SkillCategory skillcategory = SkillCategory.P) : base(cost, relatedPersistents, cardName, description)
        {
            Type = type;
            Element = element;
            switch (type)
            {
                case "Skill_A":
                    SkillCategory = SkillCategory.A;
                    CostText = $"1{element}2黑";
                    break;
                case "Skill_E":
                    SkillCategory = SkillCategory.E;
                    CostText = $"3{element}";
                    break;
                default:
                    SkillCategory = skillcategory;
                    break;
            }
        }
    }
    public class DescriptionCharacterCard : AbstractDescriptionCard
    {
        public int MaxHP { get; }
        public int MaxMP { get; }
        public List<string> Tags { get; }
        public List<DescriptionSkill> SkillList { get; }
        public DescriptionCharacterCard(int maxHP, int maxMP, List<string> tags, string cardName, string description, List<DescriptionSkill> skillList) : base(cardName, description)
        {
            MaxHP = maxHP;
            MaxMP = maxMP;
            Tags = tags;
            SkillList = skillList;
        }
    }
}
