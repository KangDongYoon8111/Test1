using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtRandomIntervals : MonoBehaviour
{
    public float minSeconds = 5f; // Minimum interval to wait before playing sound.
    public float maxSeconds = 15f; // Maximum interval to wait before playing sound.

    private AudioSource audioSource;

    // 게임 실행 시 처음 딱 한번 발동되는 메서드
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        while (true)
        {
            float waitTime = Random.Range(minSeconds, maxSeconds);
            yield return new WaitForSeconds(waitTime);
            audioSource.Play();
        }
    }
}