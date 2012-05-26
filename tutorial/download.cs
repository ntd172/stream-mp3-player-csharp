using System.Net;

namespace Download
{
  public class DownloadFile
  {
    public static void Main()
    {
      WebClient wc = new WebClient();
      wc.DownloadFile("http://mp3.stream.nixcdn.com/686d17738e633914993271c0569ddd54/4fbf286d/Unv_Audio6/BecauseILoveYou-BuddyHolly_3rzzq_hq.mp3", @"/tmp/test.mp3");
    }
  }
}
