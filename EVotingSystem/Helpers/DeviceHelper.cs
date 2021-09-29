using System;

namespace EVotingSystem.Utilities
{
    public class DeviceHelper
    {
        public static string GetVerificationCode(int length)
        {
            char[] chArray = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(1, chArray.Length);
                if (!str.Contains(chArray.GetValue(index).ToString()))
                {
                    str += chArray.GetValue(index);
                }
                else
                {
                    i--;
                }
            }
            return str;
        }
    }
}