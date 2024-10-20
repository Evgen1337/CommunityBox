﻿namespace CommunityBox.Common.GrpcBlocks.GrpcTypes
{
    public partial class DecimalValue
    {
        public const decimal NanoFactor = 1_000_000_000;

        public DecimalValue(long units, int nanos)
        {
            Units = units;
            Nanos = nanos;
        }

        public long Units { get; }
        public int Nanos { get; }

        public static implicit operator decimal(DecimalValue grpcDecimal) => ToDecimal(grpcDecimal);

        public static implicit operator DecimalValue(decimal value) => FromDecimal(value);

        public static decimal ToDecimal(DecimalValue decimalValue)
        {
            return decimalValue.Units + decimalValue.Nanos / NanoFactor;
        }

        public static DecimalValue FromDecimal(decimal value)
        {
            var units = decimal.ToInt64(value);
            var nanos = decimal.ToInt32((value - units) * NanoFactor);
            return new DecimalValue(units, nanos);
        }
    }
}