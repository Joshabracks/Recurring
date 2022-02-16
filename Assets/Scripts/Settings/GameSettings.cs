
using UnityEngine;

namespace Gameplay.Data
{

    public class GameSettings
    {
        private struct SettingsValues
        {
            public int TargetFramerate;
        }

        private SettingsValues values;

        public void Init()
        {
            Load();
            Apply();
        }

        public void Load()
        {
            values = getSettingsFromFile();
        }

        public void Apply()
        {
            Application.targetFrameRate = values.TargetFramerate;
        }

        private SettingsValues getSettingsFromFile()
        {
            FileSystem fs = new FileSystem();
            string path = $"{Application.dataPath}/Data/SettingsValues/-1.json";
            if (!fs.Exists(path))
            {
                Debug.Log("create " + path);
                fs.SaveJSON(new SettingsValues{TargetFramerate = 60});
            }
            SettingsValues settings = fs.LoadFromJSON<SettingsValues>(path);
            return settings;
        }
    }
}
