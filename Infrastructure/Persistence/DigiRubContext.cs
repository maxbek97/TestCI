using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestCI.Models;
using DomainUser = TestCI.Domain.Users.User;
using DomainRefreshToken = TestCI.Domain.Authentification.RefreshToken;
using DomainDrWallet = TestCI.Domain.DrWallets.DrWallet;
using DomainStatusWallet = TestCI.Domain.DrWallets.StatusWallet;
using DomainClient = TestCI.Domain.Clients.Client;

namespace TestCI.Infrastructure.Persistence;

public partial class DigiRubContext : DbContext
{
    public DigiRubContext()
    {
    }

    public DigiRubContext(DbContextOptions<DigiRubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DomainClient> Clients { get; set; }

    public virtual DbSet<DomainDrWallet> DrWallets { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<DomainRefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<DomainUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<DomainStatusWallet>("public", "status_wallet");

        modelBuilder.Entity<DomainClient>(entity =>
        {
            entity.HasKey(e => e.Mid).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.HasIndex(e => e.IdDr, "clients_id_dr_key").IsUnique();

            entity.Property(e => e.Mid)
                .ValueGeneratedNever()
                .HasColumnName("mid");

            entity.Property(e => e.FisrtName)
                .HasMaxLength(255)
                .HasColumnName("fisrt_name");

            entity.Property(e => e.IdDr).HasColumnName("id_dr");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(255)
                .HasColumnName("middle_name");
        });

        modelBuilder.Entity<DomainDrWallet>(entity =>
        {
            entity.HasKey(e => e.Id_DRw).HasName("dr_wallet_pkey");

            entity.ToTable("dr_wallet");

            entity.HasIndex(e => e.BillId, "dr_wallet_id_bill_key").IsUnique();

            entity.Property(e => e.Id_DRw)
                .ValueGeneratedNever()
                .HasColumnName("id_drw");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.BillId).HasColumnName("id_bill");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("status_wallet");
            //entity.HasOne(d => d.Client).WithMany(p => p.DrWallets)
            //    .HasForeignKey(d => d.ClientId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("dr_wallet_client_id_fkey");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("logs_pkey");

            entity.ToTable("logs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy)
                .HasMaxLength(100)
                .HasDefaultValueSql("CURRENT_USER")
                .HasColumnName("changed_by");
            entity.Property(e => e.NewData)
                .HasColumnType("jsonb")
                .HasColumnName("new_data");
            entity.Property(e => e.OldData)
                .HasColumnType("jsonb")
                .HasColumnName("old_data");
            entity.Property(e => e.Operation)
                .HasMaxLength(10)
                .HasColumnName("operation");
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .HasColumnName("table_name");
        });

        modelBuilder.Entity<DomainRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens");

            entity.HasIndex(e => e.Token, "refresh_tokens_token_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            //entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
            //    .HasForeignKey(d => d.UserId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("refresh_tokens_user_id_fkey");
        });

        modelBuilder.Entity<DomainUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_user_email_key").IsUnique();

            entity.HasIndex(e => e.Login, "users_user_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id_user");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("user_email");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("user_login");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
