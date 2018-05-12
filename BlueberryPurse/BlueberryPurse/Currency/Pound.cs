namespace BlueberryPurse.Currency
{
    public class Pound
    {
        private static string queensName;

        private double rate;

        public Pound(double rate)
        {
            this.rate = rate;
        }

        static Pound()
        {
            // Call service to get Queen's name.
            queensName = "Victoria III";
        }

        public string ConvertToPound(double count)
        {
            var poundsCount = count / rate;

            var poundsCountMessage = string.Format("{0} pounds of Queen {1}", poundsCount, queensName);

            return poundsCountMessage;
        }
    }
}