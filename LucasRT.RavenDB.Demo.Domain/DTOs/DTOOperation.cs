using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.DTOs
{
    [Serializable]
    [DataContract]
    public class DTOOperation(string aTitle)
    {
        [DataMember]
        public string Title { get; private set; } = aTitle;

        [DataMember]
        public DateTime StartedAt { get; private set; } = DateTime.UtcNow;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalRecords { get; private set; } = 0;

        [DataMember]
        public DateTime? FinishedAt { get; private set; }

        [DataMember]
        public string ElapsedTime
        {
            get => FinishedAt.HasValue
                    ? $"{(FinishedAt.Value - StartedAt):hh\\:mm\\:ss\\.fff}"
                    : "00:00:00";
        }

        public DTOOperation Finish(int? totalRecords = null)
        {
            TotalRecords = totalRecords;
            FinishedAt = DateTime.UtcNow;
            return this;
        }
    }
}
