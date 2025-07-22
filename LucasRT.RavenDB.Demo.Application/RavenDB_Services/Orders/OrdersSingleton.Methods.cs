using LeadSoft.Common.Library.EnvUtils;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Domain;
using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace LucasRT.RavenDB.Demo.Application.RavenDB_Services.Orders
{
    public partial class OrdersSingleton
    {
        private bool _IsOpen = false;

        private ConcurrentDictionary<Guid, Guest> _Guests { get; set; } = [];
        private ConcurrentDictionary<Guid, Beverage> _Beverages { get; set; } = [];
        private IAsyncDocumentSession _RavenDBSession { get; set; } = null;

        private readonly IDocumentStore _RavenDB = new DocumentStore
        {
            Urls = configuration.GetSection("RavenSettings:Urls").Get<string[]>(),
            Database = configuration["RavenSettings:DatabaseName"],
            Certificate = GetRavenDBCertificate(configuration)
        }.Initialize();

        private static X509Certificate2 GetRavenDBCertificate(IConfiguration configuration)
        {
            X509Certificate2? cert = null;

            if (configuration["RavenSettings:ResourceName"].IsSomething())
                cert = Assembly.GetExecutingAssembly()
                               .GetEmbeddedX509Certificate($"{configuration["RavenSettings:ResourceName"]}", EnvUtil.Get(EnvConstant.RavenDBPwd));


            return cert ?? throw new OperationCanceledException("Certificate not found!");
        }

        private async Task FillListsAsync()
        {
            int pageSize = 15000,
            currentPage = 0;

            _RavenDBSession = _RavenDB.OpenAsyncSession();
            _RavenDBSession.Advanced.MaxNumberOfRequestsPerSession = 500000;

            #region [ Guests ]
            IRavenQueryable<Guest> guestQry = _RavenDBSession.Query<Guest>();
            IEnumerable<Guest> guests = await guestQry.Statistics(out QueryStatistics qryPStats)
                                                      .Skip(currentPage * pageSize)
                                                      .Take(pageSize)
                                                      .ToListAsync();

            while (++currentPage <= qryPStats.TotalResults / pageSize)
                guests = guests.Concat(await guestQry.Skip(currentPage * pageSize)
                                                     .Take(pageSize)
                                                     .ToListAsync());

            _Guests = new(guests.ToDictionary(g => g.Guid.Value));
            #endregion

            #region [ Beverages ]
            currentPage = 0;

            IRavenQueryable<Beverage> beverageQry = _RavenDBSession.Query<Beverage>();
            IEnumerable<Beverage> beverages = await beverageQry.Statistics(out qryPStats)
                                                               .Skip(currentPage * pageSize)
                                                               .Take(pageSize)
                                                               .ToListAsync();

            while (++currentPage <= qryPStats.TotalResults / pageSize)
                beverages = beverages.Concat(await beverageQry.Skip(currentPage * pageSize)
                                                              .Take(pageSize)
                                                              .ToListAsync());

            _Beverages = new(beverages.ToDictionary(g => g.Guid.Value));
            #endregion
        }
    }
}
