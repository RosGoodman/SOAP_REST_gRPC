using ClinicService.Sesurity;

namespace Accounts.Helper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var (passwordSalt, passwordHash) = PasswordUtils.CreatePasswordHash("12345");
            Console.WriteLine(passwordSalt);
            Console.WriteLine(passwordHash);
            Console.ReadKey();
        }
    }
}