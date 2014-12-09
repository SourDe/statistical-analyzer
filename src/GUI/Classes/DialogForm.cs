using System.Windows.Forms;

namespace StatisticalAnalyzer
{
    public partial class DialogForm : Form
    {
        public DialogForm()
            : base()
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.CenterToParent();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
        }
    }
}
