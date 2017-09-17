using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ReleasedToday.Models;
using Newtonsoft.Json.Linq;
using System.Threading;
using ReleasedToday.Data;

namespace ReleasedToday.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index2(string d)
        {

            DateTime date = new DateTime();
            if(d == null)
            {
                date = DateTime.Now;
            }
            else
            {
                date = DateTime.ParseExact(d, "dd.MM.yyyy.", System.Globalization.CultureInfo.InvariantCulture);
            }

            var albums = _context.Albums
                        .Where(a => a.ReleaseDate.Day == date.Day && a.ReleaseDate.Month == date.Month)
                        .OrderBy(a => a.Name)
                        .ToList();

            var month = date.Month;
            var day = date.Day;
            var m = month.ToString();
            var da = day.ToString();
            if (month < 10) m = "0" + m;
            if (day < 10) da = "0" + da;
            ViewData["Date"] = $"{da}.{m}.{date.Year}.";
            ViewData["Date2"] = $"{day}.{month}.{date.Year}.";

            return View(albums);
        }

        public async Task<IActionResult> GetData()
        {
            string key = @"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=acc6eab6d75d472cba5f4069b3e4fc95&format=json";
            var lines = System.IO.File.ReadAllLines(@"C:\Users\Tomislav\Desktop\out3.txt");
            int i = 1;
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyClient", "1.0"));
            foreach (var line in lines)
            {
                var artist = line.Split(new[] { " - " }, StringSplitOptions.None)[0];
                var albumName = line.Split(new[] { " - " }, StringSplitOptions.None)[1].Split(',')[0];
                var date = line.Split(new[] { ", " }, StringSplitOptions.None).Last();
                if (artist == "" || albumName == "") continue;
                var path = key + "&artist=" + artist + "&album=" + albumName;
                Thread.Sleep(500);
                string json = "";
                
                bool hasId = false;
                bool exist = true;
                while (true)
                {
                    try
                    {
                        json = await client.GetStringAsync(path);
                        JToken token = JObject.Parse(json);
                        var mbid = token.SelectToken("album.mbid").ToString();
                        hasId = true;
                        var x = _context.Albums.Where(a => a.Id == mbid).SingleOrDefault();
                        if (x != null) break;
                        var imageMega = "";
                        var imageLarge = "";
                        var imageExtraLarge = "";
                        try
                        {
                            imageLarge = token.SelectToken("album.image[?(@.size=='large')].#text").ToString();
                            imageMega = token.SelectToken("album.image[?(@.size=='mega')].#text").ToString();
                            imageExtraLarge = token.SelectToken("album.image[?(@.size=='extralarge')].#text").ToString();
                        }
                        catch
                        {
                            imageLarge = token.SelectToken("album.image[?(@.size=='large')].'#text'").ToString();
                        }

                        var album = new Album
                        {
                            Id = mbid,
                            Artist = artist,
                            Name = albumName,
                            ReleaseDate = Convert.ToDateTime(date),
                            ImageLarge = imageLarge,
                            ImageMega = imageMega,
                            ImageExtraLarge = imageExtraLarge
                        };

                        _context.Add(album);

                        System.Diagnostics.Debug.WriteLine($"{i++}. {artist} - {albumName}, {date}");

                        await _context.SaveChangesAsync();
                        exist = false;
                        break;
                    }
                    catch
                    {
                        if (!hasId) break;
                        if (exist) break;
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
