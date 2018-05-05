namespace BlueberryPurse.Currency
{
    public class Dollar
    {
        public double ExchangeRate
        {
            get;
            private set;
        }

        public Dollar()
            : this(1.0)
        {
        }

        public Dollar(double exchangeRate)
        {
            ExchangeRate = exchangeRate;
        }

        public double ConvertDollars(double dollarsAmount)
        {
            var amount = dollarsAmount * ExchangeRate;

            return amount;
        }

        public double ConvertDollars(double dollarsAmount, double exchangeRate)
        {
            var amount = dollarsAmount * exchangeRate;

            return amount;
        }

        public double ConvertToDollars(double amount)
        {
            var dollarsAmount = amount / ExchangeRate;

            return dollarsAmount;
        }

        public double ConvertToDollars(double amount, double exchangeRate)
        {
            var dollarsAmount = amount / exchangeRate;

            return dollarsAmount;
        }
    }
}