using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Porter.Util.Fillup;

namespace Porter.Util
{
    public class LiveTile
    {
        private static List<string> Messages = new List<string>();

        private static void AddFillupMessages()
        {
            using (var db = Util.Database.Connection())
            {
                var fillups = db.Table<Fillup.Fillup>();
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
                        var stats = new FillupView(twoRecent[0], twoRecent[1]);

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

            XmlDocument squareXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText04);
            var tileImageElements = squareXml.GetElementsByTagName("image");
            ((XmlElement)tileImageElements[0]).SetAttribute("src", "ms-appx:///Assets/Logo.scale-100.png");

            XmlDocument wideXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150ImageAndText01);
            tileImageElements = wideXml.GetElementsByTagName("image");
            ((XmlElement)tileImageElements[0]).SetAttribute("src", "ms-appx:///Assets/WideLogo.scale-100.png");

            var squareTextAttributes = squareXml.GetElementsByTagName("text");
            var wideTextAttributes = wideXml.GetElementsByTagName("text");

            ((XmlElement)squareXml.GetElementsByTagName("binding")[0]).SetAttribute("branding", "none");
            ((XmlElement)wideXml.GetElementsByTagName("binding")[0]).SetAttribute("branding", "none");

            IXmlNode node = wideXml.ImportNode(squareXml.GetElementsByTagName("binding").Item(0), true);
            wideXml.GetElementsByTagName("visual").Item(0).AppendChild(node);

            foreach (string msg in Messages)
            {
                squareTextAttributes[0].InnerText = msg;
                wideTextAttributes[0].InnerText = msg;
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
