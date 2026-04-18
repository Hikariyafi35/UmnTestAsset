using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Interaction : MonoBehaviour
{
    [Header("Food Prefab")]
    public GameObject foodPrefab;

    [Header("Layer")]
    public LayerMask blockedLayer;
    public LayerMask trashLayer;
    public LayerMask fishLayer;

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

        // 1. Klik Fish = takut / kabur
        Collider2D fishHit = Physics2D.OverlapPoint(worldPos, fishLayer);

        if (fishHit != null)
        {
            FishMovement fish = fishHit.GetComponent<FishMovement>();

            if (fish != null)
                fish.Scare(worldPos);

            return;
        }

        // 2. Klik Trash = destroy
        Collider2D trashHit = Physics2D.OverlapPoint(worldPos, trashLayer);

        if (trashHit != null)
        {
            Destroy(trashHit.gameObject);
            return;
        }

        // 3. Tempat kosong = spawn food
        Collider2D blocked = Physics2D.OverlapPoint(worldPos, blockedLayer);

        if (blocked == null)
        {
            Instantiate(foodPrefab, worldPos, Quaternion.identity);
        }
    }
}
