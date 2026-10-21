using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public List<FoodItem> availableFood = new List<FoodItem>
    {
        new FoodItem
        {
            id = 1,
            name = "carrot",
            value = 1
        },

        new FoodItem
        {
            id = 2,
            name = "tomato",
            value = 1
        },

        new FoodItem
        {
            id = 3,
            name = "strawberries",
            value = 3
        }
    };
}
