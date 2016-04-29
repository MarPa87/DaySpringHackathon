using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaySpringApp.Results
{
    public class ZipResult : ActionResult
    {
        private Dictionary<string, byte[]> _entries = new Dictionary<string, byte[]>();

        public string FileName { get; set; }

        public void AddEntry(string filename, byte[] content)
        {
            _entries.Add(filename, content);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var pair in _entries)
                    {
                        var entry = archive.CreateEntry(pair.Key);

                        using (var entryStream = entry.Open())
                        {
                            entryStream.Write(pair.Value, 0, pair.Value.Length);
                        }
                    }
                }

                context.HttpContext.Response.ContentType = "application/zip";
                context.HttpContext.Response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(context.HttpContext.Response.OutputStream);
            }
        }
    }
}