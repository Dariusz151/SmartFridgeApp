using System.Xml;
using Newtonsoft.Json;

namespace SmartFridgeApp.Core.Extensions
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
