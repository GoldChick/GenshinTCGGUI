using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{e.NameSpace}/pattern/effect/{e.NameID}.json");
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    var converter = JsonSerializer.Deserialize<EffectTextureConverter>(json);
                    if (converter != null)
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{converter.TextureNamespace}/icon/{converter.TextureNameID}.png");
                        EffectName = converter.Name;
                        EffectText = converter.Text[Variant >= 0 ? Variant : 0];
                        Uri = File.Exists(path) ? new(path) : null;
                    }
                }
                catch (Exception)
                {
                }
            }
            Uri = Variant switch
            {
                -1 => new Uri("Resource/util/icon/weapon.png", UriKind.Relative),
                -2 => new Uri("Resource/util/icon/artifact.png", UriKind.Relative),
                -3 => new Uri("Resource/util/icon/talent.png", UriKind.Relative),
                _ => Uri
            };
        }
    }
}
