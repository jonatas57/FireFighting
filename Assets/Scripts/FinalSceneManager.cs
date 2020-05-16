using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSceneManager : MonoBehaviour
{

    public Text content;
    public Button newGame;
    public Button mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        content.text = "Jogador " + (GameManager.Instance.id_winner + 1) + " venceu";
        
        newGame.onClick.AddListener(delegate {
            GameManager.Instance.NewGame();
        });

        mainMenu.onClick.AddListener(delegate {
            GameManager.Instance.GoToMainMenu();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
