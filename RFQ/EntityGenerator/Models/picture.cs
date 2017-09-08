using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class picture
    {
        public int Id { get; set; }
        public byte[] PictureBinary { get; set; }
        public string MimeType { get; set; }
        public string SeoFilename { get; set; }
        public bool IsNew { get; set; }
    }
}
