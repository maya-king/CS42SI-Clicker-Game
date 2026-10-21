using UnityEngine;

// Goes on the Rat prefab. Each rat walks to the food, grabs it,
// walks off screen, then delivers the food to the inventory.
public class Rat : MonoBehaviour
{
    // subject to change
    public float moveSpeed = 3f;

    // filled in my ClickHandler
    public Vector3 foodPosition;
    public Vector3 exitPosition;
    public FoodItem food;
    public Inventory inventory;

    private bool hasFood = false;

    // update called once every frame
    private void Update()
    {
        if (hasFood == false)
        {
            // walk to the food
            walkTo(foodPosition);

            if (transform.position == foodPosition)
            {
                attachFood();
                hasFood = true;
            }
        }
        else
        {
            // walk off screen with the food
            walkTo(exitPosition);

            if (transform.position == exitPosition)
            {
                inventory.addFood(food);
                Destroy(gameObject);
            }
        }
    }

    // take one step toward the destination
    private void walkTo(Vector3 destination)
    {
        float stepSize = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, stepSize);
    }

    // attaches food sprite to the rat 
    private void attachFood()
    {
        Transform anchor = transform.Find("FoodAnchor");

        GameObject foodObject = new GameObject(food.name);
        foodObject.transform.SetParent(anchor, false);

        SpriteRenderer foodRenderer = foodObject.AddComponent<SpriteRenderer>();
        foodRenderer.sprite = food.sprite;
        // food on top of rat
        foodRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
    }
}