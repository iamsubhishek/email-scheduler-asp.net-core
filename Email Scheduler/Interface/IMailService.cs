using Email_Scheduler.Models;
using System.Threading.Tasks;

namespace Email_Scheduler.Interface
{
    public interface IMailService
    {
        //Task SendEmailAsync(MailRequest mailRequest);
        void SendEmail();
    }
}
