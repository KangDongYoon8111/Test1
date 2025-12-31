using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public GameObject onCollectEffect;

    void Update()
    {
        transform.Rotate(new Vector3(0,rotationSpeed,0), Space.World);
        // Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, rotationSpeed));
        // transform.rotation = transform.rotation * targetRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // 생성(원본, 생성할 위치, 생성시 주어진 회전값)
            Instantiate(onCollectEffect, transform.position, Quaternion.identity);
            // 충돌 발생 시 자신의 오브젝트를 파괴
            Destroy(gameObject);
        }
    }
}
