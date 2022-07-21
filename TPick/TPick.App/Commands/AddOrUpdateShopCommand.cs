using System.Text.Json.Serialization;
using CsMicro.Cqrs.Commands;
using CsMicro.Persistence;
using CsMicro.Utilities;
using Microsoft.EntityFrameworkCore;
using TPick.App.Services;
using TPick.Domain.Aggregates;

namespace TPick.App.Commands;

public class AddOrUpdateShopCommand : ICommand
{
    public string ShopUrl { get; init; }
    [JsonIgnore] public Action<Guid>? OnShopId { get; set; }
}

public class AddShopCommandHandler : ICommandHandler<AddOrUpdateShopCommand>
{
    private readonly IGenericRepository<Shop, Guid> _shopRepo;
    private readonly IShopDetailsCrawler _shopDetailsCrawler;
    private readonly IDateTimeProvider _dateTimeProvider;
    private static readonly TimeSpan UpdateThreshold = TimeSpan.FromMinutes(5);

    public AddShopCommandHandler(IGenericRepository<Shop, Guid> shopRepo, IShopDetailsCrawler shopDetailsCrawler,
        IDateTimeProvider dateTimeProvider)
    {
        _shopRepo = shopRepo;
        _shopDetailsCrawler = shopDetailsCrawler;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CommandResult> HandleAsync(AddOrUpdateShopCommand command, CancellationToken cancellationToken)
    {
        var shopUrl = command.ShopUrl.ToLower();
        var shop = await _shopRepo.FindOneAsync(x => x.Url == shopUrl, cancellationToken);

        if (shop is not null && _dateTimeProvider.UtcOffsetNow - shop.UpdatedTime <= UpdateThreshold)
        {
            await _shopRepo.SaveAsync(shop, cancellationToken);

            return ReturnSuccess(command, shop);
        }

        var shopDetails = await _shopDetailsCrawler.GetDetails(shopUrl, cancellationToken);
        if (shopDetails is null)
        {
            return CommandResult.Error("Shop url is not valid!");
        }

        if (shop is null)
        {
            shop = new Shop(command.ShopUrl);
            shop.OverrideDetails(shopDetails.Sections, shopDetails.Name, shopDetails.ImageUrl, shopDetails.Address,
                _dateTimeProvider.UtcOffsetNow);
            await _shopRepo.AddAsync(shop, cancellationToken);

            return ReturnSuccess(command, shop);
        }

        shop.OverrideDetails(shopDetails.Sections, shopDetails.Name, shopDetails.ImageUrl, shopDetails.Address,
            _dateTimeProvider.UtcOffsetNow);
        await _shopRepo.SaveAsync(shop, cancellationToken);

        return ReturnSuccess(command, shop);
    }

    private CommandResult ReturnSuccess(AddOrUpdateShopCommand command, Shop shop)
    {
        command.OnShopId?.Invoke(shop.Id);

        return CommandResult.Success();
    }
}