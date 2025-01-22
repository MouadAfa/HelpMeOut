namespace HelpMeOut.Repository.DTOs;

public class AddressDto
{
    public int Id { get; set; }
    public int StreetNumber { get; set; }
    public string StreetName { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int ZipCode { get; set; }
    public string Country { get; set; }

    protected bool Equals(AddressDto other)
    {
        return StreetNumber == other.StreetNumber && StreetName == other.StreetName && City == other.City && State == other.State && ZipCode == other.ZipCode && Country == other.Country;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((AddressDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StreetNumber, StreetName, City, State, ZipCode, Country);
    }
}