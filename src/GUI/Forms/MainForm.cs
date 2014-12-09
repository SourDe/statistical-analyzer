using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StatisticalData;
using StatisticalLibrary;

namespace StatisticalAnalyzer
{
    public partial class MainForm : Form
    {

        private int _numRawData, _numGroupData;
        private List<float> _lstRawData;
        private List<GroupDataClass> _lstGroupData;
        FileData.KIND _dataKind;
        private float _highestRaw, _lowestRaw;
        FrequencyDistributionClass _freqDistribution;

        NewEditRawData frmNewRawData;
        NewEditGroupData frmNewGroupData;

        public MainForm()
        {
            InitializeComponent();
            resetAll();
        }

        #region Procedures
        private void resetForm()
        {
            frmNewRawData = new NewEditRawData();
            frmNewGroupData = new NewEditGroupData();
            valRangeStep1.Text = "= NaN - NaN";
            valRange.Text = "= NaN";
            valNumberOfClassStep1.Text = "= sqrt(0)";
            valNumberOfClassStep2.Text = valNumberOfClass.Text = "= 0";
            valClassSizeStep1.Text = "= NaN - 0";
            valClassSizeStep2.Text = valClassSize.Text = "= 0";
            cmbKind.SelectedIndex = 0;
            tabControlData.SelectedIndex = 0;
            updateStatisticalDisplay();
        }
        private void resetRawData()
        {
            _numRawData = 0;
            _highestRaw = _lowestRaw = float.NaN;
            _lstRawData = new List<float>();
            lstRawData.Items.Clear();
        }
        private void resetGroupData()
        {
            _numGroupData = 0;
            _lstGroupData = new List<GroupDataClass>();
            lstGroupData.Items.Clear();
        }
        private void resetAll()
        {
            resetRawData();
            resetGroupData();
            resetForm();
        }
        private void updateStatisticalDisplay()
        {
            //
            float[] hl = Utilities.getHighestLowest(_lstRawData.ToArray());
            _highestRaw = hl[0];
            _lowestRaw = hl[1];
            //
            valHighestValue.Text = _highestRaw.ToString();
            valLowestValue.Text = _lowestRaw.ToString();
            valNumberOfData.Text = _numRawData.ToString();
        }
        private void updateFrequencyDistributionTable()
        {
            lstFrequencyDistribution.Items.Clear();
            ListViewItem[] items = _freqDistribution.ToListViewItems();
            for (int i = 0; i < items.Length; i++)
                lstFrequencyDistribution.Items.Add(items[i]);
        }
        private void updateFrequencyDiagrams()
        {
            float cs = computeClassSize(_dataKind);
            // Frequency Histogram
            chartFrequencyHistogram.ChartAreas["ChartArea1"].AxisX.Minimum = Math.Round(_freqDistribution.RowsSorted[0].ClassBoundary.LowerBound, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartFrequencyHistogram.ChartAreas["ChartArea1"].AxisX.Maximum = _freqDistribution.RowsSorted[_freqDistribution.RowsSorted.Length - 1].ClassBoundary.LowerBound;
            chartFrequencyHistogram.ChartAreas["ChartArea1"].AxisX.Interval = Math.Round(cs, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartFrequencyHistogram.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            chartFrequencyHistogram.ChartAreas["ChartArea1"].AxisY.Maximum = Utilities.getHighestFrequency(_freqDistribution.Rows);
            chartFrequencyHistogram.Series["Data"].Points.Clear();
            foreach (TableRowClass e in _freqDistribution.RowsSorted)
                chartFrequencyHistogram.Series["Data"].Points.AddXY(e.ClassBoundary.LowerBound, e.Frequency);
            // Frequency Polygon
            chartFrequencyPolygon.ChartAreas["ChartArea1"].AxisX.Minimum = Math.Round(_freqDistribution.RowsSorted[0].ClassMark, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartFrequencyPolygon.ChartAreas["ChartArea1"].AxisX.Maximum = Math.Round(_freqDistribution.RowsSorted[_freqDistribution.Rows.Length - 1].ClassMark, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartFrequencyPolygon.ChartAreas["ChartArea1"].AxisX.Interval = Math.Round(cs, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartFrequencyPolygon.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            chartFrequencyPolygon.ChartAreas["ChartArea1"].AxisY.Maximum = Utilities.getHighestFrequency(_freqDistribution.Rows);
            chartFrequencyPolygon.Series["Data1"].Points.Clear();
            chartFrequencyPolygon.Series["Data2"].Points.Clear();
            foreach (TableRowClass e in _freqDistribution.RowsSorted)
            {
                chartFrequencyPolygon.Series["Data1"].Points.AddXY(e.ClassMark, e.Frequency);
                chartFrequencyPolygon.Series["Data2"].Points.AddXY(e.ClassMark, e.Frequency);
            }
            // Cumulative Frequency (Less Than)
            chartCumulativeFrequencyLessThan.ChartAreas["ChartArea1"].AxisX.Minimum = Math.Round(_freqDistribution.RowsSorted[0].ClassBoundary.UpperBound, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartCumulativeFrequencyLessThan.ChartAreas["ChartArea1"].AxisX.Maximum = Math.Round(_freqDistribution.RowsSorted[_freqDistribution.Rows.Length - 1].ClassBoundary.UpperBound, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartCumulativeFrequencyLessThan.ChartAreas["ChartArea1"].AxisX.Interval = Math.Round(cs, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartCumulativeFrequencyLessThan.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            chartCumulativeFrequencyLessThan.ChartAreas["ChartArea1"].AxisY.Maximum = _freqDistribution.TotalFrequency;
            chartCumulativeFrequencyLessThan.Series["Data1"].Points.Clear();
            chartCumulativeFrequencyLessThan.Series["Data2"].Points.Clear();
            foreach (TableRowClass e in _freqDistribution.RowsSorted)
            {
                chartCumulativeFrequencyLessThan.Series["Data1"].Points.AddXY(e.ClassBoundary.UpperBound, e.CumulativeFrequency.LessThan);
                chartCumulativeFrequencyLessThan.Series["Data2"].Points.AddXY(e.ClassBoundary.UpperBound, e.CumulativeFrequency.LessThan);
            }
            // Cumulative Frequency (Greater Than)
            chartCumulativeFrequencyGreaterThan.ChartAreas["ChartArea1"].AxisX.Minimum = Math.Round(_freqDistribution.RowsSorted[0].ClassBoundary.LowerBound, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartCumulativeFrequencyGreaterThan.ChartAreas["ChartArea1"].AxisX.Maximum = _freqDistribution.RowsSorted[_freqDistribution.Rows.Length - 1].ClassBoundary.LowerBound;
            chartCumulativeFrequencyGreaterThan.ChartAreas["ChartArea1"].AxisX.Interval = Math.Round(cs, _freqDistribution.Rows[0].ClassBoundary.DecimalPlaces);
            chartCumulativeFrequencyGreaterThan.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            chartCumulativeFrequencyGreaterThan.ChartAreas["ChartArea1"].AxisY.Maximum = _freqDistribution.TotalFrequency;
            chartCumulativeFrequencyGreaterThan.Series["Data1"].Points.Clear();
            chartCumulativeFrequencyGreaterThan.Series["Data2"].Points.Clear();
            foreach (TableRowClass e in _freqDistribution.RowsSorted)
            {
                chartCumulativeFrequencyGreaterThan.Series["Data1"].Points.AddXY(e.ClassBoundary.LowerBound, e.CumulativeFrequency.GreaterThan);
                chartCumulativeFrequencyGreaterThan.Series["Data2"].Points.AddXY(e.ClassBoundary.LowerBound, e.CumulativeFrequency.GreaterThan);
            }
        }
        private void computeFrequencyDistribution()
        {
            //
            _freqDistribution = new FrequencyDistributionClass(_dataKind.ToString());
            List<float> tmp = _lstRawData;
            tmp.Sort();
            // data variables
            float r = computeRange(),
                  cs = computeClassSize(_dataKind);
            int ci = computeNumberOfCI();
            // increment one (1) if the highest does not have an existing interval
            if (_highestRaw > _lowestRaw + cs * (ci - 1))
                ci += 1;
            float cf = _freqDistribution.computeCIFactor();
            List<TableRowClass> rows = new List<TableRowClass>();
            // add rows
            for (int i = 0; i < ci; i++)
            {
                float x = _lowestRaw + cs * i;
                float f = FrequencyDistributionClass.computeCIFactor(cs);
                float y = x + f;
                TableRowClass row = new TableRowClass(x, y, Utilities.countDecimalPlaces(f));
                _freqDistribution.addRow(row);
            }
            // add data to rows
            foreach (float e in _lstRawData)
                _freqDistribution.addData(e);
            // finalization
            _freqDistribution.computeTotalFrequency();
            _freqDistribution.computeCumulativeFrequencies();
        }
        private float computeRange()
        {
            try
            {
                float range = 0;
                valRangeStep1.Text = "= " + _highestRaw + " - " + _lowestRaw;
                range = FrequencyDistributionClass.computeRange(_lstRawData.ToArray(), _highestRaw, _lowestRaw);
                valRange.Text = "= " + range.ToString();
                return range;
            }
            catch (Exception)
            {
                valRangeStep1.Text = "= NaN - NaN";
                valRange.Text = "= NaN";
                return float.NaN;
            }
        }
        private int computeNumberOfCI()
        {
            try
            {
                float n = 0;
                valNumberOfClassStep1.Text = "= sqrt(" + _numRawData + ")";
                n = (float)Math.Sqrt(_numRawData);
                valNumberOfClassStep2.Text = "= " + n;
                if (n != (int)n)
                    n = Utilities.roundUp(n);
                valNumberOfClass.Text = "= " + n;
                return (int)n;
            }
            catch (Exception)
            {
                valNumberOfClassStep1.Text = "= sqrt(0)";
                valNumberOfClassStep2.Text = "= 0";
                valNumberOfClass.Text = "= 0";
                return 0;
            }
        }
        private float computeClassSize(FileData.KIND k)
        {
            try
            {
                float i = 0, R = computeRange(), CI = computeNumberOfCI();
                valClassSizeStep1.Text = "= " + R + " / " + CI;
                i = R / CI;
                valClassSizeStep2.Text = "= " + i;
                if (k == FileData.KIND.DISCRETE)
                    i = Utilities.roundUp(i);
                else if (k == FileData.KIND.CONTINUOUS)
                    i = Utilities.roundOff(i);
                valClassSize.Text = "= " + i;
                return i;
            }
            catch (Exception)
            {
                valClassSizeStep1.Text = "= NaN / 0";
                valClassSizeStep2.Text = "= 0";
                valClassSize.Text = "= 0";
                return 0;
            }
        }
        private int getIndexFromGroupData(GroupDataClass g)
        {
            for (int i = 0; i < _lstGroupData.Count; i++)
            {
                if (_lstGroupData[i].Equals(g))
                    return i;
            }
            return -1;
        }
        #endregion

        #region DataMenu_RawData
        private void newRawData_Click(object sender, EventArgs e)
        {
            frmNewRawData.ShowDialog();
            if (frmNewRawData.DialogResult == DialogResult.OK)
            {
                if (frmNewRawData.Command == FileData.COMMAND.NEW)
                {
                    _lstRawData.Add(frmNewRawData.DataValue);
                    _numRawData++;
                    lstRawData.Items.Add(frmNewRawData.DataString);
                }
                else if (frmNewRawData.Command == FileData.COMMAND.EDIT)
                {
                    try
                    {
                        int t = _lstRawData.IndexOf(frmNewRawData.DataOriginal);
                        int i = lstRawData.SelectedIndices[0];
                        _lstRawData[t] = frmNewRawData.DataValue;
                        lstRawData.Items[i].Text = frmNewRawData.DataString;
                    }
                    catch (Exception err)
                    {
                        MsgBox.Error(err.Message);
                        // frmNewRawData.Command = FileData.COMMAND.NEW;
                    }
                }
                if (Utilities.isContinuous(frmNewRawData.DataValue))
                    cmbKind.SelectedIndex = 1;
                tabControlData.SelectedIndex = 0;
                updateStatisticalDisplay();
            }
        }
        private void editSelectedRawData_Click(object sender, EventArgs e)
        {
            if (lstRawData.SelectedIndices.Count == 0)
            {
                MsgBox.Error("No selected data");
                return;
            }
            else
            {
                frmNewRawData = new NewEditRawData(lstRawData.SelectedItems[0].Text);
                newRawData_Click(sender, e);
            }
        }
        private void removeSelectedRawData_Click(object sender, EventArgs e)
        {
            if (lstRawData.SelectedIndices.Count == 0)
            {
                MsgBox.Error("No selected data");
                return;
            }
            else
            {
                if (MsgBox.Confirm("Do you want to remove the data?") == DialogResult.Yes)
                {
                    string val = lstRawData.SelectedItems[0].Text;
                    int i = lstRawData.SelectedIndices[0];
                    lstRawData.Items.RemoveAt(i);
                    int t = _lstRawData.IndexOf(float.Parse(val));
                    _lstRawData.RemoveAt(t);
                    _numRawData--;
                    updateStatisticalDisplay();
                }
            }
        }
        private void removeAllRawData_Click(object sender, EventArgs e)
        {
            if (MsgBox.Confirm("Do you want to remove all data?") == DialogResult.Yes)
            {
                resetRawData();
                updateStatisticalDisplay();
            }
        }
        #endregion

        #region DataMenu_GroupData
        private void newGroupData_Click(object sender, EventArgs e)
        {
            frmNewGroupData = new NewEditGroupData();
            frmNewGroupData.ShowDialog();
            if (frmNewGroupData.DialogResult == DialogResult.OK)
            {
                if (frmNewGroupData.Command == FileData.COMMAND.NEW)
                {
                    GroupDataClass row = new GroupDataClass(frmNewGroupData.DataInterval, frmNewGroupData.DataFrequency);
                    _lstGroupData.Add(row);
                    _numGroupData += row.Frequency;
                    ListViewItem item = new ListViewItem(row.ClassInterval.Interval);
                    item.SubItems.Add(row.Frequency.ToString());
                    lstGroupData.Items.Add(item);
                }
                else if (frmNewGroupData.Command == FileData.COMMAND.EDIT)
                {
                    int t = getIndexFromGroupData(frmNewGroupData.DataGroupOriginal);
                    int i = lstGroupData.SelectedIndices[0];
                    _lstGroupData[t].setInterval(frmNewGroupData.DataInterval);
                    _lstGroupData[t].setFrequency(frmNewGroupData.DataFrequency);
                    _numGroupData -= frmNewGroupData.DataOriginalFrequency + frmNewGroupData.DataFrequency;
                    lstGroupData.Items[i].SubItems[0].Text = frmNewGroupData.DataIntervalString;
                    lstGroupData.Items[i].SubItems[1].Text = frmNewGroupData.DataFrequencyString;
                }
                if (Utilities.isContinuous(frmNewGroupData.DataInterval.UpperBound))
                    cmbKind.SelectedIndex = 1;
                tabControlData.SelectedIndex = 1;
                updateStatisticalDisplay();
            }
        }
        private void editSelectedGroupData_Click(object sender, EventArgs e)
        {
            if (lstGroupData.SelectedIndices.Count == 0)
            {
                MsgBox.Error("No selected data");
                return;
            }
            else
            {
                frmNewGroupData = new NewEditGroupData(lstGroupData.SelectedItems[0].Text, lstGroupData.SelectedItems[0].SubItems[1].Text);
                newGroupData_Click(sender, e);
            }
        }
        private void generateFromRaw_Click(object sender, EventArgs e)
        {
            float r = computeRange(), cs = computeClassSize(_dataKind);
            int ci = computeNumberOfCI();
            // increment one (1) if the highest does not have an existing interval
            if (_highestRaw > _lowestRaw + cs * (ci - 1))
                ci += 1;
            float cf = FrequencyDistributionClass.computeCIFactor(cs);
            List<GroupDataClass> group = new List<GroupDataClass>();
            // add rows
            for (int i = 0; i < ci; i++)
            {
                float x = _lowestRaw + cs * i;
                float f = FrequencyDistributionClass.computeCIFactor(cs);
                float y = x + f;
                GroupDataClass row = new GroupDataClass(x, y, Utilities.countDecimalPlaces(f), 0);
                group.Add(row);
            }
            // update frequency
            foreach (float d in _lstRawData)
            {
                foreach (GroupDataClass g in group)
                {
                    if (g.isEncapsulated(d))
                    {
                        g.updateFrequencyBy(1);
                        break;
                    }
                }
            }
            // add list
            lstGroupData.Items.Clear();
            foreach (GroupDataClass g in group)
            {
                ListViewItem item = new ListViewItem(g.ClassInterval.Interval);
                item.SubItems.Add(g.Frequency.ToString());
                lstGroupData.Items.Add(item);
            }
            _lstGroupData.AddRange(group);
        }
        #endregion

        #region FileMenu
        private void exportRawData_Click(object sender, EventArgs e)
        {
            if (lstRawData.Items.Count == 0)
            {
                MsgBox.Error("No data has been added");
                return;
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Statistical Data | *.sd";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string[] buffer = FileData.convertToUngrouped(_dataKind, _lstRawData.ToArray());
                        FileData.saveFile(buffer, dialog.FileName);
                        MsgBox.Info("Save operation successful");
                    }
                    catch (Exception err)
                    {
                        MsgBox.Error(err.Message);
                    }
                }
            }
        }
        private void exportGroupedData_Click(object sender, EventArgs e)
        {
            if (lstRawData.Items.Count == 0)
            {
                MsgBox.Error("No data has been added");
                return;
            }
            else if (_freqDistribution == null)
            {
                MsgBox.Error("No data for the frequency distribution has been initialized");
                return;
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Statistical Data | *.sd";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string[] buffer = FileData.convertToGrouped(_dataKind, _freqDistribution.Rows);
                        FileData.saveFile(buffer, dialog.FileName);
                        MsgBox.Info("Save operation successful");
                    }
                    catch (Exception err)
                    {
                        MsgBox.Error(err.Message);
                    }
                }
            }
        }
        private void importData_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Statistical Data | *.sd";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileData data = FileData.loadFile(dialog.FileName);
                float[] parsed = new float[] { };
                if (data.DataType == FileData.TYPE.GROUPED)
                {
                    TableRowClass[] pTable = FileData.parseToGrouped(data.Data);
                    List<float> tData = new List<float>();
                    _freqDistribution = new FrequencyDistributionClass(_dataKind.ToString());
                    foreach (TableRowClass row in pTable)
                    {
                        foreach (float value in row.Data)
                            tData.Add(value);
                        _freqDistribution.addRow(row);
                    }
                    parsed = tData.ToArray();
                    updateFrequencyDistributionTable();
                }
                else
                {
                    parsed = FileData.parseToUngrouped(data.Data);
                }
                foreach (float item in parsed)
                {
                    
                    _lstRawData.Add(item);
                    lstRawData.Items.Add(new ListViewItem(item.ToString()));
                }
                _numRawData = _lstRawData.Count;
                switch (data.DataKind)
                {
                    case FileData.KIND.CONTINUOUS:
                        cmbKind.SelectedIndex = 1;
                        break;
                    case FileData.KIND.DISCRETE:
                    default:
                        cmbKind.SelectedIndex = 0;
                        break;
                }
                updateStatisticalDisplay();
                MsgBox.Info("Import operation successful");
            }
        }
        #endregion

        #region EntireForm
        private void tabControlData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlData.SelectedTab == tabPageData)
            {
                frequencyDistributionToolStripMenuItem.Enabled = true;
            }
            else
            {
                frequencyDistributionToolStripMenuItem.Enabled = false;
            }
            updateStatisticalDisplay();
        }
        private void cmbKind_Change(object sender, EventArgs e)
        {
            switch (cmbKind.SelectedItem.ToString())
            {
                case "CONTINUOUS":
                    _dataKind = FileData.KIND.CONTINUOUS;
                    break;
                case "DISCRETE":
                default:
                    _dataKind = FileData.KIND.DISCRETE;
                    break;
            }
            updateStatisticalDisplay();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_lstRawData.Count != 0 && MsgBox.Confirm("Do you want to save the data?") == DialogResult.Yes)
            {
                exportRawData_Click(sender, e);
            }
        }
        #endregion

