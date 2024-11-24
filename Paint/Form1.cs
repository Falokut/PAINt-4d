using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Paint.Service;

namespace Paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UpdateSize();
            paintService = new Service.Paint();
            KeyPreview = true;
            InitPaint();

            modes = new Dictionary<Keys, PaintMode>();
            modes[Keys.F1] = PaintMode.Idle;
            modes[Keys.F2] = PaintMode.Draw;
            modes[Keys.F6] = PaintMode.Fill;
            modes[Keys.F7] = PaintMode.Pen;
            modes[Keys.F8] = PaintMode.Eraser;
            modes[Keys.F9] = PaintMode.Selection;
        }

        #region init
        void UpdateSize()
        {
            bitmap = new Bitmap(drawPanel.Width, drawPanel.Height);
            g = Graphics.FromImage(bitmap);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            temp_bitmap = new Bitmap(drawPanel.Width, drawPanel.Height);
            temp_g = Graphics.FromImage(temp_bitmap);
            drawPanel.Image = bitmap;
        }

        Graphics g;
        Graphics temp_g;
        Bitmap temp_bitmap;
        void InitPaint()
        {
            paintService.Color = Color.Black;
            paintService.Mode = PaintMode.Idle;
            paintColorPickerToolStrip1.CurrentColor = paintService.Color;
            paintService.LineThickness = 1;
            paintService.shapeType = ShapeType.Line;
            paintService.OutlineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            paintColorPickerToolStrip1.OnColorChanged += ColorChanged;
            paintBrushSizeToolStripDropdown1.BrushChangedDelegate += BrushSizeChanged;

            paintShapeToolStripButton1.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton2.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton3.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton4.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton5.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton6.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton7.ShapeShanged += ShapeTypeChanged;
            paintShapeToolStripButton8.ShapeShanged += ShapeTypeChanged;

            paintModeToolStripButton1.ModeChanged += ModeChanged;
            paintModeToolStripButton2.ModeChanged += ModeChanged;
            paintModeToolStripButton3.ModeChanged += ModeChanged;

            paintOutlineDashStypeToolStripButton1.OutlineStyleChanged += OutlineStyleChanged;
            paintOutlineDashStypeToolStripButton2.OutlineStyleChanged += OutlineStyleChanged;
            paintOutlineDashStypeToolStripButton3.OutlineStyleChanged += OutlineStyleChanged;
        }
        #endregion
        #region events
        void ColorChanged(Color color)
        {
            paintService.Color = color;
            if (paintService.Mode == PaintMode.Draw) RefreshDrawZone();
        }
        void OutlineStyleChanged(System.Drawing.Drawing2D.DashStyle style)
        {
            paintService.OutlineDashStyle = style;
            if (paintService.Mode == PaintMode.Draw) RefreshDrawZone();
        }
        void BrushSizeChanged(int newSize)
        {
            paintService.LineThickness = (UInt16)newSize;
            if (paintService.Mode == PaintMode.Draw) RefreshDrawZone();
        }
        void ShapeTypeChanged(ShapeType type)
        {
            paintService.shapeType = type;
            ModeChanged(PaintMode.Draw);
        }

        Dictionary<Keys, PaintMode> modes;
        void HandleModeKey(KeyEventArgs e)
        {
            if (modes.TryGetValue(e.KeyCode, out PaintMode mode) && paintService.Mode != mode)
                ModeChanged(mode);
        }
        void ModeChanged(PaintMode mode)
        {
            paintService.Mode = mode;
            switch (mode)
            {
                case PaintMode.Draw:
                    Cursor = System.Windows.Forms.Cursors.Cross;
                    break;
                case PaintMode.Fill:
                    Cursor = new Cursor(Properties.Resources.icons8_цвет_заливки_241.Handle);
                    break;
                case PaintMode.Pen:
                    Cursor = System.Windows.Forms.Cursors.Arrow;
                    break;
                case PaintMode.Selection:
                    Cursor = System.Windows.Forms.Cursors.Hand;
                    break;
                default:
                    Cursor = System.Windows.Forms.Cursors.Default;
                    break;
            }
        }
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            toolStripLabel2.Text = $"размер холста: {drawPanel.Width}x{drawPanel.Height} px";
            //paintService.(drawPanel.Size);
        }
        #endregion
        #region draw
        Service.Paint paintService;
        Bitmap bitmap;
        bool mouseDown;

        void ReinitTempGraphics()
        {
            temp_bitmap.Dispose();
            temp_g.Dispose();
            temp_bitmap = (Bitmap)bitmap.Clone();
            temp_g = Graphics.FromImage(temp_bitmap);
            temp_g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
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

            switch (paintService.Mode)
            {
                case PaintMode.Fill:
                    paintService.Fill(e.Location, bitmap);
                    break;
                case PaintMode.Selection:
                    paintService.SetStartSelectionPoint(e.Location);
                    break;
            }
            RefreshDrawZone();
            mouseDown = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown || !cursorOnPaintZone) return;
            switch (paintService.Mode)
            {
                case PaintMode.Draw:
                    ReinitTempGraphics();
                    paintService.ProcessDrawShape(e.Location, temp_g);
                    RefreshTempDrawZone();
                    break;
                case PaintMode.Pen:
                    paintService.ChangePixelsColor(e.Location, g);
                    RefreshDrawZone();
                    break;
                case PaintMode.Eraser:
                    paintService.ChangePixelsColor(e.Location, g, Color.White);
                    RefreshDrawZone();
                    break;
                case PaintMode.Selection:
                    ReinitTempGraphics();
                    paintService.ProcessSelectArea(e.Location, temp_g);
                    RefreshTempDrawZone();
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!cursorOnPaintZone) return;
            switch (paintService.Mode)
            {
                case PaintMode.Draw:
                    paintService.EndDrawShape(e.Location, g);
                    RefreshDrawZone();
                    break;
            }
            mouseDown = false;
        }

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

            if (paintService.Mode == PaintMode.Selection)
            {
                if (e.KeyData == (Keys.C | Keys.Control))
                {
                    paintService.CopySelection();
                }
                if (e.KeyData == (Keys.X | Keys.Control))
                {
                    paintService.CutSelection();
                }
                if (e.KeyData == (Keys.V | Keys.Control))
                {
                    paintService.PasteSelection(drawPanel.PointToClient(Cursor.Position), bitmap);
                    RefreshDrawZone();
                }
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
            if (!SaveService.Save(bitmap))
                SavePaintAs();
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
            if (dialog.ShowDialog() == DialogResult.OK) SaveService.SaveAs(dialog.FileName, bitmap);
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
                bitmap = SaveService.Load(dialog.FileName);
                g = Graphics.FromImage(bitmap);
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            bitmap.Dispose();
            bitmap = new Bitmap(drawPanel.Width, drawPanel.Height);
            drawPanel.Image = bitmap;
            g = Graphics.FromImage(bitmap);
            temp_bitmap = new Bitmap(drawPanel.Width, drawPanel.Height);
            temp_g = Graphics.FromImage(temp_bitmap);
        }
    }
}
