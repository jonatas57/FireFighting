using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSceneManager : MonoBehaviour
{

    public Text content;
    public Button restartGame;
    // Start is called before the first frame update
    void Start()
    {
       
        content.text = "Jogador " + (GameManager.Instance.id_winner + 1) + " venceu";
        restartGame.onClick.AddListener(delegate {GameManager.Instance.NewGame();});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
