using GenshinTCGGUI;
using System.IO;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using TCGBase;
namespace Prefab
{
    public enum RegistryType
    {
        CharacterCard,
        ActionCard
    }
    public class PreGamingSelectableGrid : Grid
    {
        public Image MainImage;
        public int Index { get; }
        public string NameSpace { get; }
        public string NameID { get; }
        public AbstractCardBase? Card { get; }
        public PreGamingSelectableGrid(AbstractCardCharacter c, int index) : this(RegistryType.CharacterCard, c.Namespace, c.NameID, index)
        {
            Card = c;
        }
        public PreGamingSelectableGrid(AbstractCardAction c, int index) : this(RegistryType.ActionCard, c.Namespace, c.NameID, index)
        {
            Card = c;
        }
        public PreGamingSelectableGrid(RegistryType type, string nameSpace, string nameid, int index, int gridtype = -1)
        {
            NameSpace = nameSpace;
            NameID = nameid;
            Index = index;

            Uri url = new("Resource/minecraft/action/unknown.png", UriKind.Relative);
            string path = "assets/path.png";
            switch (type)
            {
                case RegistryType.CharacterCard:
                    path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/character/{nameid}/main.png");
                    break;
                case RegistryType.ActionCard:
                    path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/action/{nameid}.png");
                    break;
            }
            if (File.Exists(path))
            {
                url = new(path);
            }
            MainImage = new()
            {
                Source = new BitmapImage(url),
                Height = gridtype == -1 ? 160 : double.NaN,
                Width = gridtype == -1 ? 84 : double.NaN
            };
            Children.Add(MainImage);
            if (gridtype>=-1)
            {
                MouseLeftButtonDown += (s, e) =>
                {
                    SelectCard.Instance.Select(type, nameSpace, nameid, index);
                };
            }
        }
    }
}