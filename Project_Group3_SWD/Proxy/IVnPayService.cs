using Project_Group3_SWD.ViewModels;

namespace Project_Group3_SWD.Proxy
{
    public interface IVnPayService 
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
