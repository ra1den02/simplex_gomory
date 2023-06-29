
using System.Windows.Forms;

namespace simplex.Classes
{
  internal class Routines
  {
    public static void InitGridView(DataGridView dataGridView)
    {
      for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; ++columnIndex)
      {
        for (int rowIndex = 0; rowIndex < dataGridView.RowCount; ++rowIndex)
          dataGridView[columnIndex, rowIndex].Value = (object) 0;
      }
    }
  }
}
