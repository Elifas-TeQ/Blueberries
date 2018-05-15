namespace BlueberryPurse.Currency
{
    public class Dollar
    {
        public double Rate
        {
            get;
            private set;
        }

        public Dollar()
            : this(1.0)
        {
        }

        public Dollar(double rate)
        {
            Rate = rate;
        }

        public double ConvertDollars(double dollarsAmount)
        {
            var amount = Rate * dollarsAmount;

            return amount;
        }

        public double ConvertDollars(double dollarsAmount, double exchangeRate)
        {
            var amount = dollarsAmount * exchangeRate;

            return amount;
        }

        public double ConvertToDollars(double amount)
        {
            var dollarsAmount = amount / Rate;

            return dollarsAmount;
        }

        public double ConvertToDollars(double amount, double exchangeRate)
        {
            var dollarsAmount = amount / exchangeRate;

            return dollarsAmount;
        }
    }
}