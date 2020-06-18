using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

  Image fade;

  private void Start() {
    fade = GetComponent<Image>();
    FadeIn();
  }

  public void FadeIn() {
    fade.DOFade(0, 1).OnComplete(() => {
      fade.enabled = false;
    });
  }

  public void FadeOut(TweenCallback callback) {
    fade.enabled = true;
    fade.DOFade(1, 1).OnComplete(callback);
  }
} 
