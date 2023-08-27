namespace Infrastructure.Services;
public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private static readonly Func<BasketItem, decimal> _calculateAmount = CalculateAmount;
    public PaymentService(IBasketRepository basketRepository,
                          IUnitOfWork unitOfWork,
                          IConfiguration configuration)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }
    public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
    {
        var stripeApiKey = _configuration.GetValue<string>("StripeSettings:SecretKey");
        StripeConfiguration.ApiKey = stripeApiKey;
        var basket = await _basketRepository.GetBasketAsync(basketId);
        var shippingPrice = 0m;
        if (basket.DeliveryMethod.HasValue)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                                                  .GetByIdAsync(basket.DeliveryMethod.Value);
            shippingPrice = deliveryMethod.Price;
        }
        await UpdateBasketItemsPrice(basket);
        await CreateOrUpdatePaymentIntent(basket, shippingPrice);
        await _basketRepository.UpdateBasketAsync(basket);
        return basket;
    }

    private async Task CreateOrUpdatePaymentIntent(CustomerBasket basket, decimal shippingPrice)
    {
        if (!string.IsNullOrWhiteSpace(basket.PaymentIntentId))
        {
            await UpdatePaymentIntent(basket, shippingPrice);
            return;
        }
        await CreatePaymentIntent(basket, shippingPrice);
    }

    private async Task CreatePaymentIntent(CustomerBasket basket, decimal shippingPrice)
    {
        var service = new PaymentIntentService();
        var intent = default(PaymentIntent);
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)basket.BasketItems.Sum(_calculateAmount) + ((long)shippingPrice * 100),
            Currency = "usd",
            PaymentMethodTypes = new List<string> { "card" }
        };
        intent = await service.CreateAsync(options);
        basket.PaymentIntentId = intent.Id;
        basket.ClientSecret = intent.ClientSecret;
    }

    private async Task UpdatePaymentIntent(CustomerBasket basket, decimal shippingPrice)
    {
        var service = new PaymentIntentService();
        var options = new PaymentIntentUpdateOptions
        {
            Amount = (long)basket.BasketItems.Sum(_calculateAmount) + ((long)shippingPrice * 100),
        };
        await service.UpdateAsync(basket.PaymentIntentId, options);
    }

    private async Task UpdateBasketItemsPrice(CustomerBasket basket)
    {
        foreach (var basketItem in basket.BasketItems)
        {
            var productItem = await _unitOfWork.Repository<Product>()
                                               .GetByIdAsync(basketItem.Id);
            if (basketItem.Price != productItem.Price)
            {
                basketItem.Price = productItem.Price;
            }
        }
    }

    private static decimal CalculateAmount(BasketItem basketItem)
    {
        var quantity = (decimal)basketItem.Quantity;
        var price = basketItem.Price * 100;
        return quantity * price;
    }
}
