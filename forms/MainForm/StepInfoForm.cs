
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace simplex.forms.MainForm
{
  public class StepInfoForm : Form
  {
    private IContainer components = (IContainer) null;
    private Panel Bottompanel;
    private Button Okbutton;
    private TextBox textBox;
    private bool FScrollToEnd = false;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.Bottompanel = new System.Windows.Forms.Panel();
            this.Okbutton = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.Bottompanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Bottompanel
            // 
            this.Bottompanel.Controls.Add(this.Okbutton);
            this.Bottompanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Bottompanel.Location = new System.Drawing.Point(0, 418);
            this.Bottompanel.Name = "Bottompanel";
            this.Bottompanel.Size = new System.Drawing.Size(685, 46);
            this.Bottompanel.TabIndex = 1;
            // 
            // Okbutton
            // 
            this.Okbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Okbutton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Okbutton.Location = new System.Drawing.Point(598, 11);
            this.Okbutton.Name = "Okbutton";
            this.Okbutton.Size = new System.Drawing.Size(75, 23);
            this.Okbutton.TabIndex = 0;
            this.Okbutton.Text = "OK";
            this.Okbutton.UseVisualStyleBackColor = false;
            this.Okbutton.Click += new System.EventHandler(this.Okbutton_Click);
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(685, 418);
            this.textBox.TabIndex = 2;
            // 
            // StepInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 464);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.Bottompanel);
            this.Name = "StepInfoForm";
            this.Shown += new System.EventHandler(this.StepInfoForm_Shown);
            this.Bottompanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    public StepInfoForm() => this.InitializeComponent();

    private void Okbutton_Click(object sender, EventArgs e) => this.Close();

    public TextBox txtBox => this.textBox;

    public bool ScrollToEnd
    {
      get => this.FScrollToEnd;
      set => this.FScrollToEnd = value;
    }

    private void StepInfoForm_Shown(object sender, EventArgs e)
    {
      if (!this.FScrollToEnd)
        return;
      this.txtBox.SelectionStart = this.txtBox.Text.Length;
      this.txtBox.ScrollToCaret();
    }
  }
}
