using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using Paint.Shapes;
using Rectangle = Paint.Shapes.Rectangle;


namespace Paint.Service
{
    public class Paint
    {
        public Paint()
        {
            outlineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }
        #region fields
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
        UInt16 lineThickness;
        #endregion
        Point startDrawPoint;

        public System.Drawing.Drawing2D.DashStyle OutlineDashStyle
        {
            get { return outlineDashStyle; }
            set
            {
                outlineDashStyle = value;
            }
        }
        System.Drawing.Drawing2D.DashStyle outlineDashStyle;
        Shape currentShape;

        private Shape GetShapeToDraw(Point currentPoint)
        {
            Point startPoint = startDrawPoint;
            Point endPoint = currentPoint;
            if (ShouldNormalizePoints() && shapeType != ShapeType.Line)
            {
                Tuple<Point, Point> normalizedPoints = NormalizePoints(startPoint, endPoint);
                startPoint = normalizedPoints.Item1;
                endPoint = normalizedPoints.Item2;
            }
            switch (shapeType)
            {
                case ShapeType.Rectangle:
                    return new Rectangle(lineThickness, color, startPoint, endPoint);
                case ShapeType.Diamond:
                    return new Diamond(lineThickness, color, startPoint, endPoint);
                case ShapeType.Elipse:
                    return new Ellipse(lineThickness, color, startPoint, endPoint);
                case ShapeType.Line:
                    return new Line(lineThickness, color, startPoint, endPoint);
                case ShapeType.Triangle:
                    return new Triangle(lineThickness, color, startPoint, endPoint);
                case ShapeType.RightAngledTriangle:
                    return new RightAngledTriangle(lineThickness, color, startPoint, endPoint);
                case ShapeType.Pentagon:
                    return new Pentagon(lineThickness, color, startPoint, endPoint);
                case ShapeType.Hexagon:
                    return new Hexagon(lineThickness, color, startPoint, endPoint);
                default:
                    return null;
            }
        }

        Tuple<Point, Point> NormalizePoints(Point startPoint, Point endPoint)
        {
            Int32 side = Math.Min(Math.Abs(endPoint.X - startPoint.X),
               Math.Abs(endPoint.Y - startPoint.Y));
            Int32 diffX = endPoint.X - startPoint.X;
            Int32 diffY = endPoint.Y - startPoint.Y;
            if (diffX > 0)
            {
                if (diffY > 0)
                    endPoint = new Point(startPoint.X + side, startPoint.Y + side);
                else
                    endPoint = new Point(startPoint.X + side, startPoint.Y - side);
            }
            else
            {
                if (diffY > 0)
                    endPoint = new Point(startPoint.X - side, startPoint.Y + side);
                else
                    endPoint = new Point(startPoint.X - side, startPoint.Y - side);
            }

            var p1 = new Point(Math.Min(startPoint.X, endPoint.X),
                Math.Min(startPoint.Y, endPoint.Y));
            var p2 = new Point(p1.X + side, p1.Y + side);
            return new Tuple<Point, Point>(p1, p2);
        }

        public void Fill(Point location, Bitmap bitmap)
        {
            Color targetColor = bitmap.GetPixel(location.X, location.Y);

            if (targetColor.ToArgb() == Color.ToArgb()) return;

            BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            int byteCount = bmpData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];

            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixels, 0, byteCount);

            Stack<Point> pixelsStack = new Stack<Point>(byteCount);
            pixelsStack.Push(location);
            bool[,] visited = new bool[bitmap.Width, bitmap.Height];

            while (pixelsStack.Count > 0)
            {
                Point pt = pixelsStack.Pop();

                if (pt.X < 0 ||
                    pt.X >= bitmap.Width ||
                    pt.Y < 0 ||
                    pt.Y >= bitmap.Height ||
                    visited[pt.X, pt.Y])
                {
                    continue;
                }

                int pixelIndex = (pt.Y * bmpData.Stride) + (pt.X * bytesPerPixel);
                Color currentColor = Color.FromArgb(
                    pixels[pixelIndex + 3],
                    pixels[pixelIndex + 2],
                    pixels[pixelIndex + 1],
                    pixels[pixelIndex]
                );

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

        bool ShouldNormalizePoints()
        {
            return Control.ModifierKeys == Keys.Shift;
        }

        public void ProcessDrawShape(Point currentPoint, Graphics g)
        {
            if (mode != PaintMode.Draw) return;
            if (currentShape == null)
            {
                startDrawPoint = currentPoint;
            }
            currentShape = GetShapeToDraw(currentPoint);
            currentShape.Draw(g, outlineDashStyle);
        }

        public void ChangePixelsColor(Point point, Graphics g, Color color)
        {
            if (mode != PaintMode.Eraser && mode != PaintMode.Pen)
                return;
            Brush brush = new SolidBrush(color);
            g.FillRectangle(brush, point.X, point.Y, lineThickness, lineThickness);
        }
        public void ChangePixelsColor(Point point, Graphics g)
        {
            ChangePixelsColor(point, g, color);
        }

        public void EndDrawShape(Point curentPoint, Graphics g)
        {
            if (currentShape == null) return;
            currentShape = GetShapeToDraw(curentPoint);
            currentShape.Draw(g, outlineDashStyle);
            currentShape = null;
        }

        private System.Drawing.Rectangle selectionRect;
        private Bitmap clipboard;
        bool cutSelection = false;

        public void SelectArea(Point startPoint, Point endPoint)
        {
            selectionRect = new System.Drawing.Rectangle(
                Math.Min(startPoint.X, endPoint.X),
                Math.Min(startPoint.Y, endPoint.Y),
                Math.Abs(endPoint.X - startPoint.X),
                Math.Abs(endPoint.Y - startPoint.Y)
            );
        }

        public void CopySelection()
        {
            cutSelection = false;
        }
        public void CutSelection()
        {
            cutSelection = true;
        }

        public void PasteSelection(Point pasteLocation, Bitmap bitmap)
        {
            if (selectionRect.Width == 0 || selectionRect.Height == 0) return;

            clipboard = bitmap.Clone(selectionRect, bitmap.PixelFormat);
            if (clipboard == null) return;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                if (cutSelection) g.FillRectangle(new SolidBrush(Color.White), selectionRect);
                g.DrawImage(clipboard, pasteLocation);
                resetClipboard();
            }
        }

        private void resetClipboard()
        {
            selectionRect = System.Drawing.Rectangle.Empty;
            clipboard.Dispose();
            clipboard = null;
        }
        public void SetStartSelectionPoint(Point point)
        {
            selectionRect = System.Drawing.Rectangle.Empty;
            selectionRect.X = point.X;
            selectionRect.Y = point.Y;
        }

        public void ProcessSelectArea(Point point, Graphics g)
        {
            selectionRect = new System.Drawing.Rectangle(
                Math.Min(selectionRect.X, point.X),
                Math.Min(selectionRect.Y, point.Y),
                Math.Abs(point.X - selectionRect.X),
                Math.Abs(point.Y - selectionRect.Y)
            );
            Pen whitePen = new Pen(Color.White, 3);
            g.DrawRectangle(whitePen, selectionRect);

            Pen blackPen = new Pen(Color.Black, 3);
            blackPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawRectangle(blackPen, selectionRect);
        }
    }
}
