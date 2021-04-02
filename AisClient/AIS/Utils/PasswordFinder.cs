using Org.BouncyCastle.OpenSsl;

namespace AIS.Utils
{
    public class PasswordFinder : IPasswordFinder
    {
        private readonly string password;

        public PasswordFinder(string password)
        {
            this.password = password;
        }


        public char[] GetPassword()
        {
            return password.ToCharArray();
        }
    }
}
