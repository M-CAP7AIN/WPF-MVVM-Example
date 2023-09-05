using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using MVVM_Example.Model;

namespace MVVM_Example.ViewModel
{
    public class JsonFileService : IFileService
    {
        public List<Flower> Open(string fileName)
        {
            List<Flower> phones = new List<Flower>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Flower>));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                phones = jsonFormatter.ReadObject(fs) as List<Flower>;
            }

            return phones;
        }

        public void Save(string fileName, List<Flower> phoneList)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Flower>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, phoneList);
            }
        }
    }
}