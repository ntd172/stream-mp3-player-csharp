using System;
using System.Text.RegularExpressions;
using System.Web;

class MainClass
{
  public static void Main()
  {
    string text = "adfasdfahref=\"http&#58;&#47;&#47;tmp.mp3.stream.nixcdn.com&#47;e354086334bce940b660e6711d522c2e&#47;4fc00ef4&#47;NhacCuaTui004&#47;HowcanItellher-Lobo_tr6.mp3\" class=";
    text = HttpUtility.HtmlDecode(text);
    int first = text.IndexOf("http");
    int last = text.LastIndexOf("mp3");
    Console.WriteLine(first + " " + last + " " + text.Length);
    Console.WriteLine(text.Substring(first, (last - first + 4)));
  }
}
