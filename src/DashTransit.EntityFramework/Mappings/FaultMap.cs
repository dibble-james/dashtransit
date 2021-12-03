namespace DashTransit.EntityFramework.Mappings;

using DashTransit.Core.Domain;
using Microsoft.EntityFrameworkCore;

public class FaultMap : IEntityTypeConfiguration<Fault>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Fault> builder)
    {
        builder.Property<long>("FaultId");
        builder.HasKey("FaultId");

        builder.Property(x => x.RaisedBy).HasConversion<UriValueOfConverter<Endpoint>>();
        builder.Property(x => x.Exception).IsRequired();
        builder.Property(x => x.ExceptionType).IsRequired();
        builder.Property(x => x.StackTrace).IsRequired();
        builder.Property(x => x.Source).IsRequired();

        builder.HasOne(x => x.Message).WithMany(x => x.Faults).HasForeignKey("MessageId").HasPrincipalKey(x => x.Id);
    }
}
