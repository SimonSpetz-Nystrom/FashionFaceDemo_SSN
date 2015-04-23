using FashionFaceMVC.Hubs;
using Microsoft.AspNet.SignalR;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace Hubs
{
    public class TwitterHub : Hub
    {
        private IFilteredStream mStream;
        private TwitterWatcher mTwitterWatcher;

        public TwitterHub() : this(TwitterWatcher.Instance)
        {
        }

        public TwitterHub(TwitterWatcher twitterWatcher)
        {
            this.mTwitterWatcher = twitterWatcher;
        }
    }
}