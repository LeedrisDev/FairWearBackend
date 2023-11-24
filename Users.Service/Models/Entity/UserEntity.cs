namespace Users.Service.Models.Entity;

public partial class UserEntity
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? LanguagePreferences { get; set; }

    public string? Theme { get; set; }

    public string FirebaseId { get; set; } = null!;

    public virtual ICollection<UserExperienceEntity> UserExperiences { get; set; } = new List<UserExperienceEntity>();

    public virtual ICollection<UserProductHistoryEntity> UserProductHistories { get; set; } =
        new List<UserProductHistoryEntity>();
}