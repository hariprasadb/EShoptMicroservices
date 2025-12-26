using Dicount.Grpc;
using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService (DiscountContext dbcontext,ILogger<DiscountService>  logger)
            :DiscountProtoService.DiscountProtoServiceBase
    {
        public  async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request object"));

            }
            dbcontext.Coupons.Add(coupon);
            await dbcontext.SaveChangesAsync();
            var model = coupon.Adapt<CouponModel>();
            logger.LogInformation("Create Discount completed for Product :{ProductName}", coupon.ProductName);
            return model;

        }
        public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons.Where(r => r.ProductName == request.ProductName)
                     .FirstOrDefaultAsync();

            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

            }
            dbcontext.Coupons.Remove(coupon);
            await dbcontext.SaveChangesAsync();
            var response = new DeleteDiscountResponse() { Success = true };
            logger.LogInformation("Delete Discount completed for Product :{ProductName}", coupon.ProductName);
            return response;
        }
        public async override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons.Where(r => r.ProductName == request.ProductName)
                          .FirstOrDefaultAsync();
            if (coupon == null)
            {
                coupon = new() { Id = 0, Amount = 0, Description = "No Discount" };
            }
            logger.LogInformation("Discount is retrived for Product : {prductName}, Amount: {amount}",
                coupon.ProductName, coupon.Amount);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;

        }
        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {

            Coupon coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request object"));

            }
            dbcontext.Coupons.Update(coupon);
            await dbcontext.SaveChangesAsync();
            var model = coupon.Adapt<CouponModel>();
            logger.LogInformation("Update Discount completed for Product :{ProductName}", coupon.ProductName);
            return model;

        }
    }
}
