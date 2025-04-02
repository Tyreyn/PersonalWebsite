using Newtonsoft.Json;
using PersonalWebsite.Models;

namespace PersonalWebsite.Services
{
    public class JsonFileService
    {
        private readonly string _fileName = "About.json";

        public PersonalInformationModel GetPersonalInformationFromFile()
        {
            string jsonStringPersonalInformation = ReadJsonFile();
            return DeserializeStringToPersonalInformationModel(jsonStringPersonalInformation);
        }

        private string ReadJsonFile()
        {
            string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), _fileName);
            try
            {
                if (File.Exists(pathToFile))
                {
                    return File.ReadAllText(pathToFile);
                }
            }
            catch (IOException ioexc)
            {
                Console.WriteLine(ioexc.ToString());
            }
            return string.Empty;
        }

        private PersonalInformationModel DeserializeStringToPersonalInformationModel(string jsonString)
        {
            if (jsonString != null)
            {
                try
                {
                    PersonalInformationModel personalInformation = JsonConvert.DeserializeObject<PersonalInformationModel>(jsonString);
                    return personalInformation;
                }
                catch (JsonSerializationException jsexc)
                {
                    Console.WriteLine(jsexc.ToString());
                }
            }
            return null;

        }
    }
}
