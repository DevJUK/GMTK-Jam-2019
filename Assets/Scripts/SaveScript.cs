using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Save Script Namespaces
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;


public class SaveScript : MonoBehaviour
{

	private Tracking GM;


	private void Awake()
	{
		GM = GetComponent<Tracking>();
	}

	// Saves the Level Scores to the file
	public void SaveData()
	{
		//// does stuff with binrary one assumes :)
		//BinaryFormatter BinFormat = new BinaryFormatter();

		//FileStream DataFile;

		//// creates a file in the data path /C/users/appdata/......
		//if (!File.Exists(Application.persistentDataPath + "/gamedata.ini"))
		//{
		//	DataFile = File.Create(Application.persistentDataPath + "/gamedata.ini");
		//}
		//else
		//{
		//	DataFile = File.Open(Application.persistentDataPath + "/gamedata.ini", FileMode.Open);
		//}

		//// creates an instance of the game data class...
		//GameData Data = new GameData();

		//// populating the new instance with the current values in the game
		//Data.PB = GM.PB;
		//Data.Last = GM.Last;

		//// Converts to binrary, using the data from the data thingy in a data file
		//BinFormat.Serialize(DataFile, Data);

		//// Closes the data file
		//DataFile.Close();


		PlayerPrefs.SetInt("PB", GM.PB);
		PlayerPrefs.SetInt("Last", GM.Last);
		PlayerPrefs.Save();
	}

	// Load the Level Scores from the file to the LevelData List
	public void LoadData()
	{
		//// checks to see if the file exsists, duh...
		//if (File.Exists(Application.persistentDataPath + "/gamedata.ini"))
		//{
		//	BinaryFormatter BinFormat = new BinaryFormatter();

		//	// Makes a local file with the file in the location and opens it :)
		//	FileStream DataFile = File.Open(Application.persistentDataPath + "/gamedata.ini", FileMode.Open);

		//	// converts the file to readable thingys :) ( "unbinraryfys" the file )
		//	GameData Data = (GameData)BinFormat.Deserialize(DataFile);

		//	Application.ExternalCall("SyncFiles");

		//	// Closes the file
		//	DataFile.Close();

		// Sets the values to the values that were in the file
		//GM.PB = Data.PB;
		//GM.Last = Data.Last;

		//}


		GM.PB = PlayerPrefs.GetInt("PB");
		GM.Last = PlayerPrefs.GetInt("Last");
	}
}

//// Class that stores the values 
//[Serializable]
//class GameData
//{
//	public int PB;
//	public int Last;
//}