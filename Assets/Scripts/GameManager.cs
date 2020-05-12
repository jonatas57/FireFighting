using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  private static GameManager instance = null;
  private Vector3 xAxis;
  private Vector3 yAxis;
  public GameObject playerPrefab;

  public static GameManager Instance {
    get {
      if (instance == null) {
        instance = new GameObject("GM").AddComponent<GameManager>();
      }
      return instance;
    }
  }

  void Awake() {
    if (instance != null) {
      DestroyImmediate(this);
      return;
    }
    instance = this;
  }

  // Start is called before the first frame update
  void Start() {
    for(int i=0; i < 2; i++){
        GameObject playerObject = Instantiate<GameObject>(playerPrefab);
        playerObject.GetComponent<PlayerController>().SetButtons(i);
        Vector3 size = playerObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        xAxis = size.x * Vector3.right;
        yAxis = size.y * Vector3.up;
        playerObject.transform.position = GridToVectorPosition(0, 0);
    }
  }

  // Update is called once per frame
  void Update() {

  }

  public Vector3 GridToVectorPosition(int i, int j) {
    return (-5.5f + i) * xAxis + (5.5f - j) * yAxis;
  }

  public Vector3 GetGridPosition(Vector3 pos) {
    int i = Mathf.RoundToInt(pos.x / xAxis.x + 5.5f);
    int j = Mathf.RoundToInt(5.5f - pos.y / yAxis.y);
    return GridToVectorPosition(i, j);
  }
}
