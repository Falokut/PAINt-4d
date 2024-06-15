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
        public string ID
        {
            get { return id; }
        }
        string id;

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
        public bool IsSelected { get { return bSelected; } }
        public float Angle {
            get { return angle; }
            set
            {
                var tmp = (int)(Math.Round(value * 100)/100.0);
                tmp = tmp % 360;
                if (value < 0.0001)
                {
                    tmp *= -1;
                }
                angle = tmp;
            }
        }

        float angle;

        protected Shape(UInt16 outlineThickness,
            Color outlineColor)
        {
            OutlineThickness = outlineThickness;
            OutlineColor = outlineColor;
            id = Guid.NewGuid().ToString();
            FillColor = Color.Transparent;
        }


        protected virtual System.Drawing.Rectangle DrawShapeOutline(Graphics g,Pen p) { return new System.Drawing.Rectangle(); }
        protected virtual Point GetCenter() { return new Point(); }
        protected virtual void DrawShape(Graphics g, Brush b) {}
        public void Draw(Graphics g) {
            using (Matrix m = new Matrix())
            {
                m.RotateAt(Angle, GetCenter());
                g.Transform = m; 

                var pen = new Pen(OutlineColor, outlineThickness);
                DrawShape(g, new SolidBrush(FillColor));

                var bounds = DrawShapeOutline(g, pen);

                m.Scale(1.0f, 1.0f);
                g.Transform = m;
                DrawSelectedBorder(g, bounds);
                g.ResetTransform();
            }
        }
        public virtual bool IsOverlapped(Point p) { return false; }
        protected bool bSelected = false;
        public virtual void StartSelecting()
        {
            bSelected = true;
        }
        public virtual void EndSelecting()
        {
            bSelected = false;
        }

        public virtual void Move(Point m) {}
        public virtual void MoveTo(Point target) {}

        protected virtual void DrawSelectedBorder(Graphics g, System.Drawing.Rectangle bounds) {
            if (!bSelected) return;
            SelectedShapeMarkers.Draw(g, bounds);
        }
    }

    public class PointUtils
    {
        public static Point RotatePoint(Point origin, Point p, double angle)
        {
            double angleInRads = (angle / 180.0) * Math.PI;
            double s = Math.Sin(angleInRads);
            double c = Math.Cos(angleInRads);
            p.X -= origin.X;
            p.Y -= origin.Y;

            // rotate point
            double xnew = p.X * c - p.Y * s;
            double ynew = p.X * s + p.Y * c;
            // translate point back:
            p.X = (int)xnew + origin.X;
            p.Y = (int)ynew + origin.Y;
            return p;
        }

        public static Tuple<Point,Point> RotateLine(Point start,Point end,double angle)
        {
            Point origin = new Point((start.X+end.X)/2, (start.Y + end.Y) / 2);
            return new Tuple<Point, Point>(RotatePoint(origin, start, angle), RotatePoint(origin, end, angle));
        }
    }
    public class SelectedShapeMarkers
    {
        static Size outlineSize = new Size(6, 6);
        static Color outlineColor = Color.Black;

        public static void Draw(Graphics g, System.Drawing.Rectangle bounds)
        {            
            // draw markers
            var top = bounds.Top - outlineSize.Height/2;
            var down = bounds.Bottom + outlineSize.Height/2;
            var left = bounds.Left - outlineSize.Width/2;
            var right = bounds.Right - outlineSize.Width / 2;

            // left up
            DrawMarker(g, new Point(left, top));
            // center up
            DrawMarker(g, new Point((left + right) / 2, top));
            // right up
            DrawMarker(g, new Point(right, top));
            // center left
            DrawMarker(g, new Point(left, (top + down) / 2));
            // center right
            DrawMarker(g, new Point(right, (top + down) / 2));
            // left down
            DrawMarker(g, new Point(left, down));
            // center down
            DrawMarker(g, new Point((left + right) / 2, down));
            // right down
            DrawMarker(g, new Point(right, down));
        }
        static void DrawMarker(Graphics g, Point location)
        {
            g.FillRectangle(new SolidBrush(Color.White),
                new System.Drawing.Rectangle(location, outlineSize));
            const int thickness = 1;
            g.DrawRectangle(new Pen(outlineColor, thickness),
                new System.Drawing.Rectangle(location, outlineSize));
        }
    }
}
