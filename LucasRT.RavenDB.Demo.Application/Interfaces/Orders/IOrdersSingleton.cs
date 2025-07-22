namespace LucasRT.RavenDB.Demo.Application.Interfaces.Menus
{
    public interface IOrdersSingleton : IDisposable
    {
        Task StartAttendanceAsync();
        Task StopAttendanceAsync();
    }
}
