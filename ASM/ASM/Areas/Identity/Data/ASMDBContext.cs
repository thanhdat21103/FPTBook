using ASM.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ASM.Models;

namespace ASM.Areas.Identity.Data;

public class ASMDBContext : IdentityDbContext<ManageUser>
{
    public ASMDBContext(DbContextOptions<ASMDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<ASM.Models.Book>? Books { get; set; }

    public DbSet<ASM.Models.Cart>? Carts { get; set; }

    public DbSet<ASM.Models.Category>? Categorys { get; set; }
}
public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ManageUser>
{
    public void Configure(EntityTypeBuilder<ManageUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(255);
    }
}
