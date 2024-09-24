using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Service;

namespace Paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UpdateSize();
            KeyPreview = true;
            InitPaint();

            modes = new Dictionary<Keys, PaintMode>();
            modes[Keys.F1] = PaintMode.Idle;
            modes[Keys.F2] = PaintMode.Draw;
            modes[Keys.F6] = PaintMode.Fill;
        }

        #region init
        void UpdateSize()
        {
            bitmap = new Bitmap(drawPanel.Width, drawPanel.Height);
            g = Graphics.FromImage(bitmap);
            temp_bitmap = new Bitmap(drawPanel.Width, drawPanel.Height);
            temp_g = Graphics.FromImage(temp_bitmap);
            drawPanel.Image = bitmap;
            service = new Service.Paint();
        }

        Graphics g;
        Graphics temp_g;
        Bitmap temp_bitmap;
        void InitPaint()
        {
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

            paintModeToolStripButton3.ModeChanged += ModeChanged;
        }
        #endregion
        #region events
        void ColorChanged(Color color)
        {
            service.Color = color;
            if (service.Mode == PaintMode.Draw) RefreshDrawZone();
        }
        void BrushSizeChanged(int newSize)
        {
            service.LineThickness = (UInt16)newSize;
            if (service.Mode == PaintMode.Draw) RefreshDrawZone();
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
        Bitmap bitmap;
        bool mouseDown;

        void ReinitTempGraphics()
        {
            temp_bitmap.Dispose();
            temp_g.Dispose();
            temp_bitmap = (Bitmap)bitmap.Clone();
            temp_g = Graphics.FromImage(temp_bitmap);
        }

        #endregion

        #region KeyEvents
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

            switch (service.Mode)
            {
                case PaintMode.Draw:
                    if (e.Button != MouseButtons.Left) return;
                    break;
                case PaintMode.Fill:
                    service.Fill(e.Location, g, bitmap);
                    break;
            }
            RefreshDrawZone();
            mouseDown = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown || !cursorOnPaintZone) return;
            if (service.Mode == PaintMode.Draw)
            {
                ReinitTempGraphics();
                service.ProcessDrawShape(e.Location, temp_g);
                RefreshTempDrawZone();
            }
            lastMousePosition = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mouseDown || !cursorOnPaintZone) return;
            if (service.Mode == PaintMode.Draw)
            {
                service.EndDrawShape(g, e.Location);
                RefreshDrawZone();
            }
            mouseDown = false;

        }

        Point lastMousePosition;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            HandleModeKey(e);
            if (e.KeyData == Keys.F5)
            {
                RefreshDrawZone();
            }

            if (e.KeyData == (Keys.S | Keys.Control))
            {
                SavePaint();
            }

        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripLabel2.Text = $"размер холста: {drawPanel.Width}x{drawPanel.Height} px";
        }

        private void RefreshDrawZone()
        {
            drawPanel.Image = bitmap;
        }
        private void RefreshTempDrawZone()
        {
            drawPanel.Image = temp_bitmap;
        }

        #region Save
        void SavePaint()
        {
            var tmp = Cursor;
            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (!service.Save(bitmap))
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
            dialog.Filter = "image files (*.png)|*.png|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            dialog.InitialDirectory = "C:\\Users\\user\\Downloads";
            if (dialog.ShowDialog() == DialogResult.OK) service.SaveAs(dialog.FileName, bitmap);
            Cursor = tmp;
        }

        void LoadPaint()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "image files (*.png)|*.png|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            dialog.CheckPathExists = true;
            dialog.InitialDirectory = "C:\\Users\\user\\Downloads";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bitmap = service.Load(dialog.FileName);
                g = Graphics.FromImage(bitmap);
                toolStripLabel2.Text = $"размер холста: {drawPanel.Width}x{drawPanel.Height} px";
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

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            toolStripLabel2.Text = $"размер холста: {drawPanel.Width}x{drawPanel.Height} px";
            // service.SetCurrentSize(drawPanel.Size);
        }
    }
}
