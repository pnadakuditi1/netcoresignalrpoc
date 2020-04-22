using iPrattle.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace iPrattle.Services
{
    public class PrattleDbContext : DbContext
    {
        public PrattleDbContext(DbContextOptions<PrattleDbContext> options) : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserContact> Contacts { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(ConfigureUser);
            builder.Entity<UserContact>(ConfigureUserContact);
            builder.Entity<Message>(ConfigureMessage);
        }

        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .IsRequired();

            builder.Property(a => a.Username)
                .IsRequired(true)
                .HasMaxLength(255);
            builder.HasAlternateKey(a => a.Username);

            builder.Property(a => a.FirstName)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(a => a.LastName)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(a => a.Password)
                .IsRequired(true)
                .HasMaxLength(255);
        }

        private void ConfigureUserContact(EntityTypeBuilder<UserContact> builder)
        {
            builder.ToTable("UserContact");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .IsRequired(true);

            builder.Property(a => a.UserId)
                .IsRequired(true);

            builder.Property(a => a.ContactId)
                .IsRequired(true);

            builder.Property(a => a.CreatedOn)
                .IsRequired(true);

            builder.HasOne(a => a.Contact)
                .WithMany(b => b.Contacts)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasAlternateKey(a  => new { a.UserId, a.ContactId });
        }

        private void ConfigureMessage(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .IsRequired(true);

            builder.Property(a => a.SenderId)
                .IsRequired(true);

            builder.Property(a => a.ReceiverId)
                .IsRequired(true);

            builder.Property(a => a.CreatedOn)
                .IsRequired(true);

            builder.Property(a => a.Body)
                .IsRequired(true);
        }
    }
}
