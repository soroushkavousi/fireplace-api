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

        public static string RandomString(int length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789".Shuffle();
            var randomString = "";
            for (int i = 0; i < length; i++)
                randomString += chars[_random.Next(chars.Length)];
            return randomString;
        }

        public static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public static void CreateParentDirectoriesOfFileIfNotExists(string filePath)
        {
            Directory.CreateDirectory(Directory.GetParent(filePath).FullName);
        }
    }
}
