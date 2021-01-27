using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Data
{
    public static class SaveSystem 
    {
        public static void savePlayer(Player player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.data";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerSaveData data = new PlayerSaveData(player);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerSaveData LoadPlayer()
        {
            string path = Application.persistentDataPath + "/player.data";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerSaveData data = formatter.Deserialize(stream) as PlayerSaveData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save File not found in" + path);
                return null;
            }
        }
       
    }
}