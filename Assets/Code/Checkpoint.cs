using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
  public void Start() {
    
  }

  public void PlayerHitCheckpoint() {
    
  }

  private IEnumerator PlayerHitCheckpointCo(int bonus) {
    yield break;
  }

  public void PlayerLeftCheckpoint() {
    
  }

  public void SpawnPlayer(Player player) {
    player.RespawnAt(transform);
  }

  public void AssignObjectToCheckpoint() {
    
  }
}