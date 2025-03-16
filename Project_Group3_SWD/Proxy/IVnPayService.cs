using A_LIÊM_SHOP.ViewModels;

namespace A_LIÊM_SHOP.Proxy
{
    public interface IVnPayService 
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
