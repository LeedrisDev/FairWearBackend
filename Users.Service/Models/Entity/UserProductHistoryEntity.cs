namespace Users.Service.Models.Entity;

public partial class UserProductHistoryEntity
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long ProductId { get; set; }

    public DateOnly? Timestamp { get; set; }

    public virtual ProductEntity ProductEntity { get; set; } = null!;

    public virtual UserEntity UserEntity { get; set; } = null!;
}