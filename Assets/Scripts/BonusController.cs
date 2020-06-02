using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusType {
    NONE,
    INCREASE_HYDRANT,
    INCREASE_WATER
}

public class BonusController : MonoBehaviour {

  private BonusType bonusType;
  public Animator animator;

  void Start() {  
    animator = GetComponent<Animator>();
  }

  public void SetBonusType(BonusType type) {
    bonusType = type;
    animator.SetInteger("bonusType", (int)type);
  }

  public BonusType GetBonusType() {
    return bonusType;
  }
}
