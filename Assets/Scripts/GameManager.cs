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
  public GameObject spriteTransitionPrefab;
  


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

  private int stateFade;
  GameObject spriteTransition;
  string nameScene;
  public bool keyBoardActive;

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
      if(modeCharacters[i] != 2) idPlayersAlive.Add(i);
    }
    SceneManager.LoadScene("GameScene");
    nameScene = "GameScene";
    keyBoardActive = false;
    stateFade = 1;
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

  public void GoToRoundScene(){
    stateFade = 1;
    nameScene = "RoundScene";
  }

  public void GoToOptionsMenu(){
    SceneManager.LoadScene("OptionsMenu");
  }

  public void SetValues(int[] modeCharacters, int qtyRounds) {
    this.qtyRounds = qtyRounds;
    this.modeCharacters = modeCharacters;
  }

  public void FixedUpdate() {
    timeEnd -= Time.deltaTime;

    if(stateFade != 0){
      RenderFade();
      return;
    }

    if(timeEnd < 0 && flag){
      flag = false;
      foreach(int idP in idPlayersAlive) id_winner = idP;
      GoToRoundScene();
      AddScorePlayer(id_winner);
    }
  }

  public void RemovePlayer(int id_player) {
    if(idPlayersAlive.Count <= 1) {
      id_winner = -10; //numero absurdo para indicar empate;
      GoToRoundScene();
      flag = false;
    }
    else if(idPlayersAlive.Count <= 2){
      timeEnd = 2;
      flag = true;
      idPlayersAlive.Remove(id_player);
    }
    idPlayersAlive.Remove(id_player);
  }

  public void AddScorePlayer(int id_player){
    playerScore[id_player]++;
  }

  public void RenderFade(){
    if(stateFade == 1){
      spriteTransition = Instantiate<GameObject>(spriteTransitionPrefab);
      Color color = new Color();
      if(nameScene == "RoundScene") color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      else if(nameScene == "GameScene")  color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
      spriteTransition.GetComponent<SpriteRenderer>().color = color;
      stateFade = 2;
    }
    else if(stateFade == 2){
      Color color = spriteTransition.GetComponent<SpriteRenderer>().color;
      if(nameScene == "RoundScene" && color.a < 1) color.a += 0.03f;
      else if(nameScene == "GameScene" && color.a > 0) color.a -= 0.03f;
      else{
        Destroy(spriteTransition);
        if(nameScene == "RoundScene") SceneManager.LoadScene("RoundScene");
        else if(nameScene == "GameScene") keyBoardActive = true;
        stateFade = 0;
      }
      spriteTransition.GetComponent<SpriteRenderer>().color = color; 
    }
  }
}
