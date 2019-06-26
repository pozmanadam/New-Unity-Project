using UnityEngine;
using System.Collections;

public class TileRandomizer : MonoBehaviour {

    public SpriteRenderer[] floorTiles;
    public SpriteRenderer[] barrierTiles;
    public SpriteRenderer[] obstacleTiles;

    void Start () {
        int[] randomRotate = { 0,90,180,270 };
        Transform tile;

        for (int i = 0; i < transform.childCount; i++) {
            tile = transform.GetChild(i);
            
            switch (tile.tag) {
                case "Floor": {
                        tile.GetComponent<SpriteRenderer>().sprite = floorTiles[Random.Range(0, floorTiles.Length)].sprite;
                        tile.Rotate(new Vector3(0f, 0f, randomRotate[Random.Range(0, randomRotate.Length)]));
                        break;
                    }
                case "Barrier": {
                        tile.GetComponent<SpriteRenderer>().sprite = barrierTiles[Random.Range(0, barrierTiles.Length)].sprite;
                        break;
                    }
                case "Obstacle": {
                        tile.GetComponent<SpriteRenderer>().sprite = obstacleTiles[Random.Range(0, obstacleTiles.Length)].sprite;
                        tile.Rotate(new Vector3(0f, 0f, randomRotate[Random.Range(0, randomRotate.Length)]));
                        break;
                    }
                default: { break; }
            }
        }
	
	}
}
