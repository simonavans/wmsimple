using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessageBus
{
    public abstract class Transaction<T>
    {
        public static T FromJson(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static string ToJson(T transaction)
        {
            return JsonConvert.SerializeObject(transaction);
        }
    }
}
