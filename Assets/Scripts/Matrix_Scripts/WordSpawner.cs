using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordSpawner : MonoBehaviour
{
    [Header("Spawn Point Lists")]
    public List<Transform> normalSpawnPoints;
    public List<Transform> targetSpawnPoints;

    [Header("Prefab Lists")]
    public List<GameObject> normalWordPrefabs;
    public List<GameObject> targetWordPrefabs;

    [Header("Normal Word Settings")]
    public float normalSpeedMin = 10f;
    public float normalSpeedMax = 30f;
    public float fontSizeMin = 20f;
    public float fontSizeMax = 50f;
    public float opacityMin = 0.2f;
    public float opacityMax = 0.8f;

    [Header("Target Word Settings")]
    public float targetSpeedMin = 10f;
    public float targetSpeedMax = 30f;

    [Header("Spawn Timing")]
    public float spawnIntervalMin = 0.5f;
    public float spawnIntervalMax = 1.5f;

    private List<SpawnerData> activeSpawners = new List<SpawnerData>();

    private void Start()
    {
        InitSpawners(normalSpawnPoints, normalWordPrefabs, normalSpeedMin, normalSpeedMax, true);
        InitSpawners(targetSpawnPoints, targetWordPrefabs, targetSpeedMin, targetSpeedMax, false);
    }

    private void InitSpawners(List<Transform> spawnPoints, List<GameObject> prefabs, float speedMin, float speedMax, bool isNormal)
    {
        List<float> uniqueSpeeds = GetUniqueRandoms(speedMin, speedMax, spawnPoints.Count);

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            SpawnerData spawner = new SpawnerData
            {
                point = spawnPoints[i],
                prefabs = prefabs,
                speed = uniqueSpeeds[i],
                isNormal = isNormal
            };
            activeSpawners.Add(spawner);
            StartCoroutine(SpawnRoutine(spawner));
        }
    }

    private IEnumerator SpawnRoutine(SpawnerData spawner)
    {
        while (true)
        {
            GameObject prefab = spawner.prefabs[Random.Range(0, spawner.prefabs.Count)];
            GameObject word = Instantiate(prefab, spawner.point.position, Quaternion.identity, spawner.point);

            // TMP özelleştirmeleri
            TMP_Text tmp = word.GetComponent<TMP_Text>();
            if (spawner.isNormal && tmp != null)
            {
                tmp.fontSize = Random.Range(fontSizeMin, fontSizeMax);
                Color color = tmp.color;
                color.a = Random.Range(opacityMin, opacityMax);
                tmp.color = color;
            }

            // Hız ataması
            WordMover mover = word.AddComponent<WordMover>();
            mover.speed = spawner.speed;

            // Bekleme süresi çekme
            WordSettings settings = word.GetComponent<WordSettings>();
            float waitTimeFromObject = (settings != null) ? settings.waitDuration : 0f;

            // Toplam bekleme süresi = objeden gelen süre + random spawn interval
            float extraInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            float totalWait = waitTimeFromObject + extraInterval;

            yield return new WaitForSeconds(totalWait);
        }
    }

    private List<float> GetUniqueRandoms(float min, float max, int count)
    {
        HashSet<float> values = new HashSet<float>();
        int attempts = 0;
        while (values.Count < count && attempts < 500)
        {
            float val = Mathf.Round(Random.Range(min, max) * 100f) / 100f;
            values.Add(val);
            attempts++;
        }

        return new List<float>(values);
    }

    private class SpawnerData
    {
        public Transform point;
        public List<GameObject> prefabs;
        public float speed;
        public bool isNormal;
    }
}
