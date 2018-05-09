using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveGameData {


	// Save the game automatically when the player quits
	public static void Save() {
		// Save the current system time as a string in the player prefs class
		PlayerPrefs.SetString ("SaveDateTime", System.DateTime.Now.ToBinary ().ToString ());

		// Save total balance
		PlayerPrefs.SetInt("Balance", GameManager.instance.balance);

		// Save all stores settings
		SaveStores();

		// Save all upgrades
		SaveUpgrades();

		// Update the preference file so we know that game is saved.
		PlayerPrefs.SetInt ("GameSaved", 1);
	}
		
	public static void SaveStores() {
		int counter = 1;
		foreach (Store storeObj in GameManager.storeList) {
			PlayerPrefs.SetInt ("storecount_" + counter, storeObj.storeCount);
			PlayerPrefs.SetInt ("storemultiplier_" + counter, storeObj.storeMultiplier);
			PlayerPrefs.SetFloat ("storecurrenttimer_" + counter, storeObj.CurrentTimer);
			PlayerPrefs.SetInt ("storetimer_" + counter, storeObj.storeTimer);
			int Unlocked = 0;
			if (storeObj.hasManager) {
				Unlocked = 1;
			}

			PlayerPrefs.SetInt ("storemanagerunlocked_" + counter, Unlocked);

			Unlocked = 0;
			if (storeObj.storeUnlocked) {
				Unlocked = 1;
			}

			PlayerPrefs.SetInt ("storeunlocked_" + counter, Unlocked);

			counter++;
		}
	}

	public static void SaveUpgrades() {

		int counter = 1;
		foreach (Upgrades storeUpgrade in GameManager.storeUpgrades) {
			PlayerPrefs.SetInt ("upgradestimes_" + counter, storeUpgrade.useUpgradeTimes);
			PlayerPrefs.SetInt ("upgradecounter_" + counter, storeUpgrade.UpgradeCounter);
			counter++;
		}
	
	}
}
