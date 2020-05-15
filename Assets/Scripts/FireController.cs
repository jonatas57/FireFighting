using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIreController : MonoBehaviour
{

    public GameObject bonusPrefab;
    public BonusType bonusType;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake() {
        bonusType = BonusType.NONE;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddBonus(BonusType bt) {
        bonusType = bt;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (bonusType != BonusType.NONE)
            {
                GameObject bonusObject = Instantiate<GameObject>(bonusPrefab);
                bonusObject.GetComponent<BonusController>().SetBonusType(bonusType);
                bonusObject.transform.position = transform.position;
            }
            
            Vector2Int pos = GameManager.Instance.VectorToGridPosition(transform.position);
            GameManager.Instance.SetTile(pos, TileType.FREE);
            Destroy(gameObject);
        }
    }

}
