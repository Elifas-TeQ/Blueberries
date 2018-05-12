using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueberryPurse.Currency
{
    public class Euro
    {
        private double moneyValue;
        private double exchangeCourseRelativelyHryvnia;

        public Euro()
        {
            moneyValue = 0.0;
            exchangeCourseRelativelyHryvnia = 0.0;
        }

        public Euro(double count, double course)
        {
            moneyValue = count;
            exchangeCourseRelativelyHryvnia = course;
        }

        public double ConvertEuroToHryvnia(Euro count)
        {
            return count.moneyValue * count.exchangeCourseRelativelyHryvnia;
        }

    }
}
