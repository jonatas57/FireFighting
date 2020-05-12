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

    public void Squirt()
    {
        int c = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Math.Abs(x) + Math.Abs(y) == 1)
                {
                    water[c] = Instantiate<GameObject>(waterPrefab);
                    water[c].transform.position = transform.position;
                    water[c].GetComponent<WaterController>().Set_info(x, y);
                    c++;
                }
            }
        }
    }

    public void SetOwner(PlayerController player)
    {
        owner = player;
    }

    private void Explode()
    {
        owner.IncreaseHydrantQtd();
        Vector2Int gridPos = GameManager.Instance.VectorToGridPosition(transform.position);
        GameManager.Instance.SetTile(gridPos.x, gridPos.y, TileType.FREE);
        for (int i = 0; i < 4; i++) Destroy(water[i]);
        Destroy(gameObject);
    }
}

