using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OxideClassLibrary.Models;

namespace OxideClassLibrary.Contexts;

public partial class OxideDbContext : DbContext
{
    public OxideDbContext()
    {
    }

    public OxideDbContext(DbContextOptions<OxideDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Draft> Drafts { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserNote> UserNotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=oxide_db;User=root;Password=KHghxrigroot1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.IconName)
                .HasMaxLength(30)
                .HasDefaultValueSql("'default'")
                .HasColumnName("icon_name");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PRIMARY");

            entity.ToTable("comment");

            entity.HasIndex(e => e.NoteId, "fk_comment_notes_idx");

            entity.HasIndex(e => e.UserId, "fk_comment_users_idx");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.Content)
                .HasMaxLength(300)
                .HasColumnName("content");
            entity.Property(e => e.NoteId).HasColumnName("note_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Note).WithMany(p => p.Comments)
                .HasForeignKey(d => d.NoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comment_notes");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comment_users");
        });

        modelBuilder.Entity<Draft>(entity =>
        {
            entity.HasKey(e => e.DraftId).HasName("PRIMARY");

            entity.ToTable("draft");

            entity.HasIndex(e => e.CategoryId, "fk_draft_categories_idx");

            entity.Property(e => e.DraftId).HasColumnName("draft_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreationTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("creation_time");
            entity.Property(e => e.IsArhived).HasColumnName("is_arhived");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.LastChangeTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_change_time");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Drafts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_draft_categories");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PRIMARY");

            entity.ToTable("note");

            entity.HasIndex(e => e.CategoryId, "fk_note_category_idx");

            entity.Property(e => e.NoteId).HasColumnName("note_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Content)
                .HasColumnType("json")
                .HasColumnName("content");
            entity.Property(e => e.CreationTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("creation_time");
            entity.Property(e => e.IsArhived).HasColumnName("is_arhived");
            entity.Property(e => e.IsCommented).HasColumnName("is_commented");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.LastChangeTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_change_time");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Notes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_note_category");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "uq_email").IsUnique();

            entity.HasIndex(e => e.Login, "uq_login").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.IsVerify).HasColumnName("is_verify");
            entity.Property(e => e.LastActiveTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_active_time");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(60)
                .IsFixedLength()
                .HasColumnName("password");

            entity.HasMany(d => d.Categories).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_user_categories_categories"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_user_categories_users"),
                    j =>
                    {
                        j.HasKey("UserId", "CategoryId").HasName("PRIMARY");
                        j.ToTable("user_categories");
                        j.HasIndex(new[] { "CategoryId" }, "fk_user_categories_categories_idx");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("CategoryId").HasColumnName("category_id");
                    });

            entity.HasMany(d => d.Drafts).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserDraft",
                    r => r.HasOne<Draft>().WithMany()
                        .HasForeignKey("DraftId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_user_drafts_drafts"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_user_drafts_users"),
                    j =>
                    {
                        j.HasKey("UserId", "DraftId").HasName("PRIMARY");
                        j.ToTable("user_drafts");
                        j.HasIndex(new[] { "DraftId" }, "fk_user_drafts_drafts_idx");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("DraftId").HasColumnName("draft_id");
                    });
        });

        modelBuilder.Entity<UserNote>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.NoteId }).HasName("PRIMARY");

            entity.ToTable("user_notes");

            entity.HasIndex(e => e.NoteId, "fk_user_notes_notes_idx");

            entity.HasIndex(e => e.RoleId, "fk_user_notes_roleid_idx");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.NoteId).HasColumnName("note_id");
            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("'1'")
                .HasColumnName("role_id");

            entity.HasOne(d => d.Note).WithMany(p => p.UserNotes)
                .HasForeignKey(d => d.NoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_notes_noteid");

            entity.HasOne(d => d.Role).WithMany(p => p.UserNotes)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_notes_roleid");

            entity.HasOne(d => d.User).WithMany(p => p.UserNotes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_notes_userid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
