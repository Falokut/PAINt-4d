using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Paint.Shapes
{
    [Serializable]
    public class Triangle : Shape
    {
        Point p1;
        Point p2;

        public Triangle(UInt16 lineThickness,
            Color color,
            Point p1,
            Point p2) : base(lineThickness, color)
        {
            this.p1 = p1;
            this.p2 = p2;
            rect = GetBounds();
        }

        protected override System.Drawing.Rectangle GetBounds()
        {
            if (p2.X > p1.X && p2.Y > p1.Y) //4 quarter
                return new System.Drawing.Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
            if (p2.X < p1.X && p2.Y > p1.Y) //3 quarter
                return new System.Drawing.Rectangle(p2.X, p1.Y, p1.X - p2.X, p2.Y - p1.Y);
            if (p2.X > p1.X && p2.Y < p1.Y) //2 quarter
                return new System.Drawing.Rectangle(p1.X, p2.Y, p2.X - p1.X, p1.Y - p2.Y);
            //1 quarter
            return new System.Drawing.Rectangle(p2.X, p2.Y, p1.X - p2.X, p1.Y - p2.Y);
        }

        System.Drawing.Rectangle rect;
        Point[] getTrianglePoints()
        {
            var left = new Point(rect.Left, rect.Bottom);
            var right = new Point(rect.Right, rect.Bottom);
            var top = new Point((rect.Left + rect.Right) / 2, rect.Top);

            return new Point[] { left, right, top, left };
        }

        protected override void DrawShape(Graphics g, Brush b)
        {
            var path = new GraphicsPath();
            path.AddLines(getTrianglePoints());
            g.FillPath(b, path);
        }
        protected override void DrawShapeOutline(Graphics g, Pen p)
        {
            var path = new GraphicsPath();
            path.AddLines(getTrianglePoints());
            g.DrawPath(p, path);
        }
    }
}
