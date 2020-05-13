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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (Random.Range(0, 5) <= 1)
            {
                GameObject bonusObject = Instantiate<GameObject>(bonusPrefab);
                bonusObject.transform.position = transform.position;
            }
            else Destroy(gameObject);
            
            Vector2Int pos = GameManager.Instance.VectorToGridPosition(transform.position);
            GameManager.Instance.SetTile(pos, TileType.FREE);
            Destroy(gameObject);
        }
    }

}
