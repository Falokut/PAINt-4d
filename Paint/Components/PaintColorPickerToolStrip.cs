using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;

namespace Paint
{
    [ToolboxItem(true)]
    [DefaultProperty("Shape")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class PaintColorPickerToolStrip : ToolStripButton
    {
        public PaintColorPickerToolStrip()
        {
            this.AutoSize = false;
            this.Size = new Size(24, 24);
            this.Click += OnClick;
        }

        public delegate void ColorChanged(Color color);
        public ColorChanged OnColorChanged;

        public Color CurrentColor
        {
            get { return currentColor; }
            set
            {
                currentColor = value;
                this.BackColor = currentColor;
                if (OnColorChanged!=null) OnColorChanged.Invoke(currentColor);
            }
        }
        Color currentColor;
        void OnClick(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;

            CurrentColor = dialog.Color;
        }
    }
}
