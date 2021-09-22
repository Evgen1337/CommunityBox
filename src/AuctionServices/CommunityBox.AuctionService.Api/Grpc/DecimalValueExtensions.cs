namespace CommunityBox.AuctionService.Api.Grpc
{
    public static class ProtoValueExtensions
    {
        public static decimal ToDecimal(this Proto.DecimalValue decimalValue) =>
            decimalValue.Units + decimalValue.Nanos / DecimalValue.NanoFactor;

        public static Proto.DecimalValue ToDecimalValue(this decimal value)
        {
            var units = decimal.ToInt64(value);
            var nanos = decimal.ToInt32((value - units) * DecimalValue.NanoFactor);

            return new Proto.DecimalValue
            {
                Nanos = nanos,
                Units = units
            };
        }
    }
}