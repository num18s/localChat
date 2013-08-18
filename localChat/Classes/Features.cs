using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using localChat.Resources;

namespace localChat
{
    public class Features
    {
        public class Tile
        {
            public static bool TileExists(string NavSource)
            {
                ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(NavSource));
                return tile == null ? false : true;
            }

            public static void DeleteTile(string NavSource)
            {
                ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(NavSource));
                if (tile == null) return;

                tile.Delete();
            }

            public static void SetTile(MessageItem item, string NavSource)
            {
                FlipTileData tileData = new FlipTileData()
                {
                    //Front square data
                    Title = item.Title + " by " + item.Author,
                    BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                    SmallBackgroundImage = new Uri("/Assets/Tiles/IconicTileSmall.png", UriKind.Relative),

                    //Back square data
                    BackTitle = "On " + item.Date + " " + item.Time,
                    BackContent = item.Msg,
                    BackBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),

                    //Wide tile data
                    WideBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative),
                    WideBackBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative),
                    WideBackContent = item.Msg
                };

                ShellTile.Create(new Uri(NavSource, UriKind.Relative), tileData, true);
            }
        }
    }
}
