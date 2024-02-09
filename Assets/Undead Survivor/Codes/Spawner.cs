using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime; //이전 프레임과 현재 프레임 사이의 시간 간격을 나타내는 값
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);


        if (timer > spawnData[level].spawnTime) {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        //자식 오브젝트에서만 선택되도록 랜덤 시작은 1부터
        //적의 위치를 랜덤한 곳에 놓는 코드
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
        
    }
}

[System.Serializable] //직렬화
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
