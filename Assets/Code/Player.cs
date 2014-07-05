using UnityEngine;

public class Player : MonoBehaviour {
  private bool _isFacingRight;
  private CharacterController2D _controller;
  private float _normalizedHorizontalSpeed;

  public float MaxSpeed = 8.0f;
  public float SpeedAccelerationOnGround = 10.0f;
  public float SpeedAccelerationInAir = 5.0f;

  public bool IsDead { get; private set; }
  
  public void Awake() {
    _controller = GetComponent<CharacterController2D>();
    _isFacingRight = transform.localScale.x > 0;
  }

  public void Update() {
    if(!IsDead && transform.position.y <= -4.0f)
      LevelManager.Instance.KillPlayer();

    if(!IsDead)
      HandleInput();

    var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
    _controller.SetHorizontalForce(
      IsDead
        ? 0
        : Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));
  }

  public void Kill() {
    _controller.HandleCollisions = false;
    collider2D.enabled = false;
    IsDead = true;

    _controller.SetForce(new Vector2(0, 10.0f));
  }

  public void RespawnAt(Transform spawnPoint) {
    if(!_isFacingRight)
      Flip();

    IsDead = false;
    collider2D.enabled = true;
    _controller.HandleCollisions = true;

    transform.position = spawnPoint.position;
  }

  private void HandleInput() {
    if(Input.GetKey(KeyCode.D)) {
      _normalizedHorizontalSpeed = 1;
      if(!_isFacingRight)
        Flip();
    } else if(Input.GetKey(KeyCode.A)) {
      _normalizedHorizontalSpeed = -1;
      if(_isFacingRight)
        Flip();
    } else {
      _normalizedHorizontalSpeed = 0;
    }

    if(_controller.CanJump && Input.GetKeyDown(KeyCode.Space)) {
      _controller.Jump();
    }
  }

  private void Flip() {
    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    _isFacingRight = transform.localScale.x > 0;
  }
}