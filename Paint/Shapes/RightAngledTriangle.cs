using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Paint.Shapes
{
    [Serializable]
    public class RightAngledTriangle : Shape
    {
        Point p1;
        Point p2;

        public RightAngledTriangle(UInt16 lineThickness,
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
            var left = new Point(p1.X, p1.Y);
            var right = new Point(p2.X, p1.Y);
            var top = new Point(p1.X, p2.Y);

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
