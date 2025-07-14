namespace LucasRT.RavenDB.Demo.Domain.Entities.Guests
{
    public partial class Guest
    {
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
