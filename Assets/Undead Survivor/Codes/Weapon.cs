using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                // update에서 이동이나 회전을 할때는 꼭 deltaTime을 넣어주기
                // Time.deltaTime은 한프레임이 소비한 시간
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // Test
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }
    // 무기레벨업
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;
            
        for(int index = 0; index < GameManager.instance.pool.prefabs.Length; index++) { 
            if (data.projectile == GameManager.instance.pool.prefabs[index]) {  
                prefabId = index;
                break;
            }
        }

        switch (id) { 
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.5f * Character.WeaponRate; // 연사속도 : 적을수록 많이발사
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int index = 0; index < count; index++) {
            Transform bullet;
            
            // 기존오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기
            if(index < transform.childCount) {
                bullet = transform.GetChild(index);
            } else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            // Bullet컴포넌트를 가져와서 데미지와 관통횟수 설정
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinity Per.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }


}
