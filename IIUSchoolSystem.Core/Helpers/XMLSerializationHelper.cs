using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IIUSchoolSystem.Core.Helpers
{
    public static class XMLSerializationHelper
    {
        public static string XmlSerialize(object obj)
        {
            if (null != obj)
            {
                // Assuming obj is an instance of an object
                var ser = new XmlSerializer(obj.GetType());
                var sb = new System.Text.StringBuilder();
                var writer = new System.IO.StringWriter(sb);
                ser.Serialize(writer, obj);
                return sb.ToString();
            }
            return string.Empty;
        }

        public static object XmlDeserialize(Type objType, string xmlDoc)
        {
            if (xmlDoc != null && objType != null)
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlDoc);
                //Assuming doc is an XML document containing a serialized object and objType is a System.Type set to the type of the object.
                var reader = new XmlNodeReader(doc.DocumentElement);
                var ser = new XmlSerializer(objType);
                return ser.Deserialize(reader);
            }
            return null;
        }

        /// <summary>
        ///     Convert the Object from the XML Representaion of it
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="xml"> </param>
        /// <returns> </returns>
        public static T FromXml<T>(this string xml) where T : class
        {
            if (String.IsNullOrEmpty(xml))
                return null;
            var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xml));

            using (
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.Unicode, XmlDictionaryReaderQuotas.Max, null))
            {
                var dataContractSerializer = new DataContractSerializer(typeof(T));

                return dataContractSerializer.ReadObject(reader) as T;
            }
        }

        /// <summary>
        /// 	Convert the Object to the XML Representaion of it
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj)
        {
            DataContractSerializer dataContractSerializer = new DataContractSerializer(obj.GetType());

            String text;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                dataContractSerializer.WriteObject(memoryStream, obj);

                byte[] data = new byte[memoryStream.Length];
                Array.Copy(memoryStream.GetBuffer(), data, data.Length);
                text = Encoding.UTF8.GetString(data);
            }

            return text;
        }
    }
}
