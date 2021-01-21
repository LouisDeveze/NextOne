using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public float PlayerSpeed;
    public string FirstMap;
    public string LastMap;
    public List<string> MapList;
    private int ActualMap;
    public bool exit = false;   //For the player spawn when he comes back to the previous map
    //private List<string> RandomList;
    private void InitiateGame()
    {
        ActualMap = 0;
        MapList.Add(FirstMap);
        MapList.Add("Maze");
        MapList.Add("Hist");
        MapList.Add("BigFight");
        MapList.Add(LastMap);

        Debug.Log("Game start");

        /*
        RandomList.Add(FirstMap);
        for (int i = 1; i < RandomList.Count-1; i++)
        {
            string temp = RandomList[i];
            int randomIndex = Random.Range(i, RandomList.Count);
            RandomList[i] = RandomList[randomIndex];
            RandomList[randomIndex] = temp;
        }
        */
    }
    public string NextMap() { 
        if (ActualMap < MapList.Count) { 
            ActualMap++;
        }
        else
        {
            //Go to menu
            ActualMap = MapList.Count - 1;
        }
        Debug.Log("Loading Map : " + MapList[ActualMap]);
        return MapList[ActualMap];
    }
    public string PreviousMap() { 
        if (ActualMap > 0) { 
            ActualMap--;
        }
        else
        {
            //Stay on starting map
            ActualMap = 0;
        }
        Debug.Log("Loading Map : " + MapList[ActualMap]);
        return MapList[ActualMap];
    }
    void Awake()
    {
        //If no instance, instance this one
        if (Instance == null)
        {
            //cross-scene persistence
            DontDestroyOnLoad(gameObject);
            Instance = this;
            InitiateGame();
        }
        //If there is another instance, destroy that one and make sure that the instance is this one
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
