using JasperFx.Events;
using Marten;
using Marten.Events.Projections;
using SmartFridgeApp.Core.Domain.ShoppingList.Events;

namespace SmartFridgeApp.Infrastructure.ShoppingList;

public class ShoppingListProjection : EventProjection
{
    public ShoppingListItemDocument Create(ItemAddedToShoppingList e, IEvent metadata)
    {
        return new ShoppingListItemDocument
        {
            Id = e.ItemId,
            KitchenId = metadata.StreamId,
            Name = e.Name,
            AddedByEmail = e.AddedByEmail,
            AddedAt = e.AddedAt
        };
    }

    public void Project(ItemBought e, IDocumentOperations ops)
    {
        ops.Delete<ShoppingListItemDocument>(e.ItemId);
    }

    public void Project(ItemRemovedFromShoppingList e, IDocumentOperations ops)
    {
        ops.Delete<ShoppingListItemDocument>(e.ItemId);
    }
}
