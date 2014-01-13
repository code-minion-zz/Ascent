using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class HeroSaveDataList  
{
	[XmlArray("HeroSaves")]
	[XmlArrayItem("HeroSaveData")]
	public List<HeroSaveData> heroSaves = new List<HeroSaveData>();
}
