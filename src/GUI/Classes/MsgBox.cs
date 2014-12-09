using System.Windows.Forms;

namespace StatisticalAnalyzer
{
    public class MsgBox
    {

        public static DialogResult Error(string message)
        {
            return MessageBox.Show(
                "[Error] " + message,
                "Exception",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );
        }

        public static DialogResult Info(string message)
        {
            return MessageBox.Show(
                "[Info] " + message,
                "Notification",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
        }

        public static DialogResult Confirm(string message)
        {
            return MessageBox.Show(
                "[Confirm] " + message,
                "Action Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );
        }

        public static DialogResult Warning(string message)
        {
            return MessageBox.Show(
                "[Warning] " + message,
                "Action Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
                );
        }

    }
}
