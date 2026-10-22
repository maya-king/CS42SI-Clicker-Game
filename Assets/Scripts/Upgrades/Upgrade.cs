using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// One item in a barter price, e.g. 10 carrot
[Serializable]
public class ItemCost
{
    // must match the food's name exactly, e.g. "strawberries"
    public string foodName;
    public int amount;

    public ItemCost() { }

    public ItemCost(string foodName, int amount)
    {
        this.foodName = foodName;
        this.amount = amount;
    }
}

// The full barter price for one level of an upgrade
[Serializable]
public class UpgradeLevel
{
    public List<ItemCost> cost = new List<ItemCost>();
}

/**
 * Base class for every upgrade. Put one of the child classes on a UI Button.
 *
 * This class handles the barter part for all upgrades:
 *  - checks if the inventory has enough items
 *  - takes the items out of the inventory
 *  - updates the button text and greys it out when you can't afford it
 *
 * Child classes only decide two things:
 *  - setDefaults(): name and prices
 *  - applyLevel(): what happens when a new level is bought
 */
[RequireComponent(typeof(Button))]
public abstract class Upgrade : MonoBehaviour
{
    public string upgradeName = "Upgrade";

    // one entry per level, so the number of entries is the max level.
    // edit prices in the Inspector. Right click the component > Reset to restore defaults
    public List<UpgradeLevel> levels = new List<UpgradeLevel>();

    // found automatically when the game starts, no need to drag them in
    public Inventory inventory;
    public ClickHandler clickHandler;
    public GameData gameData;

    // 0 means not bought yet
    protected int level = 0;

    private Button button;
    private TMP_Text label;

    // ---------- child classes fill these in ----------

    // set the name and default prices
    protected abstract void setDefaults();

    // called right after the player buys a level (1, 2, 3...)
    protected abstract void applyLevel(int newLevel);

    // ---------- Unity callbacks ----------

    // Unity calls Reset when the script is first added in the editor
    private void Reset()
    {
        setDefaults();
    }

    protected virtual void Awake()
    {
        if (levels.Count == 0)
            setDefaults();

        if (inventory == null) inventory = FindAnyObjectByType<Inventory>();
        if (clickHandler == null) clickHandler = FindAnyObjectByType<ClickHandler>();
        if (gameData == null) gameData = FindAnyObjectByType<GameData>();

        button = GetComponent<Button>();
        button.onClick.AddListener(tryBuy);

        // shrink the text automatically so it fits inside the button
        label = GetComponentInChildren<TMP_Text>();
        if (label != null)
        {
            label.enableAutoSizing = true;
            label.fontSizeMin = 6;
            label.fontSizeMax = label.fontSize;
        }
    }

    protected virtual void Update()
    {
        refreshButton();
    }

    // ---------- barter logic ----------

    public bool isMaxed()
    {
        return level >= levels.Count;
    }

    // true if the inventory has every item the next level costs
    public bool canAfford()
    {
        if (isMaxed())
            return false;

        foreach (ItemCost item in levels[level].cost)
        {
            if (inventory.getCount(item.foodName) < item.amount)
                return false;
        }
        return true;
    }

    // called when the button is clicked
    public void tryBuy()
    {
        if (!canAfford())
            return;

        // pay with food
        foreach (ItemCost item in levels[level].cost)
            inventory.removeFood(item.foodName, item.amount);

        level++;
        applyLevel(level);
        Debug.Log("Bought " + upgradeName + " level " + level);
    }

    // button text looks like:
    //   Rat Brooder (1/3)
    //   12/20 carrot  3/10 tomato
    // green = you have enough, red = you don't
    private void refreshButton()
    {
        button.interactable = canAfford();

        if (label == null)
            return;

        string text = "<b>" + upgradeName + "</b> (" + level + "/" + levels.Count + ")\n";

        if (isMaxed())
        {
            text += "MAX";
        }
        else
        {
            foreach (ItemCost item in levels[level].cost)
            {
                int have = Mathf.Min(inventory.getCount(item.foodName), item.amount);
                string color = have >= item.amount ? "#2E7D32" : "#C62828";
                text += "<color=" + color + ">" + have + "/" + item.amount + " " + item.foodName + "</color>  ";
            }
        }

        // only touch the text when it changes
        if (label.text != text)
            label.text = text;
    }

    // ---------- helpers for writing prices in setDefaults ----------

    // price(cost("carrot", 10), cost("tomato", 5)) means "10 carrot + 5 tomato"
    protected static UpgradeLevel price(params ItemCost[] items)
    {
        UpgradeLevel upgradeLevel = new UpgradeLevel();
        upgradeLevel.cost.AddRange(items);
        return upgradeLevel;
    }

    protected static ItemCost cost(string foodName, int amount)
    {
        return new ItemCost(foodName, amount);
    }
}
