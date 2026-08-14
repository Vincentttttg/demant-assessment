using System.ComponentModel.DataAnnotations;
using demant_assessment.Models;
using demant_assessment.Services;
using demant_assessment.Services.Rules;

namespace demant_assessment.Tests
{
    public class CalculatorServiceTests
    {
        private readonly CalculatorService _calculatorService;

        public CalculatorServiceTests()
        {
            _calculatorService = new([new VolumeDiscountRule(), new HighValueDiscountRule()]);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        public void CalculateFinalAmount_NoDiscount_SingleLine(int quantity)
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", 1450.1m, quantity);
            var order = new Order(orderLine1);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(1450.1m * quantity, result);
        }

        [Fact]
        public void CalculateFinalAmount_NoDiscount_MultipleLines()
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", 1450.1m, 2);
            var orderLine2 = new OrderLine("CHARGER_02", "Accessory", 180.99m, 1);
            var mockOrder = new Order(orderLine1, orderLine2);
            var result = _calculatorService.CalculateFinalAmount(mockOrder);

            Assert.Equal(3081.19m, result);
        }

        public static TheoryData<int, decimal> TestVolumeDiscountData => new()
        {
            { 9,  900m },
            { 10, 900m },     
            { 15, 1350m }
        };


        [Theory]
        [MemberData(nameof(TestVolumeDiscountData))]
        public void CalculateFinalAmount_ApplyVolumeDiscount_SingleLine(int quantity, decimal expected)
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", 100m, quantity);
            var order = new Order(orderLine1);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateFinalAmount_ApplyVolumeDiscountPerLineNotPerOrder_MixedQuantities()
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", 100m, 10);
            var orderLine2 = new OrderLine("CHARGER_02", "Accessory", 100m, 8);
            var order = new Order(orderLine1, orderLine2);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(1700m, result);
        }

        public static TheoryData<decimal, decimal> TestHighValueDiscountData => new()
        {
            { 9999.99m, 9999.99m },
            { 10000m, 9500m },
            { 15000m, 14250m }
        };


        [Theory]
        [MemberData(nameof(TestHighValueDiscountData))]
        public void CalculateFinalAmount_ApplyHighValueDiscount_SingleLine(decimal unitPrice, decimal expected)
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", unitPrice, 1);
            var order = new Order(orderLine1);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateFinalAmount_ApplyBothDiscount_SingleLine()
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", 1000m, 12);
            var order = new Order(orderLine1);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(10260m, result);
        }

        [Fact]
        public void CalculateFinalAmount_ApplyVolumeDiscount_DropsSubtotalBelowHighValueDiscountThreshold()
        {
            //Supposed to get High Value Discount before it gets cancelled due to subtotal after applying volume discount 
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", 900m, 12);
            var order = new Order(orderLine1);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(9720m, result);
        }

        [Fact]
        public void CalculateFinalAmount_NullOrder()
        {
            Order order = null;
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculateFinalAmount_EmptyOrder_ZeroLine()
        {
            var order = new Order();
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculateFinalAmount_ZeroQuantity_SingleLine()
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", 1000m, 0);
            var order = new Order(orderLine1);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateFinalAmount_IgnoreZeroQuantity_MultipleLines()
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", 100m, 0);
            var orderLine2 = new OrderLine("CHARGER_02", "Accessory", 100m, 8);
            var order = new Order(orderLine1, orderLine2);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(800m, result);
        }

        public static TheoryData<decimal, decimal> TestRoundedAmountData => new()
        {
            { 123.456m, 123.46m },     
            { 123.105m, 123.10m }
        };

        [Theory]
        [MemberData(nameof(TestRoundedAmountData))]
        public void CalculateFinalAmount_RoundsToTwoDecimalPlaces_SingleLine(decimal unitPrice, decimal expected)
        {
            var orderLine1 = new OrderLine("HA_STD_01", "HearingAid", unitPrice, 1);
            var order = new Order(orderLine1);
            var result = _calculatorService.CalculateFinalAmount(order);

            Assert.Equal(expected, result);
        }
    }
}