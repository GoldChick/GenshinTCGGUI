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
        public string Cost { get; }
        /// <summary>
        /// persistent namespace+nameid
        /// </summary>
        public string[] RelatedPersistents { get; }

        public AbstractDescriptionUsable(string cost, string[] relatedPersistents, string cardName, string description) : base(cardName, description)
        {
            Cost = cost;
            RelatedPersistents = relatedPersistents;
        }
    }
    public class DescriptionActionCard : AbstractDescriptionUsable
    {
        public string[] Tags { get; }
        public DescriptionActionCard(string[] tags, string cost, string[] relatedPersistents, string cardName, string description) : base(cost, relatedPersistents, cardName, description)
        {
            Tags = tags;
        }
    }
    public class DescriptionSkill : AbstractDescriptionUsable
    {
        public SkillCategory Category { get; }
        public DescriptionSkill(SkillCategory category, string cost, string[] relatedPersistents, string cardName, string description) : base(cost, relatedPersistents, cardName, description)
        {
            Category = category;
        }
    }
    public class DescriptionCharacterCard : AbstractDescriptionCard
    {
        public int MaxHP { get; }
        public int MaxMP { get; }
        public string[] Tags { get; }
        public DescriptionSkill[] Skills { get; }
        public DescriptionCharacterCard(int maxHP, int maxMP, string[] tags,DescriptionSkill[] skills, string cardName, string description) : base(cardName, description)
        {
            MaxHP = maxHP;
            MaxMP = maxMP;
            Tags = tags;
            Skills = skills;
        }
    }
}
