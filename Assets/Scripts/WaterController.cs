using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    public float progress;
    public int d_x;
    public int d_y;
    public float speed;


    void Start() {
        progress = 0.0f;
        speed = 2.0f;
        this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void Set_info(int d_x, int d_y){
        this.d_x = d_x;
        this.d_y = d_y;
    }

    private void FixedUpdate() {
        Vector3 right = new Vector3(0.1f, 0.0f, 0.0f);
        Vector3 up = new Vector3(0.0f, 0.1f, 0.0f);
        progress += 2.0f;
        if(progress < 100.0f){
            this.transform.localScale += ((float) d_x) * right * speed;
            this.transform.localScale += ((float) d_y) * up * speed;
        }
    }
}
