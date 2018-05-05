using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueberryPurse.Currency
{
    public class Euro
    {
        private double moneyCount;
        private double exchangeCourse;

        public Euro()
        {
            moneyCount = 0.0;
            exchangeCourse = 0.0;
        }

        public Euro(double count, double course)
        {
            moneyCount = count;
            exchangeCourse = course;
        }

        public double ConvertEuro(Euro count)
        {
            return count.moneyCount * count.exchangeCourse;
        }

    }
}
