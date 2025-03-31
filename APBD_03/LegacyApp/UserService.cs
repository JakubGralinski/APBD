using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository clientRepo;
        private readonly IUserCreditServiceFactory creditServiceFactory;
        
        public UserService() : this(new ClientRepository(), new UserCreditServiceFactory())
        {
        }
        
        public UserService(IClientRepository clientRepo, IUserCreditServiceFactory creditServiceFactory)
        {
            this.clientRepo = clientRepo;
            this.creditServiceFactory = creditServiceFactory;
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateUserData(firstName, lastName, email, dateOfBirth))
                return false;

            var client = clientRepo.GetById(clientId);
            if (client == null)
                return false;

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            SetUserCredit(user, client);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
        
        private bool ValidateUserData(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                return false;

            if (!IsValidEmail(email))
                return false;

            if (GetAge(dateOfBirth) < 21)
                return false;

            return true;
        }
        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private int GetAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;
            return age;
        }

        private void SetUserCredit(User user, Client client)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var creditService = creditServiceFactory.Create())
                {
                    int creditLimit = creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit * 2;
                    user.HasCreditLimit = true;
                }
            }
            else
            {
                using (var creditService = creditServiceFactory.Create())
                {
                    int creditLimit = creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                    user.HasCreditLimit = true;
                }
            }
        }
    }
}
