using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using FindWord.Models;

namespace FindWord.Data
{
    public class Repository
    {
        private static string fileName = @"Data\words.json";

        public static List<string> Words { get; set; }

        public static Dictionary<int,List<string>> WordsWithLength { get; set; }

        public async static Task<WordCategories> GetAll()
        {
            try
            {
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

                var file = await folder.GetFileAsync(fileName);
                
                if (file == null) 
                    return null;

                using (var fs = await file.OpenAsync(FileAccessMode.Read))
                {
                    using (var inStream = fs.GetInputStreamAt(0))
                    {
                        using (var reader = new DataReader(inStream))
                        {
                            await reader.LoadAsync((uint)fs.Size);

                            var json = reader.ReadString((uint)fs.Size);
                            reader.DetachStream();
                            return JsonConvert.DeserializeObject<WordCategories>(json);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }

            return null;
        }
    }
}