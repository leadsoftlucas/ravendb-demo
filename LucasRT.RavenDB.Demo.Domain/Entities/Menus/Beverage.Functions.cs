using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using System.Reflection;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Menus
{
    public partial class Beverage
    {
        public const string DislikeCounterName = "Dislikes";
        public const string LikeCounterName = "Likes";

        public Beverage()
        {
            CountersNames.Add(LikeCounterName);
            CountersNames.Add(DislikeCounterName);
        }

        public static IList<Beverage> GetSamples(out DTOOperation oDtoFileOperation)
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

            IList<Beverage> Beverages = json.JsonToObject<IList<Beverage>>();

            oDtoFileOperation.Finish(Beverages.Count);

            return Beverages ?? [];
        }
    }
}