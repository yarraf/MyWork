using System.IO;
using System.Runtime.Serialization;
using Ratp.Hidalgo;
using log4net;

namespace Core.Common.Core
{
    public static class SerializeObject<T>
    {
        public static string XmlSerialize(T obj, string url)
        {
            string result = null;
            var fullpath = Path.GetFullPath(url);
            using (FileStream stream = new FileStream(fullpath, System.IO.FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(T));
                ser.WriteObject(stream, obj);
            }

            return result;
        }
    }
}
