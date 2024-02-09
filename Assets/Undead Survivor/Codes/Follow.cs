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
        // ������ǥ�� ��ũ����ǥ�� �ٸ� WorldToScreenPoint�� ����ؼ� ������ǥ�� ��ũ����ǥ�� �ٲ���
        rect.position = 
            Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
            
    }
}
