using System.Runtime.Serialization;

namespace LeadSoft.Adapter.OpenAI_Bridge.DTOs
{
    [Serializable]
    [DataContract]
    public class DTORole
    {
        [DataMember]
        public string Label { get; set; }
    }
}
