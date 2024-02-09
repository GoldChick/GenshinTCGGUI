using GenshinTCGGUI;
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
using TCGClient;

namespace TCGClient
{
    public class DescriptionPanel : StackPanel
    {
        public TextBlock Head;
        public TextBlock Second;
        public TextBlock Tags;
        public StackPanel Container;
        public DescriptionPanel(bool ingame = false)
        {
            Head = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Width = 400,
                Foreground = new SolidColorBrush(Colors.Azure)
            };
            Second = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Width = 400,
            };
            Tags = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Width = 400,
            };
            Container = new StackPanel()
            {
                Background = new SolidColorBrush(Colors.AntiqueWhite)
            };
            Children.Add(Head);
            Children.Add(Second);
            Children.Add(Tags);
            Children.Add(Container);
        }
        private class DescriptionBlock : StackPanel
        {
            public TextBlock Description;
            public StackPanel PersistentContainer;
            public DescriptionBlock(string description, string[] persistents, bool ingame)
            {
                Description = new TextBlock()
                {
                    Width = 400,
                    Text = description,
                    TextWrapping = TextWrapping.Wrap,
                };
                PersistentContainer = new StackPanel()
                {
                    Width = 400,
                    Background = new SolidColorBrush(Colors.Aquamarine)
                };
                foreach (var p in persistents)
                {
                    PersistentContainer.Children.Add(new DescriptionPersistentBlock(p, ingame));
                }
                Children.Add(Description);
                Children.Add(PersistentContainer);
            }
        }
        private class DescriptionSkillBlock : StackPanel
        {
            public TextBlock Head;
            public TextBlock Cost;
            public TextBlock Category;
            public TextBlock Description;
            public StackPanel PersistentContainer;
            public DescriptionSkillBlock(string head, string cost, SkillCategory category, string description, string[] persistents, bool ingame)
            {
                int width = ingame ? 260 : 400;
                Head = new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Width = 400,
                    Text = head,
                    Foreground = new SolidColorBrush(Colors.DeepPink)
                };
                Cost = new TextBlock()
                {
                    Width = 400,
                    Text = cost,
                    TextWrapping = TextWrapping.Wrap,
                };
                Category = new TextBlock()
                {
                    Width = width,
                    Text = category switch
                    {
                        SkillCategory.A => "普通攻击",
                        SkillCategory.E => "元素战技",
                        SkillCategory.Q => "元素爆发",
                        _ => "被动技能"
                    },
                    TextWrapping = TextWrapping.Wrap,
                };
                Description = new TextBlock()
                {
                    Width = width,
                    Text = description,
                    TextWrapping = TextWrapping.Wrap,
                };
                PersistentContainer = new StackPanel()
                {
                    Width = width,
                    Background = new SolidColorBrush(Colors.Aquamarine)
                };
                foreach (var p in persistents)
                {
                    PersistentContainer.Children.Add(new DescriptionPersistentBlock(p, ingame));
                }
                Children.Add(Head);
                Children.Add(Cost);
                Children.Add(Category);
                Children.Add(Description);
                Children.Add(PersistentContainer);
            }
        }
        private class DescriptionPersistentBlock : StackPanel
        {
            public TextBlock Head;
            public TextBlock Description;
            public DescriptionPersistentBlock(string persistent, bool ingame)
            {
                int width = ingame ? 220 : 380;
                Head = new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Width = width,
                    Text = "",
                    Foreground = new SolidColorBrush(Colors.DeepPink)
                };
                Description = new TextBlock()
                {
                    Width = width,
                    Text = "",
                    TextWrapping = TextWrapping.Wrap,
                };
                if (persistent.Contains(':'))
                {
                    var strs = persistent.Split(':');
                    if (strs.Length == 2)
                    {
                        if (Prefab.Persistent.TryGetEffectTextureConverter<CardTextConverter>(strs[0], strs[1], out var converter))
                        {
                            Head.Text = converter.Name;
                            Description.Text = converter.Text[0];
                        }
                    }
                }
                Children.Add(Head);
                Children.Add(Description);
            }
        }
        private void InitPanel(Panel container)
        {
            container.Visibility = Visibility.Visible;
            Head.Text = "";
            Second.Text = "";
            Tags.Text = "";
            Container.Children.Clear();
        }
        public void PreviewActionCard(string nameSpace, string nameid, Panel container, Image? img, bool ingame = false)
        {
            InitPanel(container);
            if (TryGetDescription<DescriptionActionCard>(nameSpace, nameid, 0, out var dac))
            {
                Head.Text = dac.CardName;
                Second.Text = dac.Cost;
                string tags = "";
                foreach (var s in dac.Tags)
                {
                    tags += s;
                    tags += " ";
                }
                Tags.Text = tags;
                Container.Children.Add(new DescriptionBlock(dac.Description, dac.RelatedPersistents, ingame));
            }
            if (img != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/action/{nameid}.png");
                img.Source = new BitmapImage(File.Exists(path) ? new(path) : new("Resource/minecraft/action/unknown.png", UriKind.Relative));
            }
        }
        public void PreviewCharacterCard(string nameSpace, string nameid, Panel container, Image? img, bool ingame = false)
        {
            InitPanel(container);
            if (TryGetDescription<DescriptionCharacterCard>(nameSpace, nameid, 1, out var dcc))
            {
                Head.Text = dcc.CardName;
                Second.Text = $"最大生命值：{dcc.MaxHP} 最大充能：{dcc.MaxMP}\n";
                Second.Text += dcc.Description;
                string tags = "";
                foreach (var s in dcc.Tags)
                {
                    tags += s;
                    tags += " ";
                }
                Tags.Text = tags;
                foreach (var s in dcc.Skills)
                {
                    Container.Children.Add(new DescriptionSkillBlock(s.CardName, s.Cost, s.Category, s.Description, s.RelatedPersistents, ingame));
                }
            }
            if (img != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/character/{nameid}/main.png");
                img.Source = new BitmapImage(File.Exists(path) ? new(path) : new("Resource/minecraft/action/unknown.png", UriKind.Relative));
            }
        }
        //先写在这里
        public void PreviewCard(AbstractCardBase? card, Panel container, Image? img, bool ingame = false)
        {
            if (card != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{card.Namespace}/");
                InitPanel(container);
                if (card is AbstractCardAction)
                {
                    path += $"action/{card.NameID}.png";
                    if (TryGetDescription<DescriptionActionCard>(card.Namespace, card.NameID, 0, out var dac))
                    {
                        Head.Text = dac.CardName;
                        Second.Text = dac.Cost;
                        string tags = "";
                        foreach (var s in dac.Tags)
                        {
                            tags += s;
                            tags += " ";
                        }
                        Tags.Text = tags;
                        Container.Children.Add(new DescriptionBlock(dac.Description, dac.RelatedPersistents, ingame));
                    }
                }
                else if (card is AbstractCardCharacter)
                {
                    path += $"character/{card.NameID}/main.png";
                    if (TryGetDescription<DescriptionCharacterCard>(card.Namespace, card.NameID, 1, out var dcc))
                    {
                        Head.Text = dcc.CardName;
                        Second.Text = $"最大生命值：{dcc.MaxHP} 最大充能：{dcc.MaxMP}\n";
                        Second.Text += dcc.Description;
                        string tags = "";
                        foreach (var s in dcc.Tags)
                        {
                            tags += s;
                            tags += " ";
                        }
                        Tags.Text = tags;
                        foreach (var s in dcc.Skills)
                        {
                            Container.Children.Add(new DescriptionSkillBlock(s.CardName, s.Cost, s.Category, s.Description, s.RelatedPersistents, ingame));
                        }
                    }
                }
                if (img != null)
                {
                    img.Source = new BitmapImage(File.Exists(path) ? new(path) : new("Resource/minecraft/action/unknown.png", UriKind.Relative));
                }
            }
        }
        /// <summary>
        /// mode=0: action<br/>
        /// mode=1: character<br/>
        /// mode=2: persistent
        /// </summary>
        public static bool TryGetDescription<T>(string nameSpace, string nameid, int mode, [NotNullWhen(true)] out T? value) where T : AbstractDescriptionCard
        {
            value = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/{nameSpace}/pattern/{mode switch
            {
                1 => "character",
                2 => "persistent",
                _ => "action"
            }}/{nameid}.json");
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    value = JsonSerializer.Deserialize<T>(json);
                    return value != null;
                }
                catch (Exception)
                {
                }
            }
            return false;
        }
    }
}
