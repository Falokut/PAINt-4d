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
    [DefaultProperty("Shape")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class PaintShapeToolStripButton : ToolStripButton
    {
        public PaintShapeToolStripButton()
        {
            this.Click += (sender, args) => ShapeShanged.Invoke(type);
            AutoSize = false;
            this.Size = new Size(30,30);
        }
        private PaintShapeToolStripButton(string text, Image img, EventHandler handler) : base(text, img, handler)
        {
            ImageAlign = ContentAlignment.MiddleLeft;
            TextAlign = ContentAlignment.MiddleLeft;
            ImageScaling = ToolStripItemImageScaling.None;
            TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            
            this.Click += (sender,args)=>ShapeShanged.Invoke(type);
        }

        public delegate void OnShapeButtonPressed(ShapeType type);
        [Category("Shape")]
        public OnShapeButtonPressed ShapeShanged;


        [Category("Shape")]
        public ShapeType Type
        {
            get
            {
                return type;
            }
            set
            {
                Image = GetImageByShapeType(value);
                type = value;
            }
        }
        ShapeType type;

        public PaintShapeToolStripButton(ShapeType type, EventHandler handler) :
            base("",null,handler)
        {
            this.type = type;
            ImageScaling = ToolStripItemImageScaling.SizeToFit;
        }

        static Image GetImageByShapeType(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Square:
                    return Properties.Resources.icons8_прямоугольник_30;
                case ShapeType.Rectangle:
                    return Properties.Resources.icons8_прямоугольник_30;
                case ShapeType.Diamond:
                    return Properties.Resources.icons8_ромб_301;
                case ShapeType.Circle:
                    return Properties.Resources.icons8_круг_30;
                case ShapeType.Elipse:
                    return Properties.Resources.icons8_круг_30;
                case ShapeType.Line:
                    return Properties.Resources.icons8_линия_30;
            }
            return null;
        }
    }
}
