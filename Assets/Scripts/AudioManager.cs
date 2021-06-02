using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip antDied;

    public static AudioManager instance;
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void AntDied()
    {
        audioSource.clip = antDied;
        audioSource.Play();
    }
}
