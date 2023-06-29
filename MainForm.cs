
using Mehroz;
using simplex.Classes;
using simplex.forms.MainForm;
using simplex.forms.NewTaskForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace simplex
{
  public class MainForm : Form
  {
    private int SourceM = 2;
    private int SourceN = 2;
    private int SourceTaskType = 0;
    private bool IsNewTask = false;
    private int[] SourceSign = new int[100];
    private Fraction[] SourceVectorb = new Fraction[100];
    private Fraction[] SourceVectorc = new Fraction[100];
    private Fraction[,] SourceMatrixA = new Fraction[100, 100];
    private int CanonicalM = 2;
    private int CanonicalN = 2;
    private int CanonicalTaskType = 0;
    private int[] CanonicalSign = new int[100];
    private Fraction[] CanonicalVectorb = new Fraction[100];
    private Fraction[] CanonicalVectorc = new Fraction[100];
    private Fraction[,] CanonicalMatrixA = new Fraction[100, 100];
    private int currentSimplexTable = -1;
    private List<SimplexTable> SimplexTables = new List<SimplexTable>();
    private IContainer components = (IContainer) null;
    private ToolStrip toolStrip;
    private ToolStripButton SolveSimplexTasktoolStripButton;
    //private ToolStripButton ManuallyRecalctoolStripButton;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripComboBox TaskTypeComboBox;
    private ToolStripButton NewTabletoolStripButton;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripSeparator toolStripSeparator4;
    private Panel TopPanel;
    private Button ShowStepInfoFormButton;
    private StatusStrip statusStrip;
    private SplitContainer splitContainer;
        private DataGridView SimplexGrid;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private ListBox TaskLoglistBox;

    public MainForm() => this.InitializeComponent();

    private void PrintRest(
      TextBox txtBox,
      Fraction[,] Matrix,
      int[] Signs,
      Fraction[] Vectorb,
      int AIndex,
      int AN)
    {
      string str = "";
      for (int index = 0; index < AN; ++index)
        str = index <= 0 ? (!(Matrix[AIndex, index] >= (Fraction) 0L) ? str + string.Format(" - {0}*x{1}", (object) Math.Abs(Matrix[AIndex, index].ToDouble()), (object) (index + 1)) : str + string.Format("   {0}*x{1}", (object) Matrix[AIndex, index], (object) (index + 1))) : (!(Matrix[AIndex, index] >= (Fraction) 0L) ? str + string.Format(" - {0}*x{1}", (object) Math.Abs(Matrix[AIndex, index].ToDouble()), (object) (index + 1)) : str + string.Format(" + {0}*x{1}", (object) Matrix[AIndex, index], (object) (index + 1)));
      switch (Signs[AIndex])
      {
        case 0:
          str += " = ";
          break;
        case 1:
          str += " >= ";
          break;
        case 2:
          str += " <= ";
          break;
      }
      string text = !(Vectorb[AIndex] >= (Fraction) 0L) ? str + string.Format("-{0}", (object) Math.Abs(Vectorb[AIndex].ToDouble())) : str + string.Format("{0}", (object) Vectorb[AIndex]);
      txtBox.AppendText(text);
      txtBox.AppendText(Environment.NewLine);
    }

    private void PrintF(TextBox txtBox, Fraction[] Vectorc, int AN, int TaskType)
    {
      string text = "   F(x) =";
      for (int index = 0; index < AN; ++index)
        text = index <= 0 ? (!(Vectorc[index] >= (Fraction) 0L) ? text + string.Format(" - {0}*x{1}", (object) Math.Abs(Vectorc[index].ToDouble()), (object) (index + 1)) : text + string.Format("   {0}*x{1}", (object) Vectorc[index], (object) (index + 1))) : (!(Vectorc[index] >= (Fraction) 0L) ? text + string.Format(" - {0}*x{1}", (object) Math.Abs(Vectorc[index].ToDouble()), (object) (index + 1)) : text + string.Format(" + {0}*x{1}", (object) Vectorc[index], (object) (index + 1)));
      switch (TaskType)
      {
        case 0:
          text += " -> max";
          break;
        case 1:
          text += " -> min";
          break;
      }
      txtBox.AppendText(text);
      txtBox.AppendText(Environment.NewLine);
    }

    private void PrintSolveLog(TextBox txtBox)
    {
      if (this.IsNewTask)
      {
        this.CanonicalN = this.SourceN;
        this.CanonicalM = this.SourceM;
        this.CanonicalTaskType = this.SourceTaskType;
        for (int index1 = 0; index1 < this.SourceM; ++index1)
        {
          this.CanonicalSign[index1] = this.SourceSign[index1];
          this.CanonicalVectorb[index1] = this.SourceVectorb[index1];
          for (int index2 = 0; index2 < this.SourceN; ++index2)
          {
            this.CanonicalVectorc[index2] = this.SourceVectorc[index2];
            this.CanonicalMatrixA[index1, index2] = this.SourceMatrixA[index1, index2];
          }
        }
        txtBox.AppendText("Початкова умова:");
        txtBox.AppendText(Environment.NewLine);
        txtBox.AppendText(Environment.NewLine);
        for (int AIndex = 0; AIndex < this.SourceM; ++AIndex)
          this.PrintRest(txtBox, this.SourceMatrixA, this.SourceSign, this.SourceVectorb, AIndex, this.SourceN);
        txtBox.AppendText(Environment.NewLine);
        this.PrintF(txtBox, this.SourceVectorc, this.SourceN, this.SourceTaskType);
        txtBox.AppendText(Environment.NewLine);
        txtBox.AppendText("Приводимо задачу до канонічного вигляду: ");
        int num1 = 1;
        bool flag = true;
        for (int AIndex = 0; AIndex < this.SourceM; ++AIndex)
        {
          switch (this.SourceSign[AIndex])
          {
            case 0:
              flag = false;
              txtBox.AppendText(Environment.NewLine);
              txtBox.AppendText(string.Format("{0}. Позбавляємося знаку рівності в обмеженні - {1}. Щоб змінити знак '=' на нерівність '<=', вводимо ще одне таке ж обмеження, але з протилежними знаками: ", (object) num1, (object) (AIndex + 1)));
              txtBox.AppendText(Environment.NewLine);
              ++this.CanonicalM;
              for (int index = 0; index < this.CanonicalN; ++index)
                this.CanonicalMatrixA[this.CanonicalM - 1, index] = -this.CanonicalMatrixA[AIndex, index];
              this.CanonicalVectorb[this.CanonicalM - 1] = -this.CanonicalVectorb[AIndex];
              this.CanonicalSign[AIndex] = 2;
              this.CanonicalSign[this.CanonicalM - 1] = 2;
              txtBox.AppendText(Environment.NewLine);
              this.PrintRest(txtBox, this.CanonicalMatrixA, this.CanonicalSign, this.CanonicalVectorb, this.CanonicalM - 1, this.CanonicalN);
              ++num1;
              break;
            case 1:
              flag = false;
              txtBox.AppendText(Environment.NewLine);
              txtBox.AppendText(string.Format("{0}. Позбавляємося знаку більше-рівне в обмеженні - {1}. Щоб змінити знак нерівності '>=' на нерівність '<=', змінюємо знаки у цьому обмежені на протилежні: ", (object) num1, (object) (AIndex + 1)));
              txtBox.AppendText(Environment.NewLine);
              this.CanonicalVectorb[AIndex] = -this.CanonicalVectorb[AIndex];
              for (int index = 0; index < this.CanonicalN; ++index)
                this.CanonicalMatrixA[AIndex, index] = -this.CanonicalMatrixA[AIndex, index];
              this.CanonicalSign[AIndex] = 2;
              txtBox.AppendText(Environment.NewLine);
              this.PrintRest(txtBox, this.CanonicalMatrixA, this.CanonicalSign, this.CanonicalVectorb, AIndex, this.CanonicalN);
              ++num1;
              break;
          }
        }
        if (this.SourceTaskType == 0)
        {
          for (int index = 0; index < this.CanonicalN; ++index)
            this.CanonicalVectorc[index] = -this.CanonicalVectorc[index];
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText(string.Format("{0}. Це задача на максимізацію, тому змінюємо знаки при цільовій функції:", (object) num1));
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText(Environment.NewLine);
          this.PrintF(txtBox, this.CanonicalVectorc, this.CanonicalN, this.CanonicalTaskType);
          int num2 = num1 + 1;
        }
        if (flag)
        {
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText("Задача уже записана в канонічній формі, тому не потребує додаткових змін.");
          txtBox.AppendText(Environment.NewLine);
        }
        else
        {
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText("Записуємо задачу у канонічний вигляд, після змін: ");
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText(Environment.NewLine);
          for (int AIndex = 0; AIndex < this.CanonicalM; ++AIndex)
            this.PrintRest(txtBox, this.CanonicalMatrixA, this.CanonicalSign, this.CanonicalVectorb, AIndex, this.CanonicalN);
          txtBox.AppendText(Environment.NewLine);
          this.PrintF(txtBox, this.CanonicalVectorc, this.CanonicalN, this.CanonicalTaskType);
          txtBox.AppendText(Environment.NewLine);
        }
      }
      else
      {
        txtBox.AppendText("Початкова умова:");
        txtBox.AppendText(Environment.NewLine);
        txtBox.AppendText(Environment.NewLine);
        for (int AIndex = 0; AIndex < this.CanonicalM; ++AIndex)
          this.PrintRest(txtBox, this.CanonicalMatrixA, this.CanonicalSign, this.CanonicalVectorb, AIndex, this.CanonicalN);
        txtBox.AppendText(Environment.NewLine);
        this.PrintF(txtBox, this.CanonicalVectorc, this.CanonicalN, this.CanonicalTaskType);
        txtBox.AppendText(Environment.NewLine);
        txtBox.AppendText(Environment.NewLine);
        txtBox.AppendText("Задача уже записана в канонічній формі, тому не потребує додаткових змін.");
        txtBox.AppendText(Environment.NewLine);
      }
      for (int index3 = 0; index3 < this.SimplexTables.Count; ++index3)
      {
        SimplexTable simplexTable = this.SimplexTables[index3];
        txtBox.AppendText(Environment.NewLine);
        txtBox.AppendText("----------------------------");
        if (simplexTable.Solved)
        {
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText("Серед значень коефіцієнтів цільової функції немає від'ємних. Тому отриманий оптимальний розв'язок!");
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText(Environment.NewLine);
          for (int index4 = 0; index4 < simplexTable.N; ++index4)
          {
            txtBox.AppendText(string.Format("x{0} = {1}", (object) (index4 + 1), (object) simplexTable.Roots(index4)));
            txtBox.AppendText(Environment.NewLine);
          }
          txtBox.AppendText(Environment.NewLine);
          if (simplexTable.TaskType == 1)
          {
            txtBox.AppendText("Оскільки вихідним завданням був пошук мінімуму, оптимальне рішення є вільний член рядка F, взятий із протилежним знаком.");
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText(string.Format("Значення цільової функції: Fmin = {0}", (object) -simplexTable.FunctionValue()));
            break;
          }
          txtBox.AppendText(string.Format("Значення цільової функції: Fmin = {0}", (object) simplexTable.FunctionValue()));
          break;
        }
        if (simplexTable.NonIntegerSolved)
        {
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText("Серед значень коефіцієнтів цільової функції немає від'ємних. Тому отриманий оптимальний розв'язок!");
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText(Environment.NewLine);
          for (int index5 = 0; index5 < simplexTable.N; ++index5)
          {
            txtBox.AppendText(string.Format("x{0} = {1}", (object) (index5 + 1), (object) simplexTable.RootsFraction(index5).ToString()));
            txtBox.AppendText(Environment.NewLine);
          }
          txtBox.AppendText(Environment.NewLine);
          if (simplexTable.TaskType == 1)
          {
            txtBox.AppendText("Оскільки вихідним завданням був пошук мінімуму, оптимальне рішення є вільний член рядка F, взятий із протилежним знаком.");
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText(string.Format("Значення цільової функції: Fmin = {0}", (object) (-simplexTable.FunctionValueFraction()).ToString()));
          }
          else
            txtBox.AppendText(string.Format("Значення цільової функції: Fmin = {0}", (object) simplexTable.FunctionValueFraction().ToString()));
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText("Оптимальне рішення не є цілочисельним. Застосуємо метод Гоморі. Серед вільних членів знаходимо змінну з максимальним дробовим числом:");
          txtBox.AppendText(Environment.NewLine);
          int num3 = 0;
          double num4 = 0.0;
          txtBox.AppendText(Environment.NewLine);
          for (int index6 = 0; index6 < simplexTable.M; ++index6)
          {
            if (simplexTable.BaseVector(index6) <= simplexTable.N)
            {
              double num5 = Math.Abs(simplexTable.Table(index6, 0)) - Math.Abs(Math.Floor(simplexTable.Table(index6, 0)));
              if (num4 < num5)
              {
                num4 = num5;
                num3 = index6;
              }
              txtBox.AppendText(string.Format("x{0} = {1} = {2}", (object) simplexTable.BaseVector(index6), (object) simplexTable.FractionTable(index6, 0).ToString(), (object) simplexTable.Table(index6, 0)));
              txtBox.AppendText(Environment.NewLine);
            }
          }
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText("x" + simplexTable.BaseVector(num3).ToString() + " - вільний член із максимальним дробовим числом. Тому вводимо додаткове обмеження щодо " + (object) (num3 + 1) + " рядка:");
          txtBox.AppendText(Environment.NewLine);
          string str1 = "";
          txtBox.AppendText(Environment.NewLine);
          double num6 = simplexTable.Table(num3, 0) / simplexTable.Table(num3, 0);
          Fraction fraction1 = new Fraction((string) simplexTable.FractionTable(num3, 0));
          if (fraction1 < (Fraction) 0L)
            fraction1 *= -1;
          Fraction fraction2 = fraction1 - Math.Truncate(Math.Abs(simplexTable.FractionTable(num3, 0).ToDouble()));
          bool flag = true;
          Fraction fraction3 = fraction2 * num6 * -1;
          string str2 = str1 + fraction3.ToString() + " = ";
          for (int ColIndex = 1; ColIndex < simplexTable.Сols; ++ColIndex)
          {
            Fraction fraction4;
            if (simplexTable.FractionTable(num3, ColIndex) >= (Fraction) 0L)
            {
              double num7 = Math.Truncate(simplexTable.FractionTable(num3, ColIndex).ToDouble());
              fraction4 = -(simplexTable.FractionTable(num3, ColIndex) - num7);
            }
            else
            {
              double num8 = Math.Abs(Math.Truncate(simplexTable.FractionTable(num3, ColIndex).ToDouble()));
              fraction4 = -(simplexTable.FractionTable(num3, ColIndex) + num8 + 1);
            }
            if (fraction4 > (Fraction) 0L)
            {
              if (flag)
                str2 = str2 + fraction4.ToString() + "x" + (object) ColIndex;
              else
                str2 = str2 + " + " + fraction4.ToString() + "x" + (object) ColIndex;
              flag = false;
            }
            else
            {
              str2 = str2 + " - " + (fraction4 * -1).ToString() + "x" + (object) ColIndex;
              flag = false;
            }
          }
          string text = str2 + " + x" + (object) (simplexTable.N + simplexTable.M + 1);
          txtBox.AppendText(text);
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText(Environment.NewLine);
          txtBox.AppendText("Перераховуємо отриману таблицю:");
        }
        else
        {
          if (simplexTable.Infinity)
          {
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText("Оптимального рішення не існує! Дана задача немає обмежень.");
            break;
          }
          if (simplexTable.Unsolvable)
          {
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText("Оскільки у рядку з від'ємним вільним членом немає від'ємних елементів, задача немає рішень.");
            break;
          }
          if (simplexTable.PivotCol != -1 || simplexTable.PivotRow != -1)
          {
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText(string.Format("Ітерація № {0}:", (object) (index3 + 1)));
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText(string.Format("Напрямний стовбець: {0}", (object) this.SimplexTables[index3].PivotCol));
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText(string.Format("Напрямний рядок: {0}", (object) (this.SimplexTables[index3].PivotRow + 1)));
          }
        }
      }
    }

    private void UpdateSimplexGrid()
    {
      if (!(this.currentSimplexTable >= 0 & this.currentSimplexTable < this.SimplexTables.Count))
        return;
      SimplexTable simplexTable = this.SimplexTables[this.currentSimplexTable];
      this.SimplexGrid.RowCount = 1;
      this.SimplexGrid.ColumnCount = 1;
      this.SimplexGrid.RowCount = simplexTable.M + 1;
      this.SimplexGrid.ColumnCount = simplexTable.N + simplexTable.M + 2;
      this.SimplexGrid.Columns[0].HeaderText = "b";
      for (int index = 1; index < this.SimplexGrid.ColumnCount - 1; ++index)
        this.SimplexGrid.Columns[index].HeaderText = "x" + index.ToString();
      this.SimplexGrid.Columns[this.SimplexGrid.ColumnCount - 1].HeaderText = "O.O";
      for (int index = 0; index < this.SimplexGrid.RowCount - 1; ++index)
        this.SimplexGrid.Rows[index].HeaderCell.Value = (object) ("x" + (object) simplexTable.BaseVector(index));
      if (this.TaskTypeComboBox.SelectedIndex == 0)
        this.SimplexGrid.Rows[this.SimplexGrid.RowCount - 1].HeaderCell.Value = (object) "Fmax";
      else
        this.SimplexGrid.Rows[this.SimplexGrid.RowCount - 1].HeaderCell.Value = (object) "Fmin";
      for (int rowIndex = 0; rowIndex < this.SimplexGrid.RowCount; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < this.SimplexGrid.ColumnCount; ++columnIndex)
          this.SimplexGrid[columnIndex, rowIndex].Style.BackColor = Color.White;
      }
      if (simplexTable.PivotRow != -1)
      {
        for (int columnIndex = 0; columnIndex < this.SimplexGrid.ColumnCount; ++columnIndex)
          this.SimplexGrid[columnIndex, simplexTable.PivotRow].Style.BackColor = Color.LightYellow;
      }
      if (simplexTable.PivotCol != -1)
      {
        for (int rowIndex = 0; rowIndex < this.SimplexGrid.RowCount; ++rowIndex)
          this.SimplexGrid[simplexTable.PivotCol, rowIndex].Style.BackColor = Color.LightYellow;
        simplexTable.CalcMeritRestrictions(simplexTable.PivotCol);
      }
      if (simplexTable.PivotRow != -1 & simplexTable.PivotCol != -1)
      {
        for (int index = 0; index < simplexTable.M; ++index)
          this.SimplexGrid[this.SimplexGrid.ColumnCount - 1, index].Value = simplexTable.MRVector(index) >= 0.0 ? (object) simplexTable.MRVector(index) : (object) "Не обмежена.";
      }
      else
      {
        for (int rowIndex = 0; rowIndex < simplexTable.M; ++rowIndex)
          this.SimplexGrid[this.SimplexGrid.ColumnCount - 1, rowIndex].Value = (object) "";
      }
      simplexTable.PrintTable(this.SimplexGrid);
    }

    private void UpdateTaskLoglistBox()
    {
      this.TaskLoglistBox.Items.Clear();
      for (int index1 = 0; index1 < this.SimplexTables.Count; ++index1)
      {
        SimplexTable simplexTable = this.SimplexTables[index1];
        int num;
        if (simplexTable.PivotCol != -1 & simplexTable.PivotRow != -1)
        {
          ListBox.ObjectCollection items = this.TaskLoglistBox.Items;
          string[] strArray1 = new string[6];
          strArray1[0] = "Таблиця ";
          string[] strArray2 = strArray1;
          num = index1 + 1;
          string str1 = num.ToString();
          strArray2[1] = str1;
          strArray1[2] = ": Напрямний рядок: x";
          string[] strArray3 = strArray1;
          num = simplexTable.BaseVector(simplexTable.PivotRow);
          string str2 = num.ToString();
          strArray3[3] = str2;
          strArray1[4] = ", Напрямний стовбець: x";
          string[] strArray4 = strArray1;
          num = simplexTable.PivotCol;
          string str3 = num.ToString();
          strArray4[5] = str3;
          string str4 = string.Concat(strArray1);
          items.Add((object) str4);
        }
        else
        {
          ListBox.ObjectCollection items = this.TaskLoglistBox.Items;
          num = index1 + 1;
          string str = "Таблиця " + num.ToString();
          items.Add((object) str);
        }
        if (simplexTable.NonIntegerSolved & !simplexTable.ManuallyRecalcFlag)
          this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1] = (object) (this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1].ToString() + ": Найдено оптимальне нецілочисленне рішення. Застосуємо метод Гоморі.");
        if (simplexTable.Solved)
        {
          this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1] = (object) (this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1].ToString() + ". Знайдено оптимальне рішення!");
          this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1] = simplexTable.TaskType != 0 ? (object) (this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1].ToString() + " Значення цільової функції: " + (-1.0 * simplexTable.Table(simplexTable.M, 0)).ToString()) : (object) (this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1].ToString() + " Значенння цільової функції: " + simplexTable.Table(simplexTable.M, 0).ToString());
          this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1] = (object) (this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1].ToString() + ". Значення зміних: ");
          double[] numArray = new double[100];
          for (int index2 = 0; index2 < simplexTable.N; ++index2)
            numArray[index2] = 0.0;
          for (int index3 = 0; index3 < simplexTable.M; ++index3)
          {
            if (simplexTable.BaseVector(index3) - 1 <= simplexTable.N - 1)
              numArray[simplexTable.BaseVector(index3) - 1] = simplexTable.Table(index3, 0);
          }
          for (int index4 = 0; index4 < simplexTable.N; ++index4)
          {
            ListBox.ObjectCollection items = this.TaskLoglistBox.Items;
            int index5 = this.TaskLoglistBox.Items.Count - 1;
            object[] objArray1 = new object[6]
            {
              this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1],
              (object) "x",
              null,
              null,
              null,
              null
            };
            object[] objArray2 = objArray1;
            num = index4 + 1;
            string str5 = num.ToString();
            objArray2[2] = (object) str5;
            objArray1[3] = (object) " = ";
            objArray1[4] = (object) numArray[index4].ToString();
            objArray1[5] = (object) "; ";
            string str6 = string.Concat(objArray1);
            items[index5] = (object) str6;
          }
        }
        if (simplexTable.Infinity)
          this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1] = (object) (this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1].ToString() + ". Задача не обмежена!");
        if (simplexTable.Unsolvable)
          this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1] = (object) (this.TaskLoglistBox.Items[this.TaskLoglistBox.Items.Count - 1].ToString() + ". Задача немає розв'язків!");
      }
      if (!(this.currentSimplexTable >= 0 & this.currentSimplexTable < this.SimplexTables.Count))
        return;
      this.TaskLoglistBox.SelectedIndex = this.currentSimplexTable;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      this.CloseTableMode();
      this.TaskTypeComboBox.SelectedIndex = 0;
    }

    private void CloseTableMode()
    {
      this.TaskTypeComboBox.Enabled = false;
      this.ShowStepInfoFormButton.Enabled = false;
      this.SolveSimplexTasktoolStripButton.Enabled = false;
    }

    private void OpenTableMode()
    {
      this.TaskTypeComboBox.Enabled = true;
      this.ShowStepInfoFormButton.Enabled = true;
      this.SolveSimplexTasktoolStripButton.Enabled = true;
    }

    private void TaskLoglistBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.currentSimplexTable = this.TaskLoglistBox.SelectedIndex;
      this.UpdateSimplexGrid();
    }

    public bool ValidPivotColRow(int pivotCol, int pivotRow)
    {
      bool flag = true;
      if (this.currentSimplexTable >= 0 & this.currentSimplexTable < this.SimplexTables.Count)
      {
        SimplexTable simplexTable = this.SimplexTables[this.currentSimplexTable];
        simplexTable.CalcMeritRestrictions(pivotCol);
        flag = simplexTable.MRVector(pivotRow) >= 0.0;
      }
      return flag;
    }

    public void SetPivotColRow(int col, int row)
    {
      if (!this.SimplexGrid.Visible)
        return;
      for (int rowIndex = 0; rowIndex < this.SimplexGrid.RowCount; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < this.SimplexGrid.ColumnCount; ++columnIndex)
          this.SimplexGrid[columnIndex, rowIndex].Style.BackColor = Color.White;
      }
      if (col != -1 & row != -1)
      {
        for (int rowIndex = 0; rowIndex < this.SimplexGrid.RowCount; ++rowIndex)
          this.SimplexGrid[col + 1, rowIndex].Style.BackColor = Color.LightYellow;
        for (int columnIndex = 0; columnIndex < this.SimplexGrid.ColumnCount; ++columnIndex)
          this.SimplexGrid[columnIndex, row].Style.BackColor = Color.LightYellow;
        if (this.currentSimplexTable >= 0 & this.currentSimplexTable < this.SimplexTables.Count)
        {
          SimplexTable simplexTable = this.SimplexTables[this.currentSimplexTable];
          int lpivotCol = col + 1;
          int num = row + 1;
          simplexTable.CalcMeritRestrictions(lpivotCol);
          if (num != -1 & lpivotCol != -1)
          {
            for (int index = 0; index < simplexTable.M; ++index)
              this.SimplexGrid[this.SimplexGrid.ColumnCount - 1, index].Value = simplexTable.MRVector(index) >= 0.0 ? (object) simplexTable.MRVector(index) : (object) "Не обмежена.";
          }
          else
          {
            for (int rowIndex = 0; rowIndex < simplexTable.M; ++rowIndex)
              this.SimplexGrid[this.SimplexGrid.ColumnCount - 1, rowIndex].Value = (object) "";
          }
        }
      }
      else
      {
        for (int rowIndex = 0; rowIndex < this.SimplexGrid.RowCount - 1; ++rowIndex)
          this.SimplexGrid[this.SimplexGrid.ColumnCount - 1, rowIndex].Value = (object) "";
      }
    }

    private void TaskTypeComboBox_DropDownClosed(object sender, EventArgs e)
    {
      if (this.TaskTypeComboBox.SelectedIndex == 0)
        this.SimplexGrid.Rows[this.SimplexGrid.RowCount - 1].HeaderCell.Value = (object) "Fmax";
      else
        this.SimplexGrid.Rows[this.SimplexGrid.RowCount - 1].HeaderCell.Value = (object) "Fmin";
      if (!(this.currentSimplexTable >= 0 & this.currentSimplexTable < this.SimplexTables.Count))
        return;
      this.SimplexTables[this.currentSimplexTable].TaskType = this.TaskTypeComboBox.SelectedIndex;
    }


    private void ShowStepInfoFormButton_Click(object sender, EventArgs e)
    {
      StepInfoForm stepInfoForm = new StepInfoForm();
      this.PrintSolveLog(stepInfoForm.txtBox);
      int num = (int) stepInfoForm.ShowDialog();
    }

    private void TaskTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.TaskTypeComboBox.SelectedIndex == this.SourceTaskType)
        return;
      this.SourceTaskType = this.TaskTypeComboBox.SelectedIndex;
      this.SimplexTables.Clear();
      this.TaskLoglistBox.Items.Clear();
      if (!this.IsNewTask)
      {
        this.CanonicalTaskType = this.TaskTypeComboBox.SelectedIndex;
        for (int index = 0; index < this.CanonicalN; ++index)
          this.CanonicalVectorc[index] = -this.CanonicalVectorc[index];
      }
      this.PrintSolveLog(new StepInfoForm().txtBox);
      SimplexTable simplexTable = new SimplexTable(this.CanonicalN, this.CanonicalM);
      simplexTable.GetTable(this.CanonicalMatrixA, this.CanonicalVectorb, this.CanonicalVectorc);
      simplexTable.TaskType = this.CanonicalTaskType;
      this.SimplexTables.Add(simplexTable);
      this.currentSimplexTable = this.SimplexTables.Count - 1;
      this.UpdateSimplexGrid();
      this.UpdateTaskLoglistBox();
      this.OpenTableMode();
    }

    private void SolveSimplexTaskToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.currentSimplexTable >= 0 & this.currentSimplexTable < this.SimplexTables.Count)
      {
        SimplexTable simplexTable1 = this.SimplexTables[this.currentSimplexTable];
        if (simplexTable1.FillTable(this.SimplexGrid))
        {
          while (!simplexTable1.Solved & !simplexTable1.Unsolvable & !simplexTable1.Infinity)
          {
            if (simplexTable1.TestForInfinity())
            {
              simplexTable1.Infinity = true;
              this.UpdateTaskLoglistBox();
            }
            if (simplexTable1.TestForUnsolvable())
            {
              simplexTable1.Unsolvable = true;
              this.UpdateTaskLoglistBox();
            }
            if (simplexTable1.TestForSolved())
            {
              if (simplexTable1.TestForInteger())
              {
                simplexTable1.Solved = true;
                this.UpdateTaskLoglistBox();
              }
              else
              {
                simplexTable1.NonIntegerSolved = true;
                simplexTable1.PivotRow = simplexTable1.MaxDoubleIndex();
                SimplexTable simplexTable2 = new SimplexTable(simplexTable1.N, simplexTable1.M);
                simplexTable2.Assign(simplexTable1);
                simplexTable2.FillTable(this.SimplexGrid);
                while (this.TaskLoglistBox.SelectedIndex < this.SimplexTables.Count - 1)
                  this.SimplexTables.RemoveAt(this.TaskLoglistBox.SelectedIndex + 1);
                simplexTable2.AddNewRest();
                this.SimplexTables.Add(simplexTable2);
                this.currentSimplexTable = this.SimplexTables.Count - 1;
                simplexTable1 = this.SimplexTables[this.currentSimplexTable];
                this.UpdateSimplexGrid();
                this.UpdateTaskLoglistBox();
                continue;
              }
            }
            if (simplexTable1.FindOptimalStep())
            {
              SimplexTable simplexTable3 = new SimplexTable(simplexTable1.N, simplexTable1.M);
              simplexTable3.Assign(simplexTable1);
              simplexTable3.FillTable(this.SimplexGrid);
              while (this.TaskLoglistBox.SelectedIndex < this.SimplexTables.Count - 1)
                this.SimplexTables.RemoveAt(this.TaskLoglistBox.SelectedIndex + 1);
              this.SimplexTables.Add(simplexTable3);
              this.currentSimplexTable = this.SimplexTables.Count - 1;
              simplexTable1 = this.SimplexTables[this.currentSimplexTable];
              ++simplexTable3.PivotRow;
              simplexTable3.RecalcTable();
              simplexTable3.PivotRow = -1;
              simplexTable3.PivotCol = -1;
              this.UpdateSimplexGrid();
              this.UpdateTaskLoglistBox();
            }
          }
        }
      }
      StepInfoForm stepInfoForm = new StepInfoForm();
      this.PrintSolveLog(stepInfoForm.txtBox);
      stepInfoForm.ScrollToEnd = true;
      int num = (int) stepInfoForm.ShowDialog();
    }

    private void RecalcSimplexTableToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(this.currentSimplexTable >= 0 & this.currentSimplexTable < this.SimplexTables.Count))
        return;
      SimplexTable simplexTable1 = this.SimplexTables[this.currentSimplexTable];
      if (simplexTable1.FillTable(this.SimplexGrid) && !simplexTable1.Solved & !simplexTable1.Unsolvable & !simplexTable1.Infinity)
      {
        if (simplexTable1.TestForInfinity())
        {
          simplexTable1.Infinity = true;
          this.UpdateTaskLoglistBox();
        }
        if (simplexTable1.TestForUnsolvable())
        {
          simplexTable1.Unsolvable = true;
          this.UpdateTaskLoglistBox();
        }
        if (simplexTable1.TestForSolved())
        {
          if (simplexTable1.TestForInteger())
          {
            simplexTable1.Solved = true;
            this.UpdateTaskLoglistBox();
          }
          else
          {
            simplexTable1.ManuallyRecalcFlag = false;
            simplexTable1.NonIntegerSolved = true;
            simplexTable1.PivotRow = simplexTable1.MaxDoubleIndex();
            SimplexTable simplexTable2 = new SimplexTable(simplexTable1.N, simplexTable1.M);
            simplexTable2.Assign(simplexTable1);
            simplexTable2.FillTable(this.SimplexGrid);
            while (this.TaskLoglistBox.SelectedIndex < this.SimplexTables.Count - 1)
              this.SimplexTables.RemoveAt(this.TaskLoglistBox.SelectedIndex + 1);
            simplexTable2.AddNewRest();
            this.SimplexTables.Add(simplexTable2);
            this.currentSimplexTable = this.SimplexTables.Count - 1;
            SimplexTable simplexTable3 = this.SimplexTables[this.currentSimplexTable];
            this.UpdateSimplexGrid();
            this.UpdateTaskLoglistBox();
            return;
          }
        }
        if (simplexTable1.FindOptimalStep())
        {
          SimplexTable simplexTable4 = new SimplexTable(simplexTable1.N, simplexTable1.M);
          simplexTable4.Assign(simplexTable1);
          simplexTable4.FillTable(this.SimplexGrid);
          while (this.TaskLoglistBox.SelectedIndex < this.SimplexTables.Count - 1)
            this.SimplexTables.RemoveAt(this.TaskLoglistBox.SelectedIndex + 1);
          this.SimplexTables.Add(simplexTable4);
          this.currentSimplexTable = this.SimplexTables.Count - 1;
          ++simplexTable4.PivotRow;
          simplexTable4.RecalcTable();
          simplexTable4.PivotRow = -1;
          simplexTable4.PivotCol = -1;
          this.UpdateSimplexGrid();
          this.UpdateTaskLoglistBox();
        }
      }
    }

    private void NewTaskToolStripMenuItem_Click(object sender, EventArgs e)
    {
      simplex.NewTaskForm newTaskForm = new simplex.NewTaskForm();
      if (newTaskForm.ShowDialog() != DialogResult.OK)
        return;
      TaskEditForm taskEditForm = new TaskEditForm();
      taskEditForm.InitForm(newTaskForm.N, newTaskForm.M);
      if (taskEditForm.ShowDialog() == DialogResult.OK)
      {
        this.IsNewTask = true;
        this.TaskTypeComboBox.SelectedIndex = taskEditForm.TaskType;
        this.SimplexTables.Clear();
        this.SourceN = taskEditForm.N;
        this.SourceM = taskEditForm.M;
        this.SourceSign = taskEditForm.Signs;
        this.SourceVectorb = taskEditForm.Vectorb;
        this.SourceVectorc = taskEditForm.Vectorc;
        this.SourceMatrixA = taskEditForm.MatrixA;
        this.SourceTaskType = taskEditForm.TaskType;
        StepInfoForm stepInfoForm = new StepInfoForm();
        this.PrintSolveLog(stepInfoForm.txtBox);
        SimplexTable simplexTable = new SimplexTable(this.CanonicalN, this.CanonicalM);
        simplexTable.GetTable(this.CanonicalMatrixA, this.CanonicalVectorb, this.CanonicalVectorc);
        simplexTable.TaskType = this.CanonicalTaskType;
        this.SimplexTables.Add(simplexTable);
        this.currentSimplexTable = this.SimplexTables.Count - 1;
        this.UpdateSimplexGrid();
        this.UpdateTaskLoglistBox();
        this.OpenTableMode();
        int num = (int) stepInfoForm.ShowDialog();
        this.ShowStepInfoFormButton.Enabled = true;
        this.splitContainer.Visible = true;
      }
    }

    private void NewSimplexTableToolStripMenuItem_Click(object sender, EventArgs e)
    {
      simplex.NewTaskForm newTaskForm = new simplex.NewTaskForm();
      if (newTaskForm.ShowDialog() != DialogResult.OK)
        return;
      this.IsNewTask = false;
      this.SimplexTables.Clear();
      this.SimplexTables.Add(new SimplexTable(newTaskForm.N, newTaskForm.M));
      this.currentSimplexTable = this.SimplexTables.Count - 1;
      this.UpdateSimplexGrid();
      this.UpdateTaskLoglistBox();
      this.OpenTableMode();
      this.ShowStepInfoFormButton.Enabled = true;
    }


    private void CloseMenuToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void TaskLoglistBox_DoubleClick(object sender, EventArgs e) => this.ShowStepInfoFormButton_Click(sender, e);

    private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.TaskLoglistBox.Items.Clear();
      this.splitContainer.Visible = false;
      this.CloseTableMode();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.NewTabletoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SolveSimplexTasktoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TaskTypeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.ShowStepInfoFormButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.SimplexGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskLoglistBox = new System.Windows.Forms.ListBox();
            this.toolStrip.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SimplexGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewTabletoolStripButton,
            this.toolStripSeparator3,
            this.SolveSimplexTasktoolStripButton,
            this.toolStripSeparator2,
            this.TaskTypeComboBox,
            this.toolStripSeparator4});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(888, 25);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // NewTabletoolStripButton
            // 
            this.NewTabletoolStripButton.Name = "NewTabletoolStripButton";
            this.NewTabletoolStripButton.Size = new System.Drawing.Size(146, 22);
            this.NewTabletoolStripButton.Text = "Нова симплекс-таблиця";
            this.NewTabletoolStripButton.ToolTipText = "Нова симплекс-таблиця";
            this.NewTabletoolStripButton.Click += new System.EventHandler(this.NewTaskToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // SolveSimplexTasktoolStripButton
            // 
            this.SolveSimplexTasktoolStripButton.Name = "SolveSimplexTasktoolStripButton";
            this.SolveSimplexTasktoolStripButton.Size = new System.Drawing.Size(68, 22);
            this.SolveSimplexTasktoolStripButton.Text = "Розв\'язати";
            this.SolveSimplexTasktoolStripButton.ToolTipText = "Розв\'язати симплекс-задачу";
            this.SolveSimplexTasktoolStripButton.Click += new System.EventHandler(this.SolveSimplexTaskToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // TaskTypeComboBox
            // 
            this.TaskTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TaskTypeComboBox.Items.AddRange(new object[] {
            "MAX",
            "MIN"});
            this.TaskTypeComboBox.Name = "TaskTypeComboBox";
            this.TaskTypeComboBox.Size = new System.Drawing.Size(121, 25);
            this.TaskTypeComboBox.Tag = "";
            this.TaskTypeComboBox.DropDownClosed += new System.EventHandler(this.TaskTypeComboBox_DropDownClosed);
            this.TaskTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.TaskTypeComboBox_SelectedIndexChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TopPanel.Controls.Add(this.ShowStepInfoFormButton);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 25);
            this.TopPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(888, 35);
            this.TopPanel.TabIndex = 5;
            // 
            // ShowStepInfoFormButton
            // 
            this.ShowStepInfoFormButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ShowStepInfoFormButton.Enabled = false;
            this.ShowStepInfoFormButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.ShowStepInfoFormButton.FlatAppearance.BorderSize = 2;
            this.ShowStepInfoFormButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ShowStepInfoFormButton.Location = new System.Drawing.Point(3, 6);
            this.ShowStepInfoFormButton.Name = "ShowStepInfoFormButton";
            this.ShowStepInfoFormButton.Size = new System.Drawing.Size(307, 23);
            this.ShowStepInfoFormButton.TabIndex = 0;
            this.ShowStepInfoFormButton.Text = "Показати хід розв\'язку задачі";
            this.ShowStepInfoFormButton.UseVisualStyleBackColor = false;
            this.ShowStepInfoFormButton.Click += new System.EventHandler(this.ShowStepInfoFormButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Location = new System.Drawing.Point(0, 528);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(888, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 60);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.SimplexGrid);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer.Panel2.Controls.Add(this.TaskLoglistBox);
            this.splitContainer.Size = new System.Drawing.Size(888, 468);
            this.splitContainer.SplitterDistance = 275;
            this.splitContainer.TabIndex = 6;
            this.splitContainer.Visible = false;
            // 
            // SimplexGrid
            // 
            this.SimplexGrid.AllowUserToAddRows = false;
            this.SimplexGrid.AllowUserToDeleteRows = false;
            this.SimplexGrid.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SimplexGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.SimplexGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SimplexGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.SimplexGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SimplexGrid.Location = new System.Drawing.Point(0, 0);
            this.SimplexGrid.MultiSelect = false;
            this.SimplexGrid.Name = "SimplexGrid";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SimplexGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.SimplexGrid.RowHeadersWidth = 75;
            this.SimplexGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.SimplexGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.SimplexGrid.ShowCellErrors = false;
            this.SimplexGrid.ShowCellToolTips = false;
            this.SimplexGrid.ShowEditingIcon = false;
            this.SimplexGrid.ShowRowErrors = false;
            this.SimplexGrid.Size = new System.Drawing.Size(888, 275);
            this.SimplexGrid.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 125;
            // 
            // TaskLoglistBox
            // 
            this.TaskLoglistBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskLoglistBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.TaskLoglistBox.Font = new System.Drawing.Font("Times New Roman", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TaskLoglistBox.FormattingEnabled = true;
            this.TaskLoglistBox.ItemHeight = 16;
            this.TaskLoglistBox.Location = new System.Drawing.Point(1, 3);
            this.TaskLoglistBox.Name = "TaskLoglistBox";
            this.TaskLoglistBox.Size = new System.Drawing.Size(888, 148);
            this.TaskLoglistBox.TabIndex = 7;
            this.TaskLoglistBox.SelectedIndexChanged += new System.EventHandler(this.TaskLoglistBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(888, 550);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Name = "MainForm";
            this.Text = "Метод Гоморі";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SimplexGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

    }
}
