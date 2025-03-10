////////////////////////////////////////////////
// � https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace IdentityLib;

/// <summary>
/// Identity `ApplicationUser` context
/// </summary>
public class IdentityAppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    /// <summary>
    /// Identity `ApplicationUser` context
    /// </summary>
    public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options) : base(options)
    {
//#if DEBUG
//        Database.EnsureCreated();
//#else
        Database.Migrate();
//#endif
    }

    /// <summary>
    /// Telegram ������������
    /// </summary>
    public DbSet<TelegramUserModelDb> TelegramUsers { get; set; } = default!;

    /// <summary>
    /// ��������, ��������� � ����������� Telegram �������� � ������� ������ �����
    /// </summary>
    public DbSet<TelegramJoinAccountModelDb> TelegramJoinActions { get; set; } = default!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRoleClaim<string>>().ToTable("ApplicationRoleClaims");
        builder.Entity<IdentityUserRole<string>>().ToTable("ApplicationUserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("ApplicationUserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("ApplicationUserLogins");
        builder.Entity<IdentityUserToken<string>>().ToTable("ApplicationUserTokens");

        builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
        builder.Entity<ApplicationRole>().ToTable("ApplicationRoles");

        builder.Entity<IdentityUserRole<string>>().HasKey((IdentityUserRole<string> r) => new { r.UserId, r.RoleId });
    }
}