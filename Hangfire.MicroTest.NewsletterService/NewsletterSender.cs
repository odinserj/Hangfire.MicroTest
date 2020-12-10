using System;

namespace Hangfire.MicroTest.NewsletterService
{
    public sealed class NewsletterSender
    {
        public void Execute(long campaignId)
        {
            Console.WriteLine($"Processing newsletter '{campaignId}'");
        }
    }
}