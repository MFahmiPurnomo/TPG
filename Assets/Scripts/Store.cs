using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour {

	public GameManager gm; // Game Manager Script Reference

	public Slider progressSlider; // Progress Slider Game Object
	public Text storeCountText; // Store Count Text
	public Text buyButtonText; // Buy Store Button Text

	public int storeTimer; // Store Timer after which it will make profit
	public int profit; // Amount of profit it will make
	public int baseStorePrice; // Price of first store
	public int storeMultiplier; // Store multiplier to calculate next store price
	public int storeCount; // Total store owned

	private float currentTimer;
	public float CurrentTimer {
		get {
			return currentTimer;
		}
		set {
			currentTimer = value;
		}
	}

 // Current Time used for progress slider

	private bool startTimer; // Start Progress Slider Timer
	public bool hasManager = false; // Has store manager

	public bool storeUnlocked = false;

	// Example of Observer Design pattern
	// Update our store UI when the balance changes in the gamemanager
	void OnEnable() {
		GameManager.OnUpdateBalance += UpdateStoreUI;
	}

	// Example of Observer Design pattern
	// Remove our subscriptions if this object is disabled
	void OnDisable() {
		GameManager.OnUpdateBalance -= UpdateStoreUI;
	}

	public void CalcuateIdleProfit(float SecondsIdle, float LastTimerValue) {

		double IdleAmt = 0d;
		int amount = 0;

		// Only give profit if we are idle longer than the store timer
		if ((startTimer && SecondsIdle > 0) || (hasManager && storeCount > 0 && SecondsIdle > 0)) {
			if (SecondsIdle > storeTimer - LastTimerValue) {
				
				if (hasManager) {
					IdleAmt = CalcuateStoreProfit () * (double)(SecondsIdle / storeTimer); 
					CurrentTimer = (SecondsIdle % (storeTimer - LastTimerValue)) + LastTimerValue;
						
				} else {
					IdleAmt = CalcuateStoreProfit ();
					currentTimer = 0f;
				}
				amount = (int)IdleAmt;
				gm.addToBalance (amount);
			} else {
				CurrentTimer = LastTimerValue + SecondsIdle;
			}
		}
		Debug.Log (gameObject.name + " made $" + amount.ToString () + " while idle/offline.");
	}

	public double CalcuateStoreProfit() {
		double Amt = profit * storeCount;
		return Amt;
	}

	public void UpdateStoreUI() {
		CanvasGroup cg = this.transform.GetComponent<CanvasGroup> ();
		// Hide store until we can afford them
		if (!storeUnlocked && !gm.canBuy (baseStorePrice)) {
			cg.interactable = false;
			cg.alpha = 0;
		} else {
			cg.interactable = true;
			cg.alpha = 1;
			storeUnlocked = true;
		}
	}

	// Use this for initialization
	void Start () {
		// Set Store Count Text
		storeCountText.text = storeCount.ToString ();
		// Set buy store button text
		buyButtonText.text = "Buy ($" + baseStorePrice.ToString () + ")";
		// Set current time to 0
		currentTimer = 0;

	}
	
	// Update is called once per frame
	void Update () {
		// If timer started or store has manager then make profit
		// add profit to balance after current timer crossed store timer
		// reset current timer
		if (startTimer || (hasManager && storeCount > 0)) {
			currentTimer = currentTimer + Time.deltaTime;
			progressSlider.value = currentTimer / storeTimer;
			if (currentTimer > storeTimer) {
				startTimer = false;
				gm.addToBalance (profit * storeCount);
				progressSlider.value = 0;
				currentTimer = 0;
			}

			if (storeTimer <= 0) {
				progressSlider.value = 1;
			}
		} else {
			progressSlider.value = 0;
		}
	}

	// Click Image function to manually start timer
	public void clickme() {
		if (storeCount > 0) {
			startTimer = true;
		}
	}

	// Buy store
	public void buyStore() {
		// First check if you have amount to buy
		// Then remove that much amount from total balance
		if (gm.canBuy (baseStorePrice)) {
			gm.addToBalance (-baseStorePrice);
			storeCount++;
			storeCountText.text = storeCount.ToString ();

			// Calcuate next store price and update button text
			baseStorePrice = baseStorePrice + (storeCount * storeMultiplier);
			buyButtonText.text = "Buy ($" + baseStorePrice.ToString () + ")";
		} 
	}

}
