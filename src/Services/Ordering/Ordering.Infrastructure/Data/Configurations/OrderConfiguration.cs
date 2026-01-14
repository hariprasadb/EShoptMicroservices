using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(O=>O.Id);
            builder.Property(O=>O.Id).HasConversion(orderID=>orderID.Value,
                                                    dbId=>OrderId.Of(dbId));
            builder.HasOne<Customer>().WithMany()
                            .HasForeignKey(o => o.CustomerId).IsRequired();
            builder.HasMany(O => O.OrderItems).WithOne()
                   .HasForeignKey(O => O.OrderId);

            builder.ComplexProperty(O => O.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });
            builder.ComplexProperty(O => O.ShippingAddress, addressBuilder =>
            {
                ConfigureAddress(addressBuilder);
            });
            builder.ComplexProperty(O => O.BillingAddress, billingAddressBuilder =>
            {
                ConfigureAddress(billingAddressBuilder);
            });
            builder.ComplexProperty(O => O.Payment, 
                paymentBuilder => {
                    paymentBuilder.Property(p => p.CardName)
                     .HasMaxLength(50);
                    paymentBuilder.Property(p=>p.CardNumber)
                    .HasMaxLength(24)
                    .IsRequired();
                    paymentBuilder.Property(p => p.Expiration)
                    .HasMaxLength(10);
                    paymentBuilder.Property(p => p.CVV).HasMaxLength(3);
                    paymentBuilder.Property(p => p.PaymentMethod);
                }
            );
            builder.Property(O=>O.Status).HasDefaultValue(OrderStatus.Draft)
                .HasConversion(s=>s.ToString(),
                 dbStatus=>(OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));
            builder.Property(O => O.TotalPrice);

        }

        private void ConfigureAddress(ComplexPropertyBuilder<Address> addressBuilder)
        {
            addressBuilder.Property(n => n.FirstName)
                 .HasMaxLength(50)
                 .IsRequired();

            addressBuilder.Property(n => n.FirstName)
              .HasMaxLength(50)
              .IsRequired();

            addressBuilder.Property(n => n.EmailAddress)
             .HasMaxLength(50)
             .IsRequired();

            addressBuilder.Property(n => n.AddressLine)
             .HasMaxLength(180)
             .IsRequired();

            addressBuilder.Property(n => n.Country)
             .HasMaxLength(50);

            addressBuilder.Property(n => n.State)
             .HasMaxLength(50);

            addressBuilder.Property(n => n.ZipCode)
             .HasMaxLength(10).IsRequired();
        }

    }
}
