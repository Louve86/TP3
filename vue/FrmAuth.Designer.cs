
namespace Mediatek86.vue
{
    partial class FrmAuth
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txbidentifiant = new System.Windows.Forms.TextBox();
            this.txbmdp = new System.Windows.Forms.TextBox();
            this.btnconnexion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Identifiant";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mot de passe";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(249, 119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(8, 20);
            this.textBox1.TabIndex = 2;
            // 
            // txbidentifiant
            // 
            this.txbidentifiant.Location = new System.Drawing.Point(89, 21);
            this.txbidentifiant.Name = "txbidentifiant";
            this.txbidentifiant.Size = new System.Drawing.Size(100, 20);
            this.txbidentifiant.TabIndex = 3;
            // 
            // txbmdp
            // 
            this.txbmdp.Location = new System.Drawing.Point(89, 45);
            this.txbmdp.Name = "txbmdp";
            this.txbmdp.Size = new System.Drawing.Size(100, 20);
            this.txbmdp.TabIndex = 4;
            // 
            // btnconnexion
            // 
            this.btnconnexion.Location = new System.Drawing.Point(127, 78);
            this.btnconnexion.Name = "btnconnexion";
            this.btnconnexion.Size = new System.Drawing.Size(105, 23);
            this.btnconnexion.TabIndex = 5;
            this.btnconnexion.Text = "Se connecter";
            this.btnconnexion.UseVisualStyleBackColor = true;
            this.btnconnexion.Click += new System.EventHandler(this.btnConnexion_Click);
            // 
            // FrmAuth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 112);
            this.Controls.Add(this.btnconnexion);
            this.Controls.Add(this.txbmdp);
            this.Controls.Add(this.txbidentifiant);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FrmAuth";
            this.Text = "FrmAuth";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txbidentifiant;
        private System.Windows.Forms.TextBox txbmdp;
        private System.Windows.Forms.Button btnconnexion;
    }
}