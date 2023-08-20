using DomainModel.Enums;
using DomainModel.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using VacationDaysCalculatorWebAPI.DatabaseContext;
using VacationDaysCalculatorWebAPI.Constants;

namespace VacationDaysCalculatorWebAPI.Services
{
    public class EmailService
    {
        private string NAIS_API_KEY_NAME = "";
        private readonly string NAIS_MAIL = "Duje.Miljevic@nais.hr";
        private readonly string NAIS_USERNAME = "NAIS System";
        private readonly VacationDbContext _vacationDbContext;
        public EmailService(VacationDbContext vacationDbContext)
        {
            _vacationDbContext = vacationDbContext;
        }
        public void SendVacationResponseMail(int userId, VacationStatus status)
        {
            NAIS_API_KEY_NAME = GetApiKeyName();
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
			NAIS_API_KEY_NAME = GetApiKeyName();
			var admins = _vacationDbContext.Users.Where(u => u.Role == "Admin").ToList();
            foreach(var admin in admins)
            {
                var plainTextContent = StringConstants.GREETINGS + " " + admin.FirstName + StringConstants.NEW_VACATION_REQUEST_MESSAGE;
                var htmlContent = StringConstants.P_TAG + StringConstants.GREETINGS + " " + admin.FirstName + "," + StringConstants.CLOSED_P_TAG + StringConstants.NEW_VACATION_REQUEST_MESSAGE_P_TAG;
                SendMail(plainTextContent, htmlContent, StringConstants.VACATION_REQUEST, admin);
            }
        }

        public void SendSickLeaveClosedMail()
        {
			NAIS_API_KEY_NAME = GetApiKeyName();
			var admins = _vacationDbContext.Users.Where(u => u.Role == "Admin").ToList();
			foreach (var admin in admins)
			{
				var plainTextContent = StringConstants.GREETINGS + " " + admin.FirstName + StringConstants.SICK_LEAVE_CLOSED_MESSAGE;
				var htmlContent = StringConstants.P_TAG + StringConstants.GREETINGS + " " + admin.FirstName + "," + StringConstants.CLOSED_P_TAG + StringConstants.CLOSED_SICK_LEAVE_MESSAGE_P_TAG;
				SendMail(plainTextContent, htmlContent, StringConstants.SICK_LEAVE_CLOSED, admin);
			}
		}

        public void SendMail(string _plainTextContent, string _htmlContent, string _subject, User user)
        {
            var plainTextContent = _plainTextContent;
			var htmlContent = _htmlContent;
			var apiKey = NAIS_API_KEY_NAME;
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress(NAIS_MAIL, NAIS_USERNAME);
			var subject = _subject;
			var to = new EmailAddress(user.Email, user.FirstName);
			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			var response = client.SendEmailAsync(msg);
		}

        public string GetApiKeyName()
        {
            string apiKey = "";
            try
            {
                apiKey = File.ReadAllText("C:\\Users\\Duje\\Desktop\\ApiKey\\ApiMailKey.txt");
			}
            catch (Exception e)
            {
				Console.WriteLine("Path does not exist.");
			}
            return apiKey;
        }
    }
}
