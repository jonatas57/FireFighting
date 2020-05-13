using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

  public float progress;
  public int d_x;
  public int d_y;
  public float speed;
  public int maxLength;


  void Start() {
    progress = 0.0f;
    speed = 2f;
    transform.localScale = Vector3.one;
  }

  public void SetDirection(int d_x, int d_y){
    this.d_x = d_x;
    this.d_y = d_y;
  }

  public void SetMaxLength(int ml) {
    maxLength = ml;
  }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.name == "Fire(Clone)") Destroy(collision.gameObject);
    }

    private void FixedUpdate() {
    progress += 2.0f;
    if(progress < 100.0f) {
      Vector3 xAxis = Mathf.Clamp(transform.localScale.x + d_x * speed, 0, maxLength) * Vector3.right;
      Vector3 yAxis = Mathf.Clamp(transform.localScale.y + d_y * speed, 0, maxLength) * Vector3.up;
      transform.localScale = xAxis + yAxis;
    }
  }

  public bool isHorizontal() {
    return d_x != 0;
  }
}
