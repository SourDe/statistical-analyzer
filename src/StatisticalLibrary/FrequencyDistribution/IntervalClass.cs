using System;
namespace StatisticalLibrary
{
    public class IntervalClass
    {

        private float _lbound, _ubound, _factor;
        private int _decPlaces;
        
        public float UpperBound { get { return (float)Math.Round(_ubound + _factor, _decPlaces); } }
        public float LowerBound { get { return (float)Math.Round(_lbound - _factor, _decPlaces); } }
        public string Interval { get { return Math.Round(_lbound - _factor, _decPlaces) + " - " + Math.Round(_ubound + _factor, _decPlaces); } }
        public int DecimalPlaces { get { return _decPlaces; } }

        public static IntervalClass Null { get { return new IntervalClass(float.NaN, float.NaN); } }

        public IntervalClass(float lowerbound, float upperbound)
        {
            _lbound = lowerbound;
            _ubound = upperbound;
            initialize();
        }

        public IntervalClass(string lowerbound, string upperbound)
        {
            _lbound = float.Parse(lowerbound);
            _ubound = float.Parse(upperbound);
            initialize();
        }

        private void initialize()
        {

            _decPlaces = 2;
        }

        public void setDecimalPlaces(int value)
        {
            _decPlaces = value;
        }

        public void setFactor(float value)
        {
            _factor = value;
        }

        public bool Equals(IntervalClass i)
        {
            if (_ubound == i.UpperBound &&
                _lbound == i.LowerBound &&
                _decPlaces == i.DecimalPlaces)
                return true;
            else
                return false;
        }

    }
}
