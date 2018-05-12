using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueberryPurse.Currency
{
    public class Hryvnia
    {
        private double moneyValue;
        private double exchangeCourseRelativelyEuro;
        private double exchangeCourseRelativelyDollar;

        public Hryvnia()
        {
            moneyValue = 0.0;
            exchangeCourseRelativelyEuro = 0.0;
            exchangeCourseRelativelyDollar = 0.0;

        }

        public Hryvnia(double value, double courseRelativelyEuro, double courseRelativelyDollar)
        {
            moneyValue = value;
            exchangeCourseRelativelyEuro = courseRelativelyEuro;
            exchangeCourseRelativelyDollar = courseRelativelyDollar;

        }

        public double ConvertHryvniaToEuro(Hryvnia value)
        {
            return value.moneyValue * value.exchangeCourseRelativelyEuro;
        }

        public double ConvertHryvniaToDollar(Hryvnia value)
        {
            return value.moneyValue * value.exchangeCourseRelativelyDollar;
        }


    }
}
