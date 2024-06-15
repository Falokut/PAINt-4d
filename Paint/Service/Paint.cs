using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Paint;
using Paint.Shapes;

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
        Select,
        Move,
        Fill,
    }
    public class Paint : IShapeOwner
    {
        public Paint()
        {
            shapes = new List<Shape>();
            moveSpeed = new System.Drawing.Point(5, 5);
            shiftMoveSpeed = new System.Drawing.Point(1, 1);
            history = new History(32);
        }

        #region fields
        List<Shape> shapes;
        History history;

        public ShapeType shapeType;
        public PaintMode Mode
        {
            get { return mode; }
            set
            {
                switch (mode)
                {
                    case PaintMode.Select:
                        EndSelectShape();
                        break;
                    case PaintMode.Move:
                        EndSelectShape();
                        break;
                    case PaintMode.Draw:
                        EndDrawShape();
                        break;
                }
                mode = value;
            }
        }
        PaintMode mode;
        public System.Drawing.Color Color
        {
            get { return color; }
            set
            {
                color = value;
                if (mode == PaintMode.Select)
                {
                    ChangeSelectedColor(color);
                }
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
                ChangeSelectedThickness(lineThickness);
            }
        }
        UInt16 lineThickness { get; set; }
        #endregion
        #region Draw
        Shape drawShape;
        System.Drawing.Point startPoint;
        System.Drawing.Point lastPoint;

        private void InitCurrentShape(System.Drawing.Point currentPoint)
        {
            switch (GetShapeType())
            {
                case ShapeType.Square:
                    var sq = GetSquare(startPoint, currentPoint);
                    drawShape = new Rectangle(lineThickness, color, sq.Item1,sq.Item2);
                    break;
                case ShapeType.Rectangle:
                    drawShape = new Rectangle(lineThickness, color, startPoint, currentPoint);
                    break;
                case ShapeType.Circle:
                    var c = GetSquare(startPoint, currentPoint);
                    drawShape = new Ellipse(lineThickness, color, c.Item1,c.Item2);
                    break;
                case ShapeType.Diamond:
                    drawShape = new Diamond(lineThickness, color, startPoint, currentPoint);
                    break;
                case ShapeType.DiamondSquare:
                    var diamondSq = GetSquare(startPoint, currentPoint);
                    drawShape = new Diamond(lineThickness, color, diamondSq.Item1, diamondSq.Item2);
                    break;
                case ShapeType.Elipse:
                    drawShape = new Ellipse(lineThickness, color, startPoint, currentPoint);
                    break;
                case ShapeType.Line:
                    drawShape = new Line(lineThickness, color, startPoint, currentPoint);
                    break;
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
            return new Tuple<System.Drawing.Point, System.Drawing.Point>(p1,p2);
        }

        public void Draw(System.Drawing.Graphics g)
        {
            if (drawShape != null)
            {
                drawShape.Draw(g);
            }

            foreach (Shape s in shapes)
            {
                s.Draw(g);
            }
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

        public void ProcessDrawShape(System.Drawing.Point currentPoint)
        {
            if (mode != PaintMode.Draw) return;
            if (selected != null)
            {
                selected.EndSelecting();
                selected = null;
            }

            if (drawShape == null)
            {
                startPoint = currentPoint;
            }
            lastPoint = currentPoint;

            InitCurrentShape(currentPoint);
        }

        public void EndDrawShape()
        {
            if (drawShape == null) return;
            InitCurrentShape(lastPoint);
            shapes.Add(drawShape);

            selected = drawShape;
            selected.StartSelecting();

            history.Add(new DrawAction(drawShape, this));

            startPoint = new System.Drawing.Point();
            drawShape = null;
        }
        #endregion
        #region Select
        Shape selected;

        public bool StartSelectShape(System.Drawing.Point location)
        {
            if (selected != null) EndSelectShape();
            foreach (Shape s in shapes)
            {
                if (s.IsOverlapped(location))
                {
                    selected = s;
                    s.StartSelecting();
                    return true;
                }
            }
            return false;
        }

        public void SelectShape(Shape shape)
        {
            if (selected != null) EndSelectShape();
            selected = shape;
            selected.StartSelecting();
        }

        public bool Intercest(System.Drawing.Point location)
        {
            foreach (Shape s in shapes)
            {
                if (s.IsOverlapped(location))
                {
                    return true;
                }
            }
            return false;
        }

        public void EndSelectShape()
        {
            if (selected == null) return;

            selected.EndSelecting();
            selected = null;
        }
        public bool IsSelected() { return selected != null; }

        #region SelectedActions


        #region CopyPaste
        Shape copiedShape;
        public void DeleteSelectedShape()
        {
            if (!IsSelected()) return;
            history.Add(new DeleteAction(selected.Clone(), this));
            shapes.Remove(selected);
            selected = null;
        }

        public void CopySelectedShape()
        {
            if (selected == null) return;
            copiedShape = selected.Clone();
            copiedShape.EndSelecting();
        }

        public void PasteCopiedShape(System.Drawing.Point location)
        {
            if (copiedShape == null) return;

            copiedShape.MoveTo(location);
            shapes.Add(copiedShape);

            copiedShape.StartSelecting();
            selected = copiedShape;

            history.Add(new DrawAction(selected, this));
            copiedShape = null;
        }

        public void CutSelectedShape()
        {
            if (selected == null) return;
            copiedShape = selected.Clone();
            DeleteShape(selected.ID);
        }
        #endregion
        #region Move
        System.Drawing.Point moveSpeed;
        System.Drawing.Point shiftMoveSpeed;
        bool bMoving = false;
        Shape startMoveShape;
        void StartMoving()
        {
            if (!bMoving)
            {
                startMoveShape = selected.Clone();
                bMoving = true;
            }
        }

        public void MoveSelectedShape(System.Drawing.Point direction, bool shiftPressed)
        {
            if (selected == null || Mode != PaintMode.Move) return;
            StartMoving();

            var currentSpeed = shiftPressed ? shiftMoveSpeed : moveSpeed;
            selected.Move(new System.Drawing.Point(direction.X * currentSpeed.X, direction.Y * currentSpeed.Y));
        }

        public void EndMoveSelectedShape()
        {
            if (bMoving)
            {
                history.Add(new ShapeAction(startMoveShape.ID, startMoveShape, this));
                startMoveShape = null;
                bMoving = false;
            }
        }

        public void MoveSelectedTo(System.Drawing.Point currentPoint)
        {
            if (selected == null) return;
            StartMoving();
            selected.MoveTo(currentPoint);
        }
        #endregion

        public void ChangeSelectedColor(System.Drawing.Color color)
        {
            if (selected == null) return;

            selected.OutlineColor = color;
        }

        public void ChangeSelectedThickness(UInt16 thickness)
        {
            if (selected == null) return;
            selected.OutlineThickness = thickness;
        }

        #region Rotate
        public void RotateSelected(float angle)
        {
            if (selected == null) return;
            history.Add(new ShapeAction(selected.ID, selected.Clone(), this));
            selected.Angle += angle;
        }

        public void FlipSelected()
        {
            RotateSelected(90);
        }
        #endregion

        #region Fill
        public void FillSelectedShape()
        {
            if (selected == null) return;
            history.Add(new ShapeAction(selected.ID, selected.Clone(), this));
            selected.FillColor = color;
        }
        #endregion
        #endregion
        #endregion
        #region History
        #region IShapeOwner
        public void DeleteShape(string shapeId)
        {
            int index = shapes.FindIndex(s => s.ID == shapeId);
            if (index < 0) return;

            shapes.RemoveAt(index);
        }

        public void CreateShape(Shape shape)
        {
            if (shape == null) return;
            shapes.Add(shape);
        }

        public void ReplaceShape(string shapeId, Shape newShape)
        {
            if (newShape == null) return;
            DeleteShape(shapeId);
            shapes.Add(newShape);
        }
        #endregion
        public void Rollback()
        {
            if (!history.HasActions()) return;

            var last = history.Last();
            last.Rollback();
        }

        public bool Save()
        {
            return PaintPanel.Save(shapes, history);
        }

        public void SaveAs(string filename)
        {
            PaintPanel.SaveAs(shapes, history, filename);
        }

        public void Load(string filename)
        {
            var save = PaintPanel.Load(filename, this);
            shapes = save.Item1;
            foreach(Shape s in shapes)
            {
                s.EndSelecting();
            }
            history = save.Item2;
        }
        #endregion
    }

    #region Save
    [Serializable]
    public class PaintSave
    {
        public PaintSave() { }
        public PaintSave(Shape[] shapes, History history)
        {
            this.shapes = shapes;
            this.history = history;
        }
        public Shape[] shapes;
        public History history;
    }

    public class PaintPanel
    {
        private static string currentFilename;
        public static bool Save(List<Shape> shapes, History history)
        {
            if (currentFilename == "" || currentFilename == null) return false;
            using (Stream stream = File.OpenWrite(currentFilename))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, new PaintSave(shapes.ToArray(), history));
                return true;
            }
        }

        public static void SaveAs(List<Shape> shapes, History history, string filename)
        {
            currentFilename = filename;
            Save(shapes, history);
        }

        public static Tuple<List<Shape>, History> Load(string filename, IShapeOwner owner)
        {
            using (Stream stream = File.OpenRead(filename))
            {
                var formatter = new BinaryFormatter();
                var save = (PaintSave)formatter.Deserialize(stream);
                currentFilename = filename;
                var actions = save.history.GetActions().ToArray();
                for (int i = 0; i < actions.Length; ++i)
                {
                    actions[i].owner = owner;
                }
                save.history.SetActions(actions);
                return new Tuple<List<Shape>, History>(save.shapes.ToList(), save.history);
            }
        }
    }
    #endregion
}
