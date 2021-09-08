namespace HexaStones
{
    partial class Help
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
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.cbList = new System.Windows.Forms.ComboBox();
            this.tbxText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(317, 399);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(105, 50);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next ->";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(12, 399);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(105, 50);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<- Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(151, 29);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Hexa-Stones";
            // 
            // cbList
            // 
            this.cbList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbList.FormattingEnabled = true;
            this.cbList.Items.AddRange(new object[] {
            "Hexa-Stones",
            "Interface",
            "Pieces",
            "Setup/Drawing",
            "Moving",
            "Attacking",
            "How do you win"});
            this.cbList.Location = new System.Drawing.Point(301, 12);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(121, 21);
            this.cbList.TabIndex = 5;
            this.cbList.SelectedIndexChanged += new System.EventHandler(this.cbList_SelectedIndexChanged);
            // 
            // tbxText
            // 
            this.tbxText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxText.Location = new System.Drawing.Point(12, 50);
            this.tbxText.Multiline = true;
            this.tbxText.Name = "tbxText";
            this.tbxText.ReadOnly = true;
            this.tbxText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxText.Size = new System.Drawing.Size(410, 330);
            this.tbxText.TabIndex = 7;
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.tbxText);
            this.Controls.Add(this.cbList);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Name = "Help";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.Help_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cbList;
        private System.Windows.Forms.TextBox tbxText;
    }
}