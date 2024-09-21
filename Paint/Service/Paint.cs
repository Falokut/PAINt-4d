using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Paint;
using Paint.Shapes;
using Rectangle = Paint.Rectangle;
using System.Runtime.ConstrainedExecution;


namespace Service
{
    public enum ShapeType
    {
        None,
        Diamond,
        DiamondSquare,
        Square,
        Rectangle,
        Circle,
        Elipse,
        Line,
    };

    public enum PaintMode
    {
        Idle,
        Draw,
        Fill,
    }
    public class Paint
    {
        public Paint() { }
        #region fields
        string currentFilename;
        public ShapeType shapeType;
        public PaintMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        PaintMode mode;
        public System.Drawing.Color Color
        {
            get { return color; }
            set
            {
                color = value;
            }
        }
        System.Drawing.Color color;

        public UInt16 LineThickness
        {
            get { return lineThickness; }
            set
            {
                if (value == 0) throw new Exception("invalid line thickness value");

                lineThickness = value;
            }
        }
        UInt16 lineThickness { get; set; }
        #endregion
        #region Draw
        System.Drawing.Point startPoint;
        System.Drawing.Point lastPoint;
        Shape drawShape;

        private Shape DrawShape(System.Drawing.Point currentPoint)
        {
            switch (GetShapeType())
            {
                case ShapeType.Square:
                    var sq = GetSquare(startPoint, currentPoint);
                    return new Rectangle(lineThickness, color, sq.Item1, sq.Item2);
                case ShapeType.Rectangle:
                    return new Rectangle(lineThickness, color, startPoint, currentPoint);
                case ShapeType.Circle:
                    var c = GetSquare(startPoint, currentPoint);
                    return new Ellipse(lineThickness, color, c.Item1, c.Item2);
                case ShapeType.Diamond:
                    return new Diamond(lineThickness, color, startPoint, currentPoint);
                case ShapeType.DiamondSquare:
                    var diamondSq = GetSquare(startPoint, currentPoint);
                    return new Diamond(lineThickness, color, diamondSq.Item1, diamondSq.Item2);
                case ShapeType.Elipse:
                    return new Ellipse(lineThickness, color, startPoint, currentPoint);
                case ShapeType.Line:
                    return new Line(lineThickness, color, startPoint, currentPoint);
                default:
                    return null;
            }
        }

        Tuple<System.Drawing.Point, System.Drawing.Point>
        GetSquare(System.Drawing.Point startPoint, System.Drawing.Point currentPoint)
        {
            Int32 side = Math.Min(Math.Abs(currentPoint.X - startPoint.X),
               Math.Abs(currentPoint.Y - startPoint.Y));
            Int32 diffX = currentPoint.X - startPoint.X;
            Int32 diffY = currentPoint.Y - startPoint.Y;
            if (diffX > 0)
            {
                if (diffY > 0)
                    currentPoint = new System.Drawing.Point(startPoint.X + side, startPoint.Y + side);
                else
                    currentPoint = new System.Drawing.Point(startPoint.X + side, startPoint.Y - side);
            }
            else
            {
                if (diffY > 0)
                    currentPoint = new System.Drawing.Point(startPoint.X - side, startPoint.Y + side);
                else
                    currentPoint = new System.Drawing.Point(startPoint.X - side, startPoint.Y - side);
            }

            var p1 = new System.Drawing.Point(Math.Min(startPoint.X, currentPoint.X),
                Math.Min(startPoint.Y, currentPoint.Y));
            var p2 = new System.Drawing.Point(p1.X + side, p1.Y + side);
            return new Tuple<System.Drawing.Point, System.Drawing.Point>(p1, p2);
        }

        public void Fill(Point location)
        {
            // c.
        }
        ShapeType GetShapeType()
        {
            if (Control.ModifierKeys != Keys.Shift) return shapeType;

            switch (shapeType)
            {
                case ShapeType.Rectangle:
                    return ShapeType.Square;
                case ShapeType.Elipse:
                    return ShapeType.Circle;
                case ShapeType.Diamond:
                    return ShapeType.DiamondSquare;
            }
            return shapeType;
        }

        public void ProcessDrawShape(System.Drawing.Point currentPoint, Graphics g)
        {
            if (mode != PaintMode.Draw) return;
            if (drawShape == null)
            {
                startPoint = currentPoint;
            }
            lastPoint = currentPoint;
            drawShape = DrawShape(currentPoint);
            drawShape.Draw(g);
        }

        public void EndDrawShape(Graphics g,Point curentPoint)
        {
            if (drawShape == null) return;
            drawShape = DrawShape(curentPoint);
            drawShape.Draw(g);
            drawShape = null;
            startPoint = new System.Drawing.Point();
        }
        #endregion
        #region Save
        public bool Save(Bitmap bitmap)
        {
            if (currentFilename == "" || currentFilename == null) return false;
            bitmap.Save(currentFilename);
            return true;
        }

        public void SaveAs(string filename, Bitmap bitmap)
        {
            currentFilename = filename;
            Save(bitmap);
        }

        public Bitmap Load(string filename)
        {
            return new Bitmap(filename);
        }
        #endregion
    }
}
