using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using System.Collections.Concurrent;
using System.Reflection;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Menus
{
    public partial class Beverage
    {
        public const string PurchasesCounterName = "Purchases";
        public const string SoldCounterName = "Sold";
        public const string DislikeCounterName = "Dislikes";
        public const string LikeCounterName = "Likes";

        public Beverage()
        {
            CountersNames.Add(PurchasesCounterName);
            CountersNames.Add(SoldCounterName);
            CountersNames.Add(LikeCounterName);
            CountersNames.Add(DislikeCounterName);
        }

        public Beverage CreateVectorField()
        {
            VectorSearchField = this.ToJson().Flatten(excludedFields: ["VectorSearchField", "Id", "Guid"]);
            return this;
        }

        public static ConcurrentDictionary<Guid, Beverage> GetSamples(out DTOOperation oDtoFileOperation)
        {
            oDtoFileOperation = new("Beverage files reading...");

            Stream file;

            try
            {
                file = Assembly.GetExecutingAssembly().GetEmbeddedResourceStream($"Beverages.json");
                if (file.IsNull())
                    Console.WriteLine($"File not found.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"File not found. Error: {e.Message}.");
                throw;
            }

            string json = file.GetStreamStrContent(true);

            file.Dispose();

            if (json.IsNothing())
            {
                Console.WriteLine(@$"Arquivo vazio.");
                json = "[]";
            }

            IList<Beverage> beverages = json.JsonToObject<IList<Beverage>>();

            Parallel.ForEach(beverages, beverage =>
            {
                beverage.NewId();
            });

            oDtoFileOperation.Finish(beverages.Count);

            return new(beverages.ToDictionary(g => g.Guid.Value) ?? []);
        }
    }
}