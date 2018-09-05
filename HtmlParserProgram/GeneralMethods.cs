using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HtmlParserProgram
{
    class GeneralMethods
    {



        public HtmlDocument ParseHtmlPageSource(string pageSource)
        {
            HttpClient client = new HttpClient();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageSource);

            return pageDocument;
        }

        public HtmlNodeCollection FindSpecificElements(HtmlDocument pageDocument, string XPath)
        {
            HtmlNodeCollection htmlNodeCollections = new HtmlNodeCollection(null);
            htmlNodeCollections = pageDocument.DocumentNode.SelectNodes(XPath);

            return htmlNodeCollections;
        }

        public string IterateThroughHtmlNodes(HtmlNode element)
        {
            HtmlNode node = null;
            node = element;

            return node.OuterHtml;
        }

        public void setTimeOut(int seconds)
        {
            System.Threading.Thread.Sleep(seconds * 1000);
        }

        public HtmlNode IterateThroughChildNodes(HtmlNode nodeCollection , string htmlTag)
        {
            HtmlNode returnedNode = null;
            if (nodeCollection.Name.Equals(htmlTag) && !string.IsNullOrWhiteSpace(htmlTag))
            {
                returnedNode = nodeCollection;
            }

            return returnedNode;
        }
    }
}
