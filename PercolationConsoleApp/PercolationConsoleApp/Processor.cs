using System;

namespace PercolationConsoleApp
{
    public static class Processor
    {
        private static Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Сторона квадратної гратки L.
        /// </summary>
        private static int sideLength;

        /// <summary>
        /// Ймовірність клітинки бути зайнятою p.
        /// </summary>
        private static double probability = 0.1;

        /// <summary>
        /// Крок збільшення ймовірності клітинки бути зайнятою h_p.
        /// </summary>
        private static double probabilityIncreasingStep = 0.025;

        /// <summary>
        /// Кількість пошуків появи з'єднувального кластера m.
        /// </summary>
        private static int searchersCount = 10;

        /// <summary>
        /// Означення, який кластер вважається з'єднувальним.
        /// </summary>
        private static ClusterIs clusterIs = ClusterIs.NotDefined;

        public static void Main()
        {
            var isError = !InputData();

            if (isError)
            {
                return;
            }

            OutputData();

            ProcessAverageConnectiveClusterProbabilities();
            
            Console.ReadKey();
        }

        /// <summary>
        /// Введення початкових даних.
        /// </summary>
        /// <returns>true - ввід успішний, інакше - false.</returns>
        private static bool InputData()
        {
            Console.WriteLine("Введіть сторону квадратної гратки: L =");

            var sideLengthString = Console.ReadLine();

            if (!int.TryParse(sideLengthString, out sideLength))
            {
                Console.WriteLine("Помилка! Неправильно задане значення сторони квадратної гратки L - {0}.", sideLengthString);
                Console.ReadKey();
                Console.WriteLine("Припинення роботи програми...");
                Console.ReadKey();
                return false;
            }

            Console.Clear();

            Console.WriteLine("Можна пропустити наступний крок.\n"
                + "Тоді ймовірність клітинки бути зайнятою буде задана значенням за замовчуванням: p = {0}.\n"
                + "Введіть ймовірність клітинки бути зайнятою: p =", probability);

            var probabilityString = Console.ReadLine();

            if (!string.IsNullOrEmpty(probabilityString)
                && !double.TryParse(probabilityString, out probability)
                && probability > 1.0)
            {
                Console.WriteLine("Помилка! Неправильно задане значення ймовірності клітинки бути зайнятою p - {0}.", probabilityString);
                Console.ReadKey();
                Console.WriteLine("Припинення роботи програми...");
                Console.ReadKey();
                return false;
            }

            Console.Clear();

            Console.WriteLine("Можна пропустити наступний крок.\n"
                + "Тоді крок збільшення ймовірності клітинки бути зайнятою буде задана значенням за замовчуванням: h_p = {0}.\n"
                + "Введіть крок збільшення ймовірності клітинки бути зайнятою: h_p =", probabilityIncreasingStep);

            var probabilityIncreasingStepString = Console.ReadLine();

            if (!string.IsNullOrEmpty(probabilityIncreasingStepString)
                && !double.TryParse(probabilityIncreasingStepString, out probabilityIncreasingStep))
            {
                Console.WriteLine("Помилка! Неправильно задане значення кроку збільшення ймовірності клітинки бути зайнятою h_p - {0}.", probabilityIncreasingStepString);
                Console.ReadKey();
                Console.WriteLine("Припинення роботи програми...");
                Console.ReadKey();
                return false;
            }

            Console.Clear();

            Console.WriteLine("Можна пропустити наступний крок.\n"
                + "Тоді кількість пошуків з'єднувального кластера буде задана значенням за замовчуванням: m = {0}.\n"
                + "Введіть кількість пошуків з'єднувального кластера: m =", searchersCount);

            var searchersCountString = Console.ReadLine();

            if (!string.IsNullOrEmpty(searchersCountString)
                && !int.TryParse(searchersCountString, out searchersCount))
            {
                Console.WriteLine("Помилка! Неправильно задане значення кількості пошуків з'єднувального кластера m - {0}.", searchersCountString);
                Console.ReadKey();
                Console.WriteLine("Припинення роботи програми...");
                Console.ReadKey();
                return false;
            }

            Console.Clear();

            Console.WriteLine("Введіть, який кластер вважається з'єднувальним:\n"
                + " * горизонтальний - H,\n"
                + " * вертильканий - V,\n"
                + " * горизонтальний і вертикальний - HV,\n"
                + " * горизонтальний АБО вертикальний - HorV;");

            string clusterIsString = Console.ReadLine();

            switch (clusterIsString.ToUpperInvariant())
            {
                case "H":
                    clusterIs = ClusterIs.Horizontal;
                    break;

                case "V":
                    clusterIs = ClusterIs.Vertical;
                    break;

                case "HV":
                    clusterIs = ClusterIs.HorizontalAndVertical;
                    break;

                default:
                    Console.WriteLine("Помилка! Неправильно задане значення, який кластер вважається з'єднувальним - {0}.", clusterIsString);
                    Console.ReadKey();
                    Console.WriteLine("Припинення роботи програми...");
                    Console.ReadKey();
                    return false;
            }

            Console.Clear();

            return true;
        }

