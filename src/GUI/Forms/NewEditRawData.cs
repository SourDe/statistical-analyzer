using System;
using System.Windows.Forms;
using StatisticalData;

namespace StatisticalAnalyzer
{
    public partial class NewEditRawData : DialogForm
    {

        private FileData.COMMAND _cmd;
        private float _data;
        private float _orig;
        
        public float DataValue { get { return _data; } }
        public string DataString { get { return _data.ToString(); } }
        public FileData.COMMAND Command { get { return _cmd; } }
        public float DataOriginal { get { return _orig; } }

        public NewEditRawData()
        {
            InitializeComponent();
            Text = "New Raw Data";
            btnAdd.Text = "Add";
            _cmd = FileData.COMMAND.NEW;
            _orig = float.NaN;
        }

        public NewEditRawData(string value)
        {
            InitializeComponent();
            Text = "Edit Raw Data";
            btnAdd.Text = "Edit";
            txtValue.Text = value;
            _cmd = FileData.COMMAND.EDIT;
            _orig = float.Parse(value);
        }

        private void validateData(string value)
        {
            float n = 0; // just for parameter
            if (String.IsNullOrWhiteSpace(value))
                throw new Exception("Value is empty");
            if (!float.TryParse(value, out n))
                throw new Exception("Value is not a number");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string val = txtValue.Text.Trim();
                validateData(val);
                _data = float.Parse(val);
                txtValue.Clear();
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

    }
}
