using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Data
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public int CharacterID;
        public int season;
        public int episode;
        public int health;
        public int strength;
        public int charisma;
        public float[] position;

        public PlayerSaveData(Player player)
        {
            CharacterID = player.CharacterID;
            season = player.season;
            episode = player.episode;
            health = player.health;
            strength = player.strength;
            charisma = player.charisma;

            position = new float[3];

            position[0] = player.transform.position.x;
            position[1] = player.transform.position.y;
            position[2] = player.transform.position.z;

        }
    }
}