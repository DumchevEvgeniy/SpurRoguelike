﻿using System;
using System.Collections.Generic;

namespace SpurRoguelike.WebPlayerBot.Infractructure {
    public static class RandomExtensions {
        public static T Select<T>(this Random random, IList<T> items) {
            return items[random.Next(items.Count)];
        }
    }
}