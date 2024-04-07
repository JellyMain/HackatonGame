using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] int maxTrash;

    [SerializeField] List<GameObject> trash = new List<GameObject>();

    [SerializeField] Vector2 mapSize = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < maxTrash; i++)
        {
            GameObject fish = Instantiate(
                trash[Random.Range(0, trash.Count)],
                new Vector3(
                    RandomRange(-mapSize.x, mapSize.x),
                    RandomRange(-mapSize.y, mapSize.y), 0),
                Quaternion.identity);
            fish.transform.parent = transform;
            fish.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount < maxTrash)
        {
            GameObject fish = Instantiate(
                trash[Random.Range(0, trash.Count)],
                new Vector3(
                    RandomRange(-mapSize.x, mapSize.x),
                    RandomRange(-mapSize.y, mapSize.y), 0),
                Quaternion.identity);
            fish.transform.parent = transform;
            fish.gameObject.SetActive(true);
        }
    }


    private float RandomRange(float a, float b)
    {
        /*const int numberOfTries = 2;
        float bias = 0f;

        for(int i = 0; i < numberOfTries; i++)
        {
            bias += Random.value;
        }

        bias /= numberOfTries;*/

        return Random.Range(a, b);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, mapSize * 2);
    }
}
