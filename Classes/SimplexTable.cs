
using Mehroz;
using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace simplex.Classes
{
  internal class SimplexTable
  {
    private int n;
    private int m;
    private int rows;
    private int cols;
    private int pivotRow = -1;
    private int pivotCol = -1;
    private int tasktype;
    private bool manuallyRecalcFlag;
    private bool solved = false;
    private bool nonintegersolved = false;
    private bool infinity = false;
    private bool unsolvable = false;
    private Fraction[,] table = new Fraction[100, 100];
    private ArrayList baseVector = new ArrayList();
    private double[] mrVector = new double[100];
    private double[] FRootVector = new double[100];
    private Fraction[] FRootVectorFraction = new Fraction[100];

    public double FunctionValue() => this.table[this.m, 0].ToDouble();

    public Fraction FunctionValueFraction() => this.table[this.m, 0];

    public double Roots(int index) => this.FRootVector[index];

    public Fraction RootsFraction(int index) => this.FRootVectorFraction[index];

    public SimplexTable(int an, int am)
    {
      this.manuallyRecalcFlag = false;
      for (int index = 0; index < 100; ++index)
        this.FRootVectorFraction[index] = (Fraction) 0L;
      for (int index1 = 0; index1 < 100; ++index1)
      {
        for (int index2 = 0; index2 < 100; ++index2)
          this.table[index1, index2] = (Fraction) 0L;
      }
      this.ResizeTable(an, am);
    }

    public double MRVector(int index) => this.mrVector[index];

    public void SetMRVector(int index, double value) => this.mrVector[index] = value;

    public void FillMatrixA(Fraction[,] SourceMatrixA)
    {
      for (int index1 = 0; index1 < this.m; ++index1)
      {
        for (int index2 = 0; index2 < this.n; ++index2)
          SourceMatrixA[index1, index2] = (Fraction) this.table[index1, index2 + 1].ToDouble();
      }
    }

    public void FillVectorb(Fraction[] SourceVectorb)
    {
      for (int index = 0; index < this.m; ++index)
        SourceVectorb[index] = (Fraction) this.table[index, 0].ToDouble();
    }

    public void FillVectorc(Fraction[] SourceVectorc)
    {
      for (int index = 1; index < this.n + 1; ++index)
        SourceVectorc[index - 1] = (Fraction) this.table[this.m, index].ToDouble();
    }

    public void ResizeTable(int an, int am)
    {
      this.n = an;
      this.m = am;
      this.rows = this.m + 1;
      this.cols = 1 + this.n + this.m;
      this.tasktype = 0;
      this.ClearTable();
      this.baseVector.Clear();
      for (int index = 0; index < this.m; ++index)
        this.baseVector.Add((object) (this.n + index + 1));
    }

    public void AddNewRest()
    {
      int index1 = this.MaxDoubleIndex();
      ++this.m;
      this.rows = this.m + 1;
      this.cols = 1 + this.n + this.m;
      this.baseVector.Add((object) (this.n + this.m));
      this.table[this.m - 1, this.cols - 1] = (Fraction) 0L;
      for (int index2 = 0; index2 < this.cols; ++index2)
        this.table[this.m, index2] = this.table[this.m - 1, index2];
      double num1 = this.table[index1, 0].ToDouble() == 0.0 ? 1.0 : this.table[index1, 0].ToDouble() / this.table[index1, 0].ToDouble();
      double num2 = Math.Truncate(this.table[index1, 0].ToDouble());
      this.table[this.m - 1, 0] = this.table[index1, 0] - (double) (long) num2;
      this.table[this.m - 1, 0] = this.table[this.m - 1, 0] * num1 * -1;
      this.table[this.m - 1, this.cols - 1] = (Fraction) 1L;
      for (int index3 = 1; index3 < this.cols - 1; ++index3)
      {
        if (this.table[index1, index3] >= (Fraction) 0L)
        {
          double num3 = Math.Truncate(this.table[index1, index3].ToDouble());
          this.table[this.m - 1, index3] = -(this.table[index1, index3] - num3);
        }
        else
        {
          double num4 = Math.Abs(Math.Truncate(this.table[index1, index3].ToDouble()));
          this.table[this.m - 1, index3] = -(this.table[index1, index3] + num4 + 1);
        }
      }
    }

    public int Сols => this.cols;

    public int Rows => this.rows;

    public int N
    {
      get => this.n;
      set => this.n = value;
    }

    public int M
    {
      get => this.m;
      set => this.m = value;
    }

    public int PivotRow
    {
      get => this.pivotRow;
      set => this.pivotRow = value;
    }

    public int PivotCol
    {
      get => this.pivotCol;
      set => this.pivotCol = value;
    }

    public int TaskType
    {
      get => this.tasktype;
      set => this.tasktype = value;
    }

    public bool Solved
    {
      get => this.solved;
      set => this.solved = value;
    }

    public bool NonIntegerSolved
    {
      get => this.nonintegersolved;
      set => this.nonintegersolved = value;
    }

    public bool Unsolvable
    {
      get => this.unsolvable;
      set => this.unsolvable = value;
    }

    public bool Infinity
    {
      get => this.infinity;
      set => this.infinity = value;
    }

    public bool ManuallyRecalcFlag
    {
      get => this.manuallyRecalcFlag;
      set => this.manuallyRecalcFlag = value;
    }

    public int BaseVector(int index) => (int) this.baseVector[index];

    public int MaxDoubleIndex()
    {
      int num1 = -1;
      double num2 = 0.0;
      for (int index = 0; index < this.M; ++index)
      {
        double num3 = Math.Abs(this.table[index, 0].ToDouble()) - Math.Abs(Math.Floor(this.table[index, 0].ToDouble()));
        if (num2 < num3)
        {
          num2 = num3;
          num1 = index;
        }
      }
      return num1;
    }

    public void SwapObjFuncSigns()
    {
      for (int index = 1; index < this.n + this.m + 1; ++index)
        this.table[this.rows - 1, index] = this.table[this.rows - 1, index] * -1;
    }

    public int CheckObjFuncSigns()
    {
      int num = 0;
      if (this.tasktype == 0)
      {
        for (int index = 1; index < this.n + this.m + 1; ++index)
        {
          if (this.table[this.rows - 1, index] != 0)
            num = 1;
        }
        for (int index = 1; index < this.n + this.m + 1; ++index)
        {
          if (this.table[this.rows - 1, index] < (Fraction) 0L)
            num = 0;
        }
      }
      if (this.tasktype == 1)
      {
        for (int index = 1; index < this.n + this.m + 1; ++index)
        {
          if (this.table[this.rows - 1, index] != 0)
            num = 2;
        }
        for (int index = 1; index < this.n + this.m + 1; ++index)
        {
          if (this.table[this.rows - 1, index] > (Fraction) 0L)
            num = 0;
        }
      }
      return num;
    }

    public bool TestForInfinity()
    {
      int index1 = 0;
      bool flag = false;
      double num = this.table[index1, 0].ToDouble();
      for (int index2 = 0; index2 < this.m; ++index2)
      {
        if (Math.Abs(num) < Math.Abs(this.table[index2, 0].ToDouble()) & this.table[index2, 0] < (Fraction) 0L)
          num = this.table[index2, 0].ToDouble();
      }
      if (num >= 0.0)
      {
        for (int variable = 1; variable <= this.n + this.m; ++variable)
        {
          if (!this.BaseVariable(variable) & this.table[this.m, variable] < (Fraction) 0L)
          {
            flag = true;
            for (int index3 = 0; index3 < this.m; ++index3)
            {
              if (this.table[index3, variable] > (Fraction) 0L)
                flag = false;
            }
            if (flag)
            {
              this.pivotRow = -1;
              this.pivotCol = variable;
              return flag;
            }
          }
        }
      }
      return flag;
    }

    public void SetBaseVector(int index, int value) => this.baseVector[index] = (object) value;

    public void Assign(SimplexTable smt)
    {
      this.pivotCol = smt.PivotCol;
      this.PivotRow = smt.pivotRow;
      this.tasktype = smt.TaskType;
      for (int index = 0; index < this.m; ++index)
        this.baseVector[index] = (object) smt.BaseVector(index);
    }

    public double Table(int RowIndex, int ColIndex) => this.table[RowIndex, ColIndex].ToDouble();

    public Fraction FractionTable(int RowIndex, int ColIndex) => this.table[RowIndex, ColIndex];

    public bool ColValidToCalc(double value) => value < 0.0;

    public bool BaseVariable(int variable)
    {
      bool flag = false;
      for (int index = 0; index < this.m; ++index)
      {
        if ((int) this.baseVector[index] == variable)
          flag = true;
      }
      return flag;
    }

    public void CalcMeritRestrictions(int lpivotCol)
    {
      if (lpivotCol == -1)
        return;
      for (int index = 0; index < this.m; ++index)
        this.mrVector[index] = !(this.table[index, 0] * this.table[index, lpivotCol] < (Fraction) 0L | this.table[index, 0] == 0 & this.table[index, lpivotCol] < (Fraction) 0L | this.table[index, lpivotCol] == 0) ? Math.Abs(this.table[index, 0].ToDouble() / this.table[index, lpivotCol].ToDouble()) : -1.0;
    }

    public bool FindOptimalStep()
    {
      int index1 = -1;
      int lpivotCol1 = -1;
      bool optimalStep = false;
      double num1 = 0.0;
      double num2 = 0.0;
      for (int index2 = 0; index2 < this.m; ++index2)
      {
        if (this.table[index2, 0] < (Fraction) 0L)
        {
          num1 = this.table[index2, 0].ToDouble();
          index1 = index2;
        }
      }
      if (index1 != -1)
      {
        for (int index3 = 0; index3 < this.m; ++index3)
        {
          if (Math.Abs(num1) < Math.Abs(this.table[index3, 0].ToDouble()) & this.table[index3, 0] < (Fraction) 0L)
          {
            num1 = this.table[index3, 0].ToDouble();
            index1 = index3;
          }
        }
        if (num1 < 0.0)
        {
          double num3 = 0.0;
          for (int index4 = 1; index4 < this.cols; ++index4)
          {
            if (Math.Abs(num3) < Math.Abs(this.table[index1, index4].ToDouble()) & this.table[index1, index4].ToDouble() < 0.0)
            {
              num3 = this.table[index1, index4].ToDouble();
              lpivotCol1 = index4;
            }
          }
        }
      }
      if (lpivotCol1 > 0)
      {
        this.CalcMeritRestrictions(lpivotCol1);
        this.pivotRow = index1;
        this.pivotCol = lpivotCol1;
        return true;
      }
      int index5 = -1;
      switch (this.tasktype)
      {
        case 0:
          num2 = -1.7E+308;
          break;
        case 1:
          num2 = 1.7E+308;
          break;
      }
      for (int index6 = 1; index6 < this.cols; ++index6)
      {
        int variable = index6;
        if (this.ColValidToCalc(this.table[this.m, variable].ToDouble()) & !this.BaseVariable(variable))
        {
          int lpivotCol2 = variable;
          this.CalcMeritRestrictions(lpivotCol2);
          double num4 = -1.0;
          for (int index7 = 0; index7 < this.m; ++index7)
          {
            if (this.mrVector[index7] >= 0.0)
            {
              if (num4 < 0.0)
              {
                index5 = index7;
                num4 = this.mrVector[index5];
              }
              else if (num4 > this.mrVector[index7])
              {
                index5 = index7;
                num4 = this.mrVector[index5];
              }
            }
          }
          if (num4 >= 0.0 & index5 != -1 & lpivotCol2 != -1)
          {
            double num5 = this.table[index5, lpivotCol2].ToDouble();
            double num6 = this.table[this.m, 0].ToDouble() + this.table[index5, 0].ToDouble() / num5 * (-1.0 * this.table[this.m, lpivotCol2].ToDouble());
            if (this.tasktype == 1)
              num6 = -num6;
            if (num2 < num6 & this.tasktype == 0 | num2 > num6 & this.tasktype == 1)
            {
              optimalStep = true;
              this.pivotRow = index5;
              this.pivotCol = lpivotCol2;
              num2 = num6;
            }
          }
          else
          {
            this.pivotRow = -1;
            this.pivotCol = -1;
            return false;
          }
        }
      }
      if (this.pivotCol != -1)
        this.CalcMeritRestrictions(this.pivotCol);
      return optimalStep;
    }

    public void SaveToFile(string FileName)
    {
      FileStream fileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
      byte[] buffer1 = new byte[3]
      {
        Convert.ToByte('S'),
        Convert.ToByte('M'),
        Convert.ToByte('T')
      };
      byte[] buffer2 = new byte[2];
      byte[] buffer3 = new byte[2];
      fileStream.Write(buffer1, 0, 3);
      fileStream.Write(buffer2, 0, 2);
      fileStream.Write(buffer3, 0, 2);
      fileStream.Write(BitConverter.GetBytes(this.n), 0, 4);
      fileStream.Write(BitConverter.GetBytes(this.m), 0, 4);
      for (int index1 = 0; index1 < this.rows; ++index1)
      {
        for (int index2 = 0; index2 < this.cols; ++index2)
          fileStream.Write(BitConverter.GetBytes(this.table[index1, index2].ToDouble()), 0, 8);
      }
      fileStream.Close();
    }

    public bool LoadFromFile(string FileName)
    {
      FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
      byte[] buffer1 = new byte[8];
      byte[] buffer2 = new byte[3];
      byte[] buffer3 = new byte[2];
      byte[] buffer4 = new byte[2];
      byte[] buffer5 = new byte[4];
      fileStream.Read(buffer2, 0, 3);
      if (!((int) buffer2[0] == (int) Convert.ToByte('S') & (int) buffer2[1] == (int) Convert.ToByte('M') & (int) buffer2[2] == (int) Convert.ToByte('T')))
      {
        int num = (int) MessageBox.Show("Формат файла підтримується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        fileStream.Close();
        return false;
      }
      fileStream.Read(buffer3, 0, 2);
      fileStream.Read(buffer4, 0, 2);
      if (!(BitConverter.ToUInt16(buffer3, 0) == (ushort) 0 & BitConverter.ToUInt16(buffer4, 0) == (ushort) 0))
      {
        int num = (int) MessageBox.Show("Неправильний формат файла!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        fileStream.Close();
        return false;
      }
      fileStream.Read(buffer5, 0, 4);
      this.n = BitConverter.ToInt32(buffer5, 0);
      fileStream.Read(buffer5, 0, 4);
      this.m = BitConverter.ToInt32(buffer5, 0);
      this.ResizeTable(this.n, this.m);
      for (int index1 = 0; index1 < this.rows; ++index1)
      {
        for (int index2 = 0; index2 < this.cols; ++index2)
        {
          fileStream.Read(buffer1, 0, 8);
          this.table[index1, index2] = (Fraction) BitConverter.ToDouble(buffer1, 0);
        }
      }
      fileStream.Close();
      return true;
    }

    private void ClearTable()
    {
      for (int index1 = 0; index1 < this.rows; ++index1)
      {
        for (int index2 = 0; index2 < this.cols; ++index2)
          this.table[index1, index2] = (Fraction) 0L;
      }
      for (int index3 = 0; index3 < this.rows; ++index3)
      {
        for (int index4 = this.n + 1; index4 < this.cols; ++index4)
        {
          if (index3 + this.n + 1 == index4)
            this.table[index3, index4] = (Fraction) 1L;
        }
      }
    }

    public void PrintTable(DataGridView SimplexGrid)
    {
      for (int rowIndex = 0; rowIndex < this.rows; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < this.cols; ++columnIndex)
          SimplexGrid[columnIndex, rowIndex].Value = (object) this.table[rowIndex, columnIndex].ToString();
      }
    }

    public bool FillTable(DataGridView SimplexGrid)
    {
      bool flag = true;
      for (int rowIndex = 0; rowIndex < this.rows; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < this.cols; ++columnIndex)
        {
          try
          {
            this.table[rowIndex, columnIndex] = (Fraction) SimplexGrid[columnIndex, rowIndex].Value.ToString();
          }
          catch
          {
            flag = false;
            int num = (int) MessageBox.Show("Неправильный формат числа: \"" + SimplexGrid[columnIndex, rowIndex].Value.ToString() + "\"", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      return flag;
    }

    public bool TestForUnsolvable()
    {
      bool flag = false;
      int index1 = -1;
      double num1 = 0.0;
      for (int index2 = 0; index2 < this.m; ++index2)
      {
        if (this.table[index2, 0] < (Fraction) 0L)
        {
          num1 = this.table[index2, 0].ToDouble();
          index1 = index2;
        }
      }
      if (index1 != -1)
      {
        for (int index3 = 0; index3 < this.m; ++index3)
        {
          if (Math.Abs(num1) < Math.Abs(this.table[index3, 0].ToDouble()) & this.table[index3, 0] < (Fraction) 0L)
          {
            num1 = this.table[index3, 0].ToDouble();
            index1 = index3;
          }
        }
        if (num1 < 0.0)
        {
          flag = true;
          double num2 = 0.0;
          ++this.pivotRow;
          for (int index4 = 1; index4 < this.cols; ++index4)
          {
            if (Math.Abs(num2) < Math.Abs(this.table[index1, index4].ToDouble()) & this.table[index1, index4] < (Fraction) 0L)
            {
              this.pivotRow = -1;
              flag = false;
              break;
            }
          }
        }
      }
      return flag;
    }

    public bool TestForInteger()
    {
      bool flag = true;
      for (int index = 0; index < this.m; ++index)
      {
        if ((int) this.baseVector[index] <= this.n && this.table[index, 0].Denominator != 1L)
          flag = false;
      }
      return flag;
    }

    public bool TestForSolved()
    {
      bool flag = true;
      for (int index = 1; index < this.cols; ++index)
      {
        if (this.table[this.m, index] < (Fraction) 0L)
          flag = false;
      }
      for (int index = 0; index < this.m; ++index)
      {
        if (this.table[index, 0] < (Fraction) 0L)
          flag = false;
      }
      return flag;
    }

    public void RecalcTable()
    {
      Fraction fraction1 = new Fraction((string) this.table[this.pivotRow - 1, this.pivotCol]);
      for (int index = 0; index < this.cols; ++index)
        this.table[this.pivotRow - 1, index] = this.table[this.pivotRow - 1, index] / fraction1;
      for (int index1 = 0; index1 < this.rows; ++index1)
      {
        if (index1 != this.pivotRow - 1)
        {
          Fraction fraction2 = new Fraction((string) this.table[index1, this.pivotCol]);
          for (int index2 = 0; index2 < this.cols; ++index2)
            this.table[index1, index2] = this.table[index1, index2] - this.table[this.pivotRow - 1, index2] * fraction2;
        }
      }
      this.baseVector[this.pivotRow - 1] = (object) this.pivotCol;
      for (int index = 0; index < this.n; ++index)
      {
        this.FRootVector[index] = 0.0;
        this.FRootVectorFraction[index] = (Fraction) 0L;
      }
      for (int index = 0; index < this.m; ++index)
      {
        if ((int) this.baseVector[index] <= this.n)
        {
          this.FRootVector[(int) this.baseVector[index] - 1] = this.table[index, 0].ToDouble();
          this.FRootVectorFraction[(int) this.baseVector[index] - 1] = this.table[index, 0];
        }
      }
    }

    public void GetTable(Fraction[,] matrixA, Fraction[] vectorb, Fraction[] vectorc)
    {
      for (int index1 = 0; index1 < this.m; ++index1)
      {
        for (int index2 = 0; index2 < this.n; ++index2)
          this.table[index1, index2 + 1] = matrixA[index1, index2];
      }
      for (int index = 1; index < this.n + 1; ++index)
        this.table[this.m, index] = vectorc[index - 1];
      for (int index = 0; index < this.m; ++index)
        this.table[index, 0] = vectorb[index];
    }
  }
}
