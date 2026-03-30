using System.Threading.Tasks;
using JasperFx.Events;
using Marten;
using Marten.Events.Projections;
using SmartFridgeApp.Core.Domain.Inventory.Events;

namespace SmartFridgeApp.Infrastructure.Inventory;

public class ActiveStockItemProjection : EventProjection
{
    public ActiveStockItemDocument Create(ItemStocked e, IEvent metadata)
    {
        return new ActiveStockItemDocument
        {
            Id = e.ItemId,
            KitchenId = metadata.StreamId,
            FoodProductId = e.FoodProductId,
            MemberId = e.MemberId,
            Amount = e.Amount,
            Unit = e.Unit,
            ExpirationDate = e.ExpirationDate,
            Note = e.Note,
            Location = e.Location,
            Tags = e.Tags ?? [],
            StockedAt = e.StockedAt,
            VariantId = e.VariantId
        };
    }

    public async Task Project(ItemConsumed e, IDocumentOperations ops)
    {
        if (e.IsFullyConsumed)
        {
            ops.Delete<ActiveStockItemDocument>(e.ItemId);
        }
        else
        {
            var doc = await ops.LoadAsync<ActiveStockItemDocument>(e.ItemId);
            if (doc is not null)
            {
                doc.Amount -= e.AmountConsumed;
                ops.Store(doc);
            }
        }
    }

    public void Project(ItemWasted e, IDocumentOperations ops)
    {
        ops.Delete<ActiveStockItemDocument>(e.ItemId);
    }

    public void Project(ItemRemoved e, IDocumentOperations ops)
    {
        ops.Delete<ActiveStockItemDocument>(e.ItemId);
    }

    public async Task Project(ItemRestocked e, IDocumentOperations ops)
    {
        var doc = await ops.LoadAsync<ActiveStockItemDocument>(e.ItemId);
        if (doc is not null)
        {
            doc.Amount += e.AddedAmount;
            if (e.NewExpirationDate > doc.ExpirationDate)
                doc.ExpirationDate = e.NewExpirationDate;
            ops.Store(doc);
        }
    }
}
