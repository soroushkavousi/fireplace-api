using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FireplaceApi.Infrastructure.Entities
{
    public class FileEntity : BaseEntity
    {
        public string Name { get; set; }
        public string RealName { get; set; }
        public string RelativeUri { get; set; }
        public string RelativePhysicalPath { get; set; }

        private FileEntity() : base() { }

        public FileEntity(ulong id, string name, string realName, string relativeUri,
            string relativePhysicalPath, DateTime? creationDate = null,
            DateTime? modifiedDate = null) : base(id, creationDate, modifiedDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RealName = realName ?? throw new ArgumentNullException(nameof(realName));
            RelativeUri = relativeUri ?? throw new ArgumentNullException(nameof(relativeUri));
            RelativePhysicalPath = relativePhysicalPath ?? throw new ArgumentNullException(nameof(relativePhysicalPath));
        }

        public FileEntity PureCopy() => new FileEntity(Id, Name, RealName, RelativeUri,
            RelativePhysicalPath, CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {

        }
    }

    public class FileEntityConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> modelBuilder)
        {
            // p => principal / d => dependent / v => value

            modelBuilder.DoBaseConfiguration();

            //modelBuilder
            //    .Property(e => e.RelativeUri)
            //    .HasConversion(
            //        v => v.ToString(),
            //        v => new Uri(v, UriKind.Relative));
        }
    }
}
