using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatisticalLibrary
{
    public class CentralTendency
    {

        public static float Mean(float[] data)
        {
            return Utilities.sum(data) / data.Length;
        }

        public static float Mean(GroupDataClass[] data)
        {
            float sumFx = Utilities.sum(data),
                sumF = Utilities.sumFrequencies(data);
            return  sumFx/sumF ;
        }

        public static float Median(float[] data)
        {
            if (data.Length % 2 == 1)
                return data[(data.Length + 1) / 2];
            else
            {
                float[] tmp = Utilities.sortFloat(data);
                int i = tmp.Length / 2;
                return (tmp[i] + tmp[i + 1]) / 2;
            }
        }

        public static float Median(GroupDataClass[] data)
        {
            FrequencyDistributionClass table = new FrequencyDistributionClass("CONTINUOUS");
            foreach (GroupDataClass e in data)
                table.addRow(e);
            table.computeTotalFrequency();
            table.computeCumulativeFrequencies();
            int n2 = Utilities.sumFrequencies(data) / 2;
            int fl = 0, f = 1;
            float cm = 0;
            float cs = (float)Math.Round(table.RowsSorted[1].ClassInterval.LowerBound - table.RowsSorted[0].ClassInterval.LowerBound, table.Rows[0].ClassInterval.DecimalPlaces);
            for (int i = 0; i < data.Length; i++)
            {
                if (table.Rows[i].CumulativeFrequency.isEncapsulated(n2))
                {
                    fl = table.Rows[i].CumulativeFrequency.LessThan;
                    f = table.Rows[i + 1].Frequency;
                    cm = table.Rows[i + 1].ClassMark;
                    break;
                }
            }
            return (cm + (cs * (n2 - fl)/f));
        }

        public static float[] Mode(float[] data)
        {
            List<float> key = new List<float>();
            List<int> val = new List<int>();

            foreach (float e in data)
            {
                if (key.Contains(e))
                {
                    val[key.IndexOf(e)]++;
                }
                else
                {
                    key.Add(e);
                    val.Add(0);
                }
            }

            int maxCount = val[0];
            for (int i = 1; i < val.Count; i++)
            {
                if (val[i] > maxCount)
                    maxCount = val[i];
            }

            List<float> result = new List<float>();
            for (int i = 0; i < key.Count; i++)
            {
                if (val[i] == maxCount)
                    result.Add(key[i]);
            }

            return result.ToArray();
        }

        public static float Mode(GroupDataClass[] data)
        {
            return (3 * Median(data) - 2 * Mean(data));
        }

    }
}
