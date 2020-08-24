using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace SmartFridgeApp.Infrastructure.Extensions
{
    public static class ConvertHelper
    {
        public static string ConvertXmlToJson(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

           return JsonConvert.SerializeXmlNode(doc);
        }
    }
}
