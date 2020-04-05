﻿using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace GamingCommunityApi.Core.Tools
{
    public static class Utils
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly Random random = new Random();

        public static T CreateInstance<T>() => (T)Activator.CreateInstance(typeof(T), true);

        public static string RandomString(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789".Shuffle();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int RandomNumber(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public static void CreateParentDirectoriesOfFileIfNotExists(string filePath)
        {
            Directory.CreateDirectory(Directory.GetParent(filePath).FullName);
        }

        public static string GetSolutionDirectory()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string solutionDirectory;
            if (isWindows)
                solutionDirectory = @"D:\Projects\GamingCommunity\GamingCommunityApi\";
            else
                solutionDirectory = @"/home/bitianist/Projects/GamingCommunity/GamingCommunityApi/";
            return solutionDirectory;
        }
    }
}