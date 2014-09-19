using System.Web.Mvc;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;

namespace TumblrExample.Controllers {
    public class HomeController : Controller {
        [HttpGet]
        public ActionResult Index(string oauth_token, string oauth_verifier) {

            //var tc = new TumblrClient(new HmacSha1HashProvider(), "z35mVaZpxg4JC7IdJ1KB1GHYOqYXfwwnv0cIOlHafuMUZ7Tlat",
            //    "gn3rVOlvrYVjLZP4tYnxb1FXYNiFGFZt3twWotz84VeYb9oWbj", oauth_token);
            //var q = tc.GetBlogInfoAsync("samsonau");
            //q.Wait();

            //var qq = tc.GetDashboardPostsAsync();
            //qq.Wait();
            //Task<BlogInfo> blogInfo = GetBlockInfo(tc);
            //var qq = blogInfo.Result;

            return View();
        }

    }
}