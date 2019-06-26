using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMiniMapController : NetworkBehaviour { 
    public GameObject[] imagesList;
    public GameObject[] images;// 0-white 1-blue 2-red 
    int x, y, j;
    float camX, camY;

    public List<int> dungeonX = new List<int>();
    public List<int> dungeonY = new List<int>();

    public void Inic() {
        if (imagesList != null) {
            foreach (var item in imagesList) {
                Destroy(item);
            }
        }
        if(GameObject.Find("GameManager").GetComponent<GameManager>().dungeonX.Count == dungeonX.Count) {
            Invoke("Inic", 1);
            return;
        }
 
        dungeonX.Clear();
        dungeonY.Clear();
        
        foreach (var item in GameObject.Find("GameManager").GetComponent<GameManager>().dungeonX) {
            dungeonX.Add(item);
        }
        foreach (var item in GameObject.Find("GameManager").GetComponent<GameManager>().dungeonY) {
            dungeonY.Add(item);

        }

        imagesList = new GameObject[dungeonX.Count];
        x = 0;
        y = 0;
        j = 0;
        imagesList[0] = images[0];

        camX = 0;
        camY = 0;
        RefrehMap();

    }

    void SpawnRoom(GameObject image, int index) {
        imagesList[index] = Instantiate(imagesList[index]);
        imagesList[index].transform.SetParent(transform);
        imagesList[index].transform.localPosition = new Vector2(dungeonX[index] * 11, dungeonY[index] * 6);
        imagesList[index].GetComponent<MiniMapRoomStats>().j = index;
    }

    // Update is called once per frame
    void Update() {
        if (Camera.main.transform.position.x != camX) {
            if (Camera.main.transform.position.x < camX) x--;
            else
            if (Camera.main.transform.position.x > camX) x++;
            camX = Camera.main.transform.position.x;
            RefrehMap();
        }
        else if(Camera.main.transform.position.y != camY) {
            if (Camera.main.transform.position.y < camY) y--;
            else
            if (Camera.main.transform.position.y > camY) y++;
            camY = Camera.main.transform.position.y;
            RefrehMap();
        }

    }

    List<int> SetNeighbor() {
        List<int> RoomId = new List<int>();
        for (int i = 0; i < dungeonX.Count; i++) {
            if (dungeonX[i] == x + 1 && dungeonY[i] == y && imagesList[i] == null) RoomId.Add(i);
            else
            if (dungeonX[i] == x - 1 && dungeonY[i] == y && imagesList[i] == null) RoomId.Add(i);
            else
            if (dungeonX[i] == x && dungeonY[i] == y + 1 && imagesList[i] == null) RoomId.Add(i);
            else
            if (dungeonX[i] == x && dungeonY[i] == y - 1 && imagesList[i] == null) RoomId.Add(i);
        }
        return RoomId;
    }
    void RefrehMap() {
        if (transform.childCount != 0) {
            //replace white
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            imagesList[j] = images[1];
            SpawnRoom(imagesList[j], j);
        }
        //get current index
        for (int i = 0; i < dungeonX.Count; i++) {
            if (dungeonX[i] == x && dungeonY[i] == y) {
                j = i;
                break;
            }
        }
        //if new room discover neighbours
        if (imagesList[j] != images[1]) {
            List<int> RoomId = SetNeighbor();
            //spawn reds if have new
            for (int i = 0; i < RoomId.Count; i++) {
                imagesList[RoomId[i]] = images[2];
                SpawnRoom(imagesList[RoomId[i]], RoomId[i]);
            }
        }
        //delete where moved
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.GetComponent<MiniMapRoomStats>().j == j) {

                Destroy(transform.GetChild(i).gameObject);
                break;
            }
        }

        //spawn white where moved
        imagesList[j] = images[0];
        SpawnRoom(imagesList[j], j);

    }
}
