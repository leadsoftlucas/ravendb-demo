using LeadSoft.Adapter.OpenAI_Bridge;

namespace LucasRT.RavenDB.Demo.RestAPI.Configurations
{
    /// <summary>
    /// Provides extension methods for configuring services in an application.
    /// </summary>
    /// <remarks>This class contains methods to register scoped services in the dependency injection container. Use
    /// these methods to add application-specific services to the <see cref="IServiceCollection"/>.</remarks>
    public static class ServicesConfiguration
    {
        public static void AddSingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<IOpen_AI, Open_AI>();
        }
    }
}
