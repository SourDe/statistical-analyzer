using System;
using System.Collections.Generic;

namespace StatisticalLibrary
{
    public class TableRowClass : GroupDataClass
    {

        private float[] _data = null;
        private IntervalClass _cb;
        private float _cm;
        private CumulativeFrequencyClass _cf;
        private int _n;

        public float ClassMark { get { return _cm; } }
        public IntervalClass ClassBoundary { get { return _cb; } }
        public CumulativeFrequencyClass CumulativeFrequency { get { return _cf; } }
        public float RelativeFrequency { get { return (float)Math.Round((float)_f/_n, 3); } }
        public string PercentageFrequency { get { return Math.Round(RelativeFrequency * 100, 2).ToString() + "%"; } }
        public float[] Data { get { return _data; } }

        public TableRowClass(IntervalClass classInterval)
            : base(classInterval, 0)
        {
            initialize();
        }

        public TableRowClass(float lowerbound, float upperbound, int decimalplaces)
            : base(lowerbound, upperbound, decimalplaces, 0)
        {
            initialize();
        }

        public TableRowClass(string lowerbound, string upperbound, string frequency)
            : base(lowerbound, upperbound, frequency)
        {
            initialize();
        }

        public TableRowClass(GroupDataClass g)
            : base(g.ClassInterval, g.Frequency)
        {
            initialize();
        }

        private void initialize()
        {
            // Class Mark
            _cm = (_ci.UpperBound + _ci.LowerBound) / 2;
            // Class Boundary
            _cb = new IntervalClass(_ci.LowerBound, _ci.UpperBound);
            _cb.setDecimalPlaces(_ci.DecimalPlaces + 1);
            _cb.setFactor(getFactor(_ci.UpperBound));
            // Cumulative Frequency
            _cf = new CumulativeFrequencyClass(-1, -1);
        }

        private float getFactor(float value)
        {
            string[] arr = value.ToString().Split('.');
            int count;
            try { count = arr[1].Length; }
            catch (Exception) { count = 0; }
            return (float)(5/Math.Pow(10, count + 1));
        }

        public void addData(float value)
        {
            if (_data == null)
            {
                _data = new float[] { value };
            }
            else
            {
                List<float> lst = new List<float>();
                lst.AddRange(_data);
                lst.Add(value);
                _data = lst.ToArray();
            }
            _f++;
        }

        public void addData(string value)
        {
            addData(float.Parse(value));
        }

        public void setCumulativeFrequency(CumulativeFrequencyClass cf)
        {
            _cf = cf;
        }

        public void setCumulativeFrequency(int lessthan, int greaterthan)
        {
            _cf = new CumulativeFrequencyClass(lessthan, greaterthan);
        }

        public void setTotalFrequencyCount(int count)
        {
            _n = count;
        }

    }
}
