using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class download
    {
        public int Id { get; set; }
        public System.Guid DownloadGuid { get; set; }
        public bool UseDownloadUrl { get; set; }
        public string DownloadUrl { get; set; }
        public byte[] DownloadBinary { get; set; }
        public string ContentType { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public bool IsNew { get; set; }
    }
}
