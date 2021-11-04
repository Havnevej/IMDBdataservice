using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IMDBdataservice
{
    public partial class imdbContext : DbContext
    {
        public imdbContext()
        {
        }

        public imdbContext(DbContextOptions<imdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookmarkPerson> BookmarkPeople { get; set; }
        public virtual DbSet<BookmarkTitle> BookmarkTitles { get; set; }
        public virtual DbSet<CharacterName> CharacterNames { get; set; }
        public virtual DbSet<Crew> Crews { get; set; }
        public virtual DbSet<Episode> Episodes { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<KnownForTitle> KnownForTitles { get; set; }
        public virtual DbSet<Omdb> Omdbs { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PersonRating> PersonRatings { get; set; }
        public virtual DbSet<Principal> Principals { get; set; }
        public virtual DbSet<Profession> Professions { get; set; }
        public virtual DbSet<SearchHistory> SearchHistories { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<TitleRating> TitleRatings { get; set; }
        public virtual DbSet<TitleVersion> TitleVersions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserTitleRating> UserTitleRatings { get; set; }
        public virtual DbSet<WordIndex> WordIndices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseNpgsql("host=localhost;database=imdb;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto")
                .HasAnnotation("Relational:Collation", "Danish_Denmark.1252");

            modelBuilder.Entity<BookmarkPerson>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bookmark_person");

                entity.Property(e => e.PersonId)
                    .HasColumnType("character varying")
                    .HasColumnName("person_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("character varying")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<BookmarkTitle>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bookmark_title");

                entity.Property(e => e.TitleId)
                    .HasColumnType("character varying")
                    .HasColumnName("title_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("character varying")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<CharacterName>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.TitleId, e.CharacterName1 })
                    .HasName("character_names_pkey");

                entity.ToTable("character_names");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(255)
                    .HasColumnName("person_id");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");

                entity.Property(e => e.CharacterName1)
                    .HasMaxLength(555)
                    .HasColumnName("character_name");
            });

            modelBuilder.Entity<Crew>(entity =>
            {
                entity.HasKey(e => new { e.TitleId, e.Ordering })
                    .HasName("crew_pkey");

                entity.ToTable("crew");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");

                entity.Property(e => e.Ordering).HasColumnName("ordering");

                entity.Property(e => e.AdditionalProfession)
                    .HasMaxLength(255)
                    .HasColumnName("additional_profession");

                entity.Property(e => e.IsPrincipal).HasColumnName("is_principal");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(255)
                    .HasColumnName("person_id");

                entity.Property(e => e.PrimaryProfession)
                    .HasMaxLength(255)
                    .HasColumnName("primary_profession");
            });

            modelBuilder.Entity<Episode>(entity =>
            {
                entity.HasKey(e => new { e.ParentTitleId, e.TitleId })
                    .HasName("episodes_pkey");

                entity.ToTable("episodes");

                entity.Property(e => e.ParentTitleId)
                    .HasMaxLength(255)
                    .HasColumnName("parent_title_id");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");

                entity.Property(e => e.EpisodeNr)
                    .HasMaxLength(255)
                    .HasColumnName("episode_nr");

                entity.Property(e => e.SeasonNr)
                    .HasMaxLength(255)
                    .HasColumnName("season_nr");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => new { e.TitleId, e.GenreName })
                    .HasName("genre_pkey");

                entity.ToTable("genre");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");

                entity.Property(e => e.GenreName)
                    .HasMaxLength(255)
                    .HasColumnName("genre_name");
            });

            modelBuilder.Entity<KnownForTitle>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.TitleId })
                    .HasName("known_for_titles_pkey");

                entity.ToTable("known_for_titles");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(255)
                    .HasColumnName("person_id");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");
            });

            modelBuilder.Entity<Omdb>(entity =>
            {
                entity.HasKey(e => e.TitleId)
                    .HasName("omdb_pkey");

                entity.ToTable("omdb");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");

                entity.Property(e => e.Awards).HasColumnName("awards");

                entity.Property(e => e.Plot).HasColumnName("plot");

                entity.Property(e => e.Poster)
                    .HasMaxLength(256)
                    .HasColumnName("poster");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(255)
                    .HasColumnName("person_id");

                entity.Property(e => e.Birthyear)
                    .HasMaxLength(255)
                    .HasColumnName("birthyear");

                entity.Property(e => e.Deathyear)
                    .HasMaxLength(255)
                    .HasColumnName("deathyear");

                entity.Property(e => e.PersonName)
                    .HasMaxLength(255)
                    .HasColumnName("person_name");
            });

            modelBuilder.Entity<PersonRating>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("person_rating");

                entity.Property(e => e.NumVotes).HasColumnName("num_votes");

                entity.Property(e => e.PersonId)
                    .HasColumnType("character varying")
                    .HasColumnName("person_id");

                entity.Property(e => e.PersonName)
                    .HasColumnType("character varying")
                    .HasColumnName("person_name");

                entity.Property(e => e.Rating).HasColumnName("rating");
            });

            modelBuilder.Entity<Principal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("principals");

                entity.Property(e => e.Category)
                    .HasMaxLength(255)
                    .HasColumnName("category");

                entity.Property(e => e.Job)
                    .HasMaxLength(255)
                    .HasColumnName("job");

                entity.Property(e => e.Ordering).HasColumnName("ordering");

                entity.Property(e => e.PersonId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("person_id");

                entity.Property(e => e.TitleId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("title_id")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Profession>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.ProfessionType })
                    .HasName("profession_pkey");

                entity.ToTable("profession");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(255)
                    .HasColumnName("person_id");

                entity.Property(e => e.ProfessionType)
                    .HasMaxLength(255)
                    .HasColumnName("profession_type");
            });

            modelBuilder.Entity<SearchHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("search_history");

                entity.Property(e => e.Date)
                    .HasColumnType("character varying")
                    .HasColumnName("date");

                entity.Property(e => e.SearchString)
                    .HasColumnType("character varying")
                    .HasColumnName("search_string");

                entity.Property(e => e.UserId)
                    .HasColumnType("character varying")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.ToTable("title");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");

                entity.Property(e => e.EndYear)
                    .HasMaxLength(255)
                    .HasColumnName("end_year");

                entity.Property(e => e.IsAdult).HasColumnName("is_adult");

                entity.Property(e => e.OriginalTitle)
                    .HasMaxLength(255)
                    .HasColumnName("original_title");

                entity.Property(e => e.PrimaryTitle)
                    .HasMaxLength(255)
                    .HasColumnName("primary_title");

                entity.Property(e => e.RunTimeMinutes).HasColumnName("run_time_minutes");

                entity.Property(e => e.StartYear)
                    .HasMaxLength(255)
                    .HasColumnName("start_year");

                entity.Property(e => e.TitleType)
                    .HasMaxLength(255)
                    .HasColumnName("title_type");
            });

            modelBuilder.Entity<TitleRating>(entity =>
            {
                entity.HasKey(e => e.TitleId)
                    .HasName("title_rating_pkey");

                entity.ToTable("title_rating");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");

                entity.Property(e => e.RatingAvg).HasColumnName("rating_avg");

                entity.Property(e => e.Votes)
                    .HasMaxLength(255)
                    .HasColumnName("votes");
            });

            modelBuilder.Entity<TitleVersion>(entity =>
            {
                entity.HasKey(e => new { e.TitleId, e.TitleVersion1 })
                    .HasName("title_versions_pkey");

                entity.ToTable("title_versions");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(255)
                    .HasColumnName("title_id");

                entity.Property(e => e.TitleVersion1)
                    .HasMaxLength(255)
                    .HasColumnName("title_version");

                entity.Property(e => e.Attributes)
                    .HasMaxLength(255)
                    .HasColumnName("attributes");

                entity.Property(e => e.IsOriginalTitle).HasColumnName("is_original_title");

                entity.Property(e => e.Language)
                    .HasMaxLength(255)
                    .HasColumnName("language");

                entity.Property(e => e.Region)
                    .HasMaxLength(255)
                    .HasColumnName("region");

                entity.Property(e => e.TitleName)
                    .HasMaxLength(255)
                    .HasColumnName("title_name");

                entity.Property(e => e.Types)
                    .HasMaxLength(255)
                    .HasColumnName("types");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user");

                entity.Property(e => e.Password)
                    .HasColumnType("character varying")
                    .HasColumnName("password");

                entity.Property(e => e.UserId)
                    .HasColumnType("character varying")
                    .HasColumnName("user_id");

                entity.Property(e => e.UserName)
                    .HasColumnType("character varying")
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<UserTitleRating>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user_title_rating");

                entity.Property(e => e.Rating)
                    .HasColumnType("character varying")
                    .HasColumnName("rating");

                entity.Property(e => e.TitleId)
                    .HasColumnType("character varying")
                    .HasColumnName("title_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("character varying")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<WordIndex>(entity =>
            {
                entity.HasKey(e => new { e.TitleId, e.Word, e.Field })
                    .HasName("word_index_pkey");

                entity.ToTable("word_index");

                entity.Property(e => e.TitleId)
                    .HasMaxLength(10)
                    .HasColumnName("title_id")
                    .IsFixedLength(true);

                entity.Property(e => e.Word)
                    .HasMaxLength(255)
                    .HasColumnName("word");

                entity.Property(e => e.Field)
                    .HasMaxLength(255)
                    .HasColumnName("field");

                entity.Property(e => e.Lexeme)
                    .HasMaxLength(255)
                    .HasColumnName("lexeme");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
