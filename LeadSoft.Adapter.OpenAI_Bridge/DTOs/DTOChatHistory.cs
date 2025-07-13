using Microsoft.SemanticKernel.ChatCompletion;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace LeadSoft.Adapter.OpenAI_Bridge.DTOs
{
    [Serializable]
    [DataContract]
    public class DTOChatHistory
    {
        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [IgnoreDataMember]
        [JsonIgnore]
        public ChatHistory Context { get; set; } = [];

        [DataMember]
        public string LastMessage { get; set; } = string.Empty;

        [DataMember]
        public bool WasPositive { get; set; } = false;

        [DataMember]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataMember]
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(15);

        [DataMember]
        public bool HasExpired { get => DateTime.UtcNow > ExpiresAt; }

        public DTOChatHistory SetMessage(string aChat, bool aWasPositive = true)
        {
            LastMessage = aChat;
            ExpiresAt = DateTime.UtcNow.AddMinutes(15);
            WasPositive = aWasPositive;
            return this;
        }
    }
}
