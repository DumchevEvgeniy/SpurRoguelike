using System;

namespace SpurRoguelike.WebPlayerBot.Infractructure {
    public class Location : IEquatable<Location> {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }

        public Boolean IsInRange(Location other, Int32 range) {
            var offset = this - other;
            return Math.Abs(offset.XOffset) <= range && Math.Abs(offset.YOffset) <= range;
        }

        public static Location operator +(Location location, Offset offset) => new Location { X = location.X + offset.XOffset, Y = location.Y + offset.YOffset };

        public static Location operator -(Location location, Offset offset) => new Location { X = location.X - offset.XOffset, Y = location.Y - offset.YOffset };

        public static Offset operator -(Location location1, Location location2) => new Offset(location1.X - location2.X, location1.Y - location2.Y);

        public Boolean Equals(Location other) {
            if (other == null)
                return false;
            return X == other.X && Y == other.Y;
        }

        public override Boolean Equals(Object other) {
            if(other is null)
                return false;
            return Equals(other as Location);
        }

        public override Int32 GetHashCode() {
            unchecked {
                return (X * 397) ^ Y;
            }
        }

        public static Boolean operator ==(Location left, Location right) => left.Equals(right);

        public static Boolean operator !=(Location left, Location right) => !left.Equals(right);

        public override String ToString() => $"X: {X}, Y: {Y}";
    }
}