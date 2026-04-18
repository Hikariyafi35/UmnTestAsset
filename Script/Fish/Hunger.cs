using UnityEngine;

public class Hunger : MonoBehaviour
{
    [Range(0,100)]
    public float hungerMeter = 100f;

    public float hungerDecreasePerSecond = 10f;
    public float hungerCooldown = 5f;

    private float cooldownTimer;

    private void Start() 
    {
        hungerDecreasePerSecond = ConfigManager.Data.hungerDecreasePerSecond;
        hungerCooldown = ConfigManager.Data.hungerCooldown;
    }
    private void Update()
    {
        if (hungerMeter >= 100)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
                hungerMeter -= 1f;
        }
        else
        {
            hungerMeter -= hungerDecreasePerSecond * Time.deltaTime;
        }

        hungerMeter = Mathf.Clamp(hungerMeter, 0, 100);
    }

    public bool IsHungry()
    {
        return hungerMeter <= 0;
    }

    public void EatFood()
    {
        hungerMeter += 50f;

        if (hungerMeter >= 100)
        {
            hungerMeter = 100;
            cooldownTimer = hungerCooldown;
        }
    }
}