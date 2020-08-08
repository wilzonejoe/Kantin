using System;
using System.IO;
using Core.Models.Abstracts;

namespace Core.Models.File
{
    public class UploadFile : BaseFile
    {
        public Guid OrganisationId { get; set; }
    }
}
