namespace Porter.Util.Fillup
{
    public class Fillup : BaseItem
    {
        public Fillup()
        {
            CarID = Settings.CurrentCar;
        }

        public double Volume { get; set; }
    }
}
