using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = default;

    private Rigidbody2D _rigidbody;
    private GameInputs _inputs;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _inputs = new GameInputs();
        _inputs.Enable();
        
        _inputs.Player.Move.performed += context => MovePlayer(context.ReadValue<Vector2>());
        _inputs.Player.Move.canceled += context => MovePlayer(Vector2.zero);
    }

    private void MovePlayer(Vector2 direction)
    {
        var velocity = direction * movementSpeed;
        _rigidbody.velocity = velocity;
    }
}