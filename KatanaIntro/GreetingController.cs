using System.Web.Http;

namespace KatanaIntro
{
    public class GreetingController : ApiController
    {
        public Greeting Get(string message)
        {
            return new Greeting { Text = message };
        }
    }
}