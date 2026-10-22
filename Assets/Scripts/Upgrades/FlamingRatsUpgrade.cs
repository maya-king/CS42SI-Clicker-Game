using System.Collections.Generic;

/**
 * Flaming Rats (optional bonus upgrade).
 * Rats are on fire, so they run faster. Affects rats spawned after buying.
 * Needs its own button: duplicate one of the upgrade buttons and put this script on it.
 */
public class FlamingRatsUpgrade : Upgrade
{
    // rat speed multiplier at level 1, 2 and 3
    public float[] speedMultipliers = { 1.4f, 1.8f, 2.5f };

    protected override void setDefaults()
    {
        upgradeName = "Flaming Rats";
        levels = new List<UpgradeLevel>
        {
            price(cost("carrot", 15), cost("strawberries", 6)),
            price(cost("strawberries", 10), cost("cheese", 6)),
            price(cost("cheese", 10), cost("peanut butter", 4)),
        };
    }

    protected override void applyLevel(int newLevel)
    {
        int index = newLevel - 1;
        if (index >= speedMultipliers.Length)
            index = speedMultipliers.Length - 1;

        clickHandler.ratSpeedMultiplier = speedMultipliers[index];
    }
}
