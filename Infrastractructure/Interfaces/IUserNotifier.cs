namespace RestaurantLibrary.Interfaces
{
    public interface IUserNotifier
    {
        void ShowError(string message);
        void ShowInfo(string message);
    }
}
