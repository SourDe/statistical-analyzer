using System;
using System.Collections.Generic;
using System.IO;
using StatisticalLibrary;

namespace StatisticalData
{
    public class FileData
    {

        public enum TYPE { UNGROUPED, GROUPED }
        public enum KIND { DISCRETE, CONTINUOUS }
        public enum COMMAND { NEW, EDIT }

        private TYPE _dt;
        private KIND _dk;
        private string[] _dat;

        public TYPE DataType { get { return _dt; } }
        public KIND DataKind { get { return _dk; } }
        public string[] Data { get { return _dat; } }

        public FileData(TYPE dataType, KIND dataKind, string[] data)
        {
            _dt = dataType;
            _dk = dataKind;
            _dat = data;
        }

        public static string[] convertToUngrouped(KIND kind, float[] data)
        {
            List<string> template = new List<string>();
            template.Add("# This is the type of the data contained.");
            template.Add("TYPE = " + TYPE.UNGROUPED);
            template.Add("");
            template.Add("# This is the type of the data contained.");
            template.Add("KIND = " + kind);
            template.Add("");
            template.Add("# This is the start of data stream.");
            foreach (float e in data)
                template.Add(e.ToString());
            return template.ToArray();
        }

        public static string[] convertToGrouped(KIND kind, TableRowClass[] rows)
        {
            List<string> template = new List<string>();
            template.Add("# This is the type of the data contained.");
            template.Add("TYPE = " + TYPE.GROUPED);
            template.Add("");
            template.Add("# This is the type of the data contained.");
            template.Add("KIND = " + kind);
            template.Add("");
            template.Add("# This is the start of data stream.");
            foreach (TableRowClass e in rows)
            {
                string data = e.ClassInterval.LowerBound + "\t" + e.ClassInterval.UpperBound + "\t" + e.Frequency;
                foreach (float f in e.Data)
                    data += "\t" + f;
                template.Add(data);
            }
            return template.ToArray();
        }

        public static float[] parseToUngrouped(string[] data)
        {
            List<float> listData = new List<float>();
            foreach (string e in data)
            {
                if (!String.IsNullOrWhiteSpace(e))
                    listData.Add(float.Parse(e));
            }
            return listData.ToArray();
        }

        public static float[] parseToUngrouped(TableRowClass[] data)
        {
            List<float> list = new List<float>();
            foreach (TableRowClass e in data)
                foreach (float f in e.Data)
                    list.Add(f);
            return list.ToArray();
        }

        public static TableRowClass[] parseToGrouped(string[] data)
        {
            List<TableRowClass> listData = new List<TableRowClass>();
            foreach (string e in data)
            {
                if (!String.IsNullOrWhiteSpace(e))
                {
                    string[] buffer = e.Split('\t');
                    TableRowClass row = new TableRowClass(new IntervalClass(buffer[0], buffer[1]));
                    if (buffer.Length > 3)
                        for (int i = 3; i < buffer.Length; i++)
                            row.addData(buffer[i]);
                    else
                        row.setFrequency(buffer[2]);
                    listData.Add(row);
                }
            }
            return listData.ToArray();
        }

        public static void saveFile(string[] data, string filename)
        {
            FileWriter writer = new FileWriter(filename);
            foreach (string e in data)
                writer.WriteLine(e);
            writer.Close();
        }

        public static FileData loadFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            List<string> data = new List<string>();
            TYPE dataType = TYPE.UNGROUPED;
            KIND dataKind = KIND.DISCRETE;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string safe = line.Trim();
                if (safe.StartsWith("#"))
                    continue;
                else if (String.IsNullOrWhiteSpace(safe))
                    continue;
                else if (safe.StartsWith("TYPE"))
                {
                    string[] arr = safe.Split('=');
                    switch (arr[1].Trim())
                    {
                        case "GROUPED":
                            dataType = TYPE.GROUPED;
                            break;
                        case "UNGROUPED":
                        default:
                            dataType = TYPE.UNGROUPED;
                            break;
                    }
                }
                else if (safe.StartsWith("KIND"))
                {
                    string[] arr = safe.Split('=');
                    switch (arr[1].Trim())
                    {
                        case "CONTINUOUS":
                            dataKind = KIND.CONTINUOUS;
                            break;
                        case "DISCRETE":
                        default:
                            dataKind = KIND.DISCRETE;
                            break;
                    }
                }
                else
                    data.Add(safe);
            }
            return new FileData(dataType, dataKind, data.ToArray());
        }

    }
}
