using System;
using System.Collections.Generic;
using IMDB_App.Models;
using Microsoft.EntityFrameworkCore;

namespace IMDB_App.Data;

public partial class ImdbContext : DbContext
{
    public ImdbContext()
    {
    }

    public ImdbContext(DbContextOptions<ImdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Name> Names { get; set; }

    public virtual DbSet<Principal> Principals { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<TitleAlias> TitleAliases { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=IMDB;Integrated Security=True;Trust Server Certificate=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.TitleId);

            entity.Property(e => e.TitleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("titleID");
            entity.Property(e => e.EpisodeNumber).HasColumnName("episodeNumber");
            entity.Property(e => e.ParentTitleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("parent_titleID");
            entity.Property(e => e.SeasonNumber).HasColumnName("seasonNumber");

            entity.HasOne(d => d.ParentTitle).WithMany(p => p.EpisodeParentTitles)
                .HasForeignKey(d => d.ParentTitleId)
                .HasConstraintName("FK_Episodes_Titles1");

            entity.HasOne(d => d.Title).WithOne(p => p.EpisodeTitle)
                .HasForeignKey<Episode>(d => d.TitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Episodes_Titles");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK_Title_Genres");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Name>(entity =>
        {
            entity.Property(e => e.NameId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nameID");
            entity.Property(e => e.BirthYear).HasColumnName("birthYear");
            entity.Property(e => e.DeathYear).HasColumnName("deathYear");
            entity.Property(e => e.PrimaryName)
                .HasMaxLength(125)
                .IsUnicode(false)
                .HasColumnName("primaryName");
            entity.Property(e => e.PrimaryProfession)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("primaryProfession");
        });

        modelBuilder.Entity<Principal>(entity =>
        {
            entity.HasKey(e => new { e.TitleId, e.Ordering });

            entity.Property(e => e.TitleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("titleID");
            entity.Property(e => e.Ordering).HasColumnName("ordering");
            entity.Property(e => e.Characters)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("characters");
            entity.Property(e => e.Job)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("job");
            entity.Property(e => e.JobCategory)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("job_category");
            entity.Property(e => e.NameId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nameID");

            entity.HasOne(d => d.Name).WithMany(p => p.Principals)
                .HasForeignKey(d => d.NameId)
                .HasConstraintName("FK_Principals_Names");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.TitleId);

            entity.Property(e => e.TitleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("titleID");
            entity.Property(e => e.AverageRating)
                .HasColumnType("numeric(4, 2)")
                .HasColumnName("averageRating");
            entity.Property(e => e.NumVotes).HasColumnName("numVotes");

            entity.HasOne(d => d.Title).WithOne(p => p.Rating)
                .HasForeignKey<Rating>(d => d.TitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ratings_Titles");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.Property(e => e.TitleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("titleID");
            entity.Property(e => e.EndYear).HasColumnName("endYear");
            entity.Property(e => e.IsAdult).HasColumnName("isAdult");
            entity.Property(e => e.OriginalTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("originalTitle");
            entity.Property(e => e.PrimaryTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("primaryTitle");
            entity.Property(e => e.RuntimeMinutes).HasColumnName("runtimeMinutes");
            entity.Property(e => e.StartYear).HasColumnName("startYear");
            entity.Property(e => e.TitleType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("titleType");

            entity.HasMany(d => d.Genres).WithMany(p => p.Titles)
                .UsingEntity<Dictionary<string, object>>(
                    "TitleGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Title_Genres_Titles"),
                    l => l.HasOne<Title>().WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Title_Genres_Titles1"),
                    j =>
                    {
                        j.HasKey("TitleId", "GenreId").HasName("PK_Title_Genres_1");
                        j.ToTable("Title_Genres");
                        j.IndexerProperty<string>("TitleId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("titleID");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genreID");
                    });

            entity.HasMany(d => d.Names).WithMany(p => p.Titles)
                .UsingEntity<Dictionary<string, object>>(
                    "Director",
                    r => r.HasOne<Name>().WithMany()
                        .HasForeignKey("NameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Directors_Names"),
                    l => l.HasOne<Title>().WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Directors_Titles1"),
                    j =>
                    {
                        j.HasKey("TitleId", "NameId");
                        j.ToTable("Directors");
                        j.IndexerProperty<string>("TitleId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("titleID");
                        j.IndexerProperty<string>("NameId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("nameID");
                    });

            entity.HasMany(d => d.Names1).WithMany(p => p.Titles1)
                .UsingEntity<Dictionary<string, object>>(
                    "Writer",
                    r => r.HasOne<Name>().WithMany()
                        .HasForeignKey("NameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Writers_Names"),
                    l => l.HasOne<Title>().WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Writers_Titles"),
                    j =>
                    {
                        j.HasKey("TitleId", "NameId");
                        j.ToTable("Writers");
                        j.IndexerProperty<string>("TitleId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("titleID");
                        j.IndexerProperty<string>("NameId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("nameID");
                    });

            entity.HasMany(d => d.NamesNavigation).WithMany(p => p.TitlesNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "KnownFor",
                    r => r.HasOne<Name>().WithMany()
                        .HasForeignKey("NameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Known_For_Names"),
                    l => l.HasOne<Title>().WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Known_For_Titles"),
                    j =>
                    {
                        j.HasKey("TitleId", "NameId").HasName("PK_KnownFor");
                        j.ToTable("Known_For");
                        j.IndexerProperty<string>("TitleId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("titleID");
                        j.IndexerProperty<string>("NameId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("nameID");
                    });
        });

        modelBuilder.Entity<TitleAlias>(entity =>
        {
            entity.HasKey(e => new { e.TitleId, e.Ordering }).HasName("PK_Title_AKAs");

            entity.ToTable("Title_Aliases");

            entity.Property(e => e.TitleId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("titleId");
            entity.Property(e => e.Ordering).HasColumnName("ordering");
            entity.Property(e => e.IsOriginalTitle).HasColumnName("isOriginalTitle");
            entity.Property(e => e.Language)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("language");
            entity.Property(e => e.Region)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("region");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.TitleNavigation).WithMany(p => p.TitleAliases)
                .HasForeignKey(d => d.TitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Title_Aliases_Titles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
