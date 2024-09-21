using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Paint.Shape;

namespace Paint
{
    [Serializable]
    public class Rectangle : Shape
    {
        Point p1;
        Point p2;

        public Rectangle(UInt16 lineThickness,
            Color color,
            Point p1,
            Point p2) : base(lineThickness, color)
        {
            this.p1 = p1;
            this.p2 = p2;
            rect = GetRectangle();
        }


        public static System.Drawing.Rectangle GetRectangle(Point p1, Point p2)
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
        System.Drawing.Rectangle GetRectangle()
        {
            return GetRectangle(p1, p2);
        }

        System.Drawing.Rectangle rect;

        protected override void DrawShape(Graphics g, Brush b)
        {
            g.FillRectangle(b, rect);
        }
        protected override void DrawShapeOutline(Graphics g, Pen p)
        {
            g.DrawRectangle(p, rect);
        }
    }
}
