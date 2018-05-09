using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public GameManager gm; // Game Manager Script Reference

	public Text buyButtonText; // Manager Buy Button Text

	public int managerCost; // Cost of Manager

	// Use this for initialization
	void Start () {
		// Set Buy button text
		buyButtonText.text = "Hire ($" + managerCost.ToString () + ")";
	}

	// Buy Manager
	public void hireManager(GameObject store) {
		// First check if you have amount to buy
		// Then remove that much amount from total balance
		if (gm.canBuy (managerCost)) {
			gm.addToBalance (-managerCost);
			store.GetComponent<Store> ().hasManager = true;
			GetComponent<Button> ().interactable = false;
		}
	}
}
