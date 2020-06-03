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
}
