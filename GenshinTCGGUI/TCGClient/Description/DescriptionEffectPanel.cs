using Prefab;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TCGBase;

namespace TCGClient
{
    public class DescriptionEffectPanel : StackPanel
    {
        public TextBlock Personal;
        public DescriptionEffectContainer CharacterContainer;
        public TextBlock Team;
        public DescriptionEffectContainer TeamContainer;
        public DescriptionEffectPanel()
        {
            Personal = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Width = 400,
                Text = "角色状态",
                Foreground = new SolidColorBrush(Colors.Azure)
            };
            CharacterContainer = new DescriptionEffectContainer();
            Team = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Text = "出战状态",
                Width = 400,
            };
            TeamContainer = new DescriptionEffectContainer();
            Children.Add(Personal);
            Children.Add(CharacterContainer);
            Children.Add(Team);
            Children.Add(TeamContainer);
        }
        public class DescriptionEffectContainer : StackPanel
        {
            public DescriptionEffectContainer()
            {
                Background = new SolidColorBrush(Colors.AntiqueWhite);
            }
            public void Update(List<CharacterEffectGrid>? effects)
            {
                Children.Clear();
                effects?.ForEach(e =>
                {
                    if (e.Persistent != null)
                    {
                        if (e.Persistent.EffectName!=null)
                        {
                            if (e.Uri != null)
                            {
                                //TODO:图
                            }
                            Children.Add(new DescriptionPersistentBlock(e.Persistent, e.Persistent.AvailableTimes));
                        }
                    }
                });
            }
            public void InitPanel(Panel container)
            {
                container.Visibility = Visibility.Visible;
                Children.Clear();
            }
            private class DescriptionPersistentBlock : StackPanel
            {
                public TextBlock Head;
                public TextBlock Curr;
                public TextBlock Description;
                public DescriptionPersistentBlock(Prefab.Persistent p, int av)
                {
                    int width = 220;
                    Head = new TextBlock()
                    {
                        TextWrapping = TextWrapping.Wrap,
                        Width = width,
                        Text = p.EffectName,
                        Foreground = new SolidColorBrush(Colors.DeepPink)
                    };
                    Curr = new TextBlock()
                    {
                        Width = width,
                        Text = $"次数：{av}",
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = new SolidColorBrush(Colors.Cyan)
                    };
                    Description = new TextBlock()
                    {
                        Width = width,
                        Text = p.EffectText,
                        TextWrapping = TextWrapping.Wrap,
                    };
                    Children.Add(Head);
                    Children.Add(Curr);
                    Children.Add(Description);
                }
            }

        }

        public void Preview(List<CharacterEffectGrid>? character, List<CharacterEffectGrid>? team)
        {
            CharacterContainer.Update(character);
            TeamContainer.Update(team);
        }
        //public static bool TryGetDescription<T>(string nameSpace, string nameid, int mode, [NotNullWhen(true)] out T? value) where T : AbstractDescriptionCard
        //{
        //    value = null;
        //    string path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/pattern/{mode switch
        //    {
        //        1 => "character",
        //        2 => "persistent",
        //        _ => "action"
        //    }}/{nameid}.json");
        //    if (File.Exists(path))
        //    {
        //        try
        //        {
        //            var json = File.ReadAllText(path);
        //            value = JsonSerializer.Deserialize<T>(json);
        //            return value != null;
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    return false;
        //}
    }
}
