// Guids.cs
// MUST match guids.h
using System;

namespace Mistilteinn
{
    static class GuidList
    {
        public const string guidMistilteinnPkgString = "060f1399-0689-4205-9ee8-9c1cffcdbda0";
        public const string guidMistilteinnCmdSetString = "26295019-2819-45b0-9304-fdf5ddce357b";
        public const string guidToolWindowPersistanceString = "2BAA7C52-5040-4565-B234-2D045148181F";

        public static readonly Guid guidMistilteinnCmdSet = new Guid(guidMistilteinnCmdSetString);
    };
}