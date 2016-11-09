using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace WebApiLab.Models
{
    public class MetaParser
    {
        public List<MyData> myDataList = new List<MyData>();
        public string Source { get; set; }
        public int FromValue { get; set; }
        public int ToValue { get; set; }
        private static List<string> urls = new List<string>();

        private int _index = 0;
        private int _size = 0;

        public void Parse()
        {
            var webGet = new HtmlWeb();
            string url = GetUrl();
            var document = webGet.Load(url);
            string matchStrFirst = GetMachStr();
            string innerMather = GetMachStr2();
            var metaTags = document.DocumentNode.SelectNodes(matchStrFirst);
            int skip = 0;
            if (metaTags != null)
            {
                _size = FromValue;
                foreach (var tag in metaTags)
                {
                    if (skip < FromValue)
                    {
                        skip++;
                        continue;
                    }
                    if (skip > ToValue)
                    {
                        break;
                    }
                    if (tag.Attributes["href"] != null)
                    {
                        string pageAddress = tag.Attributes["href"].Value;
                        if (!urls.Contains(pageAddress))
                        {
                            urls.Add(pageAddress);
                            document = webGet.Load(pageAddress);
                            metaTags = document.DocumentNode.SelectNodes(innerMather);
                            ParseReqData(metaTags, pageAddress);
                            skip++;
                        }
                    }
                }
            }
        }

        private void ParseReqData(HtmlNodeCollection metaTags, string pageAddress)
        {
            if (metaTags != null)
            {
                Dictionary<string, string> tmpList = new Dictionary<string, string>();
                foreach (var tagas in metaTags)
                {
                    if (_index > 2) _index = 0;
                    if (tagas.Attributes["content"] != null)
                    {
                        string value = tagas.Attributes["content"].Value;
                        tmpList.Add(GetTag(_index), value);
                        _index++;
                    }

                    if (_index > 2)
                    {
                        if (tmpList.Count == 3)
                        {
                            myDataList.Add(new MyData { Index = _size, Address = pageAddress, List = tmpList });
                            _size++;
                        }
                        tmpList = new Dictionary<string, string>();
                    }
                }
            }
        }

        private string GetUrl()
        {
            switch (Source)
            {
                case "delfi":
                    return "http://www.delfi.lt/";
                case "technologijos":
                    return "http://www.technologijos.lt/";
                case "15min":
                    return "http://www.15min.lt/";
            }
            return null;
        }

        private string GetTag(int index)
        {
            switch (index)
            {
                case 0:
                    return "Img";
                case 1:
                    return "Title";
                case 2:
                    return "Description";
            }
            return "";
        }

        private string GetMachStr()
        {
            switch (Source)
            {
                case "delfi":
                    // or @class='article-image'
                    return "//a[@class='article-title' or @class='article-image']";
                case "technologijos":
                    return "//div[@class = 'MiddleTitle']//a";
                case "15min":
                    return "//a[@class='item-image']";
            }
            return "http://www.lrytas.lt/";
        }

        private string GetMachStr2()
        {
            switch (Source)
            {
                case "technologijos":
                    return "//meta[@name='og:title' or @name='description' or @property='og:image']";
            }
            return "//meta[@name='twitter:title' or @name='twitter:description' or @property='og:image']";
        }
    }

}

public struct MyData
{
    public int Index { get; set; }
    public string Address { get; set; }
    public Dictionary<string, string> List { get; set; }
}