using UnityEngine;
using System;


/**  custom class for food item in the shop **/

[Serializable]  // helps Unity interpret custom class
public class FoodItem
{
    // 1-indexed 
    public int id;
    public string name;
    public int value;
    // sprite image
    public Sprite sprite;
}
 