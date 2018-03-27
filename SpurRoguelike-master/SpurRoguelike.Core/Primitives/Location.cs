using System;

namespace SpurRoguelike.Core.Primitives {
    public struct Location : IEquatable<Location> {
        public Location(Int32 x, Int32 y) {
            X = x;
            Y = y;
        }

        public readonly Int32 X;

        public readonly Int32 Y;

        public Boolean IsInRange(Location other, Int32 range) {
            var offset = this - other;
            return Math.Abs(offset.XOffset) <= range && Math.Abs(offset.YOffset) <= range;
        }

        public static Location operator +(Location location, Offset offset) {
            return new Location(location.X + offset.XOffset, location.Y + offset.YOffset);
        }

        public static Location operator -(Location location, Offset offset) {
            return new Location(location.X - offset.XOffset, location.Y - offset.YOffset);
        }

        public static Offset operator -(Location location1, Location location2) {
            return new Offset(location1.X - location2.X, location1.Y - location2.Y);
        }

        public Boolean Equals(Location other) {
            return X == other.X && Y == other.Y;
        }

        public override Boolean Equals(Object other) {
            if(other is null)
                return false;
            return other is Location && Equals((Location)other);
        }

        public override Int32 GetHashCode() {
            unchecked {
                return (X * 397) ^ Y;
            }
        }

        public static Boolean operator ==(Location left, Location right) {
            return left.Equals(right);
        }

        public static Boolean operator !=(Location left, Location right) {
            return !left.Equals(right);
        }

        public override String ToString() {
            return $"X: {X}, Y: {Y}";
        }
    }
}