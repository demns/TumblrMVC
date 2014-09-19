using System.Web.Mvc;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;

namespace TumblrExample.Controllers {
    public class HomeController : Controller {
        //[HttpGet]
        //public ActionResult Index(string oauth_token, string oauth_verifier) {

        //    var tc = new TumblrClient(new HmacSha1HashProvider(), "z35mVaZpxg4JC7IdJ1KB1GHYOqYXfwwnv0cIOlHafuMUZ7Tlat",
        //        "gn3rVOlvrYVjLZP4tYnxb1FXYNiFGFZt3twWotz84VeYb9oWbj", new Token("AyfIkgc50nE5FrRMw12tcvwkzBAuPTYz9X02Tz7JMK3wfSklGn", "il1gFmryYosORAoiuHiYqYco10gPXt0Hau6f717ENzFImqLhuH"));

        //    var qq = tc.GetPostsAsync("samsonau");
        //    qq.Wait();

        //    return View();
        //}

        [HttpGet]
        public ActionResult Index() {

            var tc = new TumblrClient(new HmacSha1HashProvider(), "z35mVaZpxg4JC7IdJ1KB1GHYOqYXfwwnv0cIOlHafuMUZ7Tlat",
                "gn3rVOlvrYVjLZP4tYnxb1FXYNiFGFZt3twWotz84VeYb9oWbj");

            var qq = tc.GetPostsAsync("samsonau");
            qq.Wait();

            var q = qq.Result;

            Photo[] photoSet = ((PhotoPost)(q.Result[0])).PhotoSet;
            return View(photoSet);
        }
    }
}