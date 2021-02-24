using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
// A basic class for moving a Rigidbody2D
public class Rigidbody2DController : MonoBehaviour
{
    [Header("Rigidbody Controller Settings")]
    [SerializeField] private Rigidbody2D targetRigidbody = default;
    [SerializeField] private float movementSpeed = 1;

    private void Awake()
    {
        var controls = new Controls();
        controls.Enable();
        
        controls.Debug.Move.performed += x =>
            MovePlayer(x.ReadValue<Vector2>());

        controls.Debug.Move.canceled += x => 
            StopPlayer();
    }

    private void MovePlayer(Vector2 direction)
    {
        var velocity = direction * movementSpeed;
        targetRigidbody.velocity = velocity;
    }

    private void StopPlayer()
    {
        targetRigidbody.velocity = Vector2.zero;
    }
}