using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrantController : MonoBehaviour {

  public PlayerController owner;
  public float explosionTime;

  private void Start() {
    explosionTime = 3;
  }

  private void FixedUpdate() {
    explosionTime -= Time.deltaTime;
    if (explosionTime < 0) {
      Explode();
    }
  }

  public void SetOwner(PlayerController player) {
    owner = player;
  }

  private void Explode() {
    owner.IncreaseHydrantQtd();
    Destroy(gameObject);
  }
}

