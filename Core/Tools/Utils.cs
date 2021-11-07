using FireplaceApi.Core.Extensions;
using NLog;
using System;
using System.IO;

namespace FireplaceApi.Core.Tools
{
    public static class Utils
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly Random _random = new Random();

        public static T CreateInstance<T>() => (T)Activator.CreateInstance(typeof(T), true);

        public static string GenerateRandomString(int length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789".Shuffle();
            var randomString = "";
            for (int i = 0; i < length; i++)
                randomString += chars[_random.Next(chars.Length)];
            return randomString;
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public static ulong GenerateRandomUlongNumber(ulong min, ulong max)
        {
            ulong uRange = (max - min);
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                _random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (ulongRand % uRange) + min;
        }

        public static void CreateParentDirectoriesOfFileIfNotExists(string filePath)
        {
            Directory.CreateDirectory(Directory.GetParent(filePath).FullName);
        }
    }
}
