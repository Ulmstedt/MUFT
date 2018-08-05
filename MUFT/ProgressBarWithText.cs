using System;
using System.Drawing;
using System.Windows.Forms;

namespace MUFT
{
    class ProgressBarWithText : ProgressBar
    {
        //Property to hold the custom text
        private string m_CustomText;
        public string CustomText
        {
            get { return m_CustomText; }
            set
            {
                m_CustomText = value;
                this.Invalidate();
            }
        }

        private const int WM_PAINT = 0x000F;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            
            switch (m.Msg)
            {
                case WM_PAINT:
                    int m_Percent = Convert.ToInt32((Convert.ToDouble(Value) / Convert.ToDouble(Maximum)) * 100);
                    dynamic flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;

                    using (Graphics g = Graphics.FromHwnd(Handle))
                    {
                        using (Brush textBrush = new SolidBrush(ForeColor))
                        {
                            TextRenderer.DrawText(g, CustomText, new Font("Arial", Convert.ToSingle(8.25), FontStyle.Regular), new Rectangle(0, 0, this.Width, this.Height), Color.Black, flags);
                        }
                    }

                    break;
            }

        }
    }
}
