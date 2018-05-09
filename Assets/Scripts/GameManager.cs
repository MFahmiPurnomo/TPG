using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Text currentBalance; // Total Balance Text
	public GameObject managerPanel; // Manager Panel Game Object
	public GameObject upgradesPanel;
	public GameObject storesPanel;

	public int balance; // Total Balance

	public Scrollbar storePanelScrollbar;
	public Scrollbar managerPanelScrollbar;

	// Example of Observer Design Pattern
	// These events update all subscriber object that require it when the balance changes.
	public delegate void UpdateBalance ();
	public static event UpdateBalance OnUpdateBalance;


	// Used for Singleton desin to hold one and only one instance of the game manager
	public static GameManager instance;

	public static ArrayList storeList = new ArrayList ();
	public static ArrayList storeUpgrades = new ArrayList();


	void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	public void OnApplicationQuit() {
		SaveGameData.Save ();
	}


	// Use this for initialization
	void Start () {

		GetStores ();
		GetStoreUpgrades ();

		LoadGameData.LoadSaveGame ();

		storePanelScrollbar.value = 1;

		//Set Current Balance Text
		currentBalance.text = "$" + balance.ToString ();

		// Example of Observer pattern
		// Notify all observers that we have updated the game balance
		// This is how the interface knows to update without using updates
		if (OnUpdateBalance != null) {
			OnUpdateBalance ();
		}
	}

	private void GetStoreUpgrades() {
		Transform viewport = upgradesPanel.transform.Find ("Viewport");
		Transform content = viewport.transform.Find ("Content");
		Transform children = content.transform.Find ("StorePanel");
		foreach (Transform child in children) {
//			Debug.Log (child.gameObject.name);
			Transform upgrades = child.transform.Find("BuyButton");
			Upgrades storeUpgrade = upgrades.gameObject.GetComponent<Upgrades> ();
			storeUpgrades.Add (storeUpgrade);
		}
//		foreach (Upgrades child in storeUpgrades) {
//			Debug.Log (child.storeTimer);
//		}
	}

	private void GetStores() {
		Transform viewport = storesPanel.transform.Find ("Viewport");
		Transform content = viewport.transform.Find ("Content");
		Transform children = content.transform.Find ("StorePanel");
		foreach (Transform child in children) {
//			Debug.Log (child.gameObject.name);
			Store store = child.gameObject.GetComponent<Store>();
			storeList.Add (store);
		}

//		foreach (Store child in storeList) {
//			Debug.Log (child.profit);
//		}
	}

	// Add or Remove amount from total balance
	public void addToBalance(int amount) {
		balance += amount;
		currentBalance.text = "$" + balance.ToString ();
		// Example of Observer pattern
		// Notify all observers that we have updated the game balance
		// This is how the interface knows to update without using updates
		if (OnUpdateBalance != null) {
			OnUpdateBalance ();
		}
	}

	// Check if you have amount to buy from total balance
	public bool canBuy(int amount) {
		if (amount <= balance)
			return true;
		else
			return false;
	}

	// Show Manager Panel
	public void showManagers() {
		managerPanel.SetActive (!managerPanel.activeInHierarchy);
		managerPanelScrollbar.value = 1; 
	}

	// Hide Manager Panel
	public void hideManagers() {
		managerPanel.SetActive (false);
	}

	public void toggleUpgradesPanel() {
		upgradesPanel.SetActive (!upgradesPanel.activeInHierarchy);
	}

}
