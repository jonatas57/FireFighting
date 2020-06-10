using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

  public int numberPlayers = 2;
  public GameObject[] players;
  public int[] playerType = new int[] {1, 0};

  public float maxWaterTime;
  public float waterForce;
  public int id_winner;

  public int[] modeCharacters;
  public int qtyRounds;

  public HashSet<int> idPlayersAlive;
  public float timeEnd;
  private bool flag;

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

  public void NewGame()
  {
    SceneManager.LoadScene("GameScene");

    idPlayersAlive = new HashSet<int>();
    for(int i=0; i < numberPlayers; i++){
      idPlayersAlive.Add(i);
    }
    flag = false;
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
    GoToEndScene();
  }

  public void GoToEndScene() {
    SceneManager.LoadScene("EndScene");
  }

  public void GoToMainMenu() {
    SceneManager.LoadScene("MainMenu");
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
      GoToEndScene();
    }
  }

  public void RemovePlayer(int id_player) {
    if(idPlayersAlive.Count <= 1) {
      id_winner = -10; //numero absurdo para indicar empate;
      GoToEndScene();
      flag = false;
    }
    else if(idPlayersAlive.Count <= 2){
      timeEnd = 2;
      flag = true;
      idPlayersAlive.Remove(id_player);
    }
    idPlayersAlive.Remove(id_player);
  }
}
