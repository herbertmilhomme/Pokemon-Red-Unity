﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TilesData {
    public bool hasItemBall;
    public bool hasText, hasItem;
    public int TextID, coinamount;
    public string itemName;
}
public class TileProperties : MonoBehaviour  {
    public TilesData data;

	
}