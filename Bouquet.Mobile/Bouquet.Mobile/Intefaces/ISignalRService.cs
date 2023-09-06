namespace Bouquet.Mobile.Intefaces
{
    public interface ISignalRService
    {
        void StartSignalRConnection(string email, string message);

        void StopSignalRConnection();
    }
}
