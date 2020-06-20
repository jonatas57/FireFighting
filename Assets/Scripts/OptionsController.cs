using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionsController : MonoBehaviour
{

    private const int INF = 99999;
    public Button[] button_list;
    public int[] stateButton;
    public Text spanQtyRound;
    string[] modeList = new string[]{"Computador", "Humano", "Desativado"};
    private Button increaseButton;
    private Button decreaseButton;
    private Button startButton; 
    public int qtyRound = 1;

    // Start is called before the first frame update

    void Start()
    {
        button_list = GetComponentsInChildren<Button>();
        stateButton = new int[button_list.Length-3];

        for(int i=0; i < button_list.Length-3; i++){
            stateButton[i] = 0;
            Button btn = button_list[i];
            btn.onClick.AddListener(delegate {
                for(int j=0; j < button_list.Length-3; j++){
                    if(btn == button_list[j]) SetCharacter(j);
                }
    	    });
        }

        stateButton[0] = 1;
        button_list[0].GetComponentInChildren<Text>().text = modeList[stateButton[0]];

        increaseButton = button_list[4];
        decreaseButton = button_list[5];
        startButton = button_list[6];

        increaseButton.onClick.AddListener(delegate {
           qtyRound = Mathf.Clamp(qtyRound+1, 1, INF);
           spanQtyRound.text = qtyRound.ToString();
	    });
        decreaseButton.onClick.AddListener(delegate {
	        qtyRound = Mathf.Clamp(qtyRound-1, 1, INF);
            spanQtyRound.text = qtyRound.ToString();
	    });
        startButton.onClick.AddListener(delegate {
            StartGame();
    	});

    }

    public void SetCharacter(int index){
        int qtyH = 0;
        int qtyC = 0;
        for(int i=0; i < stateButton.Length; i++) {
            if(stateButton[i]%modeList.Length == 0) qtyC++;
            if(stateButton[i]%modeList.Length == 1) qtyH++;
        }
        if(qtyH < 2) stateButton[index]++;
        else{
            if(stateButton[index]%modeList.Length == 0) stateButton[index] += 2;
            else stateButton[index]++;
        }
        qtyH = 0;
        qtyC = 0;
        for(int i=0; i < stateButton.Length; i++) {
            if(stateButton[i]%modeList.Length == 0) qtyC++;
            if(stateButton[i]%modeList.Length == 1) qtyH++;
        }
        if(qtyH + qtyC < 2) stateButton[index] = 0;

        int state = stateButton[index];
        button_list[index].GetComponentInChildren<Text>().text = modeList[state%modeList.Length];
    }

    public void StartGame(){
        int[] modeCharacters = new int[4];
        for(int i=0; i < 4; i++){
            modeCharacters[i] = stateButton[i]%modeList.Length;
        }
        GameManager.Instance.SetValues(modeCharacters, qtyRound);
        GameManager.Instance.NewGame();
    }

}
