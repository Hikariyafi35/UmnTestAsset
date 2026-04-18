using UnityEngine;

public class Velocity : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float speed = 3f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Start() {
        rb.linearVelocity = new Vector2(1, 1)*speed;
    }
}
