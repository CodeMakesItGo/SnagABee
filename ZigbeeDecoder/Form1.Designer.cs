namespace ZigbeeDecoder
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
                if (_udpServer != null)
                {
                    _udpServer.Close();
                }

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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView_messages = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView_decoded = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxLabel_len = new ZigbeeDecoder.TextBoxLabel();
            this.textBoxLabel_seq = new ZigbeeDecoder.TextBoxLabel();
            this.textBoxLabel_srcadd = new ZigbeeDecoder.TextBoxLabel();
            this.textBoxLabel_srcpan = new ZigbeeDecoder.TextBoxLabel();
            this.textBoxLabel_destpan = new ZigbeeDecoder.TextBoxLabel();
            this.textBoxLabel_destadd = new ZigbeeDecoder.TextBoxLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_messages)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_decoded)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(826, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView_messages);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(826, 440);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataGridView_messages
            // 
            this.dataGridView_messages.AllowUserToAddRows = false;
            this.dataGridView_messages.AllowUserToDeleteRows = false;
            this.dataGridView_messages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_messages.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_messages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_messages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_messages.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_messages.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_messages.Name = "dataGridView_messages";
            this.dataGridView_messages.ReadOnly = true;
            this.dataGridView_messages.RowHeadersVisible = false;
            this.dataGridView_messages.RowTemplate.Height = 33;
            this.dataGridView_messages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_messages.ShowEditingIcon = false;
            this.dataGridView_messages.Size = new System.Drawing.Size(826, 198);
            this.dataGridView_messages.TabIndex = 0;
            this.dataGridView_messages.SelectionChanged += new System.EventHandler(this.dataGridView_messages_SelectionChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView_decoded, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(826, 240);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView_decoded
            // 
            this.dataGridView_decoded.AllowUserToAddRows = false;
            this.dataGridView_decoded.AllowUserToDeleteRows = false;
            this.dataGridView_decoded.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_decoded.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_decoded.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_decoded.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_decoded.Location = new System.Drawing.Point(2, 57);
            this.dataGridView_decoded.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_decoded.MultiSelect = false;
            this.dataGridView_decoded.Name = "dataGridView_decoded";
            this.dataGridView_decoded.RowHeadersVisible = false;
            this.dataGridView_decoded.RowTemplate.Height = 33;
            this.dataGridView_decoded.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView_decoded.ShowEditingIcon = false;
            this.dataGridView_decoded.Size = new System.Drawing.Size(822, 181);
            this.dataGridView_decoded.TabIndex = 1;
            this.dataGridView_decoded.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_decoded_CellBeginEdit);
            this.dataGridView_decoded.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_decoded_CellEndEdit);
            this.dataGridView_decoded.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_decoded_CellValueChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxLabel_len, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxLabel_seq, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxLabel_srcadd, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxLabel_srcpan, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxLabel_destpan, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxLabel_destadd, 2, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(822, 51);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // textBoxLabel_len
            // 
            this.textBoxLabel_len.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLabel_len.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLabel_len.Location = new System.Drawing.Point(3, 3);
            this.textBoxLabel_len.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLabel_len.Name = "textBoxLabel_len";
            this.textBoxLabel_len.Size = new System.Drawing.Size(268, 20);
            this.textBoxLabel_len.TabIndex = 0;
            // 
            // textBoxLabel_seq
            // 
            this.textBoxLabel_seq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLabel_seq.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLabel_seq.Location = new System.Drawing.Point(3, 28);
            this.textBoxLabel_seq.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLabel_seq.Name = "textBoxLabel_seq";
            this.textBoxLabel_seq.Size = new System.Drawing.Size(268, 20);
            this.textBoxLabel_seq.TabIndex = 1;
            // 
            // textBoxLabel_srcadd
            // 
            this.textBoxLabel_srcadd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLabel_srcadd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLabel_srcadd.Location = new System.Drawing.Point(276, 28);
            this.textBoxLabel_srcadd.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLabel_srcadd.Name = "textBoxLabel_srcadd";
            this.textBoxLabel_srcadd.Size = new System.Drawing.Size(268, 20);
            this.textBoxLabel_srcadd.TabIndex = 2;
            // 
            // textBoxLabel_srcpan
            // 
            this.textBoxLabel_srcpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLabel_srcpan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLabel_srcpan.Location = new System.Drawing.Point(276, 3);
            this.textBoxLabel_srcpan.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLabel_srcpan.Name = "textBoxLabel_srcpan";
            this.textBoxLabel_srcpan.Size = new System.Drawing.Size(268, 20);
            this.textBoxLabel_srcpan.TabIndex = 3;
            // 
            // textBoxLabel_destpan
            // 
            this.textBoxLabel_destpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLabel_destpan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLabel_destpan.Location = new System.Drawing.Point(549, 3);
            this.textBoxLabel_destpan.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLabel_destpan.Name = "textBoxLabel_destpan";
            this.textBoxLabel_destpan.Size = new System.Drawing.Size(270, 20);
            this.textBoxLabel_destpan.TabIndex = 4;
            // 
            // textBoxLabel_destadd
            // 
            this.textBoxLabel_destadd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLabel_destadd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLabel_destadd.Location = new System.Drawing.Point(549, 28);
            this.textBoxLabel_destadd.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLabel_destadd.Name = "textBoxLabel_destadd";
            this.textBoxLabel_destadd.Size = new System.Drawing.Size(270, 20);
            this.textBoxLabel_destadd.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 464);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Snag-a-Bee";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_messages)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_decoded)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView_messages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView_decoded;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private TextBoxLabel textBoxLabel_len;
        private TextBoxLabel textBoxLabel_seq;
        private TextBoxLabel textBoxLabel_srcadd;
        private TextBoxLabel textBoxLabel_srcpan;
        private TextBoxLabel textBoxLabel_destpan;
        private TextBoxLabel textBoxLabel_destadd;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    }
}

