using System.ComponentModel;

namespace LeadSoft.Adapter.OpenAI_Bridge
{
    public partial class Open_AI : IOpen_AI
    {
        private const string _WelcomeMessage = @"Current time: {0}.
                                                You are a helpful assistant for a Beverage Menu Database company.
                                                Perform these steps clearly:
                                                1. Politely ask the guest to introduce themselves to begin registration but keep.
                                                2. Extract personal information strictly according to the provided JSON template, use this template to suggest necessary information as a conversation, not as a form.
                                                   - Do NOT request sensitive information (IDs, documents, etc.).
                                                {1}";
        private const string _EnrichDataMessage = @"You will respond strictly in markdown following the instructions below, and based on the provided chat history, clearly perform:
                                                    1. Extract Data:
                                                    - Extract all required personal data as per provided JSON template; optionally ask for extra helpful (but non-mandatory) fields.
                                                    - Organize additional relevant information under 'Other_Relevant_Information', structured clearly based on conversation content.
                                                    - Ignore standard database fields (e.g., IDs, creation dates, validation flags).
                                                    2. Conditional markdown response:
                                                    - If all required data is complete:
                                                      - Start the response with '✅' and brake line.
                                                      - Create a markdown 'json' snippet fully populating the provided object and brake line.
                                                      - Include create new markdown 'plaintext' snippet with a polite message thanking and welcoming the guest (explicitly congratulating birthday if is the exact day), without repeating registration details.
                                                    - If required data is missing:
                                                      - Start the response with '❌' and brake line.
                                                      - Create a markdown 'plaintext' snippet with a polite message clearly requesting missing information, without repeating registration details.
                                                    Ensure all messages are between snippets and remain concise, professional, polite, and clear.";

        public static partial class Enums
        {
            public enum ContextLabel
            {
                [Description("System")]
                SYSTEM,
                [Description("User")]
                USER,
                [Description("Assistant")]
                ASSISTANT
            }
        }
    }
}
