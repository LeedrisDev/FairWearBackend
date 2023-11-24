namespace Users.Service.Models.Entity;

public partial class ProductEntity
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public int Rating { get; set; }

    public virtual ICollection<UserProductHistoryEntity> UserProductHistories { get; set; } =
        new List<UserProductHistoryEntity>();
}