using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) //초기화함수
    {
        this.damage = damage; //왼쪽 bullet꺼 오른쪽 매개변수로받는값
        this.per = per;
        
        if (per > -1) { // 무한대보다 크다면
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy") || per == -1)
            return;
        per--;

        if(per == -1) {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false); // 오브젝트 풀링으로 관리되기때문에 비활성화시켜주어야함
        }
    }

}