        /// <summary>
        /// Виведення початкових даних.
        /// </summary>
        private static void OutputData()
        {
            Console.WriteLine("Сторона квадратної гратки L = {0}.\n"
                + "Ймовірність клітинки бути зайнятою p = {1}.\n"
                + "Крок збільшення ймовірності клітинки бути зайнятою h_p = {2}.\n"
                + "Кількість пошуків появи з'єднувального кластера m = {3}.\n"
                + "Значення, при якому кластер вважається з'єднувальним - {4}.\n",
                sideLength, probability, probabilityIncreasingStep, searchersCount, clusterIs.ToString());
        }

        private static void ProcessAverageConnectiveClusterProbabilities()
        {
            var connectiveClusterProbabilities = CalculateConnectiveClusterProbabilities();

            OutputConnectiveClusterProbabilities(connectiveClusterProbabilities);

            var averageConnectiveClusterProbability = CalculateAverageConnectiveClusterProbability(connectiveClusterProbabilities);

            Console.WriteLine("Середнє значення ймовірності з'єднувального кластера: p_c_average = {0}.", averageConnectiveClusterProbability);
        }

        private static double[] CalculateConnectiveClusterProbabilities()
        {
            Console.WriteLine("Натисніть, щоб почати будувати гратки.");
            Console.ReadKey();
            Console.Clear();

            var connectiveClusterProbabilities = new double[searchersCount];

            for (int i = 0; i < searchersCount; i++)
            {
                connectiveClusterProbabilities[i] = CalculateClusterProbability();
            }
            
            return connectiveClusterProbabilities;
        }

        private static void OutputConnectiveClusterProbabilities(double[] connectiveClusterProbabilities)
        {
            for (int i = 0; i < searchersCount; i++)
            {
                Console.WriteLine("Ймовірність з'єднувального кластера на спробі #{0}: p_c = {1:F10}.", i + 1, connectiveClusterProbabilities[i]);
            }

            Console.WriteLine();
        }

        private static double CalculateAverageConnectiveClusterProbability(double[] connectiveClusterProbabilities)
        {
            var averageConnectiveClusterProbability = 0.0;

            for (int i = 0; i < searchersCount; i++)
            {
                averageConnectiveClusterProbability += connectiveClusterProbabilities[i];
            }

            averageConnectiveClusterProbability /= searchersCount;

            return averageConnectiveClusterProbability;
        }

        private static double CalculateClusterProbability()
        {
            var currentProbability = probability;

            while (currentProbability < 1)
            {
                var lattice = BuildLattice(currentProbability);
                
                var markedLattice = MarkClusters(lattice);

                var isConnectiveClusterPresent = CheckWhetherConnectiveClusterIsPresent(lattice, markedLattice);

                if (isConnectiveClusterPresent)
                {
                    OutputLattice(lattice);

                    OutputLattice(markedLattice);

                    Console.ReadKey();

                    return currentProbability;
                }

                currentProbability += probabilityIncreasingStep;
            }

            throw new InvalidProgramException("Something went wrong. Connective cluster was not found with probability equals one.");
        }

        private static bool[,] BuildLattice(double currentProbability)
        {
            var lattice = new bool[sideLength, sideLength];

            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    var randomNumber = GenerateRandomNumber();

                    lattice[i, j] = randomNumber < currentProbability;
                }
            }

