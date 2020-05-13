using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIreController : MonoBehaviour
{

    public GameObject bonusPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Water(Clone)")
        {
            if (Random.Range(0, 5) <= 1)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
                GameObject bonusObject = Instantiate<GameObject>(bonusPrefab);
                bonusObject.transform.position = transform.position;
                
            }
            else Destroy(gameObject);
            
        }
    }

}
