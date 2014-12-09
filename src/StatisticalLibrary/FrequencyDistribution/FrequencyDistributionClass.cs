using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.OleDb;

namespace StatisticalLibrary
{
	public class FrequencyDistributionClass
	{

        private int _n;
        private List<TableRowClass> _rows;
        private List<float> _data;
        private float _highest, _lowest;
        private string _dataKind;

        public int TotalFrequency { get { return _n; } }
        public TableRowClass[] Rows { get { return _rows.ToArray(); } }
        public TableRowClass[] RowsSorted { get { return Utilities.sortTableRow(_rows.ToArray()); } }
        public float HighestValue { get { return _highest; } }
        public float LowestValue { get { return _lowest; } }

        public FrequencyDistributionClass(string dataKind)
        {
            _n = 0;
            _rows = new List<TableRowClass>();
            _data = new List<float>();
            _highest = _lowest = float.NaN;
            _dataKind = dataKind;
        }

        public void setDataKind(string dataKind)
        {
            _dataKind = dataKind;
        }

        public void addRow(TableRowClass row)
        {
            List<TableRowClass> list = new List<TableRowClass>();
            list.AddRange(_rows);
            list.Add(row);
            TableRowClass[] sorted = Utilities.sortTableRow(list.ToArray());
            _rows = new List<TableRowClass>();
            _rows.AddRange(sorted);
        }

        public void addRow(GroupDataClass row)
        {
            List<TableRowClass> list = new List<TableRowClass>();
            list.AddRange(_rows);
            list.Add(new TableRowClass(row));
            TableRowClass[] sorted = Utilities.sortTableRow(list.ToArray());
            _rows = new List<TableRowClass>();
            _rows.AddRange(sorted);
        }

        public void addData(float data)
        {
            if (float.IsNaN(_highest) || data > _highest)
                _highest = data;
            if (float.IsNaN(_lowest) || data < _lowest)
                _lowest = data;
            _data.Add(data);
            foreach (TableRowClass e in _rows)
            {
                if (e.isEncapsulated(data))
                {
                    e.addData(data);
                    break;
                }
            }
            _n++;
        }

        public void computeTotalFrequency()
        {
            // compute
            _n = 0;
            for (int i = 0; i < _rows.Count; i++)
                _n += _rows[i].Frequency;
            // update
            for (int i = 0; i < _rows.Count; i++)
                _rows[i].setTotalFrequencyCount(_n);
        }

        public void computeCumulativeFrequencies()
        {
            int less = 0, great = _n;
            foreach (TableRowClass e in _rows)
            {
                e.setTotalFrequencyCount(_n);
                less += e.Frequency;
                e.setCumulativeFrequency(less, great);
                great -= e.Frequency;
            }
        }

        public ListViewItem[] ToListViewItems()
        {
            List<ListViewItem> list = new List<ListViewItem>();
            TableRowClass[] tmp = Utilities.sortTableRow(_rows.ToArray());
            foreach (TableRowClass e in tmp)
            {
                ListViewItem item = new ListViewItem(e.ClassInterval.Interval);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, e.Frequency.ToString()));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, e.ClassMark.ToString()));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, e.ClassBoundary.Interval));
                if (e.CumulativeFrequency.LessThan == -1 || e.CumulativeFrequency.GreaterThan == -1)
                {
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "N/A"));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "N/A"));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "N/A"));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "N/A"));
                }
                else
                {
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, e.CumulativeFrequency.LessThan.ToString()));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, e.CumulativeFrequency.GreaterThan.ToString()));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, e.RelativeFrequency.ToString()));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, e.PercentageFrequency));
                }
                item.ToolTipText = "Data: ";
                try
                {
                    for (int i = 0; i < e.Frequency; i++)
                    {
                        if (i == 0) item.ToolTipText += e.Data[i];
                        else item.ToolTipText += ", " + e.Data[i];
                    }
                }
                catch (Exception)
                {
                    item.ToolTipText += "{Not Displayable}";
                }
                list.Add(item);
            }
            return list.ToArray();
        }

        public void ToFrequencyHistogram()
        {

        }

        public float computeRange()
        {
            if (_n == 0) return float.NaN;
            else if (_n == 1) return _data[0];
            else return _highest - _lowest;
        }

        public static float computeRange(float[] values, float highest, float lowest)
        {
            if (values.Length == 0) return float.NaN;
            else if (values.Length == 1) return values[0];
            else return highest - lowest;
        }

        public int computeCI()
        {
            if (_n == 0) return 0;
            else if (_n == 1) return 1;
            else return Utilities.roundUp(Math.Sqrt(_n));
        }

        public static int computeCI(int frequency)
        {
            if (frequency == 0) return 0;
            else if (frequency == 1) return 1;
            else return Utilities.roundUp(Math.Sqrt(frequency));
        }

        public float computeCIFactor()
        {
            float cs = computeClassSize();
            float val;
            string tmp = cs.ToString();
            if (tmp.Contains("."))
                val = (float)(cs - 1 / Math.Pow(10, tmp.Split('.')[1].Length + 1));
            else
                val = (float)(cs - 1);
            return (float)Math.Round(val, 2);
        }

        public static float computeCIFactor(float classsize)
        {
            float val;
            string tmp = classsize.ToString();
            if (tmp.Contains("."))
                val = (float)(classsize - 1 / Math.Pow(10, tmp.Split('.')[1].Length + 1));
            else
                val = (float)(classsize - 1);
            return val;
        }

        public float computeClassSize()
        {
            if (_n == 0) return 0;
            else
            {
                float r = computeRange();
                int ci = computeCI();
                switch (_dataKind)
                {
                    case "CONTINUOUS":
                        return Utilities.roundOff(r / ci);
                    case "DISCRETE":
                    default:
                        return Utilities.roundUp(r / ci);
                }
            }
        }

        public static float computeClassSize(int frequency, float range, int classinterval, string datakind)
        {
            if (frequency == 0) return 0;
            else
            {
                float value = range / classinterval;
                switch (datakind)
                {
                    case "CONTINUOUS":
                        return Utilities.roundOff(value);
                    case "DISCRETE":
                    default:
                        return Utilities.roundUp(value);
                }
            }
        }

	}
}
