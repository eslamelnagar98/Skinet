namespace API.Dtos;
public class AddressDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }

    public static explicit operator AddressDto(Address address)
    {
        return new()
        {
            FirstName = address.FirstName,
            LastName = address.LastName,
            Street = address.Street,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
        };
    }


    public static explicit operator Address(AddressDto addressDto)
    {
        return new()
        {
            FirstName = addressDto.FirstName,
            LastName = addressDto.LastName,
            Street = addressDto.Street,
            City = addressDto.City,
            State = addressDto.State,
            ZipCode = addressDto.ZipCode,
        };
    }


}
