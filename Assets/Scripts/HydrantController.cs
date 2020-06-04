using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HydrantController : MonoBehaviour {

  public PlayerController owner;
  public float explosionTime;
  public GameObject waterPrefab;
  public int waterLength;

  private void Start() {
    explosionTime = 3;

    GameManager.Instance.board.SetDanger(transform.position, waterLength, 1);
  }

  private void FixedUpdate() {
    explosionTime -= Time.deltaTime;

    if (explosionTime < 0) {
      Explode();
    }
  }

  public void Squirt() {
    GameObject water = Instantiate<GameObject>(waterPrefab);
    water.transform.position = transform.position;
    WaterController wc = water.GetComponent<WaterController>();
    wc.maxLength = waterLength * GameManager.Instance.TILE_SIZE;
  }

  public void SetOwner(PlayerController player) {
    owner = player;
  }

  private void Explode() {
    Squirt();
    owner.IncreaseHydrantQtd();
    Vector2Int gridPos = GameManager.Instance.board.VectorToGridPosition(transform.position);
    GameManager.Instance.board.SetTile(gridPos.x, gridPos.y, TileType.FREE);
    Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Water")) {
      Explode();
    }
  }
}

