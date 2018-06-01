using System;
using System.Collections.Generic;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Extensions {
    internal static class LocationExtansions {
        public static IEnumerable<Quadrant> GetQuadrants(this Location centerLocation, Int32 offsetWidth, Int32 offsetHeight) {
            var quadrants = new List<Quadrant> {
            new Quadrant {
                LeftTopCorner = new Location { X = centerLocation.X - offsetWidth, Y = centerLocation.Y - offsetHeight },
                RightBottomCorner = centerLocation
            },
            new Quadrant {
                LeftTopCorner = new Location { X = centerLocation.X, Y = centerLocation.Y - offsetHeight },
                RightBottomCorner = new Location { X = centerLocation.X + offsetWidth, Y = centerLocation.Y }
            },
            new Quadrant {
                LeftTopCorner = new Location { X = centerLocation.X - offsetWidth, Y = centerLocation.Y },
                RightBottomCorner = new Location { X = centerLocation.X, Y = centerLocation.Y + offsetHeight } 
            },
            new Quadrant {
                LeftTopCorner = centerLocation,
                RightBottomCorner = new Location{ X = centerLocation.X + offsetWidth, Y = centerLocation.Y + offsetHeight } 
            },
        };
            return quadrants;
        }

        public static TurnType ToTurnType(this Offset offset, Boolean isStep) {
            var normalOffset = offset.Normalize();
            if(normalOffset.XOffset == -1 && normalOffset.YOffset == 0)
                return isStep ? TurnType.StepToTheLeft : TurnType.AttackToTheLeft;
            if(normalOffset.XOffset == 1 && normalOffset.YOffset == 0)
                return isStep ? TurnType.StepToTheRight : TurnType.AttackToTheRight;
            if(normalOffset.XOffset == 0 && normalOffset.YOffset == -1)
                return isStep ? TurnType.StepToTheTop : TurnType.AttackToTheTop;
            if(normalOffset.XOffset == 0 && normalOffset.YOffset == 1)
                return isStep ? TurnType.StepToTheBottom : TurnType.AttackToTheBottom;
            if(normalOffset.XOffset == -1 && normalOffset.YOffset == -1)
                return TurnType.AttackToTheLeftTopCorner;
            if(normalOffset.XOffset == -1 && normalOffset.YOffset == 1)
                return TurnType.AttackToTheLeftBottomCorner;
            if(normalOffset.XOffset == 1 && normalOffset.YOffset == -1)
                return TurnType.AttackToTheRightTopCorner;
            if(normalOffset.XOffset == 1 && normalOffset.YOffset == 1)
                return TurnType.AttackToTheRightBottomCorner;
            return TurnType.None;
        }
    }
}