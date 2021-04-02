namespace AIS.Model
{
    public interface IConsentUrlCallback
    {
        void OnConsentUrlReceived(string consentUrl, UserData userData);
    }
}
