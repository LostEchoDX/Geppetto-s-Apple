using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMusicManager : MonoBehaviour
{
    public AudioSource DungeonMusic;
    private float timeElapsed;
    private float timeToFade;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator startMusic() {
        float timeElapsed = 0;
        float timeToFade = 5f;
        DungeonMusic.Play();
        DungeonMusic.volume = 0;
        while(timeElapsed < timeToFade) {
            print(timeElapsed);
            DungeonMusic.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }        
    }

    private IEnumerator stopMusic() {
        float timeElapsed = 0;
        float timeToFade = 5f;
        while(timeElapsed < timeToFade) {
            DungeonMusic.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        DungeonMusic.Stop();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player")
        {
            StartCoroutine(startMusic());
        }
    }

    void OnTriggerExit(Collider collider) 
    {

        if (collider.name == "Player")
        {
            StartCoroutine(stopMusic());
        }        
    }
}
