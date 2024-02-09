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

    public void Init(float damage, int per, Vector3 dir) //�ʱ�ȭ�Լ�
    {
        this.damage = damage; //���� bullet�� ������ �Ű������ι޴°�
        this.per = per;
        
        if (per > -1) { // ���Ѵ뺸�� ũ�ٸ�
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
            gameObject.SetActive(false); // ������Ʈ Ǯ������ �����Ǳ⶧���� ��Ȱ��ȭ�����־����
        }
    }

}
