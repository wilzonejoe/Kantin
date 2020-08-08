using System;
using System.IO;

namespace Core.Models.Abstracts
{
    public abstract class BaseFile
    {
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }
        public Stream Data { get; set; }
    }
}
