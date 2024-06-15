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
using Service;

namespace Paint
{
    [ToolboxItem(true)]
    [DefaultProperty("Paint Mode")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class PaintModeToolStripButton : ToolStripButton
    {
        public PaintModeToolStripButton()
        {
            Click += (sender, args) => ModeChanged.Invoke(mode);
            AutoSize = false;
            this.Size = new Size(32, 32);
        }
        private PaintModeToolStripButton(string text, Image img, EventHandler handler) : base(text, img, handler)
        {
            ImageAlign = ContentAlignment.MiddleLeft;
            TextAlign = ContentAlignment.MiddleLeft;
            ImageScaling = ToolStripItemImageScaling.None;
            TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;

            Click += (sender, args) => ModeChanged.Invoke(mode);
        }

        public delegate void OnModeButtonPressed(PaintMode mode);
        [Category("Shape")]
        public OnModeButtonPressed ModeChanged;


        [Category("Paint Mode")]
        public PaintMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                Image = GetImageByMode(value);
                mode = value;
            }
        }
        PaintMode mode;

        public PaintModeToolStripButton(PaintMode mode, EventHandler handler) :
            base("", null, handler)
        {
            this.mode = mode;
            ImageScaling = ToolStripItemImageScaling.SizeToFit;
        }

        static Image GetImageByMode(PaintMode mode)
        {
            switch (mode)
            {
                case PaintMode.Select:
                    return Properties.Resources.icons8_курсор_рука_30;
                case PaintMode.Move:
                    return Properties.Resources.icons8_перемещение_30;
                case PaintMode.Draw:
                    return Properties.Resources.icons8_щетка_30;
                case PaintMode.Idle:
                    return Properties.Resources.icons8_курсор_30;
                case PaintMode.Fill:
                    return Properties.Resources.icons8_цвет_заливки_24;
            }
            return null;
        }
    }
}
