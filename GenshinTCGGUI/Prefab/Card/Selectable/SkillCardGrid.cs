using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System;
using System.Linq;
using static Prefab.ActionCardGrid;
using TCGBase;

namespace Prefab
{
    public class SkillCardGrid : SelectableGrid
    {
        public StackPanel CostContainer { get; }
        public SkillCardGrid(string character_nameid, int index) : base(index)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"assets/Genshin3_3/character/{character_nameid}/{index}.png");
            MainImage = new()
            {
                Source = new BitmapImage(File.Exists(path) ? new(path) : new("null", UriKind.Relative)),
            };
            CostContainer = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 64, 0, -26),
                Orientation = Orientation.Horizontal
            };

            Children.Add(MainImage);
            Children.Add(CostContainer);
        }
        public void UpdateCost(DiceCostVariable dcv) => UpdateCost(dcv.CostSame, dcv.Costs);
        public void UpdateCost(bool sameDice, int[] cost)
        {
            CostContainer.Children.Clear();
            cost.Select((x, element) => (element, x)).Where(p => p.x > 0)
                    .Select(p => new ActionCardCost(sameDice, p.element, p.x)).ToList()
                    .ForEach(c => CostContainer.Children.Add(c));
        }
        //        <Grid>
        //    <Image Source = "e.png" ></ Image >
        //    < StackPanel Margin="0,64,0,-26" Orientation="Horizontal" HorizontalAlignment="Center">
        //        <Grid>
        //            <Image Source = "Resource/Util/Cost/Trival.png" Height="23" Width="27"/>
        //            <TextBlock HorizontalAlignment = "Center" VerticalAlignment="Center" FontFamily="Arial Black" FontSize="14">0</TextBlock>
        //        </Grid>
        //        </StackPanel>
        //</Grid>
    }
}
