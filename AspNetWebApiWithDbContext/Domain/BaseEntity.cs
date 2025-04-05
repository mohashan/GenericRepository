namespace AspNetWebApiWithDbContext.Domain;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset InsertDate { get; set; } = DateTime.UtcNow;
}