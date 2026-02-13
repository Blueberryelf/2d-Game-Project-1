using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioClip interactSound;
    private AudioSource audioSource;
    private bool playerNearby = false;
    public AudioClip backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GamemanagerStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            audioSource.PlayOneShot(interactSound);
        }
    }

    public void GamemanagerStart()
    {
        backgroundMusic = GetComponent<AudioClip>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
