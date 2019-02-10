using System.Windows.Forms;

namespace ZigbeeDecoder
{
    public partial class TextBoxLabel : UserControl
    {
        public TextBoxLabel()
        {
            InitializeComponent();
            label2.Text = "";
            label1.Text = "";
        }

        public void SetName(string name)
        {
            label1.Text = name;
        }

        public void SetValue(string value)
        {
            label2.Text = value;
        }
    }
}
