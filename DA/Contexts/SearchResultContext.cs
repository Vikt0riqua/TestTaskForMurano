using DA.Models;
using Microsoft.EntityFrameworkCore;

namespace DA.Contexts
{
    public class SearchResultContext: DbContext
    {
        public DbSet<SearchResult> SearchResults { get; set; }

        public SearchResultContext(DbContextOptions<SearchResultContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchResult>().HasIndex(u => new { u.Header, u.Link}).IsUnique();
        }
    }
}