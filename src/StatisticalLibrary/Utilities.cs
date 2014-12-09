using System;

namespace StatisticalLibrary
{
    public class Utilities
    {
        public static TableRowClass[] sortTableRow(TableRowClass[] rows)
        {
            TableRowClass[] tmp = rows;
            for (int i = 0; i < tmp.Length; i++)
            {
                for (int t = i; t < tmp.Length; t++)
                {
                    if (tmp[i].ClassInterval.LowerBound > tmp[t].ClassInterval.LowerBound)
                    {
                        TableRowClass x = tmp[i];
                        tmp[i] = tmp[t];
                        tmp[t] = x;
                    }
                }
            }
            return tmp;
        }

        public static float[] sortFloat(float[] values)
        {
            float[] tmp = values;
            for (int i = 0; i < tmp.Length; i++)
            {
                for (int t = i; t < tmp.Length; t++)
                {
                    if (tmp[i] > tmp[t])
                    {
                        float x = tmp[i];
                        tmp[i] = tmp[t];
                        tmp[t] = x;
                    }
                }
            }
            return tmp;
        }

        public static int roundUp(float value)
        {
            int val = (int)value;
            if (val != value)
                return val + 1;
            else
                return val;
        }

        public static int roundUp(double value)
        {
            return roundUp(roundOff((float)value));
        }

        public static float roundOff(float value)
        {
            return (float)Math.Round(value, 2);
        }

        public static float sum(float[] values)
        {
            float sum = 0;
            foreach (float e in values)
                sum += e;
            return sum;
        }

        public static float sum(TableRowClass[] rows)
        {
            float fx = 0;
            foreach (TableRowClass e in rows)
                fx += (e.Frequency * e.ClassMark);
            return fx;
        }

        public static float sum(GroupDataClass[] groups)
        {
            float fx = 0;
            foreach (GroupDataClass e in groups)
            {
                float cm = (e.ClassInterval.UpperBound + e.ClassInterval.LowerBound) / 2;
                fx += (e.Frequency * cm);
            }
            return fx;
        }

        public static int sumFrequencies(TableRowClass[] rows)
        {
            int n = 0;
            foreach (TableRowClass e in rows)
                n += e.Frequency;
            return n;
        }

        public static int sumFrequencies(GroupDataClass[] rows)
        {
            int n = 0;
            foreach (GroupDataClass e in rows)
                n += e.Frequency;
            return n;
        }

        public static int getHighestFrequency(TableRowClass[] rows)
        {
            int highest = rows[0].Frequency;
            for (int i = 1; i < rows.Length; i++)
                if (rows[i].Frequency > highest)
                    highest = rows[i].Frequency;
            return highest;
        }

        public static int countDecimalPlaces(float value)
        {
            string[] tmp = value.ToString().Trim().Split('.');
            if (tmp.Length == 1) return 0;
            else return tmp[1].Length;
        }

        public static float[] getHighestLowest(float[] f)
        {
            if (f.Length == 0)
                return new float[] { float.NaN, float.NaN };
            else if (f.Length == 1)
                return new float[] { f[0], f[0] };
            else
            {
                float[] sorted = sortFloat(f);
                return new float[] { sorted[sorted.Length - 1], sorted[0]  };
            }
        }

        public static string formatToString(float[] f)
        {
            string txt = "";
            for (int i = 0; i < f.Length; i++)
            {
                if (i == 0)
                    txt += f[i].ToString();
                else
                    txt += ", " + f[i];
            }
            return txt;
        }

        public static bool isContinuous(float value)
        {
            int tmp = (int)value;
            return (tmp != value);
        }


    }
}
