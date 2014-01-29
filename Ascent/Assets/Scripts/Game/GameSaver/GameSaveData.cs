using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class GameSaveData  
{
	public System.DateTime totalTimePlayed;
	public int highestTowerProgression;

	[XmlArray("HeroSaves")]
	[XmlArrayItem("HeroSaveData")]
	public List<HeroSaveData> heroSaves = new List<HeroSaveData>();
}
