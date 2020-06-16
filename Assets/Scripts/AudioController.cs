using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

  public AudioSource audioSource;
  public AudioClip introSound;
  public AudioClip backgroundMusic;

  private void Start() {
    StartCoroutine(playMusic());
  }

  private IEnumerator playMusic() {
    audioSource.clip = introSound;
    audioSource.Play();
    yield return new WaitForSeconds(introSound.length);

    audioSource.clip = backgroundMusic;
    audioSource.loop = true;
    audioSource.Play();
  }
}