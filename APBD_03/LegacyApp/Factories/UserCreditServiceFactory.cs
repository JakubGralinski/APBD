namespace LegacyApp
{
    public class UserCreditServiceFactory : IUserCreditServiceFactory
    {
        public IUserCreditService Create()
        {
            return new UserCreditService();
        }
    }
}