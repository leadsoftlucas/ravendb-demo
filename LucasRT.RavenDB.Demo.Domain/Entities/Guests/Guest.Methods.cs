using LeadSoft.Common.Library.Extensions;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Guests
{
    public partial class Guest
    {
        public const string VisitsCounterName = "Visits";

        public Guest()
        {
            CountersNames.Add(VisitsCounterName);
        }

        public Guest CreateVectorField()
        {
            VectorSearchField = this.ToJson().Flatten(excludedFields: ["VectorSearchField", "Id", "Guid"]);
            return this;
        }

        public static Guest GetSample()
             => new()
             {
                 Name = "",
                 Email = "",
                 Nationality = "",
                 Other_Relevant_Information = new { },
                 SocialNetworks =
                 [
                     new ()
                 ]
             };
    }
}
