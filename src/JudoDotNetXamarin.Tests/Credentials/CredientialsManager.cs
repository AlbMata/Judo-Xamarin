using System.Collections.Generic;
using System.Linq;

namespace JudoDotNetXamarin.Tests.Credentials
{
    public static class CredientialsManager
    {
        private static readonly Dictionary<CreditialsSetKey, CredientialsSet> CredientialsSets = new Dictionary<CreditialsSetKey, CredientialsSet>();
        static CredientialsManager()
        {
            CredientialsSets.Add(CreditialsSetKey.ValidSetNoThreeDSecure, NewCredentialsSet("100144013", "0VdCngNT3ZDFaMgQ", "6b58fc9083bdcc902bd75691274946a653ed9a7e817ce2456c85ead520b5dba7"));
        }

        public static CredientialsSet GetCredientialsSetFromKey(CreditialsSetKey key)
        {
            return CredientialsSets.Single(x => x.Key == key).Value;
        }

        private static CredientialsSet NewCredentialsSet(string judoId, string token, string secret)
        {
            return new CredientialsSet(judoId, token, secret);
        }
    }
}