using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;

namespace TumblrExample.Controllers {
    public class TumblrController : Controller {
        private const string AuthorizationUrl = "http://www.tumblr.com/oauth/authorize";
        private const string AccessTokenUrl = "http://www.tumblr.com/oauth/access_token";
        private const string RequestTokenUrl = "http://www.tumblr.com/oauth/request_token";

        private static readonly ConcurrentDictionary<string, InMemoryTokenManager> TokenStore = new ConcurrentDictionary<string, InMemoryTokenManager>();

        private static readonly WebConsumer TumblrConsumer;

        static TumblrController() {
            TumblrConsumer = new WebConsumer(TumblrServiceProviderDescription, ShortTermUserSessionTokenManager);
        }

        public ActionResult Auth() {
            var callBack = new Uri("http://localhost:62385/Home/Index");
            var request = TumblrConsumer.Channel.PrepareResponse(TumblrConsumer.PrepareRequestUserAuthorization(callBack, null, null));
            request.Send();

            return new EmptyResult();
        }

        public ActionResult Callback() {
            var auth = TumblrConsumer.ProcessUserAuthorization();
            // do something with auth

            return new EmptyResult();
        }

        private static ServiceProviderDescription TumblrServiceProviderDescription {
            get {
                var tumblrDescription = new ServiceProviderDescription {
                    UserAuthorizationEndpoint = new MessageReceivingEndpoint(AuthorizationUrl, HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                    AccessTokenEndpoint = new MessageReceivingEndpoint(AccessTokenUrl, HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                    RequestTokenEndpoint = new MessageReceivingEndpoint(RequestTokenUrl, HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                    TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() }
                };

                return tumblrDescription;
            }
        }

        private static InMemoryTokenManager ShortTermUserSessionTokenManager {
            get {
                InMemoryTokenManager tokenManager;
                TokenStore.TryGetValue("Tumblr", out tokenManager);

                if (tokenManager == null) {
                    tokenManager = new InMemoryTokenManager(ConfigurationManager.AppSettings["TumblrKey"], ConfigurationManager.AppSettings["TumblrSecret"]);
                    TokenStore.TryAdd("Tumblr", tokenManager);
                }

                return tokenManager;
            }
        }
    }

    public class InMemoryTokenManager : IConsumerTokenManager {
        private readonly Dictionary<string, string> _tokensAndSecrets = new Dictionary<string, string>();

        public InMemoryTokenManager(string consumerKey, string consumerSecret) {
            if (string.IsNullOrEmpty(consumerKey)) {
                throw new ArgumentNullException("consumerKey");
            }

            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public string ConsumerKey { get; private set; }

        public string ConsumerSecret { get; private set; }

        #region ITokenManager Members

        public string GetTokenSecret(string token) {
            return _tokensAndSecrets[token];
        }

        public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response) {
            _tokensAndSecrets[response.Token] = response.TokenSecret;
        }


        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret) {
            _tokensAndSecrets.Remove(requestToken);
            _tokensAndSecrets[accessToken] = accessTokenSecret;
        }

        public TokenType GetTokenType(string token) {
            throw new NotImplementedException();
        }

        #endregion
    }
}