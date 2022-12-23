namespace hotel_Web_API.DTOs
{
    public class getFinalPriceModel
    {
        //hasDiscount = !user.isFirstBooking,
        //        priceBeforeDiscount = bookingTotal,
        //        priceAfterDiscount = priceAfterDiscount,
        //        discountValue = discountValue;

        public bool? hasDiscount { get; set; }
        public double priceBeforeDiscount { get; set; }
        public double priceAfterDiscount { get; set; }
        public double discountValue { get; set; }


    }
}
