using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  
  private Vector3 direction;
  public float speed;
  public int hydrantQtd;
  public GameObject hydrantPrefab;

  KeyCode upKey;
  KeyCode downKey;
  KeyCode leftKey;
  KeyCode rightKey;
  KeyCode hydrantKey;

  private void Start() {
    upKey = KeyCode.UpArrow;
    downKey = KeyCode.DownArrow;
    leftKey = KeyCode.LeftArrow;
    rightKey = KeyCode.RightArrow;
    hydrantKey = KeyCode.Space;

    speed = 0.1f;
    direction = Vector3.zero;
    hydrantQtd = 1;
  }

  private void FixedUpdate() {
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

    transform.position += direction * speed;
  }

  public void IncreaseHydrantQtd(int qtd = 1) {
    hydrantQtd += qtd;
  }

  private void PlaceHydrant() {
    if (hydrantQtd > 0) {
      hydrantQtd--;
      GameObject hydrant = Instantiate<GameObject>(hydrantPrefab);
      hydrant.transform.position = GameManager.Instance.GetGridPosition(transform.position) + new Vector3(0, 0, 0.5f);
      hydrant.GetComponent<HydrantController>().SetOwner(this);
    }
  }
}