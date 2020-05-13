using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HydrantController : MonoBehaviour
{

  public PlayerController owner;
  public float explosionTime;
  public GameObject waterPrefab;
  public bool flag_jet;
  private GameObject[] water;

  private void Start()
  {
    explosionTime = 3;
    water = new GameObject[4];
    flag_jet = false;
  }

  private void FixedUpdate()
  {
    explosionTime -= Time.deltaTime;

    if (explosionTime < 1 && !flag_jet)
    {
    Squirt();
    flag_jet = true;
    }


    if (explosionTime < 0)
    {
    Explode();
    }
  }

  public void Squirt() {
    for (int i = 0;i <= 1;i++) {
      water[i] = Instantiate<GameObject>(waterPrefab);
      water[i].transform.position = transform.position;
      WaterController wc = water[i].GetComponent<WaterController>();
      wc.SetDirection(i, i ^ 1);
      wc.SetMaxLength(2 * owner.waterLength + 1);
    }
  }

  public void SetOwner(PlayerController player) {
    owner = player;
  }

  private void Explode() {
    owner.IncreaseHydrantQtd();
    Vector2Int gridPos = GameManager.Instance.VectorToGridPosition(transform.position);
    GameManager.Instance.SetTile(gridPos.x, gridPos.y, TileType.FREE);
    for (int i = 0; i < 2; i++) Destroy(water[i]);
    Destroy(gameObject);
  }
}