        #region ToolsMenu_FrequencyDistribution
        private void calculateFrequencyDistribution_Click(object sender, EventArgs e)
        {
            computeFrequencyDistribution();
            updateFrequencyDistributionTable();
            tabControlContent.SelectedIndex = 1;
        }
        private void resetFrequencyDistribution_Click(object sender, EventArgs e)
        {
            if (MsgBox.Confirm("Do you want to reset the Frequency Distribution table?") == DialogResult.Yes)
            {
                lstFrequencyDistribution.Items.Clear();
            }
        }
        #endregion

        #region ToolsMenu_FrequencyDiagrams
        private void generateFrequencyDiagram_Click(object sender, EventArgs e)
        {
            if (_freqDistribution == null || lstFrequencyDistribution.Items[0].SubItems[7].Text.Equals("N/A"))
            {
                MsgBox.Error("Frequency Distribution has not been calculated yet.");
                return;
            }
            updateFrequencyDiagrams();
            tabControlContent.SelectedIndex = 2;
        }
        #endregion

        #region ToolsMenu_CentralTendency
        private void calculateCentralTendency_Click(object sender, EventArgs e)
        {
            // ungrouped
            calculateCentralTendencyUngrouped_Click(sender, e);
            // grouped
            calculateCentralTendencyGrouped_Click(sender, e);
        }
        private void calculateCentralTendencyUngrouped_Click(object sender, EventArgs e)
        {
            if (_lstRawData.Count == 0)
            {
                MsgBox.Error("There is no raw data in the records");
                return;
            }
            valUngroupedMean.Text = CentralTendency.Mean(_lstRawData.ToArray()).ToString();
            valUngroupedMedian.Text = CentralTendency.Median(_lstRawData.ToArray()).ToString();
            valUngroupedMode.Text = Utilities.formatToString(CentralTendency.Mode(_lstRawData.ToArray()));
            //
            tabControlContent.SelectedIndex = 3;
        }
        private void calculateCentralTendencyGrouped_Click(object sender, EventArgs e)
        {
            if (_lstGroupData.Count == 0)
            {
                MsgBox.Error("There is no grouped data in the records");
                return;
            }
            // fill table
            lstCentralTendency.Items.Clear();
            foreach (GroupDataClass data in _lstGroupData)
            {
                ListViewItem item = new ListViewItem(data.ClassInterval.Interval);
                item.SubItems.Add(data.Frequency.ToString());
                item.SubItems.Add(data.X.ToString());
                item.SubItems.Add(data.FX.ToString());
                lstCentralTendency.Items.Add(item);
            }
            //
            valGroupedMean.Text = CentralTendency.Mean(_lstGroupData.ToArray()).ToString();
            valGroupedMedian.Text = CentralTendency.Median(_lstGroupData.ToArray()).ToString();
            valGroupedMode.Text = CentralTendency.Mode(_lstGroupData.ToArray()).ToString();
            //
            tabControlContent.SelectedIndex = 3;
        }
        #endregion   

    }
}
