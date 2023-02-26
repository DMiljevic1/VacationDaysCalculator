using DomainModel.Enums;
using SendGrid;
using SendGrid.Helpers.Mail;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Services
{
    public class EmailService
    {
        private readonly string NAIS_API_KEY_NAME = "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";
        private readonly string NAIS_MAIL = "duje.miljevic@gmail.com";
        private readonly string NAIS_USERNAME = "NAIS Vacation System";
        private readonly VacationDbContext _vacationDbContext;
        public EmailService(VacationDbContext vacationDbContext)
        {
            _vacationDbContext = vacationDbContext;
        }
        public void SendVacationResponseMail(int userId, VacationStatus status)
        {
            var user = _vacationDbContext.Users.FirstOrDefault(u => u.Id == userId);
            var plainTextContent = "";
            var htmlContent = "";
            if (status == VacationStatus.Approved)
            {
                plainTextContent = "Hello " + user.FirstName + "Your vacation request have been approved. Enjoy! Kind regards, NAIS team";
                htmlContent = "<p>Hello " + user.FirstName + ",</p>" + "<p>Your vacation request have been approved.</p> <p>Enjoy!</p> <p>Kind regards,</p> <p>NAIS team</p>";
            }
            else if(status == VacationStatus.Declined)
            {
                plainTextContent = "Hello " + user.FirstName + "We are sorry to inform you that your vacation request have been declined.Please contact system admin for more information.Kind regards, NAIS team";
                htmlContent = "<p>Hello " + user.FirstName + ",</p>" + "<p>We are sorry to inform you that your vacation request have been declined.</p> <p>Please contact system admin for more information.</p> <p>Kind regards,</p> <p>NAIS team</p>";
            }
            var apiKey = NAIS_API_KEY_NAME;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(NAIS_MAIL, NAIS_USERNAME);
            var subject = "Response on vacation request";
            var to = new EmailAddress(user.Email, user.FirstName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
        public void SendVacationRequestMail()
        {
            var admins = _vacationDbContext.Users.Where(u => u.Role == "Admin").ToList();
            foreach(var admin in admins)
            {
                var plainTextContent = "Hello " + admin.FirstName + "You have new vacation request. Please go to NAIS Vacation System to approve or decline request.Kind regards, NAIS team";
                var htmlContent = "<p>" + "Hello " + admin.FirstName + ",</p>" + "<p>You have new vacation request.</p> <p>Please go to NAIS Vacation System to approve or decline request.</p> <p>Kind regards,</p> <p>NAIS team</p>";
                var apiKey = NAIS_API_KEY_NAME;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(NAIS_MAIL, NAIS_USERNAME);
                var subject = "Vacation request";
                var to = new EmailAddress(admin.Email, admin.FirstName);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
        }
    }
}
