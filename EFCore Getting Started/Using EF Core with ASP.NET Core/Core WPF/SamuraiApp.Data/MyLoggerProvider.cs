using System;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace SamuraiApp.Data
{
  public class MyLoggerProvider : ILoggerProvider
  {
    public ILogger CreateLogger(string categoryName) {
      return new MyLogger();
    }

    public void Dispose() {
    }

    private class MyLogger : ILogger {

      public bool IsEnabled(LogLevel logLevel) {
        return true;
      }

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter) {
        //Note, the DbCommandLogData class will be replaced at some point so be wary of using it
#pragma warning disable 618
        if (state is DbCommandLogData)
#pragma warning restore 618
        {
          Console.WriteLine(formatter(state, exception));
          Console.WriteLine();
        }
      }

      public IDisposable BeginScope<TState>(TState state) {
        return null;
      }
    }
  }
}