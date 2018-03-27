using System;

namespace SpurRoguelike.Core.Primitives {
    public struct Offset : IEquatable<Offset> {
        public Offset(Int32 xOffset, Int32 yOffset) {
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public readonly Int32 XOffset;

        public readonly Int32 YOffset;

        public Offset Normalize() {
            return new Offset(Math.Sign(XOffset), Math.Sign(YOffset));
        }

        public Offset SnapToStep(Random random) {
            var attackOffset = Normalize();

            if(XOffset == 0 || YOffset == 0)
                return attackOffset;

            return random.NextDouble() < 0.5 ? new Offset(attackOffset.XOffset, 0) : new Offset(0, attackOffset.YOffset);
        }

        public Offset SnapToStep() {
            var offset = Normalize();

            if(XOffset == 0 || YOffset == 0)
                return offset;

            return Math.Abs(XOffset) >= Math.Abs(YOffset) ? new Offset(offset.XOffset, 0) : new Offset(0, offset.YOffset);
        }

        public Boolean IsStep() {
            return Math.Abs(XOffset) == 1 && YOffset == 0 || Math.Abs(YOffset) == 1 && XOffset == 0;
        }

        public Offset Turn(Int32 quarters) {
            if(!IsStep())
                return this;

            var clockwise = Math.Sign(quarters);
            quarters = Math.Abs(quarters) % 4;

            var xOffset = XOffset;
            var yOffset = YOffset;

            for(Int32 i = 0; i < quarters; i++) {
                if(xOffset == 0) {
                    xOffset = -clockwise * yOffset;
                    yOffset = 0;
                }
                else {
                    yOffset = clockwise * xOffset;
                    xOffset = 0;
                }
            }

            return new Offset(xOffset, yOffset);
        }

        public Int32 Size() {
            return Math.Abs(XOffset) + Math.Abs(YOffset);
        }

        public Offset Abs() {
            return new Offset(Math.Abs(XOffset), Math.Abs(YOffset));
        }

        public static Offset FromDirection(StepDirection stepDirection) {
            return StepOffsets[(Int32)stepDirection];
        }

        public static Offset FromDirection(AttackDirection attackDirection) {
            return AttackOffsets[(Int32)attackDirection];
        }

        public static readonly Offset[] StepOffsets = { new Offset(0, -1), new Offset(1, 0), new Offset(0, 1), new Offset(-1, 0) };
        public static readonly Offset[] AttackOffsets = { new Offset(0, -1), new Offset(1, -1), new Offset(1, 0), new Offset(1, 1), new Offset(0, 1), new Offset(-1, 1), new Offset(-1, 0), new Offset(-1, -1) };

        public static Offset operator -(Offset offset) {
            return new Offset(-offset.XOffset, -offset.YOffset);
        }

        public Boolean Equals(Offset other) {
            return XOffset == other.XOffset && YOffset == other.YOffset;
        }

        public override Boolean Equals(Object other) {
            if(other is null)
                return false;
            return other is Offset && Equals((Offset)other);
        }

        public override Int32 GetHashCode() {
            unchecked {
                return (XOffset * 397) ^ YOffset;
            }
        }

        public static Boolean operator ==(Offset left, Offset right) {
            return left.Equals(right);
        }

        public static Boolean operator !=(Offset left, Offset right) {
            return !left.Equals(right);
        }

        public override String ToString() {
            return $"X: {XOffset}, Y: {YOffset}";
        }
    }
}