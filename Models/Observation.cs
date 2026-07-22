using SQLite;

namespace MHike.Models;

public class Observation
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int HikeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Comment { get; set; } = string.Empty;
}