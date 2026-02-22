namespace Basket.API.Services;

public class DiscountService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
{
    public async Task<CouponModel> GetDiscount(string productName)
    {
        var request = new GetDiscountRequest { ProductName = productName };
        var response = await discountProtoServiceClient.GetDiscountAsync(request);
        return response;
    }
}