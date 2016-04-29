using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DaySpringApp.Helpers
{
    public class CsvExporter : IExporter
    {
        public const string ContentType = "application/text";

        private StreamWriter _writer;

        public void Create()
        {
            _writer = new StreamWriter(new MemoryStream());
        }

        public void Save(Stream stream)
        {
            _writer.Flush();
            _writer.BaseStream.Position = 0;
            _writer.BaseStream.CopyTo(stream);
            stream.Position = 0;
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        public void WriteLine(string[] fields)
        {
            for (var i = 0; i < fields.Length - 1; i++)
            {
                var field = fields[i] ?? string.Empty;
                field = field.Replace(',', ' ');
                _writer.Write(field);
                _writer.Write(",");
            }

            _writer.WriteLine(fields[fields.Length - 1]);
        }
    }
}