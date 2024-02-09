using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] aniCon;
    public Rigidbody2D target; //������ ���󰥰���

    bool isLive; // ���Ͱ� ����ִ���

    Rigidbody2D rigid;
    Collider2D coll;
    Animator ani;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake() // �ʱ�ȭ�����ֱ�
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        ani = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate() // �������� �̵��϶� // �̰Ͷ����� �˹��� ������ �ȵ���...!
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || ani.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x; // ���ݳ���ġ���� Ÿ����ġ�� ������ true�� ����

    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        ani.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        ani.runtimeAnimatorController = aniCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if(health > 0) {
            // .. Live, Hit Action
            ani.SetTrigger("Hit");
        }
        else {
            // .. Die
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            ani.SetBool("Dead", true);
            // Dead(); �ִϸ��̼��� ��������ٰ�
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack() { 
        yield return wait; // ���� �ϳ��� ���� ������ ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

}
