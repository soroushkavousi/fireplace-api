using FireplaceApi.Domain.Extensions;
using System;
using System.IO;

namespace FireplaceApi.Domain.Tools;

public static class Utils
{
    private static readonly Random _random = new();

    public static T CreateInstance<T>() => (T)Activator.CreateInstance(typeof(T), true);

    public static string GenerateRandomString(int length, bool uppercase = false, bool special = false)
    {
        var chars = @"abcdefghijklmnopqrstuvwxyz0123456789";
        if (uppercase)
            chars += @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (special)
            chars += @"!@#$%^&";
        chars = chars.Shuffle();
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
            ulongRand = BitConverter.ToUInt64(buf, 0);
        } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

        return (ulongRand % uRange) + min;
    }

    public static void CreateParentDirectoriesOfFileIfNotExists(string filePath)
    {
        Directory.CreateDirectory(Directory.GetParent(filePath).FullName);
    }
}
