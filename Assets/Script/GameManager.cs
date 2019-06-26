using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour{

    private int level = 0;
    [SerializeField]
    GameObject startRoom;
    [SerializeField]
    GameObject[] roompoll;
    [SerializeField]
    GameObject endRoom;
    [SerializeField]
    GameObject treasureRoom;

    public GameObject SpawnPoint;

    public SyncListInt dungeonX = new SyncListInt();
    public SyncListInt dungeonY = new SyncListInt();

    List<GameObject> rooms = new List<GameObject>();

    void Start() { 
        SetDungeon();
    } 
    public void SetDungeon() {
        if (isServer) {
            SpawnPoint.transform.position = new Vector2(0, 0);

            dungeonX.Add(0);
            dungeonY.Add(0);

            level++;
            int maxRoomNumber = level + 7;

            rooms.Add(startRoom);

            List<int[]> queue = new List<int[]>();
            queue.Add(new int[] { 0, 0 });

            RoomGenerate(queue, maxRoomNumber - 3, roompoll);
            RoomGenerate(queue, 1, new GameObject[] { treasureRoom });
            queue.RemoveAt(queue.Count - 1);
            RoomGenerate(queue, 1, new GameObject[] { endRoom });
            
            for (int i = 0; i < rooms.Count; i++) {

                rooms[i].transform.position = new Vector2(dungeonX[i] * 21, dungeonY[i] * 12);
                rooms[i] = Instantiate(rooms[i]);

                NetworkServer.Spawn(rooms[i]);

            }
            Setrooms();
        }
        
        Camera.main.transform.position = new Vector3(0, 0,-10);
        GameObject.Find("MiniMap").GetComponent<UIMiniMapController>().Inic();
        NetworkManager.singleton.client.connection.playerControllers[0].gameObject.transform.position = new Vector2(0, 0);
    }
    void DeleteRooms() {
        if (!isServer) return;
        dungeonX.Clear();
        dungeonY.Clear();
        foreach (var item in rooms) {
            NetworkServer.Destroy(item);
        }      
        rooms.Clear();
    }
    public void Setrooms() {
        for (int i = 0; i < rooms.Count; i++) {
            SetExit(dungeonX[i], dungeonY[i], rooms[i]);
        }
    }

    [ClientRpc]
    public void RpcReLoadDungeon() {
        if (isServer) {
            foreach (var item in GameObject.FindGameObjectsWithTag("PlayerBullet")) {
                NetworkServer.Destroy(item);
            }
            foreach (var item in GameObject.FindGameObjectsWithTag("Item")) {
                NetworkServer.Destroy(item);
            }
            foreach (var item in GameObject.FindGameObjectsWithTag("WeaponItem")) {
                NetworkServer.Destroy(item);
            }
            DeleteRooms();
        }
        SetDungeon();

    }
    List<int[]> RoomGenerate(List<int[]> queue,int maxRoomNumber, GameObject[] roompoll) {
        int roomCounter = 0;
        while (roomCounter < maxRoomNumber) {

            int selectedIndex = Random.Range(0, queue.Count);
            int newRoomCounter = Random.Range(1, 4);

            List<int[]> places = new List<int[]>();
            places.Add(new int[] { queue[selectedIndex][0] - 1, queue[selectedIndex][1] });
            places.Add(new int[] { queue[selectedIndex][0], queue[selectedIndex][1] - 1 });
            places.Add(new int[] { queue[selectedIndex][0], queue[selectedIndex][1] + 1 });
            places.Add(new int[] { queue[selectedIndex][0] + 1, queue[selectedIndex][1] });

            while (newRoomCounter > 0 && places.Count > 0 && maxRoomNumber > roomCounter) {
                int newRoomPlace = Random.Range(0, places.Count);
                if (NeighborCount(places[newRoomPlace][0], places[newRoomPlace][1]) == 1) {
                    dungeonX.Add(places[newRoomPlace][0]);
                    dungeonY.Add(places[newRoomPlace][1]);

                    rooms.Add(roompoll[Random.Range(0, roompoll.Length)]);
                    queue.Add(new int[] { places[newRoomPlace][0], places[newRoomPlace][1] });

                    roomCounter++;
                    newRoomCounter--;

                }
                places.RemoveAt(newRoomPlace);
            }
        }
        return queue;
    }
    int NeighborCount(int x, int y) {
        int db = 0;
        for (int i = 0; i < dungeonX.Count; i++) {
            if (dungeonX[i] == (x + 1) && dungeonY[i] == y) db++;
            else
            if (dungeonX[i] == (x - 1) && dungeonY[i] == y) db++;
            else
            if (dungeonX[i] == x && dungeonY[i] == (y + 1)) db++;
            else
            if (dungeonX[i] == x && dungeonY[i] == (y - 1)) db++;
            else
            if (dungeonX[i] == x && dungeonY[i] == y) db++;
        }
        return db;
    }

    void SetExit(int x, int y, GameObject room) {
        for (int i = 0; i < dungeonX.Count; i++) {

            if (dungeonX[i] == x + 1 && dungeonY[i] == y) {
                room.GetComponent<RoomEntered>().RigthIsGate = true;
            }
            else
            if (dungeonX[i] == x - 1 && dungeonY[i] == y) {
                room.GetComponent<RoomEntered>().LeftIsGate = true;
            }
            else
            if (dungeonX[i] == x && dungeonY[i] == y + 1) {
                room.GetComponent<RoomEntered>().UpIsGate = true;
            }
            else
            if (dungeonX[i] == x && dungeonY[i] == y - 1) {
                room.GetComponent<RoomEntered>().DownIsGate = true;
            }
        }
    }

}
