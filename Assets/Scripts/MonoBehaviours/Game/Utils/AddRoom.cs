using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;
    private LevelGenerator tplates;
    public Vector2Int size;

    public bool[,] map;

    public void RoomMapping()
    {
        /*
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
        //*/
        /*
        tplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<LevelGenerator>();
        tplates.roomsSpawned.Add(this.gameObject);
        */
        Transform Grounds = GameObject.Find("Grounds").GetComponent<Transform>();
        Transform[] childrenTransforms = new Transform[Grounds.childCount];
        for (int i = 0; i < childrenTransforms.Length; i++) {
            childrenTransforms[i] = Grounds.GetChild(i);
        }


        float minX, minZ, maxX, maxZ;
        minX = childrenTransforms[0].localPosition.x / 4;
        minZ = childrenTransforms[0].localPosition.z / 4;
        maxX = childrenTransforms[0].localPosition.x / 4;
        maxZ = childrenTransforms[0].localPosition.z / 4;
        foreach (Transform T in childrenTransforms)
        {
            if (T.localPosition.x / 4 < minX) { minX = T.localPosition.x / 4; }
            if (T.localPosition.z / 4 < minZ) { minZ = T.localPosition.z / 4; }
            if (T.localPosition.x / 4 > maxX) { maxX = T.localPosition.x / 4; }
            if (T.localPosition.x / 4 > maxZ) { maxZ = T.localPosition.z / 4; }

        }
        size = new Vector2Int((int)maxX - (int)minX + 3, (int)maxZ - (int)minZ + 3);
        map = new bool[size.x, size.y];
        foreach (Transform T in childrenTransforms)
        {
            Vector2Int P = new Vector2Int((int)T.localPosition.x / 4, (int)T.localPosition.z / 4);
            map[P.x, P.y] = true;
        }
        List<Vector2Int> border = new List<Vector2Int>();

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if (map[x, z] == true)
                {
                    
                    if (x > 0 && z > 0 && !map[x - 1, z - 1]) { border.Add(new Vector2Int(x - 1, z - 1)); }
                    if (x > 0 && z < size.y && !map[x - 1, z + 1]) { border.Add(new Vector2Int(x - 1, z + 1)); }
                    if (x < size.x && z > 0 && !map[x + 1, z - 1]) { border.Add(new Vector2Int(x + 1, z - 1)); }
                    if (x < size.x && z < size.y && !map[x + 1, z + 1]) { border.Add(new Vector2Int(x + 1, z + 1)); }
                    if (z < size.y && !map[x, z + 1]) { border.Add(new Vector2Int(x, z + 1)); }
                    if (z > 0 && !map[x, z + 1]) { border.Add(new Vector2Int(x, z - 1)); }
                    if (x < size.x && !map[x + 1, z]) { border.Add(new Vector2Int(x + 1, z)); }
                    if (x > 0 && !map[x - 1, z]) { border.Add(new Vector2Int(x - 1, z)); }
                    
                }
            }
        }
        for (int i = 0; i < border.Count; i++)
        {
            Vector2Int temp = border[i];
            map[temp.x, temp.y] = true;
        }
    }
}
