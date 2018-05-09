using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour {

	public GameManager gm;

	public int upgradeCost;

	public int storeTimer;
	public int profit;

	public Text upgradeButtonText;

	public int useUpgradeTimes;
	private int upgradeCounter = 0;

	public int UpgradeCounter {
		get {
			return upgradeCounter;
		}
		set {
			upgradeCounter = value;
		}
	}

	// Use this for initialization
	void Start () {
		upgradeButtonText.text = "Upgrade ($" + upgradeCost.ToString () + ")";
	}
	
	public void ReduceStoreTimer(Store store) {
		if (gm.canBuy (upgradeCost)) {
			gm.addToBalance (-upgradeCost);
			store.storeTimer -= storeTimer;
			if (store.storeTimer <= 0) {
				store.storeTimer = 0;
			}
			upgradeCounter++;
			if (upgradeCounter >= useUpgradeTimes) {
				gameObject.transform.parent.gameObject.SetActive (false);
			}
		}
	}

	public void IncreaseProfitAmount(Store store) {
		if (gm.canBuy (upgradeCost)) {
			gm.addToBalance (-upgradeCost);
			store.profit += profit;
			upgradeCounter++;
			if (upgradeCounter >= useUpgradeTimes) {
				gameObject.transform.parent.gameObject.SetActive (false);
			}
		}
	}

	public void IncreaseProfitReduceTimer(Store store) {
		if (gm.canBuy (upgradeCost)) {
			gm.addToBalance (-upgradeCost);
			store.profit += profit;
			store.storeTimer -= storeTimer;
			if (store.storeTimer <= 0) {
				store.storeTimer = 0;
			}
			upgradeCounter++;
			if (upgradeCounter >= useUpgradeTimes) {
				gameObject.transform.parent.gameObject.SetActive (false);
			}
		}
	}
}
