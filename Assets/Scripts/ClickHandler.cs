using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class ClickHandler : MonoBehaviour
{
    public GameObject ratPrefab;
    public Transform ratSpawnPoint;
    public Transform foodSpawnPoint;
    public Transform exitPoint;
    public GameData gameData;
    public Inventory inventory;

    // [Upgrades] raised by the Twin Rats upgrade
    public int ratsPerClick = 1;
    // [Upgrades] seconds between rats in the same click, so twins don't overlap
    public float twinSpawnDelay = 0.15f;
    // [Upgrades] raised by the Flaming Rats upgrade
    public float ratSpeedMultiplier = 1f;


    private Collider2D cauldronCollider;

    // get reference to the cauldron   
    private void Start()
    {
        cauldronCollider = GetComponent<Collider2D>();
    }
    // check every frame if cauldron was clicked 
    public void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;
        
        // convert coordinates from screen to world
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (cauldronCollider.OverlapPoint(mousePosition))
            click();
    }

    // [Upgrades] one click on the cauldron. sends out ratsPerClick rats, a moment apart.
    // the Rat Brooder upgrade calls this to auto-click
    public void click()
    {
        StartCoroutine(spawnRatGroup());
    }

    private IEnumerator spawnRatGroup()
    {
        for (int i = 0; i < ratsPerClick; i++)
        {
            spawnRat();
            if (i < ratsPerClick - 1)
                yield return new WaitForSeconds(twinSpawnDelay);
        }
    }


    // creates a rat and tells it where to go and what food it will grab
    public void spawnRat()
    {
        GameObject ratObject = Instantiate(ratPrefab, ratSpawnPoint.position, ratPrefab.transform.rotation);

        Rat rat = ratObject.GetComponent<Rat>();
        rat.foodPosition = foodSpawnPoint.position;
        rat.exitPosition = exitPoint.position;
        rat.food = pickRandomFood();
        rat.inventory = inventory;
        rat.moveSpeed *= ratSpeedMultiplier; // [Upgrades]
    }

    // picks a random food. higher value foods are rarer
    // modelled after a raffle bc the other math was too complicated lol
    private FoodItem pickRandomFood()
    {
        List<FoodItem> raffleTickets = new List<FoodItem>();

        // give each food tickets: value 1 gets 10 tickets, value 3 gets 3, value 10 gets 1
        foreach (FoodItem food in gameData.availableFood)
        {
            int ticketCount = 10 / food.value;

            for (int i = 0; i < ticketCount; i++)
            {
                raffleTickets.Add(food);
            }
        } 

        // pull one ticket out of the bag
        int randomIndex = Random.Range(0, raffleTickets.Count);
            return raffleTickets[randomIndex];
    }

}
