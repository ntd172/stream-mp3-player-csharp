using System;
using System.IO;
using System.Net;
using System.Text;

namespace Examples.System.Net
{
  public class WebRequestPostExample
  {
    public static void Main()
    {
      WebRequest request = WebRequest.Create("http://www.nhaccuatui.com/api/song.ashx");
      request.Method = "POST";

      string postData = "action=download&songkey=wRqbKKJ71w&saveas=Save+link+as...+";
      byte[] byteArray = Encoding.UTF8.GetBytes (postData);
      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = byteArray.Length;
      Stream dataStream = request.GetRequestStream ();
      dataStream.Write (byteArray, 0, byteArray.Length);
      dataStream.Close ();
      WebResponse response = request.GetResponse ();
      Console.WriteLine (((HttpWebResponse)response).StatusDescription);

      dataStream = response.GetResponseStream ();
      StreamReader reader = new StreamReader (dataStream);
      string responseFromServer = reader.ReadToEnd();
      Console.WriteLine (responseFromServer);
      reader.Close();
      dataStream.Close();
      response.Close();
    }
  }
}
