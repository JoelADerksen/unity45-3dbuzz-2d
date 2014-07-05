using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  public static LevelManager Instance { get; private set; }

  public Player Player { get; private set; }
  public CameraController Camera { get; private set; }

  private List<Checkpoint> _checkpoints;
  private int _currentCheckpointIndex;

  public Checkpoint DebugSpawn;

  public void Awake() {
    Instance = this;
  }

  public void Start() {
    _checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList();
    _currentCheckpointIndex = _checkpoints.Count > 0 ? 0 : -1;

    Player = FindObjectOfType<Player>();
    Camera = FindObjectOfType<CameraController>();

#if UNITY_EDITOR
    if(DebugSpawn != null)
      DebugSpawn.SpawnPlayer(Player);
    else if(_currentCheckpointIndex != -1)
      _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
#else
    if(_currentCheckpointIndex != -1)
      _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
#endif
  }

  public void Update() {
    var isAtLastCheckpoint = _currentCheckpointIndex >= _checkpoints.Count - 1;
    if(isAtLastCheckpoint)
      return;

    var distanceToNextCheckpoint = _checkpoints[_currentCheckpointIndex + 1].transform.position.x -
                                   Player.transform.position.x;
    if(distanceToNextCheckpoint >= 0)
      return;

    _checkpoints[_currentCheckpointIndex].PlayerLeftCheckpoint();
    _checkpoints[++_currentCheckpointIndex].PlayerHitCheckpoint();

    // TODO: time bonus
  }

  public void KillPlayer() {
    StartCoroutine(KillPlayerCo());
  }

  private IEnumerator KillPlayerCo() {
    Player.Kill();
    Camera.IsFollowing = false;
    yield return new WaitForSeconds(2.0f);

    Camera.IsFollowing = true;

    if(_currentCheckpointIndex != -1)
      _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);

    // TODO: points
  }
}