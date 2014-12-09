using System;
using StatisticalLibrary;

namespace StatisticalLibrary
{
    public class GroupDataClass
    {
        protected IntervalClass _ci;
        protected int _f;

        public IntervalClass ClassInterval { get { return _ci; } }
        public int Frequency { get { return _f; } }
        public float X { get { return (float)Math.Round((_ci.UpperBound + _ci.LowerBound) / 2, _ci.DecimalPlaces); } }
        public float FX { get { return Frequency * X; } }

        public GroupDataClass(IntervalClass classInterval, int frequency)
        {
            // Class Interval
            _ci = classInterval;
            // Frequency
            _f = frequency;
        }

        public GroupDataClass(float lowerbound, float upperbound, int decimalplaces, int frequency)
        {
            // Class Interval
            _ci = new IntervalClass(lowerbound, upperbound);
            _ci.setDecimalPlaces(decimalplaces);
            // Frequency
            _f = frequency;
        }

        public GroupDataClass(string lowerbound, string upperbound, string frequency)
        {
            // Class Interval
            _ci = new IntervalClass(lowerbound, upperbound);
            // Frequency
            _f = int.Parse(frequency);
        }

        public void setFrequency(int frequency)
        {
            _f = frequency;
        }

        public void setFrequency(string frequency)
        {
            setFrequency(int.Parse(frequency));
        }

        public void updateFrequencyBy(int i)
        {
            _f += i;
        }

        public void setInterval(IntervalClass i)
        {
            _ci = i;
        }

        public bool isEncapsulated(float f)
        {
            if (_ci.LowerBound <= f && _ci.UpperBound >= f)
                return true;
            else
                return false;
        }

        public bool Equals(GroupDataClass g)
        {
            if (_ci.Equals(g.ClassInterval) &&
                _f == g.Frequency)
                return true;
            else
                return false;
        }

    }
}
