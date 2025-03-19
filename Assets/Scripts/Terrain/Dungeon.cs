using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dungeon : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    Tile tile;
    [SerializeField]
    int numRooms = 10;
    List<RectInt> rooms = new List<RectInt>();
    void GenerateRoom(RectInt roomCoords)//
    {
        rooms.Add(roomCoords);
        for(int i = 0; i < roomCoords.width; i++)
            for(int j = 0; j < roomCoords.height; j++)
                tilemap.SetTile(new Vector3Int(roomCoords.x+i,roomCoords.y+j),tile);
    }
    public void GenerateDungeon()
    {
        tilemap.ClearAllTiles();
        RectInt tempRoom = new RectInt(-2, -2, 5, 5);
        GenerateRoom(tempRoom);
        int room = 0, dir;
        while(rooms.Count < numRooms)
        {
            //Pick a side
            //Check if there is space for the room's location
            //, if not pick a side again and recheck
            //Set tiles
            //Make a hallway
            bool flag = true;
            while(flag)
            {
                flag = false;
                dir = Random.Range(0, 3);
                room = Random.Range(0, rooms.Count-1);
                switch(dir)
                {
                    case 0:
                    tempRoom.x = rooms[room].x + 7;
                    tempRoom.y = rooms[room].y + Random.Range(-2, 2);
                    break;
                    case 1:
                    tempRoom.x = rooms[room].x + Random.Range(-2, 2);
                    tempRoom.y = rooms[room].y + 7;
                    break;
                    case 2:
                    tempRoom.x = rooms[room].x - 7;
                    tempRoom.y = rooms[room].y + Random.Range(-2, 2);
                    break;
                    case 3:
                    tempRoom.x = rooms[room].x + Random.Range(-2, 2);
                    tempRoom.y = rooms[room].y - 7;
                    break;
                    default:
                    Debug.Log("Wrong direction");
                    break;
                }
                foreach(RectInt roomCoords in rooms)
                {
                    if(roomCoords.Overlaps(tempRoom))
                    {
                        flag = true;
                        Debug.Log("Failed room generation attempt");
                    }
                }
                if(!flag)
                {
                    room++;
                    GenerateRoom(tempRoom);
                    Debug.Log(dir);
                    switch(dir)//Connecting Hallway
                    {
                        case 0:
                        GenerateRoom(new RectInt(tempRoom.x - 5, tempRoom.y+1, 5, 1));
                        break;
                        case 1:
                        GenerateRoom(new RectInt(tempRoom.x+1, tempRoom.y - 5, 1, 5));
                        break;
                        case 2:
                        GenerateRoom(new RectInt(tempRoom.x + 5, tempRoom.y+1, 5, 1));
                        break;
                        case 3:
                        GenerateRoom(new RectInt(tempRoom.x+1, tempRoom.y + 5, 1, 5));
                        break;
                        default:
                        break;
                    }
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
