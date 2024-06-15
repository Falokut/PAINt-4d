using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            InitPaint();
            InitMovement();

            modes = new Dictionary<Keys, PaintMode>();
            modes[Keys.F1] = PaintMode.Idle;
            modes[Keys.F2] = PaintMode.Draw;
            modes[Keys.F3] = PaintMode.Select;
            modes[Keys.F4] = PaintMode.Move;
            modes[Keys.F6] = PaintMode.Fill;

            this.drawPanel.MouseWheel += new System.Windows.Forms.MouseEventHandler(RotateSelected);
        }

        #region init
        void InitPaint()
        {
            service = new Service.Paint();
            service.Color = Color.Black;
            service.Mode = PaintMode.Idle;
            paintColorPickerToolStrip1.CurrentColor = service.Color;
            service.LineThickness = 1;
            service.shapeType = ShapeType.Line;

            paintColorPickerToolStrip1.OnColorChanged += ColorChanged;
            paintBrushSizeToolStripDropdown1.BrushChangedDelegate += BrushSizeChanged;

            paintShapeToolStripButton1.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton2.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton3.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton4.ShapeShanged += ShapeTypeChanged;

            paintModeToolStripButton1.ModeChanged += ModeChanged;
            paintModeToolStripButton2.ModeChanged += ModeChanged;
            paintModeToolStripButton3.ModeChanged += ModeChanged;
        }
        void InitMovement()
        {
            movement = new Dictionary<Keys, Point>();

            movement[Keys.Left] = new Point(-1, 0);
            movement[Keys.Right] = new Point(1, 0);
            movement[Keys.Up] = new Point(0, -1);
            movement[Keys.Down] = new Point(0, 1);
        }
        #endregion
        #region events
        void ColorChanged(Color color)
        {
            service.Color = color;
            if (service.Mode == PaintMode.Select || service.Mode == PaintMode.Draw) RefreshDrawZone();
        }
        void BrushSizeChanged(int newSize)
        {
            service.LineThickness = (UInt16)newSize;
            if (service.Mode == PaintMode.Select || service.Mode == PaintMode.Draw) RefreshDrawZone();
        }
        void ShapeTypeChanged(ShapeType type)
        {
            service.shapeType = type;
            ModeChanged(PaintMode.Draw);
        }

        Dictionary<Keys, PaintMode> modes;
        void HandleModeKey(KeyEventArgs e)
        {
            if (modes.TryGetValue(e.KeyCode, out PaintMode mode) && service.Mode != mode)
                ModeChanged(mode);

        }
        void ModeChanged(PaintMode mode)
        {
            service.Mode = mode;
            switch (mode)
            {
                case PaintMode.Select:
                    Cursor = System.Windows.Forms.Cursors.Hand;
                    break;
                case PaintMode.Move:
                    Cursor = System.Windows.Forms.Cursors.SizeAll;
                    break;
                case PaintMode.Draw:
                    Cursor = System.Windows.Forms.Cursors.Cross;
                    break;
                case PaintMode.Idle:
                    Cursor = System.Windows.Forms.Cursors.Default;
                    break;
                case PaintMode.Fill:
                    Cursor = new Cursor(Properties.Resources.icons8_цвет_заливки_241.Handle);
                    break;
            }
        }
        #endregion
        #region draw
        Service.Paint service;
        bool mouseDown;
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            service.Draw(e.Graphics);
        }

        #endregion

        #region KeyEvents
        Dictionary<Keys, Point> movement;

        bool cursorOnPaintZone = false;
        private void drawPanel_MouseLeave(object sender, EventArgs e)
        {
            cursorOnPaintZone = false;
        }

        private void drawPanel_MouseEnter(object sender, EventArgs e)
        {
            cursorOnPaintZone = true;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!cursorOnPaintZone) return;
            if (e.Button == MouseButtons.Middle)
            {
                service.FlipSelected();
                RefreshDrawZone();
                return;
            }


            switch (service.Mode)
            {
                case PaintMode.Select:
                    service.StartSelectShape(e.Location);
                    break;
                case PaintMode.Move:
                    if (!service.IsSelected())
                        service.StartSelectShape(e.Location); 
                    else service.MoveSelectedTo(e.Location);
                    break;
                case PaintMode.Draw:
                    if (e.Button != MouseButtons.Left) return;
                    break;
                case PaintMode.Fill:
                    if (service.Intercest(e.Location))
                    {
                        service.StartSelectShape(e.Location);
                        service.FillSelectedShape();
                        service.EndSelectShape();
                    }
                    break;
            }
            RefreshDrawZone();
            mouseDown = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown || !cursorOnPaintZone) return;
            switch (service.Mode)
            {
                case PaintMode.Move:
                    service.MoveSelectedTo(e.Location);
                    break;
                case PaintMode.Draw:
                    service.ProcessDrawShape(e.Location);
                    break;
            }
            lastMousePosition = e.Location;
            RefreshDrawZone();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mouseDown || !cursorOnPaintZone) return;
            switch (service.Mode)
            {
                case PaintMode.Draw:
                    service.EndDrawShape();
                    break;
                case PaintMode.Move:
                    service.EndMoveSelectedShape();
                    break;
            }
            mouseDown = false;
            RefreshDrawZone();
        }

        private void Rollback()
        {
            service.Rollback();
            RefreshDrawZone();
        }


        Tuple<Keys, bool> ProcessMoveKey(KeyEventArgs e)
        {
            bool bShiftDown = (e.KeyData & Keys.Shift) != 0;
            if (bShiftDown)
                return new Tuple<Keys, bool>(e.KeyData & ~Keys.Shift, true);

            return new Tuple<Keys, bool>(e.KeyData, false);
        }

        Point lastMousePosition;
        void ProcessCopyPaste(KeyEventArgs e)
        {
            if ((e.KeyData & Keys.Control) == 0) return;
            var keyData = e.KeyData & ~Keys.Control;

            switch (keyData)
            {
                case Keys.C:
                    service.CopySelectedShape();
                    break;
                case Keys.X:
                    service.CutSelectedShape();
                    RefreshDrawZone();
                    break;
                case Keys.V:
                    service.PasteCopiedShape(lastMousePosition);
                    RefreshDrawZone();
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            HandleModeKey(e);
            var keyPair = ProcessMoveKey(e);
            if (service.Mode == PaintMode.Move && movement.TryGetValue(keyPair.Item1, out Point dir))
            {
                service.MoveSelectedShape(dir, keyPair.Item2);
                RefreshDrawZone();
            }
            ProcessCopyPaste(e);

            if (e.KeyData == (Keys.Z | Keys.Control))
            {
                Rollback();
            }

            if (e.KeyData == Keys.Delete && service.IsSelected())
            {
                service.DeleteSelectedShape();
                RefreshDrawZone();
            }
            if (e.KeyData == Keys.F5)
            {
                RefreshDrawZone();
            }

            if (e.KeyData == (Keys.S | Keys.Control))
            {
                SavePaint();
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (service.Mode == PaintMode.Move &&
                (e.KeyData & (Keys.Right | Keys.Left | Keys.Up | Keys.Down)) != 0)
            {
                service.EndMoveSelectedShape();
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripLabel1.Text = $"размер холста: {drawPanel.Width}x{drawPanel.Height} px";
        }

        private void RefreshDrawZone()
        {
            drawPanel.Refresh();
        }

        #region History
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Rollback();
        }


        #endregion
        #region Save
        void SavePaint()
        {
            var tmp = Cursor;
            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (!service.Save())
            {
                SavePaintAs();
            }
            Cursor = tmp;
        }

        void SavePaintAs()
        {
            var tmp = Cursor;
            Cursor = System.Windows.Forms.Cursors.WaitCursor;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.CheckPathExists = true;
            dialog.Filter = "image files (*.PAIN)|*.PAIN|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            dialog.InitialDirectory = "C:\\Users\\user\\Downloads";
            if (dialog.ShowDialog() == DialogResult.OK) service.SaveAs(dialog.FileName);

            Cursor = tmp;
        }

        void LoadPaint()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "image files (*.PAIN)|*.PAIN|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            dialog.CheckPathExists = true;
            dialog.InitialDirectory = "C:\\Users\\user\\Downloads";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                service.Load(dialog.FileName);
                RefreshDrawZone();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePaint();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePaintAs();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPaint();
        }
        #endregion

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            service.RotateSelected(10.0f);
            RefreshDrawZone();
        }

        private void RotateSelected(object sender,MouseEventArgs e)
        {
            if (service.Mode != PaintMode.Select) return;
            service.RotateSelected(1.0f*e.Delta/ SystemInformation.MouseWheelScrollDelta);
            RefreshDrawZone();
        }
    }
}
