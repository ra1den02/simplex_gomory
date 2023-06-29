
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace simplex
{
  public class NewTaskForm : Form
  {
    private IContainer components = (IContainer) null;
    private Button btnCancel;
    private Button btnOk;
    private GroupBox groupBox1;
    private TextBox MtextBox;
    private Label Mlabel;
    private Label Nlabel;
    private TextBox NtextBox;

    public NewTaskForm() => this.InitializeComponent();

    public int N => Convert.ToInt32(this.NtextBox.Text);

    public int M => Convert.ToInt32(this.MtextBox.Text);

    private void NewTaskForm_Load(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MtextBox = new System.Windows.Forms.TextBox();
            this.Mlabel = new System.Windows.Forms.Label();
            this.Nlabel = new System.Windows.Forms.Label();
            this.NtextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(229, 46);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Відміна";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOk.Location = new System.Drawing.Point(229, 17);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MtextBox);
            this.groupBox1.Controls.Add(this.Mlabel);
            this.groupBox1.Controls.Add(this.Nlabel);
            this.groupBox1.Controls.Add(this.NtextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 125);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // MtextBox
            // 
            this.MtextBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.MtextBox.Location = new System.Drawing.Point(16, 89);
            this.MtextBox.Name = "MtextBox";
            this.MtextBox.Size = new System.Drawing.Size(100, 20);
            this.MtextBox.TabIndex = 9;
            this.MtextBox.Text = "4";
            // 
            // Mlabel
            // 
            this.Mlabel.AutoSize = true;
            this.Mlabel.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Mlabel.Location = new System.Drawing.Point(13, 70);
            this.Mlabel.Name = "Mlabel";
            this.Mlabel.Size = new System.Drawing.Size(144, 17);
            this.Mlabel.TabIndex = 8;
            this.Mlabel.Text = "К-ть обмежень (m):";
            // 
            // Nlabel
            // 
            this.Nlabel.AutoSize = true;
            this.Nlabel.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Nlabel.Location = new System.Drawing.Point(13, 18);
            this.Nlabel.Name = "Nlabel";
            this.Nlabel.Size = new System.Drawing.Size(117, 17);
            this.Nlabel.TabIndex = 7;
            this.Nlabel.Text = "К-ть зміних (n):";
            // 
            // NtextBox
            // 
            this.NtextBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.NtextBox.Location = new System.Drawing.Point(16, 37);
            this.NtextBox.Name = "NtextBox";
            this.NtextBox.Size = new System.Drawing.Size(100, 20);
            this.NtextBox.TabIndex = 6;
            this.NtextBox.Text = "2";
            // 
            // NewTaskForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(316, 157);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewTaskForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Умова";
            this.Load += new System.EventHandler(this.NewTaskForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

    }
  }
}
