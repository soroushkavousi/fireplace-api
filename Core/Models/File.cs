using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Models
{
    public class File
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string RealName { get; set; }
        public Uri Uri { get; set; }
        public string PhysicalPath { get; set; }
        //[JsonIgnore]
        //public IFormFile FormFile {get;set;}

        public File(long id, string name, string realName, Uri uri, string physicalPath)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RealName = realName ?? throw new ArgumentNullException(nameof(realName));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            PhysicalPath = physicalPath ?? throw new ArgumentNullException(nameof(physicalPath));
        }

        public File PureCopy() => new File(Id, Name, RealName, Uri, PhysicalPath);

        public void RemoveLoopReferencing()
        {

        }
    }
}
