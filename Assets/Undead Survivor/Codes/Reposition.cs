using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        
        switch (transform.tag) {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY) {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY) {
                    transform.Translate(Vector3.up * dirY * 40);
                } 
                /*else {
                    transform.Translate(dirX * 40, dirY * 40, 0);
                    // dirX와 dirY에 지정된 방향으로 X축과 Y축으로 각각 40씩 이동
                }*/
                break;
            case "Enemy":
                if (coll.enabled){
                    Vector3 dist = playerPos - myPos; // myPos 몬스터위치
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
