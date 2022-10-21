﻿using Microsoft.EntityFrameworkCore;
using Projekat.Front.Infrastructure.Persistence.Models;

namespace Projekat.Front.Infrastructure.Persistence
{
    public partial class StackOverflow2010Context : DbContext
    {
        public StackOverflow2010Context()
        {
        }

        public StackOverflow2010Context(DbContextOptions<StackOverflow2010Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Badge> Badges { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<LinkType> LinkTypes { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostLink> PostLinks { get; set; } = null!;
        public virtual DbSet<PostType> PostTypes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Vote> Votes { get; set; } = null!;
        public virtual DbSet<VoteType> VoteTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Badge>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(40);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Text).HasMaxLength(700);
            });

            modelBuilder.Entity<LinkType>(entity =>
            {
                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.ClosedDate).HasColumnType("datetime");

                entity.Property(e => e.CommunityOwnedDate).HasColumnType("datetime");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LastEditDate).HasColumnType("datetime");

                entity.Property(e => e.LastEditorDisplayName).HasMaxLength(40);

                entity.Property(e => e.Tags).HasMaxLength(150);

                entity.Property(e => e.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<PostLink>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PostType>(entity =>
            {
                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DisplayName).HasMaxLength(40);

                entity.Property(e => e.EmailHash).HasMaxLength(40);

                entity.Property(e => e.LastAccessDate).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(100);

                entity.Property(e => e.WebsiteUrl).HasMaxLength(200);
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VoteType>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
