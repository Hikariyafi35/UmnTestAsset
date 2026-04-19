using System.IO;
using UnityEngine;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections;
using System;

public class TestWatcher : MonoBehaviour
{
    public string folderPath;

    FileSystemWatcher watcher;
    ConcurrentQueue<string> fileQueue = new ConcurrentQueue<string>();
    public GameObject fishPrefab;
    public GameObject trashPrefab;
    public Vector2 spawnMin;
    public Vector2 spawnMax;
    
    private void Start()
    {
        folderPath = ConfigManager.Data.watchFolder;
        // kalau kosong, pakai folder default
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            folderPath = Path.Combine(Application.dataPath, "../InputFolder");
        }

        // kalau folder belum ada, buatkan
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        watcher = new FileSystemWatcher();

        watcher.Path = folderPath;
        watcher.Filter = "*.png";

        watcher.Created += OnFileCreated;
        watcher.EnableRaisingEvents = true;

        ScanExistingFiles();
        Debug.Log("watcher aktif");
    }
    private void Update()
    {
        while (fileQueue.TryDequeue(out string path))
        {
            StartCoroutine(HandleFile(path));
        }
    }
    void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        fileQueue.Enqueue(e.FullPath);
    }
    bool IsFileLocked(string path)
    {
        try
        {
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch
        {
            return true;
        }
        return false;
    }
    IEnumerator HandleFile(string path)
    {
        
        while (IsFileLocked(path))
        {
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("File siap dipakai: " + path);
        string fileName = Path.GetFileNameWithoutExtension(path);
        string[] split = fileName.Split('_');

        if (split.Length < 3)
        {
            Debug.LogWarning("Format nama file salah");
            yield break;
        }
        string category = split[0];
        string type = split[1];

        Debug.Log($"Category: {category}, Type: {type}");
        //load image
        byte[] data = File.ReadAllBytes(path);

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(data);

        // ubah jadi sprite
        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f),150f
            );
        //get prefabs
        GameObject prefab = null;
        if (category == "FISH")
            prefab = fishPrefab;
        else if (category == "TRASH")
            prefab = trashPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Prefabs tidak ditemukan");
            yield break;
        }
        // spawn prefabs
        for (int i = 0; i < 10; i++)
        {
            Vector2 pos = GetRandomPosition();
            if (IsPositionFree(pos, 1.5f))
            {
                GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;
                Debug.Log("Spawn berhasil lokasi : " + pos);
                yield break;
            }
        }
        Debug.Log("Spawn gagal (area penuh)");
    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(
        UnityEngine.Random.Range(spawnMin.x, spawnMax.x),
        UnityEngine.Random.Range(spawnMin.y, spawnMax.y)
        );
    }

    // GameObject GetPrefab(string type, List<SpawnMapping> list)
    // {
    //     return list.Find(x => x.type == type)?.prefab;
    // }
    bool IsPositionFree(Vector2 pos, float radius)
    {
        return Physics2D.OverlapCircle(pos, radius) == null;
    }
    void ScanExistingFiles()
    {
        if (!Directory.Exists(folderPath)) return;
        string[] files = Directory.GetFiles(folderPath, "*.png");
        foreach (string file in files)
        {
            fileQueue.Enqueue(file);
        }
        Debug.Log("Scan selesai, " + files.Length + " file ditemukan");
    }
}

