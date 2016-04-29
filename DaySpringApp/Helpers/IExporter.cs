using System;
using System.IO;

namespace DaySpringApp.Helpers
{
    interface IExporter : IDisposable
    {
        void Create();

        void Save(Stream output);

        void WriteLine(string[] fields);
    }
}
