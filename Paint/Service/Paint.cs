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

        public void Fill(Point location, Graphics g, Bitmap bitmap)
        {
            Color targetColor = bitmap.GetPixel(location.X, location.Y);

            if (targetColor.ToArgb() == Color.ToArgb()) return;

            BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            int byteCount = bmpData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];

            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixels, 0, byteCount);

            Stack<Point> pixelsStack = new Stack<Point>();
            pixelsStack.Push(location);

            bool[,] visited = new bool[bitmap.Width, bitmap.Height];

            while (pixelsStack.Count > 0)
            {
                Point pt = pixelsStack.Pop();

                if (pt.X < 0 || pt.X >= bitmap.Width || pt.Y < 0 || pt.Y >= bitmap.Height || visited[pt.X, pt.Y])
                    continue;

                int pixelIndex = (pt.Y * bmpData.Stride) + (pt.X * bytesPerPixel);

                Color currentColor = Color.FromArgb(pixels[pixelIndex + 3], pixels[pixelIndex + 2], pixels[pixelIndex + 1], pixels[pixelIndex]);

                if (currentColor != targetColor)
                    continue;

                pixels[pixelIndex] = Color.B;
                pixels[pixelIndex + 1] = Color.G;
                pixels[pixelIndex + 2] = Color.R;
                pixels[pixelIndex + 3] = Color.A;
                visited[pt.X, pt.Y] = true;

                pixelsStack.Push(new Point(pt.X + 1, pt.Y)); 
                pixelsStack.Push(new Point(pt.X - 1, pt.Y)); 
                pixelsStack.Push(new Point(pt.X, pt.Y + 1)); 
                pixelsStack.Push(new Point(pt.X, pt.Y - 1)); 
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmpData.Scan0, bytesPerPixel * bitmap.Width * bitmap.Height);
            bitmap.UnlockBits(bmpData);

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

        public void EndDrawShape(Graphics g, Point curentPoint)
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
