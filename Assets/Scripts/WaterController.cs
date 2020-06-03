using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

  private float time;
  private float elapsedTime;
  private float animLength;
  private float length;
  public float maxLength;
  public float speed;
  private GameObject[] waterEnds;
  public GameObject waterEndPrefab;
  public GameObject waterPrefab;

  private Vector3[] step;
  private int counter;

  private void Start() {
    time = 2;
    elapsedTime = 0;
    animLength = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
    length = 0;
    counter = GameManager.Instance.TILE_SIZE;
    waterEnds = new GameObject[4];
    step = new Vector3[4];
    for (int i = 0;i < 4;i++) {
      waterEnds[i] = Instantiate<GameObject>(waterEndPrefab, transform);
      waterEnds[i].transform.position = transform.position;
      waterEnds[i].transform.rotation = Quaternion.Euler(0, 0, i * 90);
      step[i] = waterEnds[i].transform.rotation * Vector3.right * speed;
    }
  }

  private void FixedUpdate() {
    elapsedTime += Time.deltaTime;

    if (length < maxLength) {
      length += speed * Time.deltaTime;
      for (int i = 0;i < 4;i++) {
        waterEnds[i].transform.localPosition = Vector3.ClampMagnitude(waterEnds[i].transform.localPosition + step[i] * Time.deltaTime, maxLength);
      }

      if (length > counter) {
        for (int i = 0;i < 4;i++) {
          GameObject water = Instantiate<GameObject>(waterPrefab, transform);
          water.transform.position = GameManager.Instance.board.GetGridPosition(waterEnds[i].transform.position);
          // water.transform.position = GameManager.Instance.GetBoard().GetGridPosition(waterEnds[i].transform.position);
          water.transform.rotation = Quaternion.Euler(0, 0, i * 90);
          water.GetComponent<Animator>().Play(0, -1, (elapsedTime - (int)elapsedTime) / animLength);
        }
        counter += GameManager.Instance.TILE_SIZE;
      }
    }

    if (elapsedTime > time) {
      // GameManager.Instance.GetBoard().SetDanger(transform.position, (int)maxLength / GameManager.Instance.TILE_SIZE, -1);
      Destroy(gameObject);
    }
  }

  public Vector3 GetWaterDirection(Vector3 position) {
    return position - transform.position;
  }
  public bool isHorizontal() {
    return true;
  }
}
