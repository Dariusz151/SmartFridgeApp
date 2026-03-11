using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Services;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class FridgeItemService(
    IFridgeItemRepository fridgeItemRepository,
    IFridgeRepository fridgeRepository,
    IFridgeScoringPolicy scoringPolicy,
    IUnitOfWork unitOfWork,
    ISqlConnectionFactory sqlConnectionFactory) : IFridgeItemService
{
    // ── Reads (Dapper — lightweight projections) ──────────────────────

    public async Task<IEnumerable<FridgeItemDto>> GetFridgeItemsByMemberAsync(int memberId, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT fi."Id" AS "FridgeItemId",
                   fp."Name" AS "ProductName",
                   fi."FoodProductId",
                   c."Name" AS "CategoryName",
                   c."CategoryId",
                   fi."Value",
                   fi."Unit",
                   fi."ExpirationDate",
                   au."Name" AS "UserName",
                   au."Email" AS "UserEmail",
                   fm."Color" AS "UserColor"
            FROM app."FridgeItems" fi
            JOIN app."FridgeMembers" fm ON fi."MemberId" = fm."Id"
            JOIN app."AppUsers" au ON fm."Email" = au."Email"
            JOIN app."FoodProducts" fp ON fi."FoodProductId" = fp."FoodProductId"
            LEFT JOIN app."Categories" c ON fp."CategoryId" = c."CategoryId"
            WHERE fi."MemberId" = @MemberId
              AND fi."IsConsumed" = false
              AND fi."IsWasted" = false
            """;
        return await connection.QueryAsync<FridgeItemDto>(sql, new { MemberId = memberId });
    }

    public async Task<IEnumerable<FridgeItemDto>> GetFridgeItemsByFridgeAsync(Guid fridgeId, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT fi."Id" AS "FridgeItemId",
                   fp."Name" AS "ProductName",
                   fi."FoodProductId",
                   c."Name" AS "CategoryName",
                   c."CategoryId",
                   fi."Value",
                   fi."Unit",
                   fi."ExpirationDate",
                   au."Name" AS "UserName",
                   au."Email" AS "UserEmail",
                   fm."Color" AS "UserColor"
            FROM app."FridgeItems" fi
            JOIN app."FridgeMembers" fm ON fi."MemberId" = fm."Id"
            JOIN app."AppUsers" au ON fm."Email" = au."Email"
            JOIN app."FoodProducts" fp ON fi."FoodProductId" = fp."FoodProductId"
            LEFT JOIN app."Categories" c ON fp."CategoryId" = c."CategoryId"
            WHERE fm."FridgeId" = @FridgeId
              AND fi."IsConsumed" = false
              AND fi."IsWasted" = false
            """;
        return await connection.QueryAsync<FridgeItemDto>(sql, new { FridgeId = fridgeId });
    }

    public async Task<MonthlyWasteReportDto> GetMonthlyWasteReportAsync(Guid fridgeId, int year, int month, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT fi."Id" AS "FridgeItemId",
                   fp."Name" AS "ProductName",
                   c."Name" AS "CategoryName",
                   fi."Value" AS "Amount",
                   fi."Unit",
                   fi."WastedAt",
                   fi."WasteReason"
            FROM app."FridgeItems" fi
            JOIN app."FridgeMembers" fm ON fi."MemberId" = fm."Id"
            JOIN app."FoodProducts" fp ON fi."FoodProductId" = fp."FoodProductId"
            LEFT JOIN app."Categories" c ON fp."CategoryId" = c."CategoryId"
            WHERE fm."FridgeId" = @FridgeId
              AND fi."IsWasted" = true
              AND EXTRACT(YEAR FROM fi."WastedAt") = @Year
              AND EXTRACT(MONTH FROM fi."WastedAt") = @Month
            ORDER BY fi."WastedAt" DESC
            """;

        var items = (await connection.QueryAsync<WasteReportItemDto>(sql, new { FridgeId = fridgeId, Year = year, Month = month })).ToList();

        return new MonthlyWasteReportDto
        {
            Year = year,
            Month = month,
            TotalItemsWasted = items.Count,
            Items = items
        };
    }

    // ── Writes (Domain model + EF + UnitOfWork) ──────────────────────

    public async Task AddFridgeItemAsync(Guid fridgeId, AddFridgeItemDto dto, int memberId, CancellationToken ct = default)
    {
        var amountValue = new AmountValue(dto.Value, dto.Unit);
        var fridgeItem = new FridgeItem((short)dto.FoodProductId, dto.Note, amountValue, memberId);
        fridgeItem.SetExpirationDate(dto.ExpirationDate);

        await fridgeItemRepository.AddAsync(fridgeItem);

        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        fridge.RecordItemAdded();

        await unitOfWork.CommitAsync(ct);
    }

    public async Task RemoveFridgeItemAsync(long fridgeItemId, int memberId, Guid fridgeId, CancellationToken ct = default)
    {
        await fridgeItemRepository.DeleteAsync(fridgeItemId, memberId);

        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        fridge.RecordItemRemoved();

        await unitOfWork.CommitAsync(ct);
    }

    public async Task ConsumeFridgeItemAsync(long fridgeItemId, int memberId, Guid fridgeId, AmountValue amountValue, CancellationToken ct = default)
    {
        var item = await fridgeItemRepository.GetByIdAndMemberAsync(fridgeItemId, memberId);
        item.ConsumeFridgeItem(amountValue);

        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        fridge.RecordItemConsumed(scoringPolicy);
        if (item.IsConsumed)
            fridge.RecordItemRemoved();

        await unitOfWork.CommitAsync(ct);
    }

    public async Task ConsumeRecipeAsync(int memberId, Guid fridgeId, List<FoodProductDetails> foodProducts, CancellationToken ct = default)
    {
        int consumedCount = 0;
        foreach (var product in foodProducts)
        {
            var item = await fridgeItemRepository.GetActiveByMemberAndProductAsync(memberId, product.FoodProductId);
            if (item is null) continue;

            item.ConsumeFridgeItem(product.AmountValue);
            consumedCount++;
        }

        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        for (var i = 0; i < consumedCount; i++)
        {
            fridge.RecordItemConsumed(scoringPolicy);
            fridge.RecordItemRemoved();
        }

        // Single commit — all changes are saved atomically via EF ChangeTracker
        await unitOfWork.CommitAsync(ct);
    }

    public async Task WasteFridgeItemAsync(long fridgeItemId, int memberId, Guid fridgeId, string reason = null, CancellationToken ct = default)
    {
        var item = await fridgeItemRepository.GetByIdAndMemberAsync(fridgeItemId, memberId);
        item.WasteFridgeItem(reason);

        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        fridge.RecordItemWasted(scoringPolicy);
        fridge.RecordItemRemoved();

        await unitOfWork.CommitAsync(ct);
    }

    // ── Expiring Items ──────────────────────

    public async Task<IEnumerable<ExpiringItemDto>> GetExpiringItemsAsync(Guid fridgeId, int daysThreshold = 3, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT fi."Id" AS "FridgeItemId",
                   fp."Name" AS "ProductName",
                   c."Name" AS "CategoryName",
                   fi."Value",
                   fi."Unit",
                   fi."ExpirationDate",
                   EXTRACT(DAY FROM fi."ExpirationDate" - NOW())::int AS "DaysUntilExpiry",
                   au."Name" AS "UserName",
                   au."Email" AS "UserEmail"
            FROM app."FridgeItems" fi
            JOIN app."FridgeMembers" fm ON fi."MemberId" = fm."Id"
            JOIN app."AppUsers" au ON fm."Email" = au."Email"
            JOIN app."FoodProducts" fp ON fi."FoodProductId" = fp."FoodProductId"
            LEFT JOIN app."Categories" c ON fp."CategoryId" = c."CategoryId"
            WHERE fm."FridgeId" = @FridgeId
              AND fi."IsConsumed" = false
              AND fi."IsWasted" = false
              AND fi."ExpirationDate" <= NOW() + INTERVAL '1 day' * @DaysThreshold
            ORDER BY fi."ExpirationDate" ASC
            """;
        return await connection.QueryAsync<ExpiringItemDto>(sql, new { FridgeId = fridgeId, DaysThreshold = daysThreshold });
    }

    // ── Gamification: Fridge Score (aggregate, stored on Fridge entity) ──────────────────────

    public async Task<FridgeScoreDto> GetFridgeScoreAsync(Guid fridgeId, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);

        var rank = fridge.WasteScore switch
        {
            >= 2000 => "Eco Champion",
            >= 1500 => "Food Saver",
            >= 1000 => "Responsible",
            >= 500 => "Needs Improvement",
            _ => "Food Waster"
        };

        return new FridgeScoreDto { FridgeId = fridge.Id, WasteScore = fridge.WasteScore, Rank = rank };
    }

    public async Task<ShoppingStatusDto> GetShoppingStatusAsync(Guid fridgeId, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);

        return new ShoppingStatusDto
        {
            FridgeId = fridge.Id,
            ActiveItemCount = fridge.ActiveItemCount,
            AverageItemCount = Math.Round(fridge.AverageItemCount, 1),
            IsShoppingNeeded = fridge.IsShoppingNeeded()
        };
    }
}
