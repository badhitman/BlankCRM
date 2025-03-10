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
    public async Task<List<ObjectKLADRModelDB>> ObjectsList(KladrsListRequestModel req)
    {
        using KladrContext context = await kladrDbFactory.CreateDbContextAsync();

        if (req.Request == 0)
        {
            return await context.ObjectsKLADR.Where(x => x.CODE.EndsWith("00000000000")).ToListAsync();
        }

        throw new NotImplementedException();
    }
}