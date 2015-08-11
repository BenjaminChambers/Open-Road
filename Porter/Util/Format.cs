using System;

namespace Porter.Util
{
    public class Format
    {
        public static string Gallons(double value) { return value.ToString("#0.###gal"); }
        public static string Currency(double value) { return value.ToString("C"); }
        public static string Miles(double value) { return value.ToString("#0") + "mi"; }
        public static string MPG(double miles, double gallons) { return (miles / gallons).ToString("#0.###") + "mpg"; }
        public static string GPM(double miles, double gallons) { return (100.0 * gallons / miles).ToString("#0.###") + "g/100mi"; }
        public static string Date(DateTime value) { return value.ToString("MMM dd, yyyy"); }
    }
}