using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using System.IO.IsolatedStorage;

namespace N7_92_game4
{
    public class FileManager
    {
        static string highscoreFilename = "highscores.sav";
        static string settingsFilename = "settings.cfg";

        public struct GameSettings
        {
            public bool SoundEnabled;
            public float VolumeLevel;
            public float SoundLevel;            
        }

        public struct Highscores
        {
            public List<int> highscoreList;
        }

        public static void Initialize()
        {
            string filepath = string.Format("Content/{0}", highscoreFilename);
#if WINDOWS
            if(!File.Exists(filepath))
            {
                List<int> temp = new List<int>(6);
                for (int i = 0; i < 6; i++)
                    temp.Add(0);

                SaveHighscores(temp);
            }
#else
            filepath = highscoreFilename;
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.FileExists(filepath))
                {
                    List<int> temp = new List<int>(6);
                    for (int i = 0; i < 6; i++)
                        temp.Add(0);

                    SaveHighscores(temp);
                }
            }
#endif

        }

        public static GameSettings ReadGameSettings(string filename)
        {
            GameSettings gameSettings;
            if (File.Exists(filename))
            {
                Stream stream = File.OpenRead(filename);
                XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
                gameSettings = (GameSettings)serializer.Deserialize(stream);
                stream.Close();
            }
            else
            {
                gameSettings = new GameSettings();
                SaveGameSettings(filename, gameSettings);
            }
            return gameSettings;
        }

        public static void SaveGameSettings(string filename, GameSettings gameSettings)
        {
            if (!File.Exists(filename))
            {
                FileStream file = File.Create(filename);
                file.Dispose();
                file.Close();
            }
            Stream stream = File.OpenWrite(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            serializer.Serialize(stream, gameSettings);
            stream.Close();
        }

        #region Highscores
        public static List<int> ReadHighscores()
        {
            Highscores highscores;

            string filepath = string.Format("Content/{0}", highscoreFilename);

#if WINDOWS
            // Open file
            FileStream stream = File.Open(filepath, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {
                // Read from the file
                XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
                highscores = (Highscores)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
#else
            filepath = highscoreFilename;
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filepath, FileMode.Open, iso))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
                    highscores = (Highscores)serializer.Deserialize(stream);
                }
            }
#endif

            return highscores.highscoreList;

        }

        public static void SaveHighscores(List<int> highscores)
        {
            Highscores temp = new Highscores();
            temp.highscoreList = highscores;

            string filepath = string.Format("Content/{0}", highscoreFilename);

#if WINDOWS
            // Open file
            FileStream stream = File.Open(filepath, FileMode.OpenOrCreate);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
                serializer.Serialize(stream, temp);
            }
            finally
            {
                stream.Close();
            }
#else
            filepath = "highscores.sav";
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filepath, FileMode.OpenOrCreate, iso))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
                    serializer.Serialize(stream, temp);
                }
            }
#endif
        }
        #endregion
    }
}