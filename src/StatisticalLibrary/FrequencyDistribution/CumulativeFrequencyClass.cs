using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatisticalLibrary
{
    public class CumulativeFrequencyClass
    {
        private int _less, _greater;

        public int LessThan { get { return _less; } }
        public int GreaterThan { get { return _greater; } }

        public CumulativeFrequencyClass(int lessthan, int greaterthan)
        {
            _less = lessthan;
            _greater = greaterthan;
        }

        public bool isEncapsulated(int value)
        {
            return (_less <= value && _greater >= value);
        }

    }
}
