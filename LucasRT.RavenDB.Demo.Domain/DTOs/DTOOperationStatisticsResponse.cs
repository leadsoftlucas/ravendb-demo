using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.DTOs
{
    [Serializable]
    [DataContract]
    public class DTOOperationStatisticsResponse(string aTitle)
    {
        [DataMember]
        public string Title { get; private set; } = aTitle;

        [DataMember]
        public DateTime StartedAt { get; private set; } = DateTime.UtcNow;

        [DataMember]
        public IList<DTOOperation> DtoOperations { get; private set; } = [];

        [DataMember]
        public DateTime? FinishedAt { get; private set; }

        [DataMember]
        public string ElapsedTime
        {
            get => FinishedAt.HasValue
                    ? $"{(FinishedAt.Value - StartedAt):hh\\:mm\\:ss\\.fff}"
                    : "00:00:00";
        }

        public DTOOperationStatisticsResponse Finish()
        {
            FinishedAt = DateTime.UtcNow;
            return this;
        }
    }
}
