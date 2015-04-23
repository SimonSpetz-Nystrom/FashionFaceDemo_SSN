using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using Hubs;
using Microsoft.AspNet.SignalR;
using Tweetinvi;
using Tweetinvi.Core.Events.EventArguments;

namespace FashionFaceMVC.Hubs
{
    public class TwitterWatcher
    {
        private static readonly Lazy<TwitterWatcher> mInstance = new Lazy<TwitterWatcher>(
            () => new TwitterWatcher(GlobalHost.ConnectionManager.GetHubContext<TwitterHub>()));
        private readonly IHubContext mContext;

        private TwitterWatcher(IHubContext hubContext)
        {
            this.mContext = hubContext;

            TwitterCredentials.SetCredentials("3197547411-x457JfxqHr9sVzojna7HG5MX6Zk6nCAgbXU1siC",
                    "eCr9KTS5vCro9v2qUE8WuwCEBdBD6znCzrRzFXmNqYmdv", "Qi5y6nmjxPLrT9xhqP7lw4kfF",
                    "VTTVlvLDhbic2sX6B7BuQh2VZvurIPI76Ajw2FH5RQaimQBW1G");

            ThreadPool.QueueUserWorkItem(delegate
            {
                var stream = Stream.CreateFilteredStream();
                stream.AddTrack("Tower Hamlets");
                stream.MatchingTweetReceived += OnMatchingTweetReceived;
                stream.StartStreamMatchingAllConditions();
            }, null);
        }

        public static TwitterWatcher Instance
        {
            get { return mInstance.Value; }
        }

        private void OnMatchingTweetReceived(object sender, MatchedTweetReceivedEventArgs args)
        {
            if (args.Tweet.Media != null && args.Tweet.Media.FirstOrDefault() != null)
            {
                mContext.Clients.All.addImage(args.Tweet.Media[0].MediaURL);
            }
            mContext.Clients.All.addNewMessageToPage(args.Tweet.Creator.ToString(), args.Tweet.Text);
        }
    }
}