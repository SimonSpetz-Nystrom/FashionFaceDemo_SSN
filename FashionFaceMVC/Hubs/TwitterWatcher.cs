using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        private static readonly BetaFaceApi.BetaFaceApi mFaceApi =
            new BetaFaceApi.BetaFaceApi(
                "d45fd466-51e2-4701-8da8-04351c872236", 
                "171e8465-f548-401d-b63b-caf0dc28df5f");


        private TwitterWatcher(IHubContext hubContext)
        {
            this.mContext = hubContext;

            TwitterCredentials.SetCredentials("3197547411-x457JfxqHr9sVzojna7HG5MX6Zk6nCAgbXU1siC",
                    "eCr9KTS5vCro9v2qUE8WuwCEBdBD6znCzrRzFXmNqYmdv", "Qi5y6nmjxPLrT9xhqP7lw4kfF",
                    "VTTVlvLDhbic2sX6B7BuQh2VZvurIPI76Ajw2FH5RQaimQBW1G");

            ThreadPool.QueueUserWorkItem(delegate
            {
                var stream = Stream.CreateFilteredStream();
                stream.AddTrack("image");
                stream.MatchingTweetReceived += OnMatchingTweetReceived;
                stream.StartStreamMatchingAllConditions();
            }, null);
        }

        public static TwitterWatcher Instance
        {
            get { return mInstance.Value; }
        }

        private async void OnMatchingTweetReceived(object sender, MatchedTweetReceivedEventArgs args)
        {
            if (args.Tweet.Media != null && args.Tweet.Media.FirstOrDefault() != null)
            {
                var info = await mFaceApi.GetFaceInfoAsync(args.Tweet.Media[0].MediaURL);
                if (info.faces != null)
                {
                    Debug.Print(info.faces.Count.ToString());
                    var genders =
                        (from face in info.faces
                            select face.tags.SingleOrDefault(tag => tag.name == "gender")).ToList();

                    genders.RemoveAll(elm => elm == null);

                    var maleCount = genders.Count(elm => elm.value == "male");
                    var femaleCount = genders.Count() - maleCount;

                    if (maleCount > 0)
                    {
                        mContext.Clients.All.addMales(maleCount);
                    }
                    if (femaleCount > 0)
                    {
                        mContext.Clients.All.addFemales(femaleCount);
                    }
                }
                mContext.Clients.All.addImage(args.Tweet.Media[0].MediaURL, args.Tweet.Media[0].DisplayURL);
            }
        }
    }
}