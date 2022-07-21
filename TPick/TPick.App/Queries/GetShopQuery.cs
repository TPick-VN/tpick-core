using CsMicro.Cqrs.Queries;
using CsMicro.Persistence;
using TPick.Domain.Aggregates;

namespace TPick.App.Queries;

public class GetShopQuery : IQuery<Shop>
{
    public Guid Id { get; init; }
}

public class GetShopQueryHandler : IQueryHandler<GetShopQuery, Shop>
{
    private readonly IGenericRepository<Shop, Guid> _shopRepo;

    public GetShopQueryHandler(IGenericRepository<Shop, Guid> shopRepo)
    {
        _shopRepo = shopRepo;
    }

    public async Task<Shop> HandleAsync(GetShopQuery query, CancellationToken cancellationToken)
    {
        return await _shopRepo.FindOneAsync(query.Id, cancellationToken);
    }
}