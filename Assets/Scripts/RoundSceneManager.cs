using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoundSceneManager : MonoBehaviour
{
    public float time_change_scene;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
      if(GameManager.Instance.id_winner < 0) {
        text.text = "Rodada Empatada";
      }
      else {
        text.text = "O jogador " + (GameManager.Instance.id_winner+1) + " ganhou a rodada";
      }
      time_change_scene = 2;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       time_change_scene -= Time.deltaTime;
        if(time_change_scene < 0){
          int max_score = 0;
          for(int i=0; i < GameManager.Instance.playerScore.Length; i++) {
                max_score = Math.Max(max_score, GameManager.Instance.playerScore[i]);
          }
          if(max_score < GameManager.Instance.qtyRounds) {
            GameManager.Instance.NewRound();
          }
          else{
            GameManager.Instance.GoToEndScene();
          }
        }
    }
}
