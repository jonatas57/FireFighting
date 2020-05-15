using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {
  
  private Vector3 direction;
  public float speed;
  public int hydrantQtd;
  public int waterLength;
  public GameObject hydrantPrefab;
  private bool pulledByWater;
  private float waterTime;
  public int id_player;

  KeyCode upKey;
  KeyCode downKey;
  KeyCode leftKey;
  KeyCode rightKey;
  KeyCode hydrantKey;

  Rigidbody2D rigidbody;
  
  private void Start() {
    speed = 50;
    direction = Vector3.zero;
    hydrantQtd = 1;
    waterLength = 2;
    pulledByWater = false;
    rigidbody = GetComponent<Rigidbody2D>();
  }

  public void SetButtons(int id_p) {
    id_player = id_p;
    if(id_p == 0){
      upKey = KeyCode.UpArrow;
      downKey = KeyCode.DownArrow;
      leftKey = KeyCode.LeftArrow;
      rightKey = KeyCode.RightArrow;
      hydrantKey = KeyCode.Space;
    }
    else {
      upKey = KeyCode.W;
      downKey = KeyCode.S;
      leftKey = KeyCode.A;
      rightKey = KeyCode.D;
      hydrantKey = KeyCode.X;
    }
  }

  private void FixedUpdate() {
    if (pulledByWater) {
      rigidbody.velocity = direction * GameManager.Instance.waterForce;
      waterTime -= Time.deltaTime;
      if (waterTime < 0) pulledByWater = false;
    }
    else {
      if (Input.GetKey(upKey)) {
        direction = Vector3.up;
      }
      else if (Input.GetKey(downKey)) {
        direction = Vector3.down;
      }
      else if (Input.GetKey(leftKey)) {
        direction = Vector3.left;
      }
      else if (Input.GetKey(rightKey)) {
        direction = Vector3.right;
      }
      else direction = Vector3.zero;

      if (Input.GetKeyDown(hydrantKey)) {
        PlaceHydrant();
      }

      rigidbody.velocity = direction * speed;
    }
  }

  public void IncreaseHydrantQtd(int qtd = 1) {
      hydrantQtd += qtd;
  }


  private void PlaceHydrant() {
    Vector2Int gridPos = GameManager.Instance.VectorToGridPosition(transform.position);
    if (hydrantQtd > 0 && GameManager.Instance.isFree(gridPos.x, gridPos.y)) {
      hydrantQtd--;
      GameObject hydrant = Instantiate<GameObject>(hydrantPrefab);
      GameManager.Instance.SetTile(gridPos.x, gridPos.y, TileType.HYDRANT);
      hydrant.transform.position = GameManager.Instance.GetGridPosition(transform.position) + new Vector3(0, 0, 0.5f);
      hydrant.GetComponent<HydrantController>().SetOwner(this);
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Water") && !pulledByWater) {
      direction = Vector3.zero;
      pulledByWater = true;
      waterTime = 0.5f;
      if (other.GetComponent<WaterController>().isHorizontal()) {
        direction += Vector3.right * (transform.position.x - other.transform.position.x);
      }
      else {
        direction += Vector3.up * (transform.position.y - other.transform.position.y);
      }
      direction = direction.normalized * GameManager.Instance.waterForce;
    }
  }

  private void OnTriggerStay2D(Collider2D other) {
    if (other.CompareTag("Hole") && GameManager.Instance.CheckPosition(transform.position, TileType.HOLE)) {
      Die();
      if(id_player == 0) GameManager.Instance.SetWinner(1);
      else GameManager.Instance.SetWinner(0);
    }
  }

  private void Die() {

    Destroy(gameObject);
  }
}