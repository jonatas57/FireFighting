using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

  private static GameManager instance = null;

  public int TILE_SIZE = 21;
  public GameObject playerPrefab;
  public GameObject holePrefab;
  public GameObject firePrefab;
  public GameObject bonusPrefab;

  public Board board;
  public int boardSize = 12;

  public float maxWaterTime;
  public float waterForce;
  public int id_winner;


  public int numberPlayers = 4;
  public Vector2Int[] defaultPositions = new Vector2Int[] {
    new Vector2Int(1, 1),
    new Vector2Int(12, 12),
    new Vector2Int(12, 1),
    new Vector2Int(1, 12)
  };
  public GameObject[] players;
  public int[] modeCharacters;
  public int qtyRounds;

  public HashSet<int> idPlayersAlive;
  public float timeEnd;
  private bool flag;
  public int[] playerScore;

  

  public static GameManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new GameObject("GM").AddComponent<GameManager>();
      }
      return instance;
    }
  }

  void Awake()
  {
    if (instance != null)
    {
      DestroyImmediate(this);
      return;
    }
    instance = this;
    DontDestroyOnLoad(this);
  }

  public void NewGame() {
    NewRound();
    flag = false;
    playerScore = new int[4];
    for(int i=0; i < modeCharacters.Length; i++){
      playerScore[i] = 0;
    }
  }

  public void NewRound(){
    idPlayersAlive = new HashSet<int>();
    for(int i=0; i < modeCharacters.Length; i++){
      if(modeCharacters[i] != 2){
        idPlayersAlive.Add(i);
      }
    }
    ChangeScene("GameScene");
  }

  public void ResetLevel()
  {
    id_winner = -1;
    waterForce = 15.0f;
    boardSize = 12;
  }

  public void SetWinner(int id_player)
  {
    id_winner = id_player;
    ChangeScene("EndScene");
  }

  public void SetValues(int[] modeCharacters, int qtyRounds) {
    this.qtyRounds = qtyRounds;
    this.modeCharacters = modeCharacters;
  }

  public void FixedUpdate() {
    timeEnd -= Time.deltaTime;

    if(timeEnd < 0 && flag){
      flag = false;
      foreach(int idP in idPlayersAlive) id_winner = idP;
      ChangeScene("RoundScene");
      AddScorePlayer(id_winner);
    }
  }


  public void RemovePlayer(int id_player) {
    if(idPlayersAlive.Count <= 1) {
      id_winner = -10; //numero absurdo para indicar empate;
      flag = false;
      ChangeScene("RoundScene");
    }
    else if(idPlayersAlive.Count <= 2){
      timeEnd = 2;
      flag = true;
      idPlayersAlive.Remove(id_player);
    }
    else{
        idPlayersAlive.Remove(id_player);
    }
  }

  public void AddScorePlayer(int id_player){
    playerScore[id_player]++;
  }

  public void ChangeScene(string nextScene) {
    Fader fader = GameObject.FindGameObjectWithTag("Fade").GetComponent<Fader>();
    GameObject audioSource;
    if ((audioSource = GameObject.FindGameObjectWithTag("Audio"))) {
      audioSource.GetComponent<AudioSource>().DOFade(0, 1);
    }
    fader.FadeOut(() => SceneManager.LoadScene(nextScene));
  }
}
