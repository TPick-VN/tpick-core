using System.Reflection;
using System.Text;
using CsMicro.Cqrs.Queries;
using CsMicro.Persistence;
using Microsoft.Extensions.Configuration;
using TPick.Domain.Aggregates;

namespace TPick.App.Queries;

public class GetOrderMetaPageQuery : IQuery<string?>
{
    public Guid OrderId { get; init; }
}

public class GetOrderMetaPageQueryHandler : IQueryHandler<GetOrderMetaPageQuery, string?>
{
    private readonly IGenericRepository<Order, Guid> _orderRepo;
    private readonly IGenericRepository<Shop, Guid> _shopRepo;
    private readonly string _tPickGui;
    private static string? _template;

    public GetOrderMetaPageQueryHandler(IGenericRepository<Order, Guid> orderRepo,
        IGenericRepository<Shop, Guid> shopRepo, IConfiguration configuration)
    {
        _orderRepo = orderRepo;
        _shopRepo = shopRepo;
        _template ??= LoadTemplate().GetAwaiter().GetResult();
        _tPickGui = configuration["ExternalServices:TPickGui"];
    }

    public async Task<string?> HandleAsync(GetOrderMetaPageQuery query, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.FindOneAsync(query.OrderId, cancellationToken);
        if (order is null) return null;

        var shop = await _shopRepo.FindOneAsync(order.ShopId, cancellationToken);
        if (shop is null) return null;

        var result = new string(_template);
        result = result.Replace("$title", $"{shop.Name} | TPick | {order.Host.Name}");
        result = result.Replace("$image", $"{shop.ImageUrl}");
        result = result.Replace("$href", $"{_tPickGui}#/orders/{order.Id}/cart");

        return result;
    }

    private static async Task<string> LoadTemplate()
    {
        var assembly = Assembly.GetExecutingAssembly();
        await using var resourceStream =
            assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Assets.MetaPage.html")
            ?? throw new FileNotFoundException("Template not found");
        using var reader = new StreamReader(resourceStream!, Encoding.UTF8);
        var template = await reader.ReadToEndAsync();
        return template;
    }
}