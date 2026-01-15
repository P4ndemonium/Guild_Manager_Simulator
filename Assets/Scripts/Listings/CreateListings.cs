using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateListings : MonoBehaviour
{
    public GameObject AdventurerListing;
    public RectTransform canvas;
    public int spawnCount;

    void Start()
    {
        SpawnListings();
    }

    void SpawnListings()
    {
        spawnCount = Random.Range(3, 9);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newPanel = Instantiate(AdventurerListing, canvas);
            RectTransform panelRT = newPanel.GetComponent<RectTransform>();

            // Calculate safe boundaries
            float halfWidth = (canvas.rect.width - panelRT.rect.width) / 2;
            float halfHeight = (canvas.rect.height - panelRT.rect.height) / 2;

            float randomX = Random.Range(-halfWidth, halfWidth);
            float randomY = Random.Range(-halfHeight, halfHeight);

            panelRT.anchoredPosition = new Vector2(randomX, randomY);
        }
    }
}
