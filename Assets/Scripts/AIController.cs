using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public enum Task {
  NONE,
  GET_BONUS,
  PLACE_HYDRANT,
  GET_SAFE
}

public class AIController : MonoBehaviour {

  public Board board;
  public PlayerController controller;

  public List<Node> reachableNodes;

  public int pathErrorMargin = 1;
  public List<Vector2Int> path;
  public int pathIndex;

  public Task currentTask;

  public Vector2Int[] toNeighbours;

  public void Start() {
    controller = GetComponent<PlayerController>();
    board = controller.board;
    path = new List<Vector2Int>();
    pathIndex = 0;
    currentTask = Task.NONE;

    toNeighbours = new Vector2Int[] {Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left};
  }

  public void ResetCommands() {
    controller.aiDown = controller.aiLeft = controller.aiHydrant = controller.aiRight = controller.aiUp = false;
  }


  public int Manhattan(Vector2Int start, Vector2Int end) {
    return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);  
  }

  public Vector2Int BestPlaceToPutHydrant() {
    var weights = new List<float>();
    
    var maxDistance = 0;
    foreach (var node in reachableNodes) {
      maxDistance = Mathf.Max(maxDistance, node.distToStart);
      weights.Add(0);
    };
    
    var distanceWeight = 0.8f + Random.Range(-0.1f, 0.1f);
    var tileWeight = 1 + Random.Range(-0.1f, 0.1f);
    var aggressionWeight = 2 + Random.Range(-0.4f, 0.4f);
    
    for (int i = 0;i < reachableNodes.Count;i++) {
      if (maxDistance != 0) {
        weights[i] += distanceWeight * (maxDistance - reachableNodes[i].distToStart) / maxDistance;
      } else {
        weights[i] += distanceWeight * 1;
      }
        
      var hydrantExplosion = CountBlocksDestroyed(reachableNodes[i].coord);
      weights[i] += tileWeight * hydrantExplosion.Item1;
      weights[i] += aggressionWeight * hydrantExplosion.Item2;
    }
    
    float highestWeight = -1;
    int choosenNode = -1;
    for (int i = 0;i < reachableNodes.Count;i++) {
      if (weights[i] > highestWeight) {
        highestWeight = weights[i];
        choosenNode = i;
      } 
    }
    return reachableNodes[choosenNode].coord;
  }

  public void ResetTask() {
    currentTask = Task.NONE;
    ResetCommands();
    path.Clear();
    pathIndex = 0;
  }

  public void FollowPath() {
    if (path.Count == 0 || pathIndex > (path.Count - 2)) {
      return;
    }
    if ((currentTask == Task.GET_BONUS && board.GetTile(path.Last()) != TileType.BONUS) || !board.IsWalkable(path.Last())) {
      currentTask = Task.NONE;
      path.Clear();
      return;
    }
    
    var gridCoords = board.VectorToGridPosition(transform.position);
    
    var currentTile = path[pathIndex];
    var nextTile = path[pathIndex + 1];

    if ((currentTask != Task.GET_SAFE && !board.IsSafe(nextTile) && board.IsSafe(gridCoords))
          || board.IsWater(nextTile)) {
      return;
    }
    
    var diff = board.GridToVectorPosition(nextTile) - transform.position;
    if (Mathf.Abs(diff.x) < pathErrorMargin && Mathf.Abs(diff.y) < pathErrorMargin) {
      pathIndex++;
    }
    if (board.GetTile(nextTile) == TileType.HYDRANT) {
        currentTask = Task.NONE;
        pathIndex = path.Count - 1;
        return;
    }
    if (nextTile.x < currentTile.x ) {
      if (diff.y > pathErrorMargin) controller.aiUp = true;
      else if (diff.y < -pathErrorMargin) controller.aiDown = true;
      else controller.aiLeft = true;
    }
    else if (nextTile.x > currentTile.x) {
      if (diff.y > pathErrorMargin) controller.aiUp = true;
      else if (diff.y < -pathErrorMargin) controller.aiDown = true;
      else controller.aiRight = true;
    }
    else if (nextTile.y < currentTile.y) {
      if (diff.x > pathErrorMargin) controller.aiRight = true;
      else if (diff.x < -pathErrorMargin) controller.aiLeft = true;
      else controller.aiUp = true;
    } 
    else if (nextTile.y > currentTile.y) {
      if (diff.x > pathErrorMargin) controller.aiRight = true;
      else if (diff.x < -pathErrorMargin) controller.aiLeft = true;
      else controller.aiDown = true;
    }
  }

  public Vector2Int FindSafePlace() {
    Vector2Int safePlace = Vector2Int.one * -1;
    int minDist = 500;
    for (int i = 1;i < reachableNodes.Count;i++) {
      if (reachableNodes[i].distToStart < minDist && board.IsSafe(reachableNodes[i].coord)) {
        safePlace = reachableNodes[i].coord;
        minDist = reachableNodes[i].distToStart;
      }
    }
    return safePlace;
  }

  public void FixedUpdate() {
    if(!GameManager.Instance.keyBoardActive) return;
    ResetCommands();
    if (path.Count > 0 && (path.Count - 1 != pathIndex)) {
      FollowPath();
      return;
    }
    else {
      if (currentTask == Task.PLACE_HYDRANT && controller.hydrantQtd > 0) {
        controller.aiHydrant = true;
      }
      currentTask = Task.NONE;
    }
    
    var gridCoords = board.VectorToGridPosition(transform.position);
    reachableNodes = GetReachableNodes(gridCoords);
    var safePlace = FindSafePlace();
    
    var bonusPosition = GetBonusPosition();
    if (!board.IsSafe(gridCoords) && safePlace.x != -1 && safePlace != gridCoords) {
      currentTask = Task.GET_SAFE;
      path = AStar(gridCoords, safePlace, board);
      pathIndex = 0;
    }
    else if (bonusPosition.x != -1) {
      path = AStar(gridCoords, bonusPosition, board);
      currentTask = Task.GET_BONUS;
      pathIndex = 0;
    }
    else {
      var hydrantPosition = BestPlaceToPutHydrant();
      if (hydrantPosition.x != -1) {
        path = AStar(gridCoords, hydrantPosition, board);
        pathIndex = 0;
        currentTask = Task.PLACE_HYDRANT;
      }
    }
  }

  public List<Vector2Int> AStar(Vector2Int begin, Vector2Int goal, Board board) {
    var closedNodes = new List<Node>();
    var visit = new List<List<bool>>();
    for (int i = 0;i < board.size + 2;i++) {
      visit.Add(Enumerable.Repeat(false, board.size + 2).ToList());
    }
    
    var start = new Node(begin, 0, Manhattan(begin, goal));
    var openNodes = new List<Node>();
    openNodes.Add(start);
    for (int index = 0;openNodes.Count > 0;index++) {
      int max = 500;
      int min = -1;
      
      for (int i = 0; i < openNodes.Count; i++) {
        if (openNodes[i].GetCost() < max) {
          max = openNodes[i].GetCost();
          min = i;
        }
      }
        
      var currentNode = openNodes[0];
      openNodes.Remove(currentNode);
        
      if (currentNode.coord == goal) {
        var st = new Stack<Vector2Int>();
        while (true) {
          st.Push(currentNode.coord);
          if (currentNode.parent == -1) break;
          currentNode = closedNodes[currentNode.parent];
        }
        var path = new List<Vector2Int>();
        while (st.Count > 0) path.Add(st.Pop());
        return path;
      } 
      else {
        foreach (var del in toNeighbours) {
          var nextPos = currentNode.coord + del;
          
          if (!visit[nextPos.x][nextPos.y] && board.IsWalkable(nextPos)) {
            openNodes.Add(new Node(nextPos, currentNode.distToStart + board.GetDanger(nextPos) + 1, Manhattan(nextPos, goal), index));
            visit[nextPos.x][nextPos.y] = true;
          }
        }
        closedNodes.Add(currentNode);
      }
    }
    return new List<Vector2Int>();
  }

  public List<Node> GetReachableNodes(Vector2Int actualPosition) {
    var start = new Node(actualPosition, 0);
    var openNodes = new List<Node>();
    openNodes.Add(start);
    var closedNodes = new List<Node>();
    var visit = new List<List<bool>>();
    for (int i = 0;i < board.size + 2;i++) {
      visit.Add(Enumerable.Repeat(false, board.size + 2).ToList());
    }
    visit[actualPosition.x][actualPosition.y] = true;
    while (openNodes.Count > 0) {
        var currentNode = openNodes[0];
        openNodes.Remove(currentNode);
        closedNodes.Add(currentNode);
        foreach (var del in toNeighbours) {
            var v = new Node(currentNode.coord + del, currentNode.distToStart + 1);
            if (!visit[v.coord.x][v.coord.y] && board.IsWalkable(v.coord)) {
                openNodes.Add(v);
                visit[v.coord.x][v.coord.y] = true;
            }
        }
    }
    if (board.GetTile(actualPosition) == TileType.HYDRANT) {
        closedNodes.Remove(closedNodes[0]);
    }
    return closedNodes;
  }

  public Vector2Int GetBonusPosition() {
    var bonusPosition = Vector2Int.one * -1;
    int minDist = 500;
    foreach (var node in reachableNodes) {
      if (board.GetTile(node.coord) == TileType.BONUS && minDist > node.distToStart) {
        bonusPosition = node.coord;
        minDist = node.distToStart;
      }
    }
    return bonusPosition;
  }

  public System.Tuple<int, int> CountBlocksDestroyed(Vector2Int position) {
    int blocksDestroyed = 0;
    int players = 0;
    var playerPositions = new List<Vector2Int>();
    foreach (var player in GameManager.Instance.players) {
      if (player && !player.Equals(gameObject)) {
        playerPositions.Add(board.VectorToGridPosition(player.transform.position));
      }
    }
    for (int i = 1;i <= controller.waterLength;i++) {
      for (int j = 0;j < 4;j++) {
        var pos = position + (toNeighbours[j] * i);
        if (board.GetTile(pos) == TileType.FIRE) {
          blocksDestroyed++;
        }
        if (playerPositions.IndexOf(pos) != -1) {
          players++;
        }
      }
    }
    return new System.Tuple<int, int>(blocksDestroyed, players);
  }
}

public class Node {
  public Vector2Int coord;
  public int distToStart;
  public int distToGoal;
  public int parent;

  public Node(Vector2Int vec, int dist2start, int dist2goal = 0, int p = -1) {
     coord = vec;
     distToStart = dist2start;
     distToGoal = dist2goal;
     parent = p;
  }

  public int GetCost() {
    return distToGoal + distToStart;
  }
}
