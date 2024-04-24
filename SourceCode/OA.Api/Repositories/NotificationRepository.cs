using Microsoft.EntityFrameworkCore;
using OA.Api.Interfaces;
using OA.Api.Responses;
using OA.Entities;

namespace OA.Api.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly OAContext _context;

        public NotificationRepository(OAContext context)
        {
            _context = context;
        }

        public async Task<NotificationResponse[]> GetAllNotificationsByIdUserAsync(int userId)
        {
            var notifications = await _context.Notification
                                        .Where(i => i.UserId == userId)
                                        .OrderBy(notification => notification.MarkRead)
                                        .ThenByDescending(notification => notification.NotificationDate)
                                        .ToListAsync();
            return notifications.Select(notification => new NotificationResponse
            {
                Id = notification.Id,
                UserId = notification.UserId,
                ItemId = notification.ItemId,
                Message = notification.Message,
                NotificationDate = notification.NotificationDate.ToLocalTime(),
                MarkRead = notification.MarkRead
            }).ToArray();
        }

        public async Task<bool?> UpdateMaskReadAsync(int userId)
        {
            var notifications = await _context.Notification
            .Where(i => i.UserId == userId)
            .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.MarkRead = true;
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
