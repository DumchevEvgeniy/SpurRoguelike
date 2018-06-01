using System;
using System.Collections.Generic;
using System.Linq;

namespace SpurRoguelike.WebPlayerBot.Extensions {
    internal static class EnumerableExtensions {
        public static Boolean IsEmpty<T>(this IEnumerable<T> collection) => !collection.Any();
    }
}