using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
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
                Vector2Int pos = GameManager.Instance.board.VectorToGridPosition(transform.position);
                GameManager.Instance.board.SetTile(pos, TileType.BONUS);
            }
            else {
                Vector2Int pos = GameManager.Instance.board.VectorToGridPosition(transform.position);
                GameManager.Instance.board.SetTile(pos, TileType.FREE);
            }
            Destroy(gameObject);
        }
        else if(other.CompareTag("Hole")){
            Destroy(gameObject);
        }
    }

}
