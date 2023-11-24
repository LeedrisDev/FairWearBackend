namespace Users.Service.Models.Entity;

public partial class UserExperienceEntity
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long? Score { get; set; }

    public int? Level { get; set; }

    public int[]? Todos { get; set; }

    public virtual UserEntity UserEntity { get; set; } = null!;
}