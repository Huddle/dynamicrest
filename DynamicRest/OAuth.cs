namespace DynamicRest
{
    public class OAuth
    {
        readonly string _token;

        public OAuth(string token)
        {
            _token = token;
        }

        internal string Token
        {
            get { return string.Format(@"OAuth2 {0}", _token); }
        }
    }
}