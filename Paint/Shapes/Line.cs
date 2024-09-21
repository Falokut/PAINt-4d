using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Paint
{
    [Serializable]
    public class Line : Shape
    {
        Point start;
        Point end;
        public Line(UInt16 lineThickness,
            Color color,
            Point start,
            Point end) : base(lineThickness, color)
        {
            this.start = start;
            this.end = end;
        }

        System.Drawing.Rectangle GetBounds()
        {
            var minX = Math.Min(start.X, end.X);
            var minY = Math.Min(start.Y, end.Y);
            var maxX = Math.Max(start.X, end.X);
            var maxY = Math.Max(start.Y, end.Y);

            return new System.Drawing.Rectangle(minX, minY, maxX- minX, maxY- minY);
        }

        protected override void DrawShapeOutline(Graphics g, Pen p)
        {
            g.DrawLine(p, start,end);
        }
        protected override Point GetCenter()
        {
            return new Point((start.X + end.X)/2, (start.Y + end.Y) / 2);
        }
    }
}
