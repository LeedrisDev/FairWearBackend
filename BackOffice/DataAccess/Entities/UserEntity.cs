using Microsoft.AspNetCore.Identity;

namespace BackOffice.DataAccess.Entities;

/// <summary>Class representing a user in the database.</summary>
public class UserEntity: IdentityUser<long>
{
    /// <summary>The user's first name.</summary>
    [ProtectedPersonalData] public string FirstName { get; set; } = null!;
    
    /// <summary>The user's last name.</summary>
    [ProtectedPersonalData] public string LastName { get; set; } = null!;
}