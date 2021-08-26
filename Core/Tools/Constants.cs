using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Tools
{
    public static class Constants
    {
        public static string FilesBaseUrlPathKey { get; } = "Files:BaseUrlPath";
        public static string FilesBasePhysicalPathKey { get; } = "Files:BasePhysicalPath";
        public static int FileNameLength { get; } = 12;
    }
}
