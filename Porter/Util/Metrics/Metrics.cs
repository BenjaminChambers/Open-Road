using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porter.Util
{
    public abstract class Metrics
    {
        static TelemetryClient _client=null;
        public static TelemetryClient Client
        {
            get
            {
                if (_client == null)
                    _client = new TelemetryClient();

                return _client;
            }
        }

        public static void TrackFillup() { Client.TrackEvent("Fill-up"); }
        public static void TrackMaintenance() { Client.TrackEvent("Maintenance"); }
    }
}
