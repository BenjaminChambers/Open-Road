namespace Porter.Util.Fillup
{
    public class Fillup : BaseItem
    {
        public Fillup()
        {
            CarID = Settings.CurrentCarID;
        }

        public double Volume { get; set; }
    }
}
