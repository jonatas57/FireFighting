using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  
  private Vector3 direction;
  public float speed;

  KeyCode upKey;
  KeyCode downKey;
  KeyCode leftKey;
  KeyCode rightKey;

  private void Start() {
    upKey = KeyCode.UpArrow;
    downKey = KeyCode.DownArrow;
    leftKey = KeyCode.LeftArrow;
    rightKey = KeyCode.RightArrow;

    speed = 0.1f;
    direction = Vector3.zero;
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

    transform.position += direction * speed;
  }
}