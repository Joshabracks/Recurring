using System.IO;
using UnityEngine;

namespace Gameplay.Data
{
    public class FileSystem
    {
        public T LoadFromJSON<T>(string path)
        {
            StreamReader reader = new StreamReader(path);
            string result = reader.ReadToEnd();
            reader.Close();
            return JsonUtility.FromJson<T>(result);
        }
        public string getPath<T>(T data, int index = -1)
        {
            return $"{Application.dataPath}/Data/{typeof(T).Name}/{index.ToString()}.json";
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
        public bool Exists<T>(T data, int index = -1)
        {
            return File.Exists(getPath<T>(data, index));
        }

        private void SecurePath<T>(T data)
        {
            string path = $"{Application.dataPath}/Data/{typeof(T).Name}";
            SecurePath(path);
        }
        private void SecurePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.Log(path + " does not exist.  Creating Directory");
                Directory.CreateDirectory(path);
            }
        }

        public void SaveJSON<T>(T data, int index = -1)
        {
            SecurePath<T>(data);
            string path = getPath(data, index);
            Debug.Log("PATH IS: " + path);
            StreamWriter writer = new StreamWriter(path);
            writer.Write(JsonUtility.ToJson(data, true));
            writer.Close();
        }
    }
}
