using DomainModel.DtoModels;
using System.Security.Cryptography;
using System.Text;
using VacationDaysCalculatorWebAPI.Repositories;

namespace VacationDaysCalculatorWebAPI.Services
{
    public class CommonService
    {
        private readonly CommonRepository _commonRepository;

        public CommonService(CommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }
        public string GetUserPassword(int userId)
        {
            var user = _commonRepository.GetUser(userId);
            return user.Password;
        }
        public void ChangePassword(Password password)
        {
            var user = _commonRepository.GetUser(password.UserId);
            user.Password = HashUserPassword(password.NewPassword);
            _commonRepository.SaveChanges();
        }
        private string HashUserPassword(string plainTextPassword)
        {
            string hashedPassword;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, plainTextPassword);
            }
            return hashedPassword;
        }
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
