using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarlo
{
    public static class Statistics
    {
        public static int count = 0;
        public static List<float> values = new List<float>();


        public static void AddValue(float val)
        {
            count++;
            values.Add(val);
        }

        public static void Reset()
        {
            count = 0;
            values.Clear();
        }

        public static void ToCSV(StreamWriter writer)
        {
            foreach (float val in values)
            {
                writer.WriteLine("" +val);
            }
        }
    }
}
