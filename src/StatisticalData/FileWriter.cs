using System.IO;

namespace StatisticalData
{
    public class FileWriter : StreamWriter
    {

        public FileWriter(string directory)
            : base(directory, false)
        {
        }

        public override void WriteLine(string line)
        {
            base.WriteLine(line);
            base.Flush();
        }

        public override void Write(string line)
        {
            base.Write(line);
            base.Flush();
        }

        public override void Close()
        {
            base.Flush();
            base.Close();
        }

    }
}
