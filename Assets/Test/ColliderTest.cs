using System;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"{other.gameObject.name}과 현재 충돌을 시작하였습니다!");
    }

    private void OnCollisionStay(Collision other)
    {
        Debug.Log($"{other.gameObject.name}과 현재 충돌 중 입니다!");
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log($"{other.gameObject.name}과 충돌에서 벗어났습니다!");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name}과 트리거 충돌이 시작되었습니다!");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"{other.gameObject.name}과 트리거 충돌 중 입니다!");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"{other.gameObject.name}과 트리거 충돌이 종료되었습니다!");
    }
}
