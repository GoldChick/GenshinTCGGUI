using System.Collections.Generic;
using System.Text.Json.Serialization;
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
    /// <summary>
    /// 支援、召唤右上角显示的数字，默认为可用次数
    /// </summary>
    public enum EffectNumberType
    {
        AvailableTime,
        DataCount
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
        public EffectTextureConverter(string textureNamespace, string textureNameID, string name, string text, EffectShowCategory showCategory, List<string>? tags) : base(name, text, tags)
        {
            TextureNamespace = textureNamespace;
            TextureNameID = textureNameID;
            ShowCategory = showCategory;
        }
    }
    public record SideTextureConverter : CardTextConverter
    {
        public EffectShowCategory ShowCategory { get; }
        public int Damage { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Element { get; }
        public SideTextureConverter(string name, string text, EffectShowCategory showCategory, int damage, ElementCategory element, List<string>? tags) : base(name, text, tags)
        {
            Damage = damage;
            Element = element;
            ShowCategory = showCategory;
        }
    }
}
