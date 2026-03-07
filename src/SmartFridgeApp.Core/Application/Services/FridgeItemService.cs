using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class FridgeItemService(
    IFridgeRepository fridgeRepository,
    IFoodProductRepository foodProductRepository,
    IUnitOfWork unitOfWork,
    ISqlConnectionFactory sqlConnectionFactory) : IFridgeItemService
{
    public async Task<IEnumerable<FridgeItemDto>> GetFridgeItemsByUserAsync(Guid userId, Guid fridgeId, CancellationToken ct = default)
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
                   u."Name" AS "UserName",
                   u."Email" AS "UserEmail",
                   COALESCE(fm."Color", '#000000') AS "UserColor"
            FROM app."FridgeItems" fi
            JOIN app."Users" u ON fi."UserId" = u."Id"
            JOIN app."FoodProducts" fp ON fi."FoodProductId" = fp."FoodProductId"
            LEFT JOIN app."Categories" c ON fp."CategoryId" = c."CategoryId"
            LEFT JOIN app."FridgeMembers" fm ON fm."Email" = u."Email" AND fm."FridgeId" = u."FridgeId"
            WHERE fi."UserId" = @UserId
            """;

        var fridgeItems = await connection.QueryAsync<FridgeItemDto>(sql, new { UserId = userId });
        return fridgeItems.AsEnumerable();
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
                   u."Name" AS "UserName",
                   u."Email" AS "UserEmail",
                   COALESCE(fm."Color", '#000000') AS "UserColor"
            FROM app."FridgeItems" fi
            JOIN app."Users" u ON fi."UserId" = u."Id"
            JOIN app."FoodProducts" fp ON fi."FoodProductId" = fp."FoodProductId"
            LEFT JOIN app."Categories" c ON fp."CategoryId" = c."CategoryId"
            LEFT JOIN app."FridgeMembers" fm ON fm."Email" = u."Email" AND fm."FridgeId" = u."FridgeId"
            WHERE u."FridgeId" = @FridgeId
            """;

        var fridgeItems = await connection.QueryAsync<FridgeItemDto>(sql, new { FridgeId = fridgeId });
        return fridgeItems.AsEnumerable();
    }

    public async Task AddFridgeItemAsync(Guid fridgeId, AddFridgeItemDto fridgeItemDto, Guid userId, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        var user = fridge.GetFridgeUser(userId);

        var foodProduct = await foodProductRepository.GetByIdAsync(fridgeItemDto.FoodProductId);

        var fridgeItem = new FridgeItem(
            foodProduct.FoodProductId,
            fridgeItemDto.Note,
            new AmountValue(fridgeItemDto.Value, fridgeItemDto.Unit));

        user.AddFridgeItem(fridgeItem);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task RemoveFridgeItemAsync(long fridgeItemId, Guid userId, Guid fridgeId, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        var user = fridge.GetFridgeUser(userId);
        user.RemoveFridgeItem(fridgeItemId);

        await unitOfWork.CommitAsync(ct);
    }

    public async Task ConsumeFridgeItemAsync(long fridgeItemId, Guid userId, Guid fridgeId, AmountValue amountValue, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        var user = fridge.GetFridgeUser(userId);
        user.ConsumeFridgeItem(fridgeItemId, amountValue);

        await unitOfWork.CommitAsync(ct);
    }

    public async Task ConsumeRecipeAsync(Guid userId, Guid fridgeId, List<FoodProductDetails> foodProducts, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        var user = fridge.GetFridgeUser(userId);
        user.ConsumeRecipe(foodProducts);

        await unitOfWork.CommitAsync(ct);
    }

    public async Task WasteFridgeItemAsync(long fridgeItemId, Guid userId, Guid fridgeId, string reason = null, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        var user = fridge.GetFridgeUser(userId);
        user.WasteFridgeItem(fridgeItemId, reason);

        await unitOfWork.CommitAsync(ct);
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
            JOIN app."Users" u ON fi."UserId" = u."Id"
            JOIN app."FoodProducts" fp ON fi."FoodProductId" = fp."FoodProductId"
            LEFT JOIN app."Categories" c ON fp."CategoryId" = c."CategoryId"
            WHERE u."FridgeId" = @FridgeId
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
}
