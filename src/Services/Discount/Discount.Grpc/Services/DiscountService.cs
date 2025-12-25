using Dicount.Grpc;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService :DiscountProtoService.DiscountProtoServiceBase
    {
        public override Task<CoupounModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            return base.CreateDiscount(request, context);
        }
        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            return base.DeleteDiscount(request, context);
        }
        public override Task<CoupounModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            return base.GetDiscount(request, context);

        }
        public override Task<CoupounModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            return base.UpdateDiscount(request, context);
        }
    }
}
