using OA.Api.Responses;

namespace OA.Api.Interfaces
{
    public interface INotificationRepository
    {
        Task<NotificationResponse[]> GetAllNotificationsByIdUserAsync(int userId);

        Task<bool?> UpdateMaskReadAsync(int userId);
    }
}
