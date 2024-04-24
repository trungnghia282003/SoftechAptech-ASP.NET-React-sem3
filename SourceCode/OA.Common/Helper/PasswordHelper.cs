namespace OA.Common.Helper
{
    public static class PasswordHelper
    {
        public static byte[] EncryptPassword(string password)
        {
            System.Text.ASCIIEncoding encryptpwd = new System.Text.ASCIIEncoding();
            return encryptpwd.GetBytes(password);
        }

        public static string DecryptPassword(byte[] encryptedPassword)
        {
            System.Text.ASCIIEncoding decryptpwd = new System.Text.ASCIIEncoding();
            return decryptpwd.GetString(encryptedPassword);
        }
    }
}
