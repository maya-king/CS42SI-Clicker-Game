using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class Inventory : MonoBehaviour
{
    public TMP_Text inventoryText;

    // key: food name, value: amt the player has
    private Dictionary<string, int> foodCounts = new Dictionary<string, int>();

    // adds one food and updates the text
    public void addFood(FoodItem food)
    {
        if (!foodCounts.ContainsKey(food.name))
            foodCounts[food.name] = 0;

        foodCounts[food.name]++;
        updateDisplay();
    }

    // rewrites the inventory text, one line per food
    private void updateDisplay()
    {
        inventoryText.text = "";
        foreach (KeyValuePair<string, int> entry in foodCounts)
            inventoryText.text += entry.Key + ": " + entry.Value + "\n";
    }
}