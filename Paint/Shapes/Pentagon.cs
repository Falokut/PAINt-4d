using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Paint.Shapes
{
    [Serializable]
    public class Pentagon : Shape
    {
        Point p1;
        Point p2;

        public Pentagon(UInt16 lineThickness,
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

        protected override void DrawShape(Graphics g, Brush b)
        {
            var path = new GraphicsPath();
            path.AddLines(getPentagonPoints());
            g.FillPath(b, path);
        }
        protected override void DrawShapeOutline(Graphics g, Pen p)
        {
            var path = new GraphicsPath();
            path.AddLines(getPentagonPoints());
            g.DrawPath(p, path);
        }

        protected Point[] getPentagonPoints()
        {
            float centerX = (p1.X + p2.X) / 2.0f;
            float centerY = (p1.Y + p2.Y) / 2.0f;

            float radius = Math.Min(Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y)) / 2.0f;

            float angle = 2 * (float)Math.PI / 5;
            Point[] points = new Point[6];

            for (int i = 0; i < points.Length; i++)
            {
                float x = centerX + radius * (float)Math.Cos(angle * i);
                float y = centerY + radius * (float)Math.Sin(angle * i);
                points[i] = new Point((int)x, (int)y);
            }

            return points;
        }
    }
}
