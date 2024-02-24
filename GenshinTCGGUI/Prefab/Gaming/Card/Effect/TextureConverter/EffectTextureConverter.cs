using Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGBase;

namespace Prefab
{
    /// <summary>
    /// 对于Effect，不为None就会显示使用次数<br/>
    /// 对于Summon或者Support，不为None就会在右上角显示[可用次数]
    /// </summary>
    public enum EffectShowCategory
    {
        None,
        Simple,
        HourGlass,
        Shield,
        Timer
    }
    public enum EffectTriggerCategory
    {
        Shine,
        Skill
    }
    public record CardTextConverter
    {
        public string Name { get; }
        public string Text { get; }
        public List<string> Tags { get; }
        public CardTextConverter(string name, string? text, List<string>? tags)
        {
            Name = name;
            Text = text ?? "text to build";
            Tags = tags ?? new();
        }
    }
    public record EffectTextureConverter : CardTextConverter
    {
        public string TextureNamespace { get; }
        public string TextureNameID { get; }
        public EffectShowCategory ShowCategory { get; }
        public EffectTriggerCategory TriggerCategory { get; }

        public EffectTextureConverter(string textureNamespace, string textureNameID, string name, string text, EffectShowCategory showCategory, EffectTriggerCategory triggerCategory, List<string>? tags) : base(name, text, tags)
        {
            TextureNamespace = textureNamespace;
            TextureNameID = textureNameID;
            ShowCategory = showCategory;
            TriggerCategory = triggerCategory;
        }
    }
    public record SideTextureConverter : CardTextConverter
    {
        public EffectShowCategory ShowCategory { get; }
        public EffectTriggerCategory TriggerCategory { get; }
        public int Damage { get; }
        public ElementCategory[] Element { get; }
        public SideTextureConverter(string name, string text, EffectShowCategory showCategory, EffectTriggerCategory triggerCategory, int damage, ElementCategory[] element, List<string>? tags) : base(name, text, tags)
        {
            Damage = damage;
            Element = element ?? new ElementCategory[] { ElementCategory.Cryo };
            ShowCategory = showCategory;
            TriggerCategory = triggerCategory;
        }
    }
}
