namespace Core.Entities.OrderAggregate;
public class Address
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public Address() { }
    public Address(string firstName, string lastName, string city, string street, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        City = city;
        Street = street;
        State = state;
        ZipCode = zipCode;
    }
}
