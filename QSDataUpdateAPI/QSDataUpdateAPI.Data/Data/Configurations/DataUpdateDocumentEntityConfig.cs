using QSDataUpdateAPI.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace QSDataUpdateAPI.Data.Data.Configurations
{
    public class DataUpdateDocumentEntityConfig : IEntityTypeConfiguration<DataUpdateDocument>
    {
        public void Configure(EntityTypeBuilder<DataUpdateDocument> entity)
        {
            entity.ToTable("DATA_UPDATE_DOCS");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.AccOpeningReqId)
                .HasColumnName("DATA_UPDATE_REQ_ID")
                .IsRequired();

            entity.Property(e => e.ContentOrPath)
                .IsRequired();

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasColumnName("FILE_NAME")
                .HasMaxLength(200);

            entity.Property(e => e.Title)
                .HasColumnName("TITLE")
                .HasMaxLength(250);

            entity.Property(e => e.ContentType)
                .IsRequired()
                .HasColumnName("DOCUMENT_CONTENT_TYPE")
                .HasMaxLength(200);

            entity.HasOne(d => d.AccountOpeningRequest)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.AccOpeningReqId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DATA_UPDATE_DOCS_DATA_UPDATE_REQ_ID");
        }
    }
}
