using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;

namespace Paint
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class PaintBrushSizeToolStripButton : ToolStripButton 
    {
        public PaintBrushSizeToolStripButton() {
   
        }
        public PaintBrushSizeToolStripButton(string text, Image img, EventHandler handler) : base(text, img, handler)
        {
            ImageAlign = ContentAlignment.MiddleLeft;
            TextAlign = ContentAlignment.MiddleLeft;
            ImageScaling = ToolStripItemImageScaling.None;
            ToolTipText = text;
            this.Size = new Size(100, 30); 
            AutoSize = true;
            TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
        }
    }
}
