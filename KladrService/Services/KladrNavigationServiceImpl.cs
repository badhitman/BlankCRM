////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace KladrService;

/// <inheritdoc/>
public class KladrNavigationServiceImpl(IDbContextFactory<KladrContext> kladrDbFactory) : IKladrNavigationService
{
    /// <inheritdoc/>
    public async Task<List<UniversalBaseModel>> ObjectsList(KladrsListRequestModel req)
    {
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();
        throw new NotImplementedException();
    }
}