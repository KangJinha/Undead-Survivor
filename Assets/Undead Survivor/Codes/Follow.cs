using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        // 월드좌표와 스크린좌표가 다름 WorldToScreenPoint를 사용해서 월드좌표를 스크린좌표로 바꿔줌
        rect.position = 
            Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
            
    }
}
