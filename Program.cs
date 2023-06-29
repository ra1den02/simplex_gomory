// Decompiled with JetBrains decompiler
// Type: simplex.Program
// Assembly: simplex_gomory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1727A725-0BA6-4AF5-B255-BB389DB6F119
// Assembly location: C:\Users\Sasha\Downloads\Telegram Desktop\simplex_gomory\simplex_gomory\simplex_gomory.exe

using System;
using System.Windows.Forms;

namespace simplex
{
  internal static class Program
  {
    public static MainForm MainForm;

    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Program.MainForm = new MainForm();
      Application.Run((Form) Program.MainForm);
    }
  }
}
