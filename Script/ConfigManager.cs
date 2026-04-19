using System.IO;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigData Data;
    public string watchFolder = "";

    void Awake()
    {
        LoadConfig();
    }

    void LoadConfig()
    {
        string path = Path.Combine(Application.dataPath, "../config.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data = JsonUtility.FromJson<ConfigData>(json);
            Debug.Log("Config Loaded");
        }
        else
        {
            Debug.Log("Config Not Found");
            Data = new ConfigData();
        }
    }
}

[System.Serializable]
public class ConfigData
{
    public string watchFolder = "";
    public float fishMinSpeed = 1.5f;
    public float fishMaxSpeed = 3f;
    public float fishDetectionRadius = 15f;
    public float fishScareDuration = 6f;
    public float fishScareSpeedMultiplier = 12f;
    public float scareForce = 8f;

    public float hungerDecreasePerSecond = 10f;
    public float hungerCooldown = 5f;

    public float foodFallSpeed = 1.5f;

    public float pushForce = 2f;

    public float masterVolume = 1f;
    public float fishClickVolume = 0.8f;
    public float fishEatVolume = 0.7f;
}