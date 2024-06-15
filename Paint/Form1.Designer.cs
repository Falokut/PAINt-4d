namespace Paint
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.paintColorPickerToolStrip1 = new Paint.PaintColorPickerToolStrip();
            this.paintShapeToolStripButton1 = new Paint.PaintShapeToolStripButton();
            this.paintShapeToolStripButton2 = new Paint.PaintShapeToolStripButton();
            this.paintShapeToolStripButton3 = new Paint.PaintShapeToolStripButton();
            this.paintShapeToolStripButton4 = new Paint.PaintShapeToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.paintBrushSizeToolStripDropdown1 = new Paint.PaintBrushSizeToolStripDropdown();
            this.paintModeToolStripButton1 = new Paint.PaintModeToolStripButton();
            this.paintModeToolStripButton2 = new Paint.PaintModeToolStripButton();
            this.paintModeToolStripButton3 = new Paint.PaintModeToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.drawPanel = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drawPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem,
            this.сохранитьКакToolStripMenuItem,
            this.открытьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить как";
            this.сохранитьКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьКакToolStripMenuItem_Click);
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 42);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 42);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paintColorPickerToolStrip1,
            this.toolStripSeparator1,
            this.paintShapeToolStripButton1,
            this.paintShapeToolStripButton2,
            this.paintShapeToolStripButton3,
            this.paintShapeToolStripButton4,
            this.toolStripSeparator4,
            this.paintBrushSizeToolStripDropdown1,
            this.toolStripSeparator2,
            this.paintModeToolStripButton1,
            this.paintModeToolStripButton2,
            this.paintModeToolStripButton3,
            this.toolStripSeparator3,
            this.toolStripButton2,
            this.toolStripSeparator5,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.Size = new System.Drawing.Size(1184, 42);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // paintColorPickerToolStrip1
            // 
            this.paintColorPickerToolStrip1.AutoSize = false;
            this.paintColorPickerToolStrip1.CurrentColor = System.Drawing.Color.Empty;
            this.paintColorPickerToolStrip1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintColorPickerToolStrip1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.paintColorPickerToolStrip1.Margin = new System.Windows.Forms.Padding(5);
            this.paintColorPickerToolStrip1.Name = "paintColorPickerToolStrip1";
            this.paintColorPickerToolStrip1.Size = new System.Drawing.Size(32, 32);
            // 
            // paintShapeToolStripButton1
            // 
            this.paintShapeToolStripButton1.AutoSize = false;
            this.paintShapeToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintShapeToolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("paintShapeToolStripButton1.Image")));
            this.paintShapeToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintShapeToolStripButton1.Name = "paintShapeToolStripButton1";
            this.paintShapeToolStripButton1.Size = new System.Drawing.Size(32, 32);
            this.paintShapeToolStripButton1.Type = Service.ShapeType.Line;
            // 
            // paintShapeToolStripButton2
            // 
            this.paintShapeToolStripButton2.AutoSize = false;
            this.paintShapeToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintShapeToolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("paintShapeToolStripButton2.Image")));
            this.paintShapeToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintShapeToolStripButton2.Name = "paintShapeToolStripButton2";
            this.paintShapeToolStripButton2.Size = new System.Drawing.Size(32, 32);
            this.paintShapeToolStripButton2.Type = Service.ShapeType.Rectangle;
            // 
            // paintShapeToolStripButton3
            // 
            this.paintShapeToolStripButton3.AutoSize = false;
            this.paintShapeToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintShapeToolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("paintShapeToolStripButton3.Image")));
            this.paintShapeToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintShapeToolStripButton3.Name = "paintShapeToolStripButton3";
            this.paintShapeToolStripButton3.Size = new System.Drawing.Size(32, 32);
            this.paintShapeToolStripButton3.Type = Service.ShapeType.Elipse;
            // 
            // paintShapeToolStripButton4
            // 
            this.paintShapeToolStripButton4.AutoSize = false;
            this.paintShapeToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintShapeToolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("paintShapeToolStripButton4.Image")));
            this.paintShapeToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintShapeToolStripButton4.Name = "paintShapeToolStripButton4";
            this.paintShapeToolStripButton4.Size = new System.Drawing.Size(30, 30);
            this.paintShapeToolStripButton4.Type = Service.ShapeType.Diamond;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 42);
            // 
            // paintBrushSizeToolStripDropdown1
            // 
            this.paintBrushSizeToolStripDropdown1.AutoSize = false;
            this.paintBrushSizeToolStripDropdown1.AutoToolTip = false;
            this.paintBrushSizeToolStripDropdown1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintBrushSizeToolStripDropdown1.Image = global::Paint.Properties.Resources.icons8_толщина_линии_30;
            this.paintBrushSizeToolStripDropdown1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.paintBrushSizeToolStripDropdown1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintBrushSizeToolStripDropdown1.Name = "paintBrushSizeToolStripDropdown1";
            this.paintBrushSizeToolStripDropdown1.Size = new System.Drawing.Size(32, 32);
            this.paintBrushSizeToolStripDropdown1.Text = "paintBrushSizeToolStripDropdown1";
            this.paintBrushSizeToolStripDropdown1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.paintBrushSizeToolStripDropdown1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.paintBrushSizeToolStripDropdown1.ToolTipText = "ширина линий в px";
            // 
            // paintModeToolStripButton1
            // 
            this.paintModeToolStripButton1.AutoSize = false;
            this.paintModeToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintModeToolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("paintModeToolStripButton1.Image")));
            this.paintModeToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintModeToolStripButton1.Mode = Service.PaintMode.Select;
            this.paintModeToolStripButton1.Name = "paintModeToolStripButton1";
            this.paintModeToolStripButton1.Size = new System.Drawing.Size(32, 32);
            // 
            // paintModeToolStripButton2
            // 
            this.paintModeToolStripButton2.AutoSize = false;
            this.paintModeToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintModeToolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("paintModeToolStripButton2.Image")));
            this.paintModeToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintModeToolStripButton2.Mode = Service.PaintMode.Move;
            this.paintModeToolStripButton2.Name = "paintModeToolStripButton2";
            this.paintModeToolStripButton2.Size = new System.Drawing.Size(32, 32);
            // 
            // paintModeToolStripButton3
            // 
            this.paintModeToolStripButton3.AutoSize = false;
            this.paintModeToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintModeToolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("paintModeToolStripButton3.Image")));
            this.paintModeToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintModeToolStripButton3.Mode = Service.PaintMode.Fill;
            this.paintModeToolStripButton3.Name = "paintModeToolStripButton3";
            this.paintModeToolStripButton3.Size = new System.Drawing.Size(32, 32);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 42);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AutoToolTip = false;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Paint.Properties.Resources.icons8_вверх_налево_30;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 39);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "повернуть на 10 градусов против часовой";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 42);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoSize = false;
            this.toolStripButton1.AutoToolTip = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Paint.Properties.Resources.icons8_вернуть_30;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(32, 32);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "вернуть";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.Location = new System.Drawing.Point(0, 696);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1184, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(90, 22);
            this.toolStripLabel1.Text = "размер холста:";
            // 
            // drawPanel
            // 
            this.drawPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drawPanel.BackColor = System.Drawing.Color.White;
            this.drawPanel.Location = new System.Drawing.Point(40, 100);
            this.drawPanel.Margin = new System.Windows.Forms.Padding(0);
            this.drawPanel.Name = "drawPanel";
            this.drawPanel.Size = new System.Drawing.Size(1100, 560);
            this.drawPanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.drawPanel.TabIndex = 0;
            this.drawPanel.TabStop = false;
            this.drawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.drawPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.drawPanel.MouseEnter += new System.EventHandler(this.drawPanel_MouseEnter);
            this.drawPanel.MouseLeave += new System.EventHandler(this.drawPanel_MouseLeave);
            this.drawPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.drawPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1184, 721);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.drawPanel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "PAINt 4d";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drawPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox drawPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private PaintColorPickerToolStrip paintColorPickerToolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private PaintShapeToolStripButton paintShapeToolStripButton1;
        private PaintShapeToolStripButton paintShapeToolStripButton2;
        private PaintShapeToolStripButton paintShapeToolStripButton3;
        private PaintBrushSizeToolStripDropdown paintBrushSizeToolStripDropdown1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private PaintModeToolStripButton paintModeToolStripButton1;
        private PaintModeToolStripButton paintModeToolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private PaintModeToolStripButton paintModeToolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private PaintShapeToolStripButton paintShapeToolStripButton4;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

