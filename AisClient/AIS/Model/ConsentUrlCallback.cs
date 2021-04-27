using System;
using AIS.Utils;
using Common.Logging;

namespace AIS.Model
{
    public class ConsentUrlCallback
    {
        public event EventHandler<OnConsentUrlReceivedArgs> OnConsentUrlReceived;
        private static ILog logger = LogManager.GetLogger<ConsentUrlCallback>();

        public void RaiseConsentUrlReceivedEvent(string url, UserData userData, Trace trace)
        {
            var handler = OnConsentUrlReceived;
            if (handler != null)
            {
                handler(this, new OnConsentUrlReceivedArgs
                {
                    Url = url,
                    UserData = userData
                });
            }
            else
            {
                logger.Warn("Consent URL was received from AIS, but no consent URL callback was configured (in UserData). " +
                            $"This transaction will probably fail - {trace.Id}");
            }
        }
    }

    public class OnConsentUrlReceivedArgs
    {
        public string Url { get; set; }
        public UserData UserData { get; set; }
    }
}
