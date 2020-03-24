using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GamingCommunityApi.Infrastructure.Entities
{
    public class FileEntity
    {
        public string Name { get; set; }
        public string RealName { get; set; }
        public string RelativeUri { get; set; }
        public string RelativePhysicalPath { get; set; }
        public long? Id { get; set; }

        private FileEntity() { }

        public FileEntity(string name, string realName, string relativeUri, 
            string relativePhysicalPath, long? id = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RealName = realName ?? throw new ArgumentNullException(nameof(realName));
            RelativeUri = relativeUri ?? throw new ArgumentNullException(nameof(relativeUri));
            RelativePhysicalPath = relativePhysicalPath ?? throw new ArgumentNullException(nameof(relativePhysicalPath));
            Id = id;
        }

        public FileEntity PureCopy() => new FileEntity(Name, RealName, RelativeUri,
            RelativePhysicalPath, Id);

        public void RemoveLoopReferencing()
        {

        }
    }

    public class FileEntityConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> modelBuilder)
        {
            // p => principal / d => dependent / v => value

            //modelBuilder
            //    .Property(e => e.RelativeUri)
            //    .HasConversion(
            //        v => v.ToString(),
            //        v => new Uri(v, UriKind.Relative));
        }
    }
}
