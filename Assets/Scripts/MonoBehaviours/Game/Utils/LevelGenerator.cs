using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject corridorAll;
    public GameObject corridorBT;
    public GameObject corridorBTL;
    public GameObject corridorBTR;
    public GameObject corridorBLR;
    public GameObject corridorBL;
    public GameObject corridorBR;
    public GameObject corridorTR;
    public GameObject corridorTL;
    public GameObject corridorTLR;
    public GameObject corridorLR;

    public int mapSize = 200;
    public int spawningAttempt = 20;
    private bool[,] Map;

    public GameObject[] roomsPrefabs;


    public List<GameObject> roomsSpawned;
    public List<GameObject> corridorsSpawned;
    public List<GameObject> doors;
    public void AddDoor(GameObject door) { doors.Add(door);  }

    //Check if no room exist in this area
    private bool CheckPlacement(AddRoom room, Vector3 P)
    {
        if (P.x / 4 + room.size.x > mapSize || P.z / 4 + room.size.y > mapSize) return false;
        for(int x=0; x < room.size.x; x++)
        {
            for (int z = 0; z < room.size.y; z++)
            {
                //Check all position at true in room -> false in Map
                if (room.map[x , z] == true)
                {
                    if(x+(int)P.x/4 < mapSize && z + (int)P.z/4 < mapSize)
                    {
                        if (Map[x + (int)P.x/4, z + (int)P.z/4])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        for (int x = 0; x < room.size.x; x++)
        {
            for (int z = 0; z < room.size.y; z++)
            {
                //Check all position at true in room -> false in Map
                if (room.map[x, z] == true)
                {
                    if (x + (int)P.x/4 < mapSize && z + (int)P.z/4 < mapSize)
                    {
                        Map[x + (int)P.x/4, z + (int)P.z/4] = true;
                    }

                }
            }
        }
        return true;        
    }
    //Spawn randomly rooms of the array "rooms"
    private void RoomSpawner()
    {
         
        for (int i = 0; i < spawningAttempt; i++)
        {
            Vector3 myPosition;

            //Get random room among availible rooms
            int rand = Random.Range(0, roomsPrefabs.Length);
            //
            GameObject Prefab = roomsPrefabs[rand];
            //
            AddRoom roomRandom = GameObject.Instantiate(Prefab,this.transform).GetComponent<AddRoom>();
            myPosition = new Vector3(Random.Range(0, mapSize)*4, 0, Random.Range(0, mapSize)*4);
            
            //Get map of the room
            roomRandom.RoomMapping();
            roomRandom.transform.position = myPosition;
            

            bool wasPlaced=false;
            for(int j = 0; j<5; j++)
            {
                wasPlaced = CheckPlacement(roomRandom, myPosition);
                if (wasPlaced) break;
                myPosition = new Vector3(Random.Range(0, mapSize) * 4, 0, Random.Range(0, mapSize) * 4);
                roomRandom.transform.position = myPosition;
            }
            if (!wasPlaced) GameObject.Destroy(roomRandom.gameObject);
        }
    }


    //Spawn corridors linking rooms
    private void CorridorSpawner()
    {

    }

    public void CreateMap()
    {
        Map = new bool[mapSize, mapSize];
    }

    private void Start()
    {
        
        CreateMap();
        RoomSpawner();
        CorridorSpawner();
    }
}
