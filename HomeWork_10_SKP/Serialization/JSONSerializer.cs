using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_10_SKP
{
    static public class JSONSerializer
    {
        public static void JSONSerializeClients(ObservableCollection<ClientViewModel> clients)
        {            
            string json = JsonConvert.SerializeObject(clients);

            File.WriteAllText("clients.json", json);
        }

        public static ObservableCollection<ClientViewModel> JSONDeserializeClients()
        {
            string json = File.ReadAllText("clients.json");

            return JsonConvert.DeserializeObject<ObservableCollection<ClientViewModel>>(json);
        }
    }
}
