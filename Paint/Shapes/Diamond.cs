using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Shapes
{
    [Serializable]
    public class Diamond : Shape
    {
        Point p1;
        Point p2;
        public Diamond(UInt16 lineThickness,
            Color outlineColor,
            Point p1,
            Point p2) : base(lineThickness, outlineColor)
        {
            if (p1.X > p2.X)
            {
                this.p1 = p2;
                this.p2 = p1;
            }
            else
            {
                this.p1 = p1;
                this.p2 = p2;
            }
        }

        System.Drawing.Rectangle GetBounds()
        {
            int x = p1.X;
            int height = Math.Abs(p1.Y - p2.Y);
            int y = Math.Min(p1.Y,p2.Y);
            int width = Math.Abs(p1.X - p2.X);
            return new System.Drawing.Rectangle(x, y, width, height);
        }

        protected override void DrawShape(Graphics g, Brush b)
        {
            var path = new GraphicsPath();
            path.AddLines(getDiamondPoints());
            g.FillPath(b, path);
        }

        Point[] getDiamondPoints()
        {
            var center = GetCenter();

            var left = new Point(p1.X, center.Y);
            var right = new Point(p2.X, center.Y);

            int height = Math.Abs(p1.Y - p2.Y);

            var top = new Point(center.X, center.Y + height/2);
            var bottom = new Point(center.X, center.Y - height/2);

            return new Point[] { left, top, right, bottom, left };
        }

        protected override void DrawShapeOutline(Graphics g, Pen p)
        {
            var path = new GraphicsPath();
            path.AddLines(getDiamondPoints());
            g.DrawPath(p,path);
        }
        protected override Point GetCenter()
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }
    }
}
