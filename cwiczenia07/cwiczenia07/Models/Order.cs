namespace cwiczenia07.Models;

public class Order
{
    public int IdOrder { get; set; }
    public int IdProduct { get; set; }
    public int Amount { get; set; }
    public string CreatedAt { get; set; }
    public string FullfilledAt { get; set; }
}