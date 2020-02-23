using System;
using System.Collections.Generic;
using System.Text;

namespace Kantin.Data.Exceptions.Models
{
    public class PropertyErrorResult
    {
        public string FieldName { get; set; }
        public string FieldErrors { get; set; }
    }
}
