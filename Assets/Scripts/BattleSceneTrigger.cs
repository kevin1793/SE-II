using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneTrigger : MonoBehaviour {

	private bool triggerActive = true;

	// Use this for initialization
	void Start () {
		if (GameManager.instance.finalBattleTriggered) {
			triggerActive = false;
		}
	}

	public void battleTrigger() {
		if (triggerActive) {
			GameManager.instance.finalBattleTriggered = true;
			StartCoroutine(GameManager.instance.EnemyEncounter ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
