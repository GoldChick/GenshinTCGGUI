using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using TCGBase;

namespace Prefab
{

    public class Persistent
    {
        public Uri? Uri { get; init; }
        public int Variant { get; }
        public int AvailableTimes { get; set; }
        public string NameSpace { get; }
        public string NameID { get; }
        public string? EffectName { get; }
        public string? EffectText { get; }
        public Persistent(ReadonlyPersistent e)
        {
            NameSpace = e.NameSpace;
            NameID = e.NameID;
            Variant = e.Variant;
            AvailableTimes = e.AvailableTimes;
            if (TryGetEffectTextureConverter<EffectTextureConverter>(e.NameSpace, e.NameID, out var converter))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{converter.TextureNamespace}/icon/{converter.TextureNameID}.png");
                EffectName = converter.Name;
                EffectText = converter.Text[Variant >= 0 ? Variant : 0];
                Uri = File.Exists(path) ? new(path) : null;
            }
            Uri = Variant switch
            {
                -1 => new Uri("Resource/util/icon/weapon.png", UriKind.Relative),
                -2 => new Uri("Resource/util/icon/artifact.png", UriKind.Relative),
                -3 => new Uri("Resource/util/icon/talent.png", UriKind.Relative),
                _ => Uri
            };
        }
        public static bool TryGetEffectTextureConverter<T>(string nameSpace, string nameid, [NotNullWhen(true)] out T? converter) where T : CardTextConverter
        {
            converter = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/pattern/persistent/{nameid}.json");
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
