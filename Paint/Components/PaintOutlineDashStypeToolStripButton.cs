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
using static System.Net.Mime.MediaTypeNames;
using Paint.Properties;

namespace Paint.Components
{
    [ToolboxItem(true)]
    [DefaultProperty("Paint outline style")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class PaintOutlineDashStypeToolStripButton : ToolStripButton
    {
        public PaintOutlineDashStypeToolStripButton()
        {
            init();
        }
        public PaintOutlineDashStypeToolStripButton(string text, System.Drawing.Image img, EventHandler handler) : base(text, img, handler)
        {
            init();
            this.Text = text;
        }
        void init()
        {
            Click += (sender, args) => OutlineStyleChanged.Invoke(style);
            ImageAlign = ContentAlignment.MiddleLeft;
            TextAlign = ContentAlignment.MiddleLeft;
            ImageScaling = ToolStripItemImageScaling.None;
            this.Size = new Size(100, 30);
            AutoSize = true;
            TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
        }

        public delegate void OnOutlineStyleChanged(System.Drawing.Drawing2D.DashStyle style);

        [Category("Outline style")]
        public OnOutlineStyleChanged OutlineStyleChanged;

        [Category("Outline style")]
        public System.Drawing.Drawing2D.DashStyle Style
        {
            get
            {
                return style;
            }
            set
            {
                Image = GetImageByStyle(value);
                style = value;
            }
        }
        public System.Drawing.Drawing2D.DashStyle style;

        static System.Drawing.Image GetImageByStyle(System.Drawing.Drawing2D.DashStyle style)
        {
            switch (style)
            {
                case System.Drawing.Drawing2D.DashStyle.Solid:
                    return Resources.Solid_Outline_Style;
                case System.Drawing.Drawing2D.DashStyle.Dash:
                    return Resources.Dashed_Outline_Style;
                case System.Drawing.Drawing2D.DashStyle.DashDot:
                    return Resources.DashDot_Outline_Style;
            }
            return Resources.Solid_Outline_Style;
        }
    }
}