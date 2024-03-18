public enum Status
{
    Approved = 1,
    Pending = 2,
    Rejected = 4
}
public class Jesta
{
    public int Id { get; set; }
    public int OfferedId { get; set; }

    public int OffereeId { get; set; }

    public int TransactionId { get; set; }

    public Status Status { get; set; }

    public string? Details { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}