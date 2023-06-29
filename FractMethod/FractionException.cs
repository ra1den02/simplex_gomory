using System;

namespace Mehroz
{
  public class FractionException : Exception
  {
    public FractionException()
    {
    }

    public FractionException(string Message)
      : base(Message)
    {
    }

    public FractionException(string Message, Exception InnerException)
      : base(Message, InnerException)
    {
    }
  }
}
