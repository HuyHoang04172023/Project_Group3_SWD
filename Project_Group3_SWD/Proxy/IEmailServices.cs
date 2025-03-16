
namespace Project_Group3_SWD.Proxy {
    public interface IEmailServices
    {
        Task SendEmail(string receptor, string subject, string body);
    }
}