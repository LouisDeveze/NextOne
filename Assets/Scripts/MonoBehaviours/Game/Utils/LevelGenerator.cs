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


    public GameObject doorT;
    public GameObject doorB;
    public GameObject doorR;
    public GameObject doorL;


    public GameObject ground;

    //In logical coordinates
    public int mapSize = 10;
    public int sizeOfChunks = 10;
    public int spawningAttempt = 20;

    public GameObject StartingRoom;
    public GameObject FinishRoom;

    private bool[,] Map;

    private Chunk[,] mapOfChunks;
    private Point[,] mapOfPoints;
    private List<List<Point>> roadList;
    public List<Door> doors;
    public List<Door> doors_toLink;
    private readonly int corridor_width = 1;

    public GameObject[] roomsPrefabs;


    public List<GameObject> roomsSpawned;
    public List<GameObject> corridorsSpawned;

    public class Chunk
    {
        public int coord_x, coord_z, size;
        public int real_x, real_z;
        public bool isTaken;

        public void Book()
        {
            isTaken = true;
        }
        public void Free()
        {
            isTaken = false;
        }
        public Chunk(int x, int z, int x_r, int z_r, int Size, bool IsTaken)
        {
            coord_x = x;
            coord_z = z;
            real_x = x_r;
            real_z = z_r;
            size = Size;
            isTaken = IsTaken;
        }
    }
    public class Door
    {
        public int coord_x, coord_z, ID_room, ID_Door, ID_nextDoor;
        public Point gate;
        public Door(int x, int z)
        {
            coord_x = x;
            coord_z = z;
        }
        public Door(int x, int z, int IDroom, int ID)
        {
            coord_x = x;
            coord_z = z;
            ID_Door = ID;
            ID_room = IDroom;
        }
        public void SetNextDoor(int ID_room)
        {
            ID_nextDoor = ID_room;
        }
        public Door Clone()
        {
            Door d = new Door(coord_x, coord_z, ID_room, ID_Door);
            d.ID_nextDoor = ID_nextDoor;
            d.gate = gate.Clone();
            return d;
        }
    }
    public class Point
    {
        //x, z in chunk +/- coordinates
        public int x, z, real_x, real_z;
        public bool visited;
        public List<Point> neighbours;
        public Point prev;
        public Point(int coord_x, int coord_z)
        {
            x = coord_x;
            z = coord_z;
            visited = false;
            neighbours = new List<Point>();
        }
        public void MarkPoint() { visited = true; }
        public void FreePoint() { visited = false; }
        public Point Clone()
        {
            Point p = new Point(x, z);
            p.real_x = real_x;
            p.real_z = real_z;
            p.visited = visited;
            for (int i = 0; i < neighbours.Count; i++) p.neighbours.Add(neighbours[i]);
            return p;
        }
        public void SetCoordinates(int x, int z)
        {
            real_x = x;
            real_z = z;
        }
    }
    
    /// <summary>
    /// Calculate the number of chunk needed for placing a room.
    /// Coordinates in real space
    /// </summary>
    /// <param name="size_i"></param>
    /// <returns></returns>
    public int ChunkNeeded_i(int size_i)
    {
        //In real coordinates /!\
        if (size_i <= (sizeOfChunks * 4))
        {
            //Debug.Log(sizeOfChunks*4 + " room size : " + size_i + " " + "size = 1");
            return 1;
        }
        else
        {
            float size = (float)size_i / (float)(sizeOfChunks * 4); // /!\ Cast needed, if not -> big bug for the chunk
            int r = (int)size;
            //Debug.Log(size + " vs  " + r + " -> " + size_i + "<" + sizeOfChunks*4);
            if ( (float)(size - (int)size) > 0) return r+1;
            else
            {
                return r;
            }
        }
    }

    /// <summary>
    /// Check if the coordinates are good for placing a room
    /// </summary>
    /// <param name="position"></param>
    /// <param name="nb_x"></param>
    /// <param name="nb_z"></param>
    /// <returns></returns>
    private bool CheckPlacement(Vector3 position, int nb_x, int nb_z)
    {
        for (int x = 0; x < nb_x; x++) for (int z = 0; z < nb_z; z++) if (mapOfChunks[(int)position.x + x, (int)position.z + z].isTaken) return false;
        return true;
    }

    /// <summary>
    /// Try to place any room given in argument
    /// </summary>
    /// <param name="room"></param>
    public void RoomPlacement(GameObject room)
    {
        Transform Grounds = room.GetComponent<Transform>().Find("Grounds");
        Transform[] childrenTransforms = new Transform[Grounds.childCount];
        for (int i = 0; i < childrenTransforms.Length; i++)
        {
            childrenTransforms[i] = Grounds.GetChild(i);
        }

        //Initiate minums and maximus in real coordinates /!\
        float minX, minZ, maxX, maxZ;
        minX = childrenTransforms[0].localPosition.x;
        minZ = childrenTransforms[0].localPosition.z;
        maxX = childrenTransforms[0].localPosition.x;
        maxZ = childrenTransforms[0].localPosition.z;

        //Get min & max in real position  /!\
        foreach (Transform T in childrenTransforms)
        {
            if (T.localPosition.x < minX) { minX = T.localPosition.x; }
            if (T.localPosition.z < minZ) { minZ = T.localPosition.z; }
            if (T.localPosition.x > maxX) { maxX = T.localPosition.x; }
            if (T.localPosition.z > maxZ) { maxZ = T.localPosition.z; }
        }

        int width_room = (int)(maxX - minX);
        int length_room = (int)(maxZ - minZ);
        //Debug.Log(room.name + " max, min x: " + maxX + ", " + minX + " z: " + maxZ + ", " + minZ + " WxL : " + width_room + "x" + length_room);

        //Get coordinates of the middle of the room according to the doors
        List<Door> listDoors = new List<Door>();
        Transform D = room.GetComponent<Transform>().Find("Doors");
        Transform[] childrenTransformsDoor = new Transform[D.childCount];
        for (int j = 0; j < childrenTransformsDoor.Length; j++) childrenTransformsDoor[j] = D.GetChild(j);
        foreach (Transform T in childrenTransformsDoor) if ((int)(T.localPosition.y + room.transform.position.y) == 0) listDoors.Add(new Door((int)(T.localPosition.x), (int)(T.localPosition.z)));


        //Determine how much chunk needed in x and z in
        int nb_chunk_x = ChunkNeeded_i(width_room);
        int nb_chunk_z = ChunkNeeded_i(length_room);
        //Debug.Log(room.name + " " + nb_chunk_x + "x" + nb_chunk_z + " chunks");

        //If there is a door in x or z
        bool xDoor = false;
        bool zDoor = false;
        int x_Door = 0;
        int z_Door = 0;
        int delta = 10;
        for (int i = 0; i < listDoors.Count; i++)
        {
            if (listDoors[i].coord_x < delta || listDoors[i].coord_x > maxX - delta)
            {
                zDoor = true;  //If we have a door at the extrum of Z
                z_Door = listDoors[i].coord_z + 2;
            }             
            if (listDoors[i].coord_z < delta || listDoors[i].coord_z > maxZ - delta)
            {
                xDoor = true;  //If we have a door at the extrum of X --> offset of 2 so -2
                x_Door = listDoors[i].coord_x + 2;
            }
        }
        if (xDoor) 
        {
            nb_chunk_x += 1;
            //Debug.Log("+1 chunk in x");
        }
        if (zDoor)
        {
            nb_chunk_z += 1;
            //Debug.Log("+1 chunk in z");
        }

        //Debug.Log(room.name + " " + nb_chunk_x + " " + nb_chunk_z);

        //Search free chunks 
        int increment = 0;
        Vector3 myPosition;
        bool goodPosition;
        do
        {
            increment++;
            myPosition = new Vector3(Random.Range(0, mapSize - nb_chunk_x), 0, Random.Range(0, mapSize - nb_chunk_z));  //In chunk corrdinates
            goodPosition = CheckPlacement(myPosition, nb_chunk_x, nb_chunk_z);                                          //In chunk corrdinates
            //if (!goodPosition) Debug.Log("Collision");
        } while (increment < spawningAttempt && !goodPosition);

        //If all chunks free, book them and place the room
        if (goodPosition == true)
        {
            //Mark the chunks used
            for (int x = 0; x < nb_chunk_x; x++) for(int z = 0; z < nb_chunk_z; z++) mapOfChunks[(int)myPosition.x + x, (int)myPosition.z + z].Book();
            
            //Calcul real coordinates of the room
            //Calcul the width and height of the group of chunks in real coordinates
            int widht = (sizeOfChunks * 4 * nb_chunk_x) + (corridor_width * 4 * (nb_chunk_x - 1));
            int hight = (sizeOfChunks * 4 * nb_chunk_z) + (corridor_width * 4 * (nb_chunk_z - 1));

            //Calcul the positive offset to spawn the room in the middle of the chunks (offset de la room : 4)
            int offset_x, offset_z;
            if (xDoor)
            {
                offset_x = (sizeOfChunks * 4) - x_Door + 4;
            } else
            {
                offset_x = ((widht - width_room) / 2);
            }
            if (zDoor)
            {
                offset_z = (sizeOfChunks * 4) - z_Door + 4;
            } else
            {
                offset_z = ((hight - length_room) / 2) + 4;
            }            

            //Calcul the real position of the initial chunk (origin)

            //Debug.Log(myPosition.x + " = " + nb_chunk_x + " x 4 x " + sizeOfChunck + " + " + offset_x);
            //Debug.Log(myPosition.x + " " + myPosition.z + " W x H : " + widht + "x" + hight + " size : " + (maxX - minX)+ "x" + (maxZ - minZ));
            Chunk temp = mapOfChunks[(int)myPosition.x, (int)myPosition.z];
            //change myPosition in real coordinates
            myPosition.x = temp.real_x;
            myPosition.z = temp.real_z;

            //Stock real coordinates
            Vector3 myRealPosition = new Vector3(myPosition.x + offset_x, 0, myPosition.z + offset_z);
            //Instantiate the room
            AddRoom newRoom = GameObject.Instantiate(room, this.transform).GetComponent<AddRoom>();
            newRoom.transform.position = myRealPosition;
            roomsSpawned.Add(newRoom.gameObject);
            //Debug.Log(newRoom.name + " " + (maxX-minX) + " x " + (maxZ-minZ) + " position : [" + myRealPosition.x + ", " + myRealPosition.z + "] " + nb_chunk_x + "x" + nb_chunk_z + " chunks") ;            
            //Debug.Log("Position of first chunk = [" + temp.real_x + ", " + temp.real_z + "] room offset = " + offset_x + ":" + offset_z);  


            //Mark the points under the room
            for (int x = 0; x < Mathf.Sqrt(mapOfPoints.Length); x++)
            {
                for (int z = 0; z < Mathf.Sqrt(mapOfPoints.Length); z++)
                {
                    if (mapOfPoints[x, z].real_x > myRealPosition.x && mapOfPoints[x, z].real_x < myRealPosition.x + width_room)
                    {
                        if (mapOfPoints[x, z].real_z > myRealPosition.z && mapOfPoints[x, z].real_z < myRealPosition.z + length_room)
                        {
                            mapOfPoints[x, z].MarkPoint();
                            //Debug.Log("x between : " + myRealPosition.x + " & " + (myRealPosition.x + width_room) + " z between : " + myRealPosition.z + " & " + (myRealPosition.x + length_room) );
                            //Debug.Log("point deleted " + x + ", " + z + " : (" + mapOfPoints[x, z].real_x + ", " + mapOfPoints[x, z].real_z + ")");
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Get every door of the map
    /// </summary>
    public void GetDoors()
    {
        //Initialize List of doors
        doors = new List<Door>();
        doors_toLink = new List<Door>();

        //Get all doors of all rooms that are on the ground
        for (int i = 0; i < roomsSpawned.Count; i++)
        {
            //  i = ID_room
            Transform D = roomsSpawned[i].GetComponent<Transform>().Find("Doors");
            Transform[] childrenTransforms = new Transform[D.childCount];
            for (int j = 0; j < childrenTransforms.Length; j++) childrenTransforms[j] = D.GetChild(j);
            foreach (Transform T in childrenTransforms) 
                if ((int)(T.localPosition.y + roomsSpawned[i].transform.position.y) == 0) 
                    doors.Add(new Door((int)(T.localPosition.x + roomsSpawned[i].transform.position.x), (int)(T.localPosition.z + roomsSpawned[i].transform.position.z), i, doors.Count));
        }
        //Get the nearest door on the map that doesn't belong to the same room
        for(int i = 0; i < doors.Count; i++)
        {
            int delta = sizeOfChunks * mapSize * 4;
            for (int j = 0; j < doors.Count; j++)
            {
                if(doors[i].ID_room != doors[j].ID_room)
                {
                    // dist = ( (x2-x1)^2 + (z2-z1)^2 )^0.5
                    int dist = (int)Mathf.Sqrt( Mathf.Pow(doors[j].coord_x - doors[i].coord_x, 2) + Mathf.Pow(doors[j].coord_z - doors[i].coord_z, 2));
                    if(dist < delta)
                    {
                        delta = dist;
                        doors[i].SetNextDoor(doors[j].ID_Door);
                    }
                }
            }
        }
        //Get the nearest point
        for(int i = 0; i < doors.Count; i++)
        {
            int delta = sizeOfChunks * mapSize * 4;
            for (int x = 0; x < Mathf.Sqrt(mapOfPoints.Length); x++)
            {
                for (int z = 0; z < Mathf.Sqrt(mapOfPoints.Length); z++)
                {
                    if(!mapOfPoints[x, z].visited)
                    {
                        int dist = (int)Mathf.Sqrt(Mathf.Pow(doors[i].coord_x - mapOfPoints[x, z].real_x, 2) + Mathf.Pow(doors[i].coord_z - mapOfPoints[x, z].real_z, 2));
                        if (dist < delta)
                        {
                            delta = dist;
                            doors[i].gate = mapOfPoints[x, z].Clone();
                        }
                    }                    
                }
            }
        }

        //for (int i=0; i<doors.Count;i++) Debug.Log("Door n°" + doors[i].ID_Door + " : (" + doors[i].coord_x + ", " + doors[i].coord_z + ") from room " + doors[i].ID_room + " Next door ID : " + doors[i].ID_nextDoor + " at (" + doors[doors[i].ID_nextDoor].coord_x + ", " + doors[doors[i].ID_nextDoor].coord_z + ") Point of exit : " + doors[i].gate.x + ", " + doors[i].gate.z + " (" + doors[i].gate.real_x + ", " + doors[i].gate.real_z + ")");

    }

    /// <summary>
    /// Fuck ça marche pas
    /// </summary>
    public void GetDoorToLink()
    {
        doors_toLink = new List<Door>();
        for(int i = 0; i < doors.Count; i++)
        {
            bool inList = false;
            for (int j = 0; j < doors.Count; j++) if (doors[i].ID_nextDoor == doors_toLink[j].ID_Door) inList = true;
            if (!inList) doors_toLink.Add(doors[i]);
        }
    }

    public void SetPoints()
    {
        mapOfPoints = new Point[mapSize + 1, mapSize + 1];
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                mapOfPoints[x, z] = new Point(x, z);
                mapOfPoints[x, z].SetCoordinates(mapOfChunks[x, z].real_x - (4 * corridor_width), mapOfChunks[x, z].real_z - (4 * corridor_width));   //Set bottom left corner
            }
        }
        for(int i = 0; i < mapSize; i++) //Set right row
        {
            int x = mapOfChunks[mapSize - 1, 0].real_x + (sizeOfChunks * 4);
            int z = mapOfChunks[0, mapSize - 1].real_z + (sizeOfChunks * 4);
            mapOfPoints[mapSize, i] = new Point(mapSize, i);
            mapOfPoints[i, mapSize] = new Point(i, mapSize);
            mapOfPoints[mapSize, i].SetCoordinates(x, mapOfChunks[mapSize - 1, i].real_z - 4);      // Right colum  -> x fixe
            mapOfPoints[i, mapSize].SetCoordinates(mapOfChunks[i, mapSize - 1].real_x - 4, z);      // Top row      -> z fixe
        }
        mapOfPoints[mapSize, mapSize] = new Point(mapSize, mapSize);    // Corner top right
        mapOfPoints[mapSize, mapSize].SetCoordinates(mapOfChunks[mapSize-1, mapSize-1].real_x + (sizeOfChunks * 4), mapOfChunks[mapSize-1, mapSize-1].real_z + (sizeOfChunks * 4));

        
    }
    /// <summary>
    /// BFS function : search for the shortest path between 2 points on the grid
    /// </summary>
    /// <param name="mapPoints"></param>
    /// <param name="entry"></param>
    /// <param name="exit"></param>
    /// <returns></returns>
    public List<Point> BFS(Point[,] map, Point entry, Point exit)
    {
        Queue<Point> queue = new Queue<Point>();
        List<Point> road = new List<Point>();

        queue.Enqueue(map[entry.x, entry.z]);
        map[entry.x, entry.z].MarkPoint();

        while (queue.Count > 0)
        {
            Point temp = queue.Dequeue();
            if (temp.x == exit.x && temp.z == exit.z)
            {
                break;
            }
            for (int i = 0; i < temp.neighbours.Count; i++)
            {
                if (!map[temp.neighbours[i].x, temp.neighbours[i].z].visited)
                {
                    int x = map[temp.x, temp.z].neighbours[i].x;
                    int z = map[temp.x, temp.z].neighbours[i].z;
                    queue.Enqueue(map[x, z]);
                    map[x, z].MarkPoint();

                    map[temp.neighbours[i].x, temp.neighbours[i].z].prev = map[temp.x, temp.z];
                }
            }
        }
        Point current = map[exit.x, exit.z];
        while (current.x != entry.x || current.z != entry.z)
        {
            road.Insert(0, current);
            current = current.prev;
            //if (current.x == entry.x && current.z == entry.z) Debug.Log("entry found");
        }
        road.Insert(0, current);
        //road.Insert(0, map[entry.x, entry.z]);

        return road;
    }
    
    /// <summary>
    /// Fill the gap between the doors and their nearest point
    /// </summary>
    /// <param name="map"></param>
    public void FillGap(bool[,] map)
    {
        for(int i = 0; i < doors.Count; i++)
        {
            //Coordinates of the door + 2
            int d_x = ((doors[i].coord_x - 2) / 4) + 1;
            int d_z = ((doors[i].coord_z - 2) / 4) + 1;
            //Cordinates of the point
            int p_x = ((mapOfPoints[doors[i].gate.x, doors[i].gate.z].real_x) / 4) + 1;
            int p_z = ((mapOfPoints[doors[i].gate.x, doors[i].gate.z].real_z) / 4) + 1;
            // x side
            if (d_x < p_x) for (int j = d_x; j < p_x; j++) map[j + 1, d_z] = true;
            if (d_x > p_x) for (int j = p_x; j < d_x; j++) map[j + 1, d_z] = true;
            // z side
            if (d_z < p_z) for (int j = d_z; j < p_z; j++) map[d_x, j + 1] = true;
            if (d_z > p_z) for (int j = p_z; j < d_z; j++) map[d_x, j + 1] = true;
        }
    }

    /// <summary>
    /// Get the type of cooridor to spawn
    /// </summary>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int GetTypeCor(bool[,] map, int x, int z)
    {
        /* 0 => width
         * 1 => hight
         * 2 => BL
         * 3 => BR
         * 4 => TL
         * 5 => TR
         * 6 => BLR
         * 7 => TLR
         * 8 => BTL
         * 9 => BTR
         * 10=>ALL
         * 11 door T
         * 12 door B
         * 13 door L
         * 14 door R
         */
        int type = 0;
        int size = (int)Mathf.Sqrt(map.Length);

        if (x > 0) if (map[x - 1, z])           type = 14;     //Door right
        if (x < size - 1) if (map[x + 1, z])    type = 13;     //Door left
        if (z > 0) if (map[x, z - 1])           type = 11;     //Door top
        if (z < size - 1) if (map[x, z + 1])    type = 12;     //Door bottom

        if (x > 0 && x < size - 1)    if (map[x - 1, z] && map[x + 1, z]) type = 0;     //LR
        if (z < size - 1 && z > 0)    if (map[x, z - 1] && map[x, z + 1]) type = 1;     //BT

        if (x > 0 && z > 0)         if (map[x - 1, z] && map[x, z - 1]) type = 2;   //BL
        if (x < size - 1 && z > 0)  if (map[x + 1, z] && map[x, z - 1]) type = 3;   //BR

        if (x > 0 && z < size - 1)         if (map[x - 1, z] && map[x, z + 1]) type = 4;   //TL
        if (x < size - 1 && z < size - 1)  if (map[x + 1, z] && map[x, z + 1]) type = 5;   //TR

        if (x > 0 && x < size - 1 && z > 0)         if (map[x - 1, z] && map[x + 1, z] && map[x, z - 1]) type = 6;   //BLR
        if (x > 0 && x < size - 1 && z < size - 1)  if (map[x - 1, z] && map[x + 1, z] && map[x, z + 1]) type = 7;   //TLR
        if (x > 0 && z < size - 1 && z > 0)         if (map[x - 1, z] && map[x, z - 1] && map[x, z + 1]) type = 8;   //BTL
        if (x < size - 1 && z > 0 && z < size - 1)  if (map[x + 1, z] && map[x, z - 1] && map[x, z + 1]) type = 9;   //BTR

        if (x > 0 && x < size - 1 && z > 0 && z < size - 1) if (map[x - 1, z] && map[x + 1, z] && map[x, z - 1] && map[x, z + 1]) type = 10;   //ALL

        return type;
    }

    /// <summary>
    /// Mark the route given in parameters
    /// Set the marked points to true
    /// </summary>
    /// <param name="map"></param>
    /// <param name="route"></param>
    public void MarkRoute(bool[,] map, List<Point> route)
    {
        if(route.Count > 1)
        {
            for (int i = 1; i < route.Count; i++)
            {
                
                //Check that the distance separating the 2 points is 1 or less
                //Meaning that the 2 points are neighbours
                bool isNeighbour = true;
                if (Mathf.Sqrt(Mathf.Pow(route[i].x - route[i - 1].x, 2)) > 1 || Mathf.Sqrt(Mathf.Pow(route[i].z - route[i - 1].z, 2)) > 1) isNeighbour = false;
                if (isNeighbour)
                { //*/
                    //Debug.Log( "points to link : (" + route[i].x + "," + route[i].z + ") -> (" + route[i - 1].x + "," + route[i - 1].z + ")");
                    if (route[i].x > route[i - 1].x) for (int j = route[i - 1].x * (sizeOfChunks + corridor_width); j <= route[i].x * (sizeOfChunks + corridor_width); j++) map[j, (route[i].z * (sizeOfChunks + corridor_width))] = true;
                    if (route[i].x < route[i - 1].x) for (int j = route[i].x * (sizeOfChunks + corridor_width); j <= route[i - 1].x * (sizeOfChunks + corridor_width); j++) map[j, (route[i].z * (sizeOfChunks + corridor_width))] = true;
                    if (route[i].z > route[i - 1].z) for (int j = route[i - 1].z * (sizeOfChunks + corridor_width); j <= route[i].z * (sizeOfChunks + corridor_width); j++) map[(route[i].x * (sizeOfChunks + corridor_width)), j] = true;
                    if (route[i].z < route[i - 1].z) for (int j = route[i].z * (sizeOfChunks + corridor_width); j <= route[i - 1].z * (sizeOfChunks + corridor_width); j++) map[(route[i].x * (sizeOfChunks + corridor_width)), j] = true;
                }                
            }
        }        
    }

    /// <summary>
    /// Spawn corridors on marked points
    /// </summary>
    public void SpawnCorridor()
    {
        // Mark the map
        int size = (sizeOfChunks * mapSize) + (corridor_width * mapSize) + 1; // size = 111 -> in logical coordinates -> x4 - 4 for real cordinates
        bool[,] map = new bool[size, size];
        FillGap(map);
        for (int i = 0; i < roadList.Count; i++) MarkRoute(map, roadList[i]);
        // Spawn prefabs of cooridors
        int y = 0;
        for(int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                if (map[x, z])
                {
                    Vector3 position = new Vector3((x * 4) - 4, y, (z * 4) - 4);
                    int type = GetTypeCor(map, x, z);
                    /* 0 => width LR
                     * 1 => hight
                     * 2 => BL
                     * 3 => BR
                     * 4 => TL
                     * 5 => TR
                     * 6 => BLR
                     * 7 => TLR
                     * 8 => BTL
                     * 9 => BTR
                     * 10=>ALL
                     * 11 door T
                     * 12 door B
                     * 13 door L
                     * 14 door R
                     */
                    switch (type)
                    {
                        case 0:
                            Instantiate(corridorLR, position, Quaternion.identity);
                            break;
                        case 1:
                            Instantiate(corridorBT, position, Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(corridorBL, position, Quaternion.identity);
                            break;
                        case 3:
                            Instantiate(corridorBR, position, Quaternion.identity);
                            break;
                        case 4:
                            Instantiate(corridorTL, position, Quaternion.identity);
                            break;
                        case 5:
                            Instantiate(corridorTR, position, Quaternion.identity);
                            break;
                        case 6:
                            Instantiate(corridorBLR, position, Quaternion.identity);
                            break;
                        case 7:
                            Instantiate(corridorTLR, position, Quaternion.identity);
                            break;
                        case 8:
                            Instantiate(corridorBTL, position, Quaternion.identity);
                            break;
                        case 9:
                            Instantiate(corridorBTR, position, Quaternion.identity);
                            break;
                        case 10:
                            Instantiate(corridorAll, position, Quaternion.identity);
                            break;
                        case 11:
                            Instantiate(doorT, position, Quaternion.identity);
                            break;
                        case 12:
                            Instantiate(doorB, position, Quaternion.identity);
                            break;
                        case 13:
                            Instantiate(doorL, position, Quaternion.identity);
                            break;
                        case 14:
                            Instantiate(doorR, position, Quaternion.identity);
                            break;
                    }
                }
            }
        }

    }

    /// <summary>
    /// Spawn points to show the grid, making the chunks appear
    /// </summary>
    public void SpawnPoints()
    {
        int y = 8;
        int size = (int)Mathf.Sqrt(mapOfPoints.Length);
        for(int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                if (!mapOfPoints[x, z].visited) Instantiate(ground, new Vector3(mapOfPoints[x, z].real_x + 4, y, mapOfPoints[x, z].real_z + 4), Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// initialize the map and generate it
    /// </summary>
    public void Mapping()
    {
        //In logical space

        //Initialize the map
        mapOfChunks = new Chunk[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                //Set corrdinates in logic and real space + Instantiate map of chunks
                //sizeOfChunck, corridor_width in logical coordinates : 1 Real = 4 Logical
                int real_x = (i * sizeOfChunks * 4) + (i * corridor_width * 4);
                int real_z = (j * sizeOfChunks * 4) + (j * corridor_width * 4);
                mapOfChunks[i, j] = new Chunk(i, j, real_x, real_z, sizeOfChunks, false); //Can be verified with the spawngrid() function
            }
        }

        //Initialize the points
        SetPoints();

        //If we want to place specific rooms, do it here before the random generation
        //Place rooms randomly and mark points under the rooms as visited
        RoomPlacement(StartingRoom);
        RoomPlacement(FinishRoom);
        for (int i = 0; i < spawningAttempt; i++) RoomPlacement(roomsPrefabs[Random.Range(0, roomsPrefabs.Length)]);

        //Get doors list
        GetDoors();

        //Add the neighbours of each points when world generated -> for BFS
        int size = (int)Mathf.Sqrt(mapOfPoints.Length);
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                if(!mapOfPoints[x, z].visited)
                {
                    if (x < size - 1 && !mapOfPoints[x + 1, z].visited) mapOfPoints[x, z].neighbours.Add(mapOfPoints[x + 1, z]);
                    if (z < size - 1 && !mapOfPoints[x, z + 1].visited) mapOfPoints[x, z].neighbours.Add(mapOfPoints[x, z + 1]);
                    if (x > 0 && !mapOfPoints[x - 1, z].visited) mapOfPoints[x, z].neighbours.Add(mapOfPoints[x - 1, z]);
                    if (z > 0 && !mapOfPoints[x, z - 1].visited) mapOfPoints[x, z].neighbours.Add(mapOfPoints[x, z - 1]);
                }                
            }
        }
        //Print all neighbours of all points
        //for (int x = 0; x < size; x++) for (int z = 0; z < size; z++) for (int i = 0; i < mapOfPoints[x, z].neighbours.Count; i++) Debug.Log("Point (" + mapOfPoints[x, z].x + ", " + mapOfPoints[x, z].z + ") neighbour " + i + " = (" + mapOfPoints[x, z].neighbours[i].x + ", " + mapOfPoints[x, z].neighbours[i].z + ")");

        //Call BFS for each door        
        roadList = new List<List<Point>>();
        //GetDoorToLink();
        for (int i = 0; i < doors.Count; i++)
        {
            //List<Point> t = new List<Point>();
            Point entry = new Point(doors[i].gate.x, doors[i].gate.z);
            Point exit = new Point(0,0) ;
            Point[,] map = new Point[mapSize + 1, mapSize + 1];
            //Copying mapOfPoints into map
            for (int x = 0; x < mapSize + 1; x++) for (int z = 0; z < mapSize + 1; z++) map[x, z] = mapOfPoints[x, z].Clone();

            //Search and copy the exit door into exit
            exit.x = doors[doors[i].ID_nextDoor].gate.x;
            exit.z = doors[doors[i].ID_nextDoor].gate.z;
            List<Point> t = BFS(map, entry, exit);
            roadList.Add(t);
            /*
            if (t.Count > 0)
            {
                string str = i + " BFS entry point (" + entry.x + "," + entry.z + ") exit (" + exit.x + "," + exit.z + ") : ";
                for (int j = 0; j < t.Count; j++)
                {
                    str += "(" + t[j].x + "," + t[j].z + ") ";
                }
                Debug.Log(str);
            } //*/
        }
    }
    private void Start()
    {
        int random = Random.Range(0, 999999999);
        Random.seed = random;
        Debug.Log(random);

        //Generate the world
        Mapping();
        SpawnCorridor();

        //Optionnal;
        //SpawnPoints();


        Debug.Log("All Clear");
        //Play
    }

}
