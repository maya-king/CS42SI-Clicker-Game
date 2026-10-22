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

    // [Upgrades] how many of a food the player has (0 if they have none yet)
    public int getCount(string foodName)
    {
        if (foodCounts.ContainsKey(foodName))
            return foodCounts[foodName];
        return 0;
    }

    // [Upgrades] takes food out to pay for an upgrade.
    // returns false and removes nothing if there isn't enough
    public bool removeFood(string foodName, int amount)
    {
        if (getCount(foodName) < amount)
            return false;

        foodCounts[foodName] -= amount;
        updateDisplay();
        return true;
    }

    // rewrites the inventory text, one line per food
    private void updateDisplay()
    {
        inventoryText.text = "";
        foreach (KeyValuePair<string, int> entry in foodCounts)
            inventoryText.text += entry.Key + ": " + entry.Value + "\n";
    }
}