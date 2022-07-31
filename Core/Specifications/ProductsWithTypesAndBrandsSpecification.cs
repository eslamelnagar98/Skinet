namespace Core.Specifications;
public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(int id)
        : base(product => product.Id == id)
        
    {
        AddInclude(product => product.ProductType);
        AddInclude(product => product.ProductBrand);
    }
    public ProductsWithTypesAndBrandsSpecification()
    {
        AddInclude(product => product.ProductType);
        AddInclude(product => product.ProductBrand);
    }
}

