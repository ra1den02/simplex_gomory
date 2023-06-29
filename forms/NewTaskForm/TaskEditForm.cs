
using Mehroz;
using simplex.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace simplex.forms.NewTaskForm
{
  public class TaskEditForm : Form
  {
    private int m = 2;
    private int n = 2;
    private List<ComboBox> signs = new List<ComboBox>();
    public Fraction[,] MatrixA = new Fraction[100, 100];
    public Fraction[] Vectorb = new Fraction[100];
    public Fraction[] Vectorc = new Fraction[100];
    public int[] Signs = new int[100];
    private IContainer components = (IContainer) null;
    private DataGridView MatrixAGridView;
    private Label label1;
    private Label label2;
    private DataGridView MatrixCGridView;
    private Label label3;
    private DataGridView MatrixBGridView;
    private Button btnCancel;
    private Button btnOk;
    private Panel BottomPanel;
    private ComboBox TaskTypecomboBox;

    public TaskEditForm() => this.InitializeComponent();

    public void InitForm(int an, int am)
    {
      this.n = an;
      this.m = am;
      this.MatrixAGridView.RowCount = this.m;
      this.MatrixAGridView.ColumnCount = this.n;
      this.MatrixAGridView.Height = this.MatrixAGridView.RowCount * this.MatrixAGridView.RowTemplate.Height + 3;
      this.MatrixAGridView.Width = this.MatrixAGridView.ColumnCount * 50;
      this.signs.Clear();
      for (int index = 0; index < this.m; ++index)
      {
        ComboBox comboBox = new ComboBox();
        comboBox.Parent = (Control) this;
        comboBox.Width = 50;
        comboBox.Items.Add((object) "=");
        comboBox.Items.Add((object) ">=");
        comboBox.Items.Add((object) "<=");
        comboBox.SelectedIndex = 0;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox.Left = this.MatrixAGridView.Left + this.MatrixAGridView.Width + 15;
        comboBox.Top = this.MatrixAGridView.Top + index * this.MatrixAGridView.RowTemplate.Height;
        this.signs.Add(comboBox);
      }
      this.label3.Left = this.MatrixAGridView.Left + this.MatrixAGridView.Width + 80;
      this.MatrixBGridView.Left = this.MatrixAGridView.Left + this.MatrixAGridView.Width + 80;
      this.MatrixBGridView.Height = this.MatrixAGridView.RowCount * this.MatrixAGridView.RowTemplate.Height + 3;
      this.MatrixBGridView.RowCount = this.m;
      this.MatrixBGridView.ColumnCount = 1;
      this.label2.Top = this.MatrixAGridView.Top + this.MatrixAGridView.Height + 15;
      this.MatrixCGridView.Top = this.label2.Top + this.label2.Height + 5;
      this.MatrixCGridView.RowCount = 1;
      this.MatrixCGridView.ColumnCount = this.n;
      this.MatrixCGridView.Width = this.MatrixCGridView.ColumnCount * 50;
      this.MatrixCGridView.Height = this.MatrixCGridView.RowCount * this.MatrixCGridView.RowTemplate.Height + 3;
      this.BottomPanel.Top = this.MatrixCGridView.Top + this.MatrixCGridView.Height + 15;
      if (this.label3.Width < this.MatrixBGridView.Width)
        this.BottomPanel.Width = this.MatrixBGridView.Left + this.MatrixBGridView.Width - this.BottomPanel.Left;
      else
        this.BottomPanel.Width = this.label3.Left + this.label3.Width - this.BottomPanel.Left;
      this.Width = this.BottomPanel.Left + this.BottomPanel.Width + 15;
      this.Height = this.BottomPanel.Top + this.BottomPanel.Height + 70;
      Routines.InitGridView(this.MatrixAGridView);
      Routines.InitGridView(this.MatrixBGridView);
      Routines.InitGridView(this.MatrixCGridView);
      this.TaskTypecomboBox.Top = this.btnOk.Top;
      if (this.TaskTypecomboBox.Bounds.Right > this.btnOk.Left)
        this.TaskTypecomboBox.Width = this.btnOk.Left - 30;
      this.TaskTypecomboBox.SelectedIndex = 0;
    }

    private void NewTaskForm_Load(object sender, EventArgs e)
    {
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.btnOk.DialogResult = DialogResult.OK;
      for (int index = 0; index < this.m; ++index)
        this.Signs[index] = this.signs[index].SelectedIndex;
      ((CultureInfo) CultureInfo.CurrentCulture.Clone()).NumberFormat.NumberDecimalSeparator = ",";
      for (int rowIndex = 0; rowIndex < this.MatrixAGridView.RowCount; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < this.MatrixAGridView.ColumnCount; ++columnIndex)
        {
          try
          {
            string str = this.MatrixAGridView[columnIndex, rowIndex].Value.ToString().Replace(".", ",");
            this.MatrixA[rowIndex, columnIndex] = (Fraction) str;
          }
          catch
          {
            this.btnOk.DialogResult = DialogResult.None;
            int num = (int) MessageBox.Show("Неправильний формат числа: \"" + this.MatrixAGridView[columnIndex, rowIndex].Value.ToString() + "\"", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      for (int rowIndex = 0; rowIndex < this.MatrixBGridView.RowCount; ++rowIndex)
      {
        try
        {
          string str = this.MatrixBGridView[0, rowIndex].Value.ToString().Replace(".", ",");
          this.Vectorb[rowIndex] = (Fraction) str;
        }
        catch
        {
          this.btnOk.DialogResult = DialogResult.None;
          int num = (int) MessageBox.Show("Неправильний формат числа: \"" + this.MatrixBGridView[0, rowIndex].Value.ToString() + "\"", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      for (int columnIndex = 0; columnIndex < this.MatrixCGridView.ColumnCount; ++columnIndex)
      {
        try
        {
          string str = this.MatrixCGridView[columnIndex, 0].Value.ToString().Replace(".", ",");
          this.Vectorc[columnIndex] = (Fraction) str;
        }
        catch
        {
          this.btnOk.DialogResult = DialogResult.None;
          int num = (int) MessageBox.Show("Неправильний формат числа: \"" + this.MatrixCGridView[columnIndex, 0].Value.ToString() + "\"", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      if (this.btnOk.DialogResult != DialogResult.OK)
        return;
      this.DialogResult = DialogResult.OK;
    }

    public int N => this.n;

    public int M => this.m;

    public int TaskType => this.TaskTypecomboBox.SelectedIndex;

    private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
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
            this.MatrixAGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MatrixCGridView = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.MatrixBGridView = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.TaskTypecomboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixAGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixCGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixBGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // MatrixAGridView
            // 
            this.MatrixAGridView.AllowUserToAddRows = false;
            this.MatrixAGridView.AllowUserToDeleteRows = false;
            this.MatrixAGridView.AllowUserToResizeColumns = false;
            this.MatrixAGridView.AllowUserToResizeRows = false;
            this.MatrixAGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MatrixAGridView.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.MatrixAGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MatrixAGridView.ColumnHeadersVisible = false;
            this.MatrixAGridView.Location = new System.Drawing.Point(15, 25);
            this.MatrixAGridView.Name = "MatrixAGridView";
            this.MatrixAGridView.RowHeadersVisible = false;
            this.MatrixAGridView.RowHeadersWidth = 25;
            this.MatrixAGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.MatrixAGridView.ShowCellErrors = false;
            this.MatrixAGridView.ShowCellToolTips = false;
            this.MatrixAGridView.ShowEditingIcon = false;
            this.MatrixAGridView.ShowRowErrors = false;
            this.MatrixAGridView.Size = new System.Drawing.Size(165, 64);
            this.MatrixAGridView.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Коефіцієнти обмежень:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Коефіцієнти цільової функції:";
            // 
            // MatrixCGridView
            // 
            this.MatrixCGridView.AllowUserToAddRows = false;
            this.MatrixCGridView.AllowUserToDeleteRows = false;
            this.MatrixCGridView.AllowUserToResizeColumns = false;
            this.MatrixCGridView.AllowUserToResizeRows = false;
            this.MatrixCGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MatrixCGridView.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.MatrixCGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MatrixCGridView.ColumnHeadersVisible = false;
            this.MatrixCGridView.Location = new System.Drawing.Point(15, 163);
            this.MatrixCGridView.Name = "MatrixCGridView";
            this.MatrixCGridView.RowHeadersVisible = false;
            this.MatrixCGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.MatrixCGridView.ShowCellErrors = false;
            this.MatrixCGridView.ShowCellToolTips = false;
            this.MatrixCGridView.ShowEditingIcon = false;
            this.MatrixCGridView.ShowRowErrors = false;
            this.MatrixCGridView.Size = new System.Drawing.Size(173, 49);
            this.MatrixCGridView.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(299, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Вільні члени:";
            // 
            // MatrixBGridView
            // 
            this.MatrixBGridView.AllowUserToAddRows = false;
            this.MatrixBGridView.AllowUserToDeleteRows = false;
            this.MatrixBGridView.AllowUserToResizeColumns = false;
            this.MatrixBGridView.AllowUserToResizeRows = false;
            this.MatrixBGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MatrixBGridView.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.MatrixBGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MatrixBGridView.ColumnHeadersVisible = false;
            this.MatrixBGridView.Location = new System.Drawing.Point(302, 25);
            this.MatrixBGridView.Name = "MatrixBGridView";
            this.MatrixBGridView.RowHeadersVisible = false;
            this.MatrixBGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.MatrixBGridView.Size = new System.Drawing.Size(85, 101);
            this.MatrixBGridView.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(435, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Відміна";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnOk.Location = new System.Drawing.Point(354, 244);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // BottomPanel
            // 
            this.BottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BottomPanel.Location = new System.Drawing.Point(15, 218);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(200, 1);
            this.BottomPanel.TabIndex = 8;
            // 
            // TaskTypecomboBox
            // 
            this.TaskTypecomboBox.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TaskTypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.TaskTypecomboBox.ForeColor = System.Drawing.SystemColors.MenuText;
            this.TaskTypecomboBox.FormattingEnabled = true;
            this.TaskTypecomboBox.Items.AddRange(new object[] {
            "Макс",
            "Мін"});
            this.TaskTypecomboBox.Location = new System.Drawing.Point(15, 246);
            this.TaskTypecomboBox.Name = "TaskTypecomboBox";
            this.TaskTypecomboBox.Size = new System.Drawing.Size(173, 21);
            this.TaskTypecomboBox.TabIndex = 9;
            this.TaskTypecomboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged_1);
            // 
            // TaskEditForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(522, 279);
            this.Controls.Add(this.TaskTypecomboBox);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.MatrixBGridView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MatrixCGridView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MatrixAGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskEditForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Умова";
            this.Load += new System.EventHandler(this.NewTaskForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MatrixAGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixCGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MatrixBGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
