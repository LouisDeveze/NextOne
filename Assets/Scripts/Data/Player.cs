using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    public class Player : MonoBehaviour
    {
        public int CharacterID = 0;
        public int season = 1;
        public int episode = 0;
        public int health = 150;
        public int strength = 37;
        public int charisma = 12;
        public void SavePlayer()
        {
            SaveSystem.savePlayer(this);
        }

        public void LoadPlayer()
        {
            PlayerSaveData data =  SaveSystem.LoadPlayer();
            CharacterID = data.CharacterID;
            season = data.season;
            episode = data.episode;
            health = data.health;
            strength = data.strength;
            charisma = data.charisma;
            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];

            transform.position = position;
        }
    }  
}