using SystemMath = System.Math;

namespace CalcClass
{
    public static class Math
    {
        private static string _lastError = "";

        public static string lastError => _lastError;
        
        #region Methods
        public static int Add(long a, long b)
        {
            var result = a + b;

            var integerResult = (int)result;

            return integerResult;
        }

        public static int Sub(long a, long b)
        {
            return Add(a, -b);
        }

        public static int Mult(long a, long b)
        {
            var result = a * b;

            var integerResult = (int)result;

            return integerResult;
        }

        public static int Div(long a, long b)
        {
            var result = a / b;

            var integerResult = (int)result;

            return integerResult;
        }

        public static int Mod(long a, long b)
        {
            SystemMath.DivRem(a, b, out long result);

            var integerResult = (int)result;

            return integerResult;
        }

        public static int ABS(long a)
        {
            var result = SystemMath.Abs(a);

            var integerResult = (int)result;

            return integerResult;
        }

        public static int IABS(long a)
        {
            return -ABS(a);
        }
        #endregion
    }
}