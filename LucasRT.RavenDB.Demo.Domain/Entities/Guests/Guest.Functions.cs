using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using System.Collections.Concurrent;
using System.Reflection;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Guests
{
    public partial class Guest
    {
        public const string VisitsCounterName = "Visits";


        public static ConcurrentDictionary<Guid, Guest> GetSamples(out DTOOperation oDtoFileOperation)
        {
            oDtoFileOperation = new("Guests files reading...");

            Stream file;

            try
            {
                file = Assembly.GetExecutingAssembly().GetEmbeddedResourceStream($"Guests.json");
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

            IList<Guest> guests = json.JsonToObject<IList<Guest>>();

            oDtoFileOperation.Finish(guests.Count);

            return new(guests.ToDictionary(g => g.Guid.Value) ?? []);
        }
    }
}