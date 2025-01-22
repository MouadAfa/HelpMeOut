namespace HelpMeOut.Models;

public class Address
{
    public int StreetNumber { get; set; }
    public required string StreetName { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public int ZipCode { get; set; }
    public required string Country { get; set; }
}