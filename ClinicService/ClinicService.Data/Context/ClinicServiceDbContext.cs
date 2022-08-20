﻿using ClinicService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Data.Context;

public class ClinicServiceDbContext : DbContext
{
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Pet> Pets { get; set; }
    public virtual DbSet<Consultation> Consultations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Consultation>()
            .HasOne(p => p.Pet)
            .WithMany(c => c.Consultations)
            .HasForeignKey(p => p.PetId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public ClinicServiceDbContext(DbContextOptions options) : base(options)
    {
    }
}
