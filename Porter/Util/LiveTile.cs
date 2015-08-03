using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Porter.Util
{
    public class LiveTile
    {
        private static List<string> Messages = new List<string>();

        private static void AddFillupMessages()
        {
            using (var db = Util.Database.Connection())
            {
                var fillups = db.Table<Util.Models.Fillup>();
                switch (fillups.Count())
                {
                    case 0:
                        Messages.Add("No fill-up data to show.");
                        break;
                    case 1:
                        Messages.Add("You've filled your tank once. Do it again for stats!");
                        break;
                    default:
                        var twoRecent = fillups.OrderByDescending(item => item.Odometer).Take(2).ToList();
                        var stats = new Util.ViewModels.FillupView(twoRecent[0], twoRecent[1]);

                        Messages.Add("You drove " + stats.Miles + " on your last tank of gas.");
                        Messages.Add("You got " + stats.Efficiency + " on your last tank of gas.");
                        break;
                }
            }
        }

        private static void CreateTile()
        {
            var TileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            TileUpdater.Clear();
            TileUpdater.EnableNotificationQueue(true);

            XmlDocument wideXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150ImageAndText01);
            var tileImageElements = wideXml.GetElementsByTagName("image");
            ((XmlElement)tileImageElements[0]).SetAttribute("src", "ms-appx:///Assets/WideLogo.scale-100.png");

            var tileTextAttributes = wideXml.GetElementsByTagName("text");

            var bind = wideXml.GetElementsByTagName("binding");
            ((XmlElement)bind[0]).SetAttribute("branding", "none");

            foreach (string msg in Messages)
            {
                tileTextAttributes[0].InnerText = msg;
                TileNotification notification = new TileNotification(wideXml);
                TileUpdater.Update(notification);
            }
        }

        public static void Render()
        {
            Messages.Clear();
            AddFillupMessages();
            CreateTile();
        }
    }
}