            return lattice;
        }

        private static int[,] MarkClusters(bool[,] lattice)
        {
            var mark = 0;

            var markedLattice = new int[sideLength, sideLength];

            for (int i = sideLength - 1; 0 <= i; i--)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    if (lattice[i, j])
                    {
                        if (j - 1 >= 0
                            && lattice[i, j - 1])
                        {
                            markedLattice[i, j] = markedLattice[i, j - 1];

                            if (i + 1 < sideLength
                                && lattice[i + 1, j]
                                && markedLattice[i + 1, j] != markedLattice[i, j])
                            {
                                RemarkLattice(ref markedLattice, markedLattice[i, j], markedLattice[i + 1, j]);
                            }
                        }
                        else
                        if (i + 1 < sideLength
                            && lattice[i + 1, j])
                        {
                            markedLattice[i, j] = markedLattice[i + 1, j];
                        }
                        else
                        {
                            mark++;

                            markedLattice[i, j] = mark;
                        }
                    }
                }
            }

            return markedLattice;
        }

        private static void RemarkLattice(ref int[,] lattice, int oldValue, int newValue)
        {
            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    if (lattice[i, j] == oldValue)
                    {
                        lattice[i, j] = newValue;
                    }
                }
            }
        }

        private static double GenerateRandomNumber(double bottomLimit = 0, double upperLimit = 1)
        {
            var number = _random.Next(0, int.MaxValue);

            var ratio = int.MaxValue / (upperLimit - bottomLimit);

            var numberInLimits = number / ratio;

            return numberInLimits;
        }

        private static void OutputLattice<T>(T[,] lattice) where T : struct
        {
            for (int i = 0; i < sideLength; i++)
            {
                Console.Write("\t");

                for (int j = 0; j < sideLength; j++)
                {
                    Console.Write("{0}\t", lattice[i, j]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static bool CheckWhetherConnectiveClusterIsPresent(bool[,] lattice, int[,] markedLattice)
        {
            var isHorizontal = CheckWhetherHorizontalClusterIsPresent(lattice, markedLattice);
            
            var isVertical = CheckWhetherVerticalClusterIsPresent(lattice, markedLattice);

            Func<bool> isConnectiveClusterPresentFunc;

            switch (clusterIs)
            {
                case ClusterIs.Horizontal:
                    isConnectiveClusterPresentFunc = () => isHorizontal;
                    break;

                case ClusterIs.Vertical:
                    isConnectiveClusterPresentFunc = () => isVertical;
                    break;

                case ClusterIs.HorizontalAndVertical:
                    isConnectiveClusterPresentFunc = () => isHorizontal && isVertical;
                    break;

                case ClusterIs.HorizontalOrVertical:
                    isConnectiveClusterPresentFunc = () => isHorizontal || isVertical;
                    break;

                default:
                    isConnectiveClusterPresentFunc = () => false;
                    break;
            }

            var isConnectiveClusterPresent = isConnectiveClusterPresentFunc();

            return isConnectiveClusterPresent;
        }
        
        private static bool CheckWhetherHorizontalClusterIsPresent(bool[,] lattice, int[,] markedLattice)
        {
            for (int i = 0; i < sideLength; i++)
            {
                if (lattice[i, 0])
                {
                    for (int ii = 0; ii < sideLength; ii++)
                    {
                        if (lattice[ii, sideLength - 1]
                            && markedLattice[i, 0] == markedLattice[ii, sideLength - 1])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool CheckWhetherVerticalClusterIsPresent(bool[,] lattice, int[,] markedLattice)
        {
            for (int j = 0; j < sideLength; j++)
            {
                if (lattice[0, j])
                {
                    for (int jj = 0; jj < sideLength; jj++)
                    {
                        if (lattice[sideLength - 1, jj]
                            && markedLattice[0, j] == markedLattice[sideLength - 1, jj])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private enum ClusterIs
        {
            NotDefined = 0,
            Horizontal = 1,
            Vertical = 2,
            HorizontalAndVertical = 3,
            HorizontalOrVertical = 4,
        }
    }
}