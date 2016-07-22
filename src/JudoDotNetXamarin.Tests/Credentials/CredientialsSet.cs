namespace JudoDotNetXamarin.Tests.Credentials
{
    public class CredientialsSet
    {
        public string JudoId { get; private set; }
        public string Token { get; private set; }
        public string Secret { get; private set; }

        public CredientialsSet(string judoId, string token, string secret)
        {
            JudoId = judoId;
            Token = token;
            Secret = secret;
        }
    }
}