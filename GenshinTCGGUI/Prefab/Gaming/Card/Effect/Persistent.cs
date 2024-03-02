using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using TCGBase;

namespace Prefab
{
    public enum EffectIconType
    {
        Equipment,
        Effect,
    }
    public class Persistent
    {
        public Uri? Uri { get; init; }
        public EffectIconType EffectIconType { get; }
        public int AvailableTimes { get; set; }
        public string NameSpace { get; }
        public string NameID { get; }
        public string? EffectName { get; }
        public string? EffectText { get; }
        public Persistent(ReadonlyPersistent e)
        {
            NameSpace = e.NameSpace;
            NameID = e.NameID;
            AvailableTimes = e.AvailableTimes;
            switch (e.Type)
            {
                case ReadonlyPersistent.PersistentType.Weapon:
                    Uri = new Uri("Resource/util/icon/weapon.png", UriKind.Relative);
                    break;
                case ReadonlyPersistent.PersistentType.Artifact:
                    Uri = new Uri("Resource/util/icon/artifact.png", UriKind.Relative);
                    break;
                case ReadonlyPersistent.PersistentType.Talent:
                    Uri = new Uri("Resource/util/icon/talent.png", UriKind.Relative);
                    break;
                default:
                    if (TryGetEffectTextureConverter<EffectTextureConverter>(e.NameSpace, e.NameID, out var converter))
                    {
                        EffectName = converter.Name;
                        EffectText = converter.Text;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{converter.TextureNamespace}/icon/{converter.TextureNameID}.png");
                        Uri = File.Exists(path) ? new(path) : null;
                        EffectIconType = EffectIconType.Effect;
                    }
                    break;
            }

        }
        public static bool TryGetEffectTextureConverter<T>(string nameSpace, string nameid, [NotNullWhen(true)] out T? converter) where T : CardTextConverter
        {
            converter = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"mods/{nameSpace}/effect/{nameid}.json");
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    converter = JsonSerializer.Deserialize<T>(json);
                    return converter != null;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }
    }
}
