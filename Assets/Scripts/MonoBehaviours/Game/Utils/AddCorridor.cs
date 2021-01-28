using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCorridor : MonoBehaviour
{

    public Vector2Int size;
    public bool[,] map;

    public void CorridorMapping()
    {
        Transform Grounds = GameObject.Find("Grounds").GetComponent<Transform>();
        Transform[] childrenTransforms = new Transform[Grounds.childCount];
        for (int i = 0; i < childrenTransforms.Length; i++)
        {
            childrenTransforms[i] = Grounds.GetChild(i);
        }

        //Get map size
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
        //Initiate map's size 
        size = new Vector2Int((int)maxX - (int)minX + 3, (int)maxZ - (int)minZ + 3);
        //Initiate room's map
        map = new bool[size.x, size.y];
        //Assign value to the map when ground detected
        foreach (Transform T in childrenTransforms)
        {
            Vector2Int P = new Vector2Int((int)T.localPosition.x / 4, (int)T.localPosition.z / 4);
            map[P.x, P.y] = true;
        }
        //List the position bordering the edge of the room
        List<Vector2Int> border = new List<Vector2Int>();
        //Assign values to border list
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
        //Set to occupied the positions at the border of the room
        for (int i = 0; i < border.Count; i++)
        {
            Vector2Int temp = border[i];
            map[temp.x, temp.y] = true;
        }
        /*
        for (int x = 0; x < size.x; x++)
        {
            string str = "";
            for (int z = 0; z < size.y; z++)
            {
                str += map[x, z] + " ";
            }
            Debug.Log(str);
        }
        //*/
    }
    public void Start()
    {
        CorridorMapping();
    }
}
