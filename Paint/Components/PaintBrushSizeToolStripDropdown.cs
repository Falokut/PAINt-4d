using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Resources;
using Paint.Properties;
using System.Windows.Forms.Design;

namespace Paint
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class PaintBrushSizeToolStripDropdown : ToolStripDropDownButton
    {
        public PaintBrushSizeToolStripDropdown():base(Properties.Resources.icons8_толщина_линии_30)
        {
            TextImageRelation = TextImageRelation.TextBeforeImage;
            TextAlign = ContentAlignment.MiddleLeft;
            ImageAlign = ContentAlignment.MiddleLeft;
            ImageScaling = ToolStripItemImageScaling.SizeToFit;
            ToolTipText = "ширина линий в px";
            initMenuItems();
        }

        void initMenuItems()
        {
            DropDownItems.Add(new PaintBrushSizeToolStripButton("1px", 
                Properties.Resources.line_1px, delegate (object sender, EventArgs e)
            {
                BrushChangedDelegate.Invoke(1);
            }));
            DropDownItems.Add(new PaintBrushSizeToolStripButton("3px", 
                Properties.Resources.line_3px, delegate (object sender, EventArgs e)
            {
                BrushChangedDelegate.Invoke(3);
            }));
            DropDownItems.Add(new PaintBrushSizeToolStripButton("5px",
                Properties.Resources.line_5px, delegate (object sender, EventArgs e)
            {
                BrushChangedDelegate.Invoke(5);
            }));
            DropDownItems.Add(new PaintBrushSizeToolStripButton("8px",
                Properties.Resources.line_8px, delegate (object sender, EventArgs e)
            {
                BrushChangedDelegate.Invoke(8);
            }));
        }

        public delegate void OnBrushSizeChanged(int newSize);
        public OnBrushSizeChanged BrushChangedDelegate;
    }
}
