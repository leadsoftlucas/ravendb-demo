using System.Runtime.Serialization;

namespace LeadSoft.Adapter.OpenAI_Bridge.DTOs
{
    [Serializable]
    [DataContract]
    public class DTOContext
    {
        [DataMember]
        public DTORole Role { get; set; }

        [DataMember]
        public string Content { get; set; }
    }
}
