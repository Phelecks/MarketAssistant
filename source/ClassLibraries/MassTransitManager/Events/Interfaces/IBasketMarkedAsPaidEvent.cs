using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IBasketMarkedAsPaidEvent : CorrelatedBy<Guid>
{
    List<BasketItem> BasketItems { get; }

    public class BasketItem
    {
        public long ProductId { get; }
        public int Quantity { get; }

        public BasketItem(long productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}