

namespace SSE_sample.Contracts
{
    public record StockNewsFeed(int Id, string Symbol, string Headline, decimal Price, DateTime Timestamp);
}