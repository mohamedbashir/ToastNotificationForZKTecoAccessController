namespace ToastNotification
{
    partial class ErrorForm
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
            pl_border = new Panel();
            picicon = new PictureBox();
            lb_type = new Label();
            lb_meg = new Label();
            ((System.ComponentModel.ISupportInitialize)picicon).BeginInit();
            SuspendLayout();
            // 
            // pl_border
            // 
            pl_border.BackColor = Color.FromArgb(57, 155, 53);
            pl_border.Location = new Point(-19, -3);
            pl_border.Name = "pl_border";
            pl_border.Size = new Size(34, 117);
            pl_border.TabIndex = 0;
            // 
            // picicon
            // 
            picicon.Image = Properties.Resources.icons8_success_96;
            picicon.Location = new Point(33, 18);
            picicon.Name = "picicon";
            picicon.Size = new Size(50, 51);
            picicon.SizeMode = PictureBoxSizeMode.Zoom;
            picicon.TabIndex = 1;
            picicon.TabStop = false;
            // 
            // lb_type
            // 
            lb_type.AutoSize = true;
            lb_type.Font = new Font("Arial", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lb_type.Location = new Point(89, 21);
            lb_type.Name = "lb_type";
            lb_type.Size = new Size(63, 27);
            lb_type.TabIndex = 2;
            lb_type.Text = "Type";
            // 
            // lb_meg
            // 
            lb_meg.AutoSize = true;
            lb_meg.Font = new Font("Arial", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lb_meg.Location = new Point(91, 52);
            lb_meg.Name = "lb_meg";
            lb_meg.Size = new Size(116, 19);
            lb_meg.TabIndex = 3;
            lb_meg.Text = "Toast Message";
            // 
            // ErrorForm
            // 
            AutoScaleDimensions = new SizeF(9F, 22F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(470, 90);
            Controls.Add(lb_meg);
            Controls.Add(lb_type);
            Controls.Add(picicon);
            Controls.Add(pl_border);
            Font = new Font("Arial Narrow", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ErrorForm";
            Text = "ErrorForm";
            Load += ErrorForm_Load;
            ((System.ComponentModel.ISupportInitialize)picicon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pl_border;
        private PictureBox picicon;
        private Label lb_type;
        private Label lb_meg;
    }
}