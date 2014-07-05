using UnityEngine;

public class InstaKill : MonoBehaviour {
  public void OnTriggerEnter2D(Collider2D other) {
    if(other.GetComponent<Player>() != null)
      LevelManager.Instance.KillPlayer();
  }
}