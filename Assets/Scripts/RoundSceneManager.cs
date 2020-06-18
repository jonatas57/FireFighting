using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoundSceneManager : MonoBehaviour
{
    public Text text;

    void Start() {

      StartCoroutine(_Start());
    }

    private IEnumerator _Start(){
      if(GameManager.Instance.id_winner < 0) text.text = "Rodada Empatada";
      else text.text = "O jogador " + (GameManager.Instance.id_winner+1) + " ganhou a rodada";

      yield return new WaitForSeconds(2.0f);

      int max_score = 0;

      for(int i=0; i < GameManager.Instance.playerScore.Length; i++) {
        max_score = Math.Max(max_score, GameManager.Instance.playerScore[i]);
      }

      if(max_score < GameManager.Instance.qtyRounds) {
        // GameManager.Instance.RenderFade(false);
        // yield return new WaitForSeconds(0.5f);
        GameManager.Instance.NewRound();
      }
      else{
        // GameManager.Instance.RenderFade(false);
        // yield return new WaitForSeconds(0.5f);
        GameManager.Instance.ChangeScene("EndScene");
      }
    }
}
