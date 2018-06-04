using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatticeEnergyConsoleApp
{
    public static class Processor
    {
        private static readonly double epsilon = 119.8;
        private static readonly double sigma = 3.405;

        /// <summary>
        /// Ширина трикутної гратки.
        /// </summary>
        private static double widthTriangleLattice;

        /// <summary>
        /// Кількість частинок у стовпці або стрічці.
        /// </summary>
        private static int particlesCount;

        public static void Main()
        {
            var isError = !InputData();

            if (isError)
            {
                return;
            }

            OutputData();

            Process();

            Console.ReadKey();
        }
        
        /// <summary>
        /// Введення початкових даних.
        /// </summary>
        /// <returns>true - ввід успішний, інакше - false.</returns>
        private static bool InputData()
        {
            Console.WriteLine("Введіть ширину трикутної гратки: L_x =");

            var widthTriangleLatticeString = Console.ReadLine();

            if (!double.TryParse(widthTriangleLatticeString, out widthTriangleLattice))
            {
                Console.WriteLine("Помилка! Неправильно задане значення ширини трикутної гратки L_x - {0}.", widthTriangleLatticeString);
                Console.ReadKey();
                Console.WriteLine("Припинення роботи програми...");
                Console.ReadKey();
                return false;
            }

            Console.Clear();

            Console.WriteLine("Введіть кількість частинок у стовпці або стрічці: n_c =");

            var particlesCountString = Console.ReadLine();

            if (!int.TryParse(particlesCountString, out particlesCount))
            {
                Console.WriteLine("Помилка! Неправильно задане значення кількість частинок у стовпці або стрічці n_c - {0}.", particlesCountString);
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
            Console.WriteLine("Ширина трикутної гратки L_x = {0}.\n"
                + "Кількість частинок у стовпці або стрічці n_c = {1}.\n",
                widthTriangleLattice, particlesCount);
        }

        private static void Process()
        {
            var squareLennardJonesPotential = 0.0;
            var squareSixthPotential = 0.0;
            var squareQuarterPotential = 0.0;
            var triangleLennardJonesPotential = 0.0;
            var triangleSixthPotential = 0.0;
            var triangleQuarterPotential = 0.0;

            double heightTriangleLattice = Math.Sqrt(3) * widthTriangleLattice / 2;
            double triangleColumnDistance = widthTriangleLattice * 1.0 / particlesCount;
            double triangleRowDistance = Math.Sqrt(3) * triangleColumnDistance / 2;

            double lengthSquareLattice = Math.Sqrt(widthTriangleLattice * heightTriangleLattice);
            double squareRowColumnDistance = lengthSquareLattice / particlesCount;

            for (int i = 0; i < particlesCount; i++)
            {
                for (int j = 0; j < particlesCount; j++)
                {
                    for (int ii = i, jj = j; ; )
                    {
                        jj++;
                        if (jj == particlesCount)
                        {
                            ii++;
                            if (ii == particlesCount)
                            {
                                break;
                            }
                            jj = 0;
                        }
                        
                        var squareDistance = CalculateDistanceForSquareLattice(
                            Math.Abs(i - ii), Math.Abs(j - jj), squareRowColumnDistance);

                        squareLennardJonesPotential += 4.0 * epsilon *
                            (Math.Pow(sigma / squareDistance, 12) - Math.Pow(sigma / squareDistance, 6));

                        squareSixthPotential += 1.0 / Math.Pow(squareDistance, 6);

                        squareQuarterPotential += 1.0 / Math.Pow(squareDistance, 4);

                        var triangleDistance = CalculateDistanceForTriangleLattice(
                            Math.Abs(i - ii), Math.Abs(j - jj), triangleRowDistance, triangleColumnDistance);

                        triangleLennardJonesPotential += 4.0 * epsilon *
                            (Math.Pow(sigma / triangleDistance, 12) - Math.Pow(sigma / triangleDistance, 6));

                        triangleSixthPotential += 1.0 / Math.Pow(triangleDistance, 6);

                        triangleQuarterPotential += 1.0 / Math.Pow(triangleDistance, 4);
                    }
                }
            }

            Console.WriteLine("Енергія трикутної гратки для потенціалу Леннарда Джонса: E = {0:F10}.", triangleLennardJonesPotential);
            Console.WriteLine("Енергія трикутної гратки для потенціалу V(r) = 1 / r^6: E = {0:F10}.", triangleSixthPotential);
            Console.WriteLine("Енергія трикутної гратки для потенціалу V(r) = 1 / r^4: E = {0:F10}.", triangleQuarterPotential);
            Console.WriteLine("Енергія квадратної гратки для потенціалу Леннарда Джонса: E = {0:F10}.", squareLennardJonesPotential);
            Console.WriteLine("Енергія трикутної гратки для потенціалу V(r) = 1 / r^6: E = {0:F10}.", squareSixthPotential);
            Console.WriteLine("Енергія трикутної гратки для потенціалу V(r) = 1 / r^4: E = {0:F10}.", squareQuarterPotential);
        }

        private static double CalculateDistanceForSquareLattice(int rowsCount, int columnsCount, double rowColumnDistance)
        {
            var rowsDistance = rowsCount * rowColumnDistance;

            var columnsDistance = columnsCount * rowColumnDistance;

            var squareDistance = Math.Sqrt(rowsDistance * rowsDistance + columnsDistance * columnsDistance);

            return squareDistance;
        }

        private static double CalculateDistanceForTriangleLattice(int rowsCount, int columnsCount, double rowDistance, double columnDistance)
        {
            var rowsDistance = rowsCount * rowDistance;

            var columnsDistance = columnsCount * columnDistance;

            if (rowsCount % 2.0 != 0.0)
            {
                columnDistance += columnDistance / 2;
            }

            var squareDistance = Math.Sqrt(rowsDistance * rowsDistance + columnsDistance * columnsDistance);

            return squareDistance;
        }
    }
}