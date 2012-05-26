using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using NhacCuaTui.DownloadOneSong;
using NhacCuaTui.DownloadManySongs;
using System.Web;
using System.Collections.Generic;

namespace NhacCuaTui
{
  namespace DownloadOneSong
  {
    public class DownloadFile
    {
      const string webServer = "http://www.nhaccuatui.com/api/song.ashx";
      private string url = string.Empty;

      public DownloadFile(string url)
      {
        this.url = url;
      }


      public void Download()
      {
        string hiddenLink = GetHiddenLink(url);
        if (hiddenLink == null)
          return;
        Console.WriteLine("Downloading file ... ");
        Console.WriteLine(hiddenLink);

        // get name of the song
        int index = hiddenLink.LastIndexOf("/");
        string name = hiddenLink.Substring(index + 1);
        Console.WriteLine(name);

        WebClient wc = new WebClient();
        wc.DownloadFile(hiddenLink, name);
      }

      public string GetHiddenLink(string url) 
      {
        WebRequest webRequest = WebRequest.Create(webServer);
        // Set method to get download link for the file 
        webRequest.Method = "POST";

        // using regular expression to get out the key of the song
        Regex regex = new Regex(@"=");
        string key = regex.Split(url)[1];

        Console.WriteLine(key);
        string postData = "action=download&songkey=" + key + "&saveas=Save+link+as...+";
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.ContentLength = byteArray.Length;

        // get the Stream of Request and write data on this
        Stream dataStream = webRequest.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        
        // Get Response Back from Server
        WebResponse response = webRequest.GetResponse();
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);

        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();
        Console.WriteLine(responseFromServer);

        reader.Close();
        dataStream.Close();
        response.Close();

        string text = HttpUtility.HtmlDecode(responseFromServer);
        if (text.Contains("\"status\":\"fail\""))
          return null;
        int first = text.IndexOf("http");
        int last = text.LastIndexOf("mp3");
        text = text.Substring(first, (last - first + 3));
        return text;
      }
    }
  }

  namespace DownloadManySongs
  {
    public class DownloadPlayList
    {
      private string url = string.Empty;
      const string webServer = "http://www.nhaccuatui.com/api/playlist.ashx";

      public DownloadPlayList(string url)
      {
        this.url = url;
      }

      public void Download()
      {
        string hiddenLink = GetHiddenLink(url);
        Console.WriteLine(hiddenLink);
      }


      public string GetHiddenLink(string url)
      {

        // using regular expression to get out the key of the playlist
        Regex regex = new Regex(@"=");
        string key = regex.Split(url)[1];

        int page = 1;
        string text = string.Empty;
        while (true) 
        {
          WebRequest webRequest = WebRequest.Create(webServer);
          // Set method to get download link for the file 
          webRequest.Method = "POST";

          string postData;
          if (page == 1)
          {
            postData = "action=get_song_of_playlist&listkey=" + key + "&ismore=fasle";
          }
          else
          {
            postData = "action=get_song_of_playlist&listkey=" + key + "&ismore=true&page=" + page;
          }
          byte[] byteArray = Encoding.UTF8.GetBytes(postData);
          webRequest.ContentType = "application/x-www-form-urlencoded";
          webRequest.ContentLength = byteArray.Length;

          // get the Stream of Request and write data on this
          Stream dataStream = webRequest.GetRequestStream();
          dataStream.Write(byteArray, 0, byteArray.Length);
          dataStream.Close();

          // Get Response Back from Server
          WebResponse response = webRequest.GetResponse();
          Console.WriteLine(((HttpWebResponse)response).StatusDescription);

          dataStream = response.GetResponseStream();
          StreamReader reader = new StreamReader(dataStream);
          string responseFromServer = reader.ReadToEnd();
          text += HttpUtility.HtmlDecode(responseFromServer);

          if (text.Contains("\"more\":\"False\""))
            break;
          page += 1;
        }

        regex = new Regex("nghe\\?[^\\s]*");
        Match match = regex.Match(text);
        HashSet<string> set = new HashSet<string>();
        while (match.Success)
        {
          string temp = match.Value.Replace("\\\"", "");
          if (!set.Contains(temp))
          {
            Console.WriteLine(temp);
            DownloadFile  song = new DownloadFile(temp);
            song.Download();
            set.Add(temp);
          }
          match = match.NextMatch();
        }
        return null;
      }
    }
  }
}

class MainClass
{
  public static void Main()
  {
    DownloadPlayList download = new DownloadPlayList("http://www.nhaccuatui.com/nghe?L=HN2caPI2r2RZ");
    download.Download();
  }
}
