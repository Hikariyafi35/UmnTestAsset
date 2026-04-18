using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Interaction : MonoBehaviour
{
    [Header("Food Prefab")]
    public GameObject foodPrefab;

    [Header("Block Spawn Layer")]
    public LayerMask blockedLayer; // ikan / UI / object lain
    public LayerMask trashLayer; // tempat sampah

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleLeftClick();
        }
    }

    void HandleLeftClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = cam.ScreenToWorldPoint(mousePos);

        // 1. Jika klik sampah -> destroy
        Collider2D waste = Physics2D.OverlapPoint(worldPos, trashLayer);

        if (waste != null)
        {
            Destroy(waste.gameObject);
            return;
        }

        // 2. Jika tempat kosong -> spawn food
        Collider2D blocked = Physics2D.OverlapPoint(worldPos, blockedLayer);

        if (blocked == null)
        {
            Instantiate(foodPrefab, worldPos, Quaternion.identity);
        }
    }
}
