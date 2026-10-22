using System.Collections.Generic;
using UnityEngine;

/**
 * Rat Brooder (autoclick).
 * A rat mother keeps sending out rats for you by clicking the cauldron automatically.
 * Each level makes her click more often.
 * Goes on the "Autoclick" button.
 */
public class RatBrooderUpgrade : Upgrade
{
    // seconds between automatic clicks at level 1, 2 and 3
    public float[] clickIntervals = { 2f, 1f, 0.5f };

    private float timer = 0f;

    protected override void setDefaults()
    {
        upgradeName = "Rat Brooder";
        levels = new List<UpgradeLevel>
        {
            price(cost("carrot", 10)),
            price(cost("carrot", 20), cost("tomato", 10)),
            price(cost("tomato", 25), cost("strawberries", 8)),
        };
    }

    protected override void applyLevel(int newLevel)
    {
        timer = 0f;
    }

    protected override void Update()
    {
        // keep the button text updated (code in Upgrade.cs)
        base.Update();

        if (level == 0)
            return;

        // if there are more levels than intervals, use the last interval
        int index = Mathf.Min(level, clickIntervals.Length) - 1;
        float interval = clickIntervals[index];

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer -= interval;
            clickHandler.click();
        }
    }
}
