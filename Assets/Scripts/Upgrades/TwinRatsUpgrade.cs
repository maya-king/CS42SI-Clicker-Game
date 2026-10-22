using System.Collections.Generic;

/**
 * Twin Rats (more rats per click).
 * Each level sends one extra rat every time the cauldron is clicked:
 * 2 rats at level 1, 3 at level 2, 4 at level 3.
 * Also works with the Rat Brooder's automatic clicks.
 * Goes on the "Spawn More Rats" button.
 */
public class TwinRatsUpgrade : Upgrade
{
    protected override void setDefaults()
    {
        upgradeName = "Twin Rats";
        levels = new List<UpgradeLevel>
        {
            price(cost("carrot", 15), cost("tomato", 10)),
            price(cost("tomato", 25), cost("strawberries", 10)),
            price(cost("cheese", 10), cost("peanut butter", 3)),
        };
    }

    protected override void applyLevel(int newLevel)
    {
        clickHandler.ratsPerClick = 1 + newLevel;
    }
}
