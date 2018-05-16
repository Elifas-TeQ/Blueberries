using System;
using SystemMath = System.Math;

namespace CalcClass
{
    public static class Math
    {
        private static string _lastError = "";

        public static string lastError => _lastError;
        
        #region Public methods
        public static int Add(long a, long b)
        {
            return HandleBinaryOperation(a, b, () => a + b);
        }

        public static int Sub(long a, long b)
        {
            return HandleBinaryOperation(a, b, () => a - b);
        }

        public static int Mult(long a, long b)
        {
            return HandleBinaryOperation(a, b, () => a * b);
        }

        public static int Div(long a, long b)
        {
            var isError = CheckWhetherNumberIsZero(b);

            if (isError)
            {
                return 0;
            }

            return HandleBinaryOperation(a, b, () => a / b);
        }

        public static int Mod(long a, long b)
        {
            var isError = CheckWhetherNumberIsZero(b);

            if (isError)
            {
                return 0;
            }

            return HandleBinaryOperation(a, b, () =>
            {
                SystemMath.DivRem(a, b, out long result);

                return result;
            });
        }

        public static int ABS(long a)
        {
            return HandleUnaryOperation(a, () => SystemMath.Abs(a));
        }

        public static int IABS(long a)
        {
            return HandleUnaryOperation(a, () => -SystemMath.Abs(a));
        }
        #endregion

        #region Private methods
        private static int HandleUnaryOperation(long a, Func<long> operation)
        {
            var isError = !CheckWhetherNumberIsInIntegerRange(a, false);

            if (isError)
            {
                return 0;
            }

            var result = operation();

            var integerResult = (int)result;

            return integerResult;
        }

        private static int HandleBinaryOperation(long a, long b, Func<long> operation)
        {
            var isError = !CheckWhetherNumberIsInIntegerRange(a) || !CheckWhetherNumberIsInIntegerRange(b);

            if (isError)
            {
                return 0;
            }

            var result = operation();

            isError = !CheckWhetherNumberIsInIntegerRange(result);

            if (isError)
            {
                return 0;
            }

            var integerResult = (int)result;

            return integerResult;
        }

        private static bool CheckWhetherNumberIsInIntegerRange(long number, bool couldBeMinValue = true)
        {
            var isNotInRange = number > int.MaxValue || number < int.MinValue;

            if (!couldBeMinValue)
            {
                isNotInRange |= number <= int.MinValue;
            }
            
            if (isNotInRange)
            {
                _lastError = "Error 06 - Дуже мале, або дуже велике значення числа для int.\nЧисла повинні бути в межах від -2147483648 до 2147483647.";

                return false;
            }

            return true;
        }

        private static bool CheckWhetherNumberIsZero(long number)
        {
            if (number == 0.0)
            {
                _lastError = "Error 09 - Помилка ділення на 0.";

                return true;
            }

            return false;
        }
        #endregion
    }
}