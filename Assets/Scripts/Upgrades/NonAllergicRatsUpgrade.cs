using System.Collections.Generic;

/**
 * Non-Allergic Rats (expand inventory).
 * Regular rats are allergic to some foods. Each level hires rats that aren't,
 * so a new food gets added to what rats can steal.
 * Goes on the "Expand Items" button.
 */
public class NonAllergicRatsUpgrade : Upgrade
{
    // one new food per level. drag sprites in through the Inspector once they're drawn.
    // value controls rarity the same way as GameData: 10 / value = raffle tickets
    public List<FoodItem> unlockedFoods = new List<FoodItem>
    {
        new FoodItem { id = 4, name = "bread", value = 2 },          // gluten
        new FoodItem { id = 5, name = "cheese", value = 5 },         // dairy
        new FoodItem { id = 6, name = "peanut butter", value = 10 }, // nuts
    };

    protected override void setDefaults()
    {
        upgradeName = "Non-Allergic Rats";
        levels = new List<UpgradeLevel>
        {
            price(cost("tomato", 15), cost("strawberries", 5)),
            price(cost("bread", 20), cost("strawberries", 10)),
            price(cost("cheese", 15), cost("bread", 15)),
        };
    }

    protected override void applyLevel(int newLevel)
    {
        if (newLevel > unlockedFoods.Count)
            return;

        FoodItem food = unlockedFoods[newLevel - 1];

        // don't add the same food twice
        bool alreadyAvailable = gameData.availableFood.Exists(f => f.name == food.name);
        if (!alreadyAvailable)
            gameData.availableFood.Add(food);
    }
}
