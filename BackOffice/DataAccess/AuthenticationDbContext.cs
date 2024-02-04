using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.DataAccess;

/// <summary>Class representing the authentication database context.</summary>
public class AuthenticationDbContext : IdentityDbContext<UserEntity, IdentityRole<long>, long>
{
    /// <summary>Initializes a new instance of the <see cref="AuthenticationDbContext"/> class.</summary>
    protected AuthenticationDbContext() { }
    
    /// <summary>Initializes a new instance of <see cref="AuthenticationDbContext"/>.</summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AuthenticationDbContext(DbContextOptions options) : base(options) { }
}