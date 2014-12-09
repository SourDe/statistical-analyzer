using System;
using System.Windows.Forms;
using StatisticalData;
using StatisticalLibrary;

namespace StatisticalAnalyzer
{
    public partial class NewEditGroupData : DialogForm
    {

        private FileData.COMMAND _cmd;
        private int _dataFrequency;
        private IntervalClass _dataInterval;
        private int _origFrequency;
        private IntervalClass _origInterval;

        public int DataFrequency { get { return _dataFrequency; } }
        public string DataFrequencyString { get { return _dataFrequency.ToString(); } }
        public IntervalClass DataInterval { get { return _dataInterval; } }
        public string DataIntervalString { get { return _dataInterval.Interval; } }
        public GroupDataClass DataGroup { get { return new GroupDataClass(_dataInterval, _dataFrequency); } }
        public FileData.COMMAND Command { get { return _cmd; } }

        public int DataOriginalFrequency { get { return _origFrequency; } }
        public IntervalClass DataOriginalInterval { get { return _origInterval; } }
        public GroupDataClass DataGroupOriginal { get { return new GroupDataClass(_origInterval, _origFrequency); } }

        public NewEditGroupData()
        {
            InitializeComponent();
            Text = "New Group Data";
            btnAdd.Text = "Add";
            _cmd = FileData.COMMAND.NEW;
            _origFrequency = -1;
            _origInterval = IntervalClass.Null;
            txtLowerBound.Focus();
        }

        public NewEditGroupData(string interval, string value)
        {
            InitializeComponent();
            Text = "Edit Group Data";
            btnAdd.Text = "Edit";
            string[] arr = interval.Split('-');
            txtLowerBound.Text = arr[0].Trim();
            txtUpperBound.Text = arr[1].Trim();
            txtFrequency.Text = value;
            _cmd = FileData.COMMAND.EDIT;
            _origInterval = new IntervalClass(arr[0].Trim(), arr[1].Trim());
            _origFrequency = int.Parse(value);
            txtLowerBound.Focus();
        }

        private void validateData(string lbound, string ubound, string freq)
        {
            int i = 0; // just for parameter
            float f = 0; // just for parameter
            if (String.IsNullOrWhiteSpace(lbound))
                throw new Exception("Lowerbound is empty");
            if (!float.TryParse(ubound, out f))
                throw new Exception("Upperbound is not a number");
            if (String.IsNullOrWhiteSpace(ubound))
                throw new Exception("Upperbound is empty");
            if (!float.TryParse(lbound, out f))
                throw new Exception("Lowerbound is not a number");
            if (float.Parse(lbound) > float.Parse(ubound))
                throw new Exception("Lowerbound is greater than Upperbound");
            if (String.IsNullOrWhiteSpace(freq))
                throw new Exception("Frequency is empty");
            if (!int.TryParse(freq, out i))
                throw new Exception("Frequency is not a number");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string lbound = txtLowerBound.Text.Trim();
                string ubound = txtUpperBound.Text.Trim();
                string freq = txtFrequency.Text.Trim();
                validateData(lbound, ubound, freq);
                _dataInterval = new IntervalClass(lbound, ubound);
                _dataFrequency = int.Parse(freq);
                txtLowerBound.Clear();
                txtUpperBound.Clear();
                txtFrequency.Clear();
                DialogResult = DialogResult.OK;
            }
            catch (Exception err)
            {
                MsgBox.Error(err.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtLowerBound_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtLowerBound.Text[txtLowerBound.TextLength - 1] == '-')
                {
                    txtLowerBound.Text = txtLowerBound.Text.Substring(0, txtLowerBound.TextLength - 1);
                    txtUpperBound.Focus();
                }
                if (txtLowerBound.Text.Contains("-"))
                {
                    string[] arr = txtLowerBound.Text.Split('-');
                    txtLowerBound.Text = arr[0].Trim();
                    txtUpperBound.Text = arr[1].Trim();
                    txtFrequency.Focus();
                }
            }
            catch (Exception) { }
        }

    }
}
