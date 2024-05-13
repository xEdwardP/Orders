using Orders.Shared.Responses;

namespace Orders.Backend.Helpers.MailHelper
{
    public interface IMailHelper
    {
        ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body);
    }
}
