using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context;

public class LeagueDbContext : DbContext
{
    public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
        : base(options)
    {
    }

    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Referee> Referees => Set<Referee>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Team Configuration ──
        _ = modelBuilder.Entity<Team>(entity =>
        {
            _ = entity.HasKey(s => s.Id);
            _ = entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            _ = entity.Property(s => s.City)
                  .IsRequired()
                  .HasMaxLength(100);
            _ = entity.Property(s => s.Stadium)
                  .HasMaxLength(150);
            _ = entity.Property(s => s.LogoUrl)
                  .HasMaxLength(500);
            _ = entity.Property(s => s.CreatedAt)
                  .IsRequired();
            _ = entity.Property(s => s.UpdatedAt)
                  .IsRequired(false);
            _ = entity.HasIndex(s => s.Name)
                  .IsUnique();
        });

        // ── Player Configuration ──
        _ = modelBuilder.Entity<Player>(entity =>
        {
            _ = entity.HasKey(p => p.Id);
            _ = entity.Property(p => p.FirstName)
                  .IsRequired()
                  .HasMaxLength(80);
            _ = entity.Property(p => p.LastName)
                  .IsRequired()
                  .HasMaxLength(80);
            _ = entity.Property(p => p.BirthDate)
                  .IsRequired();
            _ = entity.Property(p => p.Number)
                  .IsRequired();
            _ = entity.Property(p => p.Position)
                  .IsRequired();
            _ = entity.Property(p => p.CreatedAt)
                  .IsRequired();
            _ = entity.Property(p => p.UpdatedAt)
                  .IsRequired(false);

            // Relación 1:N con Team
            _ = entity.HasOne(p => p.Team)
                  .WithMany(t => t.Players)
                  .HasForeignKey(p => p.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índice único compuesto: número de camiseta único por equipo
            _ = entity.HasIndex(p => new { p.TeamId, p.Number })
                  .IsUnique();
        });
        // ── Referee Configuration ──
        _ = modelBuilder.Entity<Referee>(entity =>
        {
            _ = entity.HasKey(r => r.Id);
            _ = entity.Property(r => r.FirstName)
                  .IsRequired()
                  .HasMaxLength(80);
            _ = entity.Property(r => r.LastName)
                  .IsRequired()
                  .HasMaxLength(80);
            _ = entity.Property(r => r.Nationality)
                  .IsRequired()
                  .HasMaxLength(80);
            _ = entity.Property(r => r.CreatedAt)
                  .IsRequired();
            _ = entity.Property(r => r.UpdatedAt)
                  .IsRequired(false);
        });

        // ── Tournament Configuration ──
        _ = modelBuilder.Entity<Tournament>(entity =>
        {
            _ = entity.HasKey(s => s.Id);
            _ = entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(150);
            _ = entity.Property(s => s.Season)
                  .IsRequired()
                  .HasMaxLength(20);
            _ = entity.Property(s => s.StartDate)
                  .IsRequired();
            _ = entity.Property(s => s.EndDate)
                  .IsRequired();
            _ = entity.Property(s => s.Status)
                  .IsRequired();
            _ = entity.Property(s => s.CreatedAt)
                  .IsRequired();
            _ = entity.Property(s => s.UpdatedAt)
                  .IsRequired(false);
        });

        // ── TournamentTeam Configuration ──
        _ = modelBuilder.Entity<TournamentTeam>(entity =>
        {
            _ = entity.HasKey(tt => tt.Id);
            _ = entity.Property(tt => tt.RegisteredAt)
                  .IsRequired();
            _ = entity.Property(tt => tt.CreatedAt)
                  .IsRequired();
            _ = entity.Property(tt => tt.UpdatedAt)
                  .IsRequired(false);

            // Relación con Tournament
            _ = entity.HasOne(tt => tt.Tournament)
                  .WithMany(t => t.TournamentTeams)
                  .HasForeignKey(tt => tt.TournamentId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relación con Team
            _ = entity.HasOne(tt => tt.Team)
                  .WithMany(t => t.TournamentTeams)
                  .HasForeignKey(tt => tt.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índice único compuesto: un equipo solo una vez por torneo
            _ = entity.HasIndex(tt => new { tt.TournamentId, tt.TeamId })
                  .IsUnique();
        });


        //-- Sponsor Configuration --
        _ = modelBuilder.Entity<Sponsor>(entity =>
        {
            _ = entity.HasKey(s => s.Id);
            _ = entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(150);
            _ = entity.Property<string>(s => s.ContactEmail)
                .IsRequired()
                .HasMaxLength(150);
            _ = entity.Property(s => s.Phone)
                .HasMaxLength(20);
            _ = entity.Property(s => s.WebsiteUrl)
                .HasMaxLength(150);
            _ = entity.Property(s => s.Category)
                .IsRequired();
            _ = entity.Property(s => s.CreatedAt)
                .IsRequired();
            _ = entity.Property(s => s.UpdatedAt)
                .IsRequired(false);


            // Índice único: un patrocinador solo puede patrocinar a un equipo o torneo específico
            _ = entity.HasIndex(s => new { s.Name })
                  .IsUnique();

            //

        });


    }
}



