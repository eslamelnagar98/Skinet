namespace Infrastructure.Data.DTOs;
public class ProductSeedDto
{
    public bool IsEmpty { get; set; }
    public Func<Task> ProductSeedMethod { get; set; }

    

}

