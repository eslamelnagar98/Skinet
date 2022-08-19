namespace Infrastructure.Data.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> productBuilder)
    {
        productBuilder.Property(product => product.Id)
                      .IsRequired();

        productBuilder.Property(product => product.Name)
                      .IsRequired()
                      .HasMaxLength(100);

        productBuilder.Property(product => product.Description)
                      .IsRequired()
                      .HasMaxLength(1000);

        productBuilder.Property(product => product.Price)
                      .HasColumnType("decimal(18,2)");

        productBuilder.Property(product => product.PictureUrl)
                      .IsRequired();

        productBuilder.HasOne(product => product.ProductBrand)
                      .WithMany()
                      .HasForeignKey(product => product.ProductBrandId);

        productBuilder.HasOne(product => product.ProductType)
                      .WithMany()
                      .HasForeignKey(product => product.ProductTypeId);


    }
}
