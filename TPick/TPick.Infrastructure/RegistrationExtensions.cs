using CsMicro;
using CsMicro.Core;
using CsMicro.Cqrs;
using CsMicro.Persistence;
using CsMicro.Persistence.EfCore;

namespace TPick.Infrastructure;

public static class RegistrationExtensions
{
    public static ICsMicroBuilder AddInfrastructure(this ICsMicroBuilder builder)
    {
        return builder
            .AddCore()
            .AddCqrs("TPick.App")
            .AddPersistence(o => { o.UseEfCore(); });
    }
}