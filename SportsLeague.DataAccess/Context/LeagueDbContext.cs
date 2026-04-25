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
    public DbSet<Sponsor> Sponsors => Set<Sponsor>();
    public DbSet<TournamentSponsor> TournamentSponsors => Set<TournamentSponsor>();
    public DbSet<Match> Matches => Set<Match>();


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

            // Índice único: nombre de patrocinador único
            _ = entity.HasIndex(s => s.Name)
                  .IsUnique();
        });

        // ── TournamentSponsor Configuration ──
        _ = modelBuilder.Entity<TournamentSponsor>(entity =>
        {
            _ = entity.HasKey(ts => ts.Id);
            _ = entity.Property(ts => ts.ContractAmount)
                  .IsRequired()
                  .HasPrecision(18, 2);
            _ = entity.Property(ts => ts.CreatedAt)
                  .IsRequired();
            _ = entity.Property(ts => ts.UpdatedAt)
                  .IsRequired(false);

            // Relación con Tournament
            _ = entity.HasOne(ts => ts.Tournament)
                  .WithMany(t => t.TournamentSponsors)
                  .HasForeignKey(ts => ts.TournamentId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relación con Sponsor
            _ = entity.HasOne(ts => ts.Sponsor)
                  .WithMany(s => s.TournamentSponsors)
                  .HasForeignKey(ts => ts.SponsorId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índice único compuesto: un sponsor solo una vez por torneo
            _ = entity.HasIndex(ts => new { ts.TournamentId, ts.SponsorId })
                  .IsUnique();
        });
        // ── Match Configuration ──
        _ = modelBuilder.Entity<Match>(entity =>
        {
            _ = entity.HasKey(m => m.Id);
            _ = entity.Property(m => m.MatchDate)
                  .IsRequired();
            _ = entity.Property(m => m.Venue)
                  .HasMaxLength(150);
            _ = entity.Property(m => m.Matchday)
                  .IsRequired();
            _ = entity.Property(m => m.Status)
                  .IsRequired();
            _ = entity.Property(m => m.CreatedAt)
                  .IsRequired();
            _ = entity.Property(m => m.UpdatedAt)
                  .IsRequired(false);

            // Relación con Tournament (Cascade: eliminar torneo elimina partidos)
            _ = entity.HasOne(m => m.Tournament)
                  .WithMany(t => t.Matches)
                  .HasForeignKey(m => m.TournamentId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relación con HomeTeam (Restrict: evita ciclo de cascada)
            _ = entity.HasOne(m => m.HomeTeam)
                  .WithMany(t => t.HomeMatches)
                  .HasForeignKey(m => m.HomeTeamId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relación con AwayTeam (Restrict: evita ciclo de cascada)
            _ = entity.HasOne(m => m.AwayTeam)
                  .WithMany(t => t.AwayMatches)
                  .HasForeignKey(m => m.AwayTeamId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relación con Referee (Restrict: no eliminar árbitro con partidos)
            _ = entity.HasOne(m => m.Referee)
                  .WithMany(r => r.Matches)
                  .HasForeignKey(m => m.RefereeId)
                  .OnDelete(DeleteBehavior.Restrict);

            // ── MatchResult Configuration ──
            modelBuilder.Entity<MatchResult>(entity =>
            {
                entity.HasKey(mr => mr.Id);
                entity.Property(mr => mr.HomeGoals).IsRequired();
                entity.Property(mr => mr.AwayGoals).IsRequired();
                entity.Property(mr => mr.Observations).HasMaxLength(500);
                entity.Property(mr => mr.CreatedAt).IsRequired();
                entity.Property(mr => mr.UpdatedAt).IsRequired(false);

                // Relación 1:1 con Match
                entity.HasOne(mr => mr.Match)
                      .WithOne(m => m.MatchResult)
                      .HasForeignKey<MatchResult>(mr => mr.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único en MatchId garantiza relación 1:1
                entity.HasIndex(mr => mr.MatchId).IsUnique();
            });

            // ── Goal Configuration ──
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Minute).IsRequired();
                entity.Property(g => g.Type).IsRequired();
                entity.Property(g => g.CreatedAt).IsRequired();
                entity.Property(g => g.UpdatedAt).IsRequired(false);

                entity.HasOne(g => g.Match)
                      .WithMany(m => m.Goals)
                      .HasForeignKey(g => g.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(g => g.Player)
                      .WithMany(p => p.Goals)
                      .HasForeignKey(g => g.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Card Configuration ──
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Minute).IsRequired();
                entity.Property(c => c.Type).IsRequired();
                entity.Property(c => c.CreatedAt).IsRequired();
                entity.Property(c => c.UpdatedAt).IsRequired(false);

                entity.HasOne(c => c.Match)
                      .WithMany(m => m.Cards)
                      .HasForeignKey(c => c.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Player)
                      .WithMany(p => p.Cards)
                      .HasForeignKey(c => c.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });



        });

    }
}



