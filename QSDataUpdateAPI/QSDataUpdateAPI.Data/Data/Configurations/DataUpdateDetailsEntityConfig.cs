using QSDataUpdateAPI.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace QSDataUpdateAPI.Data.Data.Configurations
{
    public class DataUpdateDetailsEntityConfig : IEntityTypeConfiguration<DataUpdateDetails>
    {
        public void Configure(EntityTypeBuilder<DataUpdateDetails> entity)
        {
            entity.ToTable("DATA_UPDATE_DETAILS");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.CustomerReqId).HasColumnName("CUSTOMER_REQ_ID")
                .IsRequired();

            entity.Property(e => e.ExistingAccType)
                .HasColumnName("EXISTING_ACC_TYPE")
                .HasMaxLength(100);

            entity.Property(e => e.ExistingAccountSegment)
                .HasColumnName("EXISTING_ACC_SEGMENT")
                .HasMaxLength(100);

            entity.Property(e => e.PhoneNumber)
                .HasColumnName("PHONE_NUMBER")
                .HasMaxLength(20);

            entity.Property(e => e.BvnId)
                .HasColumnName("BVN")
                .HasMaxLength(50);

            // Data
            entity.Property(e => e.DataToUpdate).HasColumnName("DATA_TO_UPDATE")
                .HasMaxLength(100);

            entity.Property(e => e.HouseNumber).HasColumnName("HOUSE_NUMBER")
                .HasMaxLength(50);

            entity.Property(e => e.StreetName)
                .HasColumnName("STREET_NAME")
                .HasMaxLength(100);

            entity.Property(e => e.CityTown)
                .HasColumnName("CITY_TOWN")
                .HasMaxLength(100);

            entity.Property(e => e.State)
                .HasColumnName("STATE")
                .HasMaxLength(20);

            entity.Property(e => e.Lga)
                .HasColumnName("LGA")
                .HasMaxLength(50);

            entity.Property(e => e.BusStop)
                .HasColumnName("BUS_STOP")
                .HasMaxLength(100);

            entity.Property(e => e.Alias)
                .HasColumnName("ALIAS")
                .HasMaxLength(100);

            entity.Property(e => e.HouseDescription)
                .HasColumnName("HOUSE_DESCRIPTION")
                .HasMaxLength(100);

            entity.Property(e => e.NewPhoneNumber)
                .HasColumnName("NEW_PHONE_NUMBER")
                .HasMaxLength(20);

            entity.Property(e => e.CountryCode)
                .HasColumnName("COUNTRY_CODE")
                .HasMaxLength(20);

            entity.Property(e => e.NewEmail)
                .HasColumnName("NEW_EMAIL")
                .HasMaxLength(50);

            // ID Info
            entity.Property(e => e.IdType)
                .HasColumnName("ID_TYPE")
                .HasMaxLength(100);

            entity.Property(e => e.IdNumber)
                .HasColumnName("ID_NUMBER")
                .HasMaxLength(100);

            // Session
            entity.Property(e => e.CaseId)
                .HasColumnName("CASE_ID")
                .HasMaxLength(100);

            entity.Property(e => e.CurrentStep)
                .HasColumnName("CURRENT_STEP")
                .HasMaxLength(100);

            entity.Property(e => e.Submitted)
                .HasColumnName("SUBMITTED")
                .HasMaxLength(100);

            entity.Property(e => e.TinNumber)
                .HasColumnName("TIN")
                .HasMaxLength(100);


            // Terms and Conditions
            entity.Property(e => e.IAcceptTermsAndCondition)
                .HasColumnName("I_ACCEPT_TERMS_AND_CONDITIONS")
                .HasMaxLength(100);

            entity.Property(e => e.DateOfAcceptingTAndC)
                .HasColumnName("DATE_OF_ACCEPTING_T_AND_C")
                .HasMaxLength(100);

            entity.HasOne(d => d.CustomerReq)
                .WithMany(p => p.DataUpdateDetails)
                .HasForeignKey(d => d.CustomerReqId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DATA_UPDATE_OPENING_DETAILS");
        }
    }
}
