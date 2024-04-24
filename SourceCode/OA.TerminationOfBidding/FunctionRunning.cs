using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OA.Common;
using OA.Common.Helper;
using OA.Entities;

namespace OA.TerminationOfBidding
{
    public class FunctionRunning
    {
        private readonly ILogger _logger;
        private readonly OAContext _context;
        private List<Item> listItemSaving;
        private List<Bid> listBidSaving;
        private List<Rating> listRatingSaving;
        private List<Notification> listNotificationSaving;

        public FunctionRunning(ILoggerFactory loggerFactory, OAContext context)
        {
            _logger = loggerFactory.CreateLogger<FunctionRunning>();
            _context = context;
        }

        public void InitSavingList()
        {
            listItemSaving = new List<Item>();
            listBidSaving = new List<Bid>();
            listRatingSaving = new List<Rating>();
            listNotificationSaving = new List<Notification>();
        }

        [Function("FunctionRunning")]
        public async Task RunAsync([TimerTrigger("* */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Termination of bidding executed at: {DateTime.Now}");

            try
            {
                InitSavingList();

                // Get items are happening
                var today = DateTime.Now.Date;
                var items = await _context.Item.Where(i =>
                                                    (i.BidStatus == (byte)BidStatusEnums.UpComming && i.AuctionStartDate.Date <= today)
                                                    ||
                                                    (i.BidStatus == (byte)BidStatusEnums.Happening && today > i.AuctionEndDate.Date)
                                                )
                                               .ToListAsync();
                var itemIds = items.Select(i => i.Id).ToList();
                var bids = await _context.Bid.Where(b => itemIds.Contains(b.ItemId)).ToListAsync();
                var sellerIds = items.Select(i => i.SellerId).ToList();
                var sellers = await _context.User.Where(u => sellerIds.Contains(u.Id)).ToListAsync();
                var buyerIds = bids.Select(i => i.BuyerId).ToList();
                var buyers = await _context.User.Where(u => buyerIds.Contains(u.Id)).ToListAsync();

                _logger.LogInformation($" There are: {items.Count} items for processing");

                foreach (var item in items)
                {
                    _logger.LogInformation($"- Begin ItemId: {item.Id} | Name: {item.Name}");

                    try
                    {
                        AddDataIntoListSaving(item, bids);
                    }
                    catch (Exception exItem)
                    {
                        _logger.LogError($"Something went wrong during AddDataIntoListSaving: {exItem.Message}");
                    }

                    _logger.LogInformation($"- End ItemId: {item.Id} | Name: {item.Name}");
                }

                _context.Item.UpdateRange(listItemSaving);
                _context.Bid.UpdateRange(listBidSaving);
                _context.Rating.AddRange(listRatingSaving);
                _context.Notification.AddRange(listNotificationSaving);
                await _context.SaveChangesAsync();

                await SendEmailAsync(items, bids, sellers, buyers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
            }

            _logger.LogInformation($"Termination of bidding ended at: {DateTime.Now}");
        }

        public void AddDataIntoListSaving(Item item, List<Bid> bids)
        {
            if (item.BidStatus == (byte)BidStatusEnums.UpComming)
            {
                AddDataForUpCommingStatus(item);
            }
            else
            {
                AddDataForHappeningStatus(item, bids);
            }
        }

        public void AddDataForUpCommingStatus(Item item)
        {
            item.BidStatus = (byte)BidStatusEnums.Happening;
            listItemSaving.Add(item);
        }

        public void AddDataForHappeningStatus(Item item, List<Bid> bids)
        {
            item.BidStatus = (byte)BidStatusEnums.Ended;
            listItemSaving.Add(item);

            var achievedBid = bids.Where(b => b.ItemId == item.Id && b.Amount == item.CurrentBidPrice).FirstOrDefault();
            if (achievedBid == null)
            {
                return;
            }

            _logger.LogInformation($"-- Proceed Bid for ItemId {item.Id} | Name: {item.Name} --");
            achievedBid.IsAchieved = true;
            listBidSaving.Add(achievedBid);

            _logger.LogInformation($"-- Generate Rating for ItemId {item.Id} | Name: {item.Name} ");
            listRatingSaving.Add(new Rating
            {
                ItemId = item.Id,
                SellerId = item.SellerId,
                BuyerId = achievedBid.BuyerId,
            });

            _logger.LogInformation($"-- Generate Notification for ItemId {item.Id} | Name: {item.Name} ");
            listNotificationSaving.Add(new Notification
            {
                ItemId = item.Id,
                UserId = achievedBid.BuyerId,
                MarkRead = false,
                NotificationDate = DateTime.Now.ToUniversalTime(),
                Message = $"Auction has ItemId {item.Id} | Name: {item.Name} is ended. Please click to do rating for Seller!"
            });
            listNotificationSaving.Add(new Notification
            {
                ItemId = item.Id,
                UserId = item.SellerId,
                MarkRead = false,
                NotificationDate = DateTime.Now.ToUniversalTime(),
                Message = $"Auction has ItemId {item.Id} | Name: {item.Name} is ended. Please click to do rating for Buyer!"
            });
        }

        public async Task SendEmailAsync(List<Item> items, List<Bid> bids, List<User> sellers, List<User> buyers)
        {
            _logger.LogInformation($"- Begin sending email -");

            foreach (var item in items)
            {
                try
                {
                    var achievedBid = bids.Where(b => b.ItemId == item.Id && b.Amount == item.CurrentBidPrice && b.IsAchieved == true).FirstOrDefault();
                    if (achievedBid == null)
                    {
                        return;
                    }

                    _logger.LogInformation($"-- Begin sending email for ItemId: {item.Id} | Name: {item.Name}");

                    // Send for Seller
                    await SendEmailForUserAsync(item, "Congratulation Seller: ", sellers, item.SellerId);

                    // Send for Buyer
                    await SendEmailForUserAsync(item, "Congratulation Buyer: ", buyers, achievedBid.BuyerId);

                    _logger.LogInformation($"-- End sending email for ItemId: {item.Id} | Name: {item.Name}");
                }
                catch (Exception exItem)
                {
                    _logger.LogError($"Something went wrong during SendEmailForUserAsync: {exItem.Message}");
                }
            }

            _logger.LogInformation($"- End sending email -");
        }

        private async Task SendEmailForUserAsync(Item item, string subject, List<User> users, int userId)
        {
            var body = "Congratulations, you have successfully bid for an item " + item.Name;
            var user = users.Where(u => u.Id == userId).FirstOrDefault();
            subject += user?.FullName;
            await EmailHelper.SendMailAsync(user?.Email, body, subject);
        }
    }
}
