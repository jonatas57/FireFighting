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

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public void SetBonusType(BonusType type) {
    bonusType = type;
  }

  public BonusType GetBonusType() {
    return bonusType;
  }
}
