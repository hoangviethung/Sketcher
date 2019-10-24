using System;

namespace MainProject.Framework.Helper
{
    public static class GenerateCodeHelper
    {
        public static string CreateRandomCode(int codeLength)
        {
            string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[codeLength];
            Random rd = new Random();

            for (int i = 0; i < codeLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
