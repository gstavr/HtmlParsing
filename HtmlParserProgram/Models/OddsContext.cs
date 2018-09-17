using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HtmlParserProgram.Models
{
    public partial class OddsContext : DbContext
    {
        public OddsContext()
        {
        }

        public OddsContext(DbContextOptions<OddsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<Competition> Competition { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GamePick> GamePick { get; set; }
        public virtual DbSet<GamePickValue> GamePickValue { get; set; }
        public virtual DbSet<GamePickValueLog> GamePickValueLog { get; set; }
        public virtual DbSet<GroupCompetition> GroupCompetition { get; set; }
        public virtual DbSet<OddCategory> OddCategory { get; set; }
        public virtual DbSet<Sport> Sport { get; set; }
        public virtual DbSet<XGlobalCounters> XGlobalCounters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=GINOS\\SQLEXPRESS03;Database=Odds;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Companies>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descr).HasMaxLength(255);

                entity.Property(e => e.DynamicParam).HasMaxLength(255);
            });

            modelBuilder.Entity<Competition>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AlternativeDescr).HasMaxLength(255);

                entity.Property(e => e.DateUpdated).HasColumnType("date");

                entity.Property(e => e.Descr).HasMaxLength(255);

                entity.Property(e => e.DynamicId)
                    .HasColumnName("DynamicID")
                    .HasMaxLength(255);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.SportId).HasColumnName("SportID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Competition)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Competition$GroupID_ref_GroupCompetition");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Competition)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_Competition$SportID_ref_Sport");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AwayTeam).HasMaxLength(255);

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.DateUpdated).HasColumnType("date");

                entity.Property(e => e.Descr).HasMaxLength(255);

                entity.Property(e => e.DynamicId)
                    .HasColumnName("DynamicID")
                    .HasMaxLength(255);

                entity.Property(e => e.HomeTeam).HasMaxLength(255);

                entity.Property(e => e.MatchDate).HasColumnType("datetime");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.CompetitionId)
                    .HasConstraintName("FK_Game$Competition_ref_Competition");
            });

            modelBuilder.Entity<GamePick>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descr).HasMaxLength(255);

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamePick)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamePick$GameID_ref_Game");
            });

            modelBuilder.Entity<GamePickValue>(entity =>
            {
                entity.ToTable("GamePick_Value");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AlternativeDescr).HasMaxLength(255);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Descr).HasMaxLength(255);

                entity.Property(e => e.GamePickId).HasColumnName("GamePickID");

                entity.Property(e => e.OddsUpdated).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.GamePickValue)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_GamePick_Value$CompanyID_ref_Companies");

                entity.HasOne(d => d.GamePick)
                    .WithMany(p => p.GamePickValue)
                    .HasForeignKey(d => d.GamePickId)
                    .HasConstraintName("FK_GamePick_Value$GamePickID_ref_GamePick");
            });

            modelBuilder.Entity<GamePickValueLog>(entity =>
            {
                entity.ToTable("GamePick_Value_Log");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlternativeDescr).HasMaxLength(255);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Descr).HasMaxLength(255);

                entity.Property(e => e.GamePickId).HasColumnName("GamePickID");

                entity.Property(e => e.OddsUpdated).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.GamePickValueLog)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_GamePick_Value_Log$CompanyID_ref_Companies");

                entity.HasOne(d => d.GamePick)
                    .WithMany(p => p.GamePickValueLog)
                    .HasForeignKey(d => d.GamePickId)
                    .HasConstraintName("FK_GamePick_Value_Log$GamePick_ref_GamePick");
            });

            modelBuilder.Entity<GroupCompetition>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AlternativeDescr).HasMaxLength(255);

                entity.Property(e => e.Descr).HasMaxLength(255);
            });

            modelBuilder.Entity<OddCategory>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descr).HasMaxLength(255);
            });

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descr).HasMaxLength(255);
            });

            modelBuilder.Entity<XGlobalCounters>(entity =>
            {
                entity.HasKey(e => e.TblName);

                entity.ToTable("X_GlobalCounters");

                entity.Property(e => e.TblName)
                    .HasColumnName("tblName")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Counter).HasColumnName("counter");

                entity.Property(e => e.Step).HasColumnName("step");
            });
        }
    }
}
