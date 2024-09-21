using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    [Serializable]
    public abstract class Shape
    {
        public Shape Clone()
        {
            return (Shape)MemberwiseClone();
        }

        public UInt16 OutlineThickness {
            get { return outlineThickness; }
            set
            {
                if (value == 0) throw new Exception("invalid thickness value");

                outlineThickness = value;
            }
        }

        protected UInt16 outlineThickness;
        public Color OutlineColor { get; set; }
        public Color FillColor { get; set; }

        protected Shape(UInt16 outlineThickness,
            Color outlineColor)
        {
            OutlineThickness = outlineThickness;
            OutlineColor = outlineColor;
            FillColor = Color.Transparent;
        }

        protected virtual void DrawShapeOutline(Graphics g, Pen p) { }
        protected virtual Point GetCenter() { return new Point(); }
        protected virtual void DrawShape(Graphics g, Brush b) {}
        public void Draw(Graphics g) {
                var pen = new Pen(OutlineColor, outlineThickness);
                DrawShape(g, new SolidBrush(FillColor));
                DrawShapeOutline(g, pen);
        }
    }
}
