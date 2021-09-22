using System;
using Google.Protobuf.WellKnownTypes;

namespace CommunityBox.Common.GrpcBlocks.GrpcTypes
{
    public static class TypeConverters
    {
        public static Timestamp ToTimestampWithKindUtc(this DateTime? input) =>
            input?.ToTimestampWithKindUtc();

        public static Timestamp ToTimestampWithKindUtc(this DateTime input) =>
            DateTime.SpecifyKind(input, DateTimeKind.Utc).ToTimestamp();
    }
}