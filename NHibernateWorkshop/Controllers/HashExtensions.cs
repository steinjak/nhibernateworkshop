using System.Security.Cryptography;
using System.Text;

namespace NHibernateWorkshop.Controllers
{
    public static class HashExtensions
    {
        private static readonly MD5 md5 = MD5.Create();

        public static string Md5(this string input)
        {
            return md5.ComputeHash(Encoding.UTF8.GetBytes(input)).ToHex(false);
        }

        public static string ToHex(this byte[] bytes, bool upperCase)
        {
            var result = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
            {
                result.Append(b.ToString(upperCase ? "X2" : "x2"));
            }

            return result.ToString();
        } 
    }
}