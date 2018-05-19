using System;
using System.Collections;
using System.Text;

namespace AnalaizerClass
{
    public class Brain
    {
        private static string buffer = string.Empty;

        private static int erposition = 0;

        public static string expression = "";

        public static bool ShowMessage = true;

        #region Public methods
        public static bool CheckCurrency()
        {
            var bracketsAccordanceCounter = 0;

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    bracketsAccordanceCounter++;

                    if (bracketsAccordanceCounter > 3)
                    {
                        HandleWrongStructureInBracketsError(i);

                        return false;
                    }
                }
                else
                if (expression[i] == ')')
                {
                    if (bracketsAccordanceCounter > 0)
                    {
                        bracketsAccordanceCounter--;
                    }
                    else
                    {
                        HandleWrongSyntacticConstructionError(i);

                        return false;
                    }
                }
            }

            if (bracketsAccordanceCounter != 0)
            {
                HandleIncompleteExpression();

                return false;
            }

            return true;
        }
        
        public static string Format()
        {
            var outputBuilder = new StringBuilder();

            var lastReadSymbolType = SymbolType.Null;

            var lastWrittenSymbolType = SymbolType.Null;

            var isLastSpaceWritten = false;

            for (int i = 0; i < buffer.Length; i++)
            {
                var isError = false;

                lastReadSymbolType = RecogniseSymbolType(buffer, i);

                switch (lastReadSymbolType)
                {
                    case SymbolType.NotRecognised:
                        HandleUnknownOperatorError(i);

                        isError = true;

                        break;


                    case SymbolType.Number:
                        ProcessNumberDetected(buffer, i, outputBuilder,
                            ref lastWrittenSymbolType, ref isLastSpaceWritten, ref isError);

                        break;


                    case SymbolType.UnaryOperator:
                        ProcessUnaryOperatorDetected(buffer, i, outputBuilder,
                            ref lastWrittenSymbolType, ref isLastSpaceWritten, ref isError);

                        break;


                    case SymbolType.BinaryOperator:
                        ProcessBinaryOperatorDetected(buffer, i, outputBuilder,
                            ref lastWrittenSymbolType, ref isLastSpaceWritten, ref isError);

                        break;


                    case SymbolType.ModOperator:
                        ProcessModOperatorDetected(buffer, i, outputBuilder,
                            ref lastWrittenSymbolType, ref isLastSpaceWritten, ref isError);

                        // After incrementation in for-loop, 'mod' must be behind index 'i'.
                        i += 2;

                        break;


                    case SymbolType.OpeningBracket:
                        ProcessOpeningBracketDetected(buffer, i, outputBuilder,
                            ref lastWrittenSymbolType, ref isLastSpaceWritten, ref isError);

                        break;


                    case SymbolType.ClosingBracket:
                        ProcessClosingBracketDetected(buffer, i, outputBuilder,
                            ref lastWrittenSymbolType, ref isLastSpaceWritten, ref isError);

                        break;


                    case SymbolType.Space:
                        ProcessSpaceDetected(buffer, i, outputBuilder,
                            lastWrittenSymbolType, ref isLastSpaceWritten);

                        break;
                }
                
                if (isError)
                {
                    return string.Format("&{0}", buffer);
                }
            }

            if (lastWrittenSymbolType == SymbolType.UnaryOperator
                || lastWrittenSymbolType == SymbolType.BinaryOperator
                || lastWrittenSymbolType == SymbolType.ModOperator)
            {
                HandleIncompleteExpression();

                return string.Format("&{0}", buffer);
            }

            buffer = outputBuilder.ToString().TrimEnd();

            return buffer;
        }
        
        public static ArrayList CreateStack()
        {
            //TODO: implement.
            return new ArrayList();
        }

        public static string RunEstimate()
        {
            //TODO: implement.
            return string.Empty;
        }

        public static string Estimate()
        {
            //TODO: implement it in the order below.

            var isValid = CheckCurrency();

            if (!isValid)
            {
                // Error.
                return buffer;
            }

            var result = Format();

            if (result[0] == '&')
            {
                // Error.
                return buffer;
            }

            var stack = CreateStack();

            var estimatedResult = RunEstimate();

            return string.Empty;
        }
        #endregion
        
        #region Private methods
        private static SymbolType RecogniseSymbolType(string buffer, int index)
        {
            if (char.IsDigit(buffer[index]))
            {
                return SymbolType.Number;
            }
            else
            if (IsBinaryOperator(buffer[index]))
            {
                return SymbolType.BinaryOperator;
            }
            else
            if (IsModOperator(buffer, index))
            {
                return SymbolType.ModOperator;
            }
            else
            if (IsUnaryOperator(buffer[index]))
            {
                return SymbolType.UnaryOperator;
            }
            else
            if (buffer[index] == '(')
            {
                return SymbolType.OpeningBracket;
            }
            else
            if (buffer[index] == ')')
            {
                return SymbolType.ClosingBracket;
            }
            else
            if (buffer[index] == ' ')
            {
                return SymbolType.Space;
            }

            return SymbolType.NotRecognised;
        }

        private static bool IsBinaryOperator(char symbol)
        {
            var isBinaryOperator = symbol == '+'
                || symbol == '-'
                || symbol == '*'
                || symbol == '/';

            return isBinaryOperator;
        }

        private static bool IsModOperator(string symbols, int index)
        {
            var isModOperator = symbols.Length > index + 2
                && buffer.Substring(index, 3) == "mod";

            return isModOperator;
        }

        /// <summary>
        /// Must be called after method 'IsModOperator'.
        /// In other cases, there is could be wrong return value.
        /// Operators 'm' and 'mod' has the same first symbol.
        /// </summary>
        private static bool IsUnaryOperator(char symbol)
        {
            var isUnaryOperator = symbol == 'p'
                || symbol == 'm';

            return isUnaryOperator;
        }

        #region Symbol processors
        private static void ProcessNumberDetected(string buffer, int index, StringBuilder outputBuilder,
            ref SymbolType lastWrittenSymbolType, ref bool isLastSpaceWritten, ref bool isError)
        {
            switch (lastWrittenSymbolType)
            {
                case SymbolType.Null:
                    outputBuilder.Append(buffer[index]);

                    break;


                case SymbolType.Number:
                    if (isLastSpaceWritten)
                    {
                        HandleWrongSyntacticConstructionError(index);

                        isError = true;

                        return;
                    }
                    else
                    {
                        outputBuilder.Append(buffer[index]);

                        break;
                    }


                case SymbolType.UnaryOperator:
                case SymbolType.BinaryOperator:
                case SymbolType.ModOperator:
                case SymbolType.OpeningBracket:
                case SymbolType.ClosingBracket:
                    if (isLastSpaceWritten)
                    {
                        outputBuilder.Append(buffer[index]);

                        isLastSpaceWritten = false;
                    }
                    else
                    {
                        outputBuilder.Append($" {buffer[index]}");
                    }

                    break;
            }

            lastWrittenSymbolType = SymbolType.Number;
        }

        private static void ProcessUnaryOperatorDetected(string buffer, int index, StringBuilder outputBuilder,
            ref SymbolType lastWrittenSymbolType, ref bool isLastSpaceWritten, ref bool isError)
        {
            switch (lastWrittenSymbolType)
            {
                case SymbolType.Null:
                    outputBuilder.Append(buffer[index]);

                    break;


                case SymbolType.Number:
                case SymbolType.UnaryOperator:
                case SymbolType.BinaryOperator:
                case SymbolType.ModOperator:
                case SymbolType.OpeningBracket:
                    if (isLastSpaceWritten)
                    {
                        outputBuilder.Append(buffer[index]);

                        isLastSpaceWritten = false;
                    }
                    else
                    {
                        outputBuilder.Append($" {buffer[index]}");
                    }

                    break;


                case SymbolType.ClosingBracket:
                    HandleWrongStructureInBracketsError(index);

                    isError = true;

                    return;
            }

            lastWrittenSymbolType = SymbolType.UnaryOperator;
        }

        private static void ProcessBinaryOperatorDetected(string buffer, int index, StringBuilder outputBuilder,
            ref SymbolType lastWrittenSymbolType, ref bool isLastSpaceWritten, ref bool isError)
        {
            switch (lastWrittenSymbolType)
            {
                case SymbolType.Null:
                    HandleWrongSyntacticConstructionError(index);

                    isError = true;

                    return;


                case SymbolType.Number:
                case SymbolType.ClosingBracket:
                    if (isLastSpaceWritten)
                    {
                        outputBuilder.Append(buffer[index]);

                        isLastSpaceWritten = false;
                    }
                    else
                    {
                        outputBuilder.Append($" {buffer[index]}");
                    }

                    break;


                case SymbolType.UnaryOperator:
                case SymbolType.BinaryOperator:
                case SymbolType.ModOperator:
                    HandleTwoConsecutiveOperatorsError(index);

                    isError = true;

                    return;


                case SymbolType.OpeningBracket:
                    HandleWrongStructureInBracketsError(index);

                    isError = true;

                    return;
            }

            lastWrittenSymbolType = SymbolType.BinaryOperator;
        }

        private static void ProcessModOperatorDetected(string buffer, int index, StringBuilder outputBuilder,
            ref SymbolType lastWrittenSymbolType, ref bool isLastSpaceWritten, ref bool isError)
        {
            switch (lastWrittenSymbolType)
            {
                case SymbolType.Null:
                    HandleWrongSyntacticConstructionError(index);

                    isError = true;

                    return;


                case SymbolType.Number:
                case SymbolType.ClosingBracket:
                    if (isLastSpaceWritten)
                    {
                        outputBuilder.Append(buffer[index]);

                        isLastSpaceWritten = false;
                    }
                    else
                    {
                        outputBuilder.Append($" {buffer[index]}");
                    }

                    break;


                case SymbolType.UnaryOperator:
                case SymbolType.BinaryOperator:
                case SymbolType.ModOperator:
                    HandleTwoConsecutiveOperatorsError(index);

                    isError = true;

                    return;


                case SymbolType.OpeningBracket:
                    HandleWrongStructureInBracketsError(index);

                    isError = true;

                    return;
            }

            lastWrittenSymbolType = SymbolType.ModOperator;
        }

        private static void ProcessOpeningBracketDetected(string buffer, int index, StringBuilder outputBuilder,
            ref SymbolType lastWrittenSymbolType, ref bool isLastSpaceWritten, ref bool isError)
        {
            switch (lastWrittenSymbolType)
            {
                case SymbolType.Null:
                    outputBuilder.Append(buffer[index]);

                    break;


                case SymbolType.Number:
                case SymbolType.UnaryOperator:
                case SymbolType.ClosingBracket:
                    HandleWrongSyntacticConstructionError(index);

                    isError = true;

                    return;


                case SymbolType.BinaryOperator:
                case SymbolType.ModOperator:
                case SymbolType.OpeningBracket:
                    if (isLastSpaceWritten)
                    {
                        outputBuilder.Append(buffer[index]);

                        isLastSpaceWritten = false;
                    }
                    else
                    {
                        outputBuilder.Append($" {buffer[index]}");
                    }

                    break;
            }

            lastWrittenSymbolType = SymbolType.OpeningBracket;
        }

        private static void ProcessClosingBracketDetected(string buffer, int index, StringBuilder outputBuilder,
            ref SymbolType lastWrittenSymbolType, ref bool isLastSpaceWritten, ref bool isError)
        {
            switch (lastWrittenSymbolType)
            {
                case SymbolType.Number:
                case SymbolType.ClosingBracket:
                    if (isLastSpaceWritten)
                    {
                        outputBuilder.Append(buffer[index]);

                        isLastSpaceWritten = false;
                    }
                    else
                    {
                        outputBuilder.Append($" {buffer[index]}");
                    }

                    break;


                case SymbolType.UnaryOperator:
                case SymbolType.BinaryOperator:
                case SymbolType.ModOperator:
                case SymbolType.OpeningBracket:
                    HandleWrongStructureInBracketsError(index);

                    isError = true;

                    return;
            }

            lastWrittenSymbolType = SymbolType.ClosingBracket;
        }

        private static void ProcessSpaceDetected(string buffer, int index, StringBuilder outputBuilder,
            SymbolType lastWrittenSymbolType, ref bool isLastSpaceWritten)
        {
            if (isLastSpaceWritten)
            {
                return;
            }

            switch (lastWrittenSymbolType)
            {
                case SymbolType.Null:
                    return;

                case SymbolType.Number:
                case SymbolType.UnaryOperator:
                case SymbolType.BinaryOperator:
                case SymbolType.ModOperator:
                case SymbolType.OpeningBracket:
                case SymbolType.ClosingBracket:
                    outputBuilder.Append(buffer[index]);

                    break;
            }

            isLastSpaceWritten = true;
        }
        #endregion

        #region Error handlers
        /// <summary>
        /// Error #1.
        /// </summary>
        private static void HandleWrongStructureInBracketsError(int index)
        {
            // index: index, position: i + 1.
            erposition = index + 1;

            buffer = string.Format("Error 01 at {0} - Неправильна структура в дужках, помилка на {0} символі.", erposition);
        }

        /// <summary>
        /// Error #2.
        /// </summary>
        private static void HandleUnknownOperatorError(int index)
        {
            // index: index, position: i + 1.
            erposition = index + 1;

            buffer = string.Format("Error 2 - Невідомий оператор на {0} символі.", erposition);
        }

        /// <summary>
        /// Error #3.
        /// </summary>
        private static void HandleWrongSyntacticConstructionError(int index)
        {
            // index: index, position: i + 1.
            erposition = index + 1;

            buffer = "Error 03 - Невірна синтаксична конструкція вхідного виразу.";
        }

        /// <summary>
        /// Error #4.
        /// </summary>
        private static void HandleTwoConsecutiveOperatorsError(int index)
        {
            // index: index, position: i + 1.
            erposition = index + 1;

            buffer = string.Format("Error 04 at {0} - Два підряд оператори на {0} символі.", erposition);
        }

        /// <summary>
        /// Error #5.
        /// </summary>
        private static void HandleIncompleteExpression()
        {
            // index: expresion.Length, position: expresion.Length.
            erposition = expression.Length;

            buffer = "Error 05 - Незавершений вираз.";
        }
        #endregion
        #endregion

        private enum SymbolType
        {
            Null = 0,
            NotRecognised = 9,
            Number = 11,
            UnaryOperator = 23,
            BinaryOperator = 24,
            ModOperator = 25,
            OpeningBracket = 67,
            ClosingBracket = 68,
            Space = 99,
        }
    }
}