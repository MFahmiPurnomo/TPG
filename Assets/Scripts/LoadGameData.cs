using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class LoadGameData {

	// Date Time if loading from saved game
	static DateTime currentDate;
	static DateTime oldDate;

	public static void LoadSaveGame() {

		// Check if game is saved before doing loading
		if (IsGameSaved ()) {

			Debug.Log ("Loading Saved Game");

			// Get the idle time that has been passed since we saved our game last time.
			float getIdleTime = GetIdleTime();

			// Get the total saved balance
			GameManager.instance.balance = PlayerPrefs.GetInt("Balance");

			// Load all stores settings
			LoadSavedStoreData(getIdleTime);

			// Load all upgrades settings
			LoadStoreUpgrades();

		}
	}

	public static bool IsGameSaved() {
		try {
			int GameSavedFlag = PlayerPrefs.GetInt("GameSaved");
			if(GameSavedFlag == 1) {
				return true;
			} else {
				return false;
			}
		} catch {
			Debug.Log ("Can't read GameSaved...Loading new game");
			return false;
		}
	}

	public static float GetIdleTime() {
		float idleTime = 0;

		try {
			// Get stored string into a 64bit int
			long temp = Convert.ToInt64(PlayerPrefs.GetString("SaveDateTime"));

			// Convert old time from binary to a date time variable
			DateTime oldDate = DateTime.FromBinary(temp);

			// Use subtract method and store the result as a timespan variable
			currentDate = System.DateTime.Now;
			TimeSpan difference = currentDate.Subtract(oldDate);

			// Save the idle time in sec
			idleTime = (float)difference.TotalSeconds;
		} catch {
			Debug.Log ("exception caught.... starting a new game");

		}
		return idleTime;
	}

	private static void LoadSavedStoreData(float IdleTime) {
		int counter = 1;
		foreach (Store storeObj in GameManager.storeList) {

			//Get the number of store that player owns
			storeObj.storeCount = PlayerPrefs.GetInt("storecount_" + counter);
			storeObj.storeCountText.text = storeObj.storeCount.ToString ();

			// Get the multiplier for that store
			storeObj.storeMultiplier = PlayerPrefs.GetInt("storemultiplier_" + counter);

			// Check and see if store is unlocked or not
			int Unlocked = PlayerPrefs.GetInt("storeunlocked_" + counter);
			if (Unlocked == 1) {
				storeObj.storeUnlocked = true;
			} else {
				storeObj.storeUnlocked = false;
			}

			// check and see if store manager is unlocked or not

			Unlocked = PlayerPrefs.GetInt ("storemanagerunlocked_" + counter);
			if (Unlocked == 1) {
				storeObj.hasManager = true;

				float LastTimerValue = PlayerPrefs.GetFloat ("storecurrenttimer_" + counter);
				storeObj.storeTimer = PlayerPrefs.GetInt ("storetimer_" + counter);

				storeObj.CalcuateIdleProfit (IdleTime, LastTimerValue);
			}

			counter++;
		}
	}

	private static void LoadStoreUpgrades() {
		int counter = 1;

		foreach (Upgrades StoreUpgrade in GameManager.storeUpgrades) {
			StoreUpgrade.useUpgradeTimes = PlayerPrefs.GetInt ("useupgradestimes_" + counter.ToString ());
			StoreUpgrade.UpgradeCounter = PlayerPrefs.GetInt ("upgradecounter_" + counter.ToString ());

			counter++;
		}
	}
}
