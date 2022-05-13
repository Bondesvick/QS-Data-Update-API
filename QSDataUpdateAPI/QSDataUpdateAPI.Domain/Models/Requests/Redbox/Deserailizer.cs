using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace QSDataUpdateAPI.Domain.Models.Requests.Redbox
{
    public static class Deserailizer
    {
        public static T DeserializeXML<T>(string objectData)
        {
            objectData = objectData.Replace("\n", "");
            var serializer = new XmlSerializer(typeof(T));
            object result;
            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }
            return (T)result;
        }
    }
}