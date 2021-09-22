using CommunityBox.Common.GrpcBlocks.GrpcTypes;
using AuctionProto = CommunityBox.AuctionService.Api.Proto;

namespace CommunityBox.Api.WebGateway.Protos
{
    public static class ProtoValueExtensions
    {
        public static decimal ToDecimal(this AuctionProto.DecimalValue decimalValue) =>
            decimalValue.Units + decimalValue.Nanos / DecimalValue.NanoFactor;

        public static AuctionProto.DecimalValue ToDecimalValue(this decimal value)
        {
            var units = decimal.ToInt64(value);
            var nanos = decimal.ToInt32((value - units) * DecimalValue.NanoFactor);

            return new AuctionProto.DecimalValue
            {
                Nanos = nanos,
                Units = units
            };
        }
    }
}