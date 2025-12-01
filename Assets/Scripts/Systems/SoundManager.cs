using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    //Queue<GameObject> soundQueue;

    [SerializeField]
    public AudioClip[] audioQueue;

    int curSoundIndex = 0;
    AudioSource audioSource;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject tmp = new GameObject();
                    tmp.name = typeof(SoundManager).Name;
                    instance = tmp.AddComponent<SoundManager>();
                }
            }
            DontDestroyOnLoad(instance);
            return instance;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioQueue[curSoundIndex];
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            ++curSoundIndex;
            if(curSoundIndex >= audioQueue.Length)
            {
                curSoundIndex = 0;
            }

            audioSource.clip = audioQueue[curSoundIndex];
            audioSource.Play();
        }


    }

}
