﻿namespace OA.Api.Responses
{
    public class BidResponse
    {
        public int Id { get; set; }

        public int BuyerId { get; set; }

        public int ItemId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsAchieved { get; set; }
    }
}