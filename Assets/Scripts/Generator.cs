using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public Path path;
    public GameObject[] polygons;
    public GameObject[] obstacles;
    public GameObject[] structures;
    public GameObject connector;
    public float width = 20.0f;
    public Transform staticObjectHolder;
    public Transform dynamicObjectHolder;

    Queue< List<GameObject> > mapQueue = new Queue< List<GameObject> >();

    float Angle(Vector2 p1, Vector2 p2)
    {
        return Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
    }

    private void Start()
    {
        StartCoroutine(GenerateSegment(1));
    }

    public IEnumerator GenerateSegment(int i)
    {
        if (i >= 1 && i < path.points.Count - 2)
        {

            if (mapQueue.Count > 2)
            {
                foreach (GameObject go in mapQueue.Peek())
                {
                    Destroy(go);
                }
                mapQueue.Dequeue();
            }

            List<GameObject> map = new List<GameObject>();

            float noiseOffset = Random.Range(0, 100.0f);

            for (float t = 0.0f; t < 1.0f; t += 0.07f)
            {
                float noise = Mathf.PerlinNoise(i + t, noiseOffset) + 0.75f;
                Vector2 current = path.Evaluate(i, i + 1, t);
                Vector2 diff = (current - path.Evaluate(i, i + 1, t + 0.1f)).normalized;
                Vector2 perpendicularLeft = new Vector2(-diff.y, diff.x) * width / 2 * noise;
                map.Add(Instantiate(polygons[Random.Range(0, polygons.Length)], current + perpendicularLeft + Random.insideUnitCircle, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)), staticObjectHolder));

                Vector2 perpendicularRight = new Vector2(diff.y, -diff.x) * width / 2 * noise;
                map.Add(Instantiate(polygons[Random.Range(0, polygons.Length)], current + perpendicularRight + Random.insideUnitCircle, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)), staticObjectHolder));

                if (Random.Range(0, 100) < 25)
                {
                    int numDynamic = Random.Range(1, 3);
                    for (int j = 0; j < numDynamic; j++)
                    {
                        GameObject go = Instantiate(obstacles[Random.Range(0, obstacles.Length)], current + Random.insideUnitCircle * 3, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)), dynamicObjectHolder);
                        go.transform.localScale = new Vector3(Random.Range(1.0f, 3.0f), Random.Range(1.0f, 3.0f), 1);
                    }
                }

                if (noise > 1.2f && Random.Range(0, 100) < 10)
                {
                    map.Add(Instantiate(structures[Random.Range(0, structures.Length)], current + perpendicularLeft * 0.75f + Random.insideUnitCircle, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)), staticObjectHolder));
                }

                if (Random.Range(0, 100) < 30)
                {
                    int num = Random.Range(1, 3);
                    for (int j = 0; j < num; j++)
                    {
                        GameObject go = Instantiate(connector, current + Random.insideUnitCircle, Quaternion.Euler(0, 0, Angle(Vector2.zero, diff) + Random.Range(70.0f, 110.0f)), staticObjectHolder);
                        go.transform.localScale = new Vector3(Vector2.Distance(current + perpendicularLeft, current + perpendicularRight), go.transform.localScale.y, 1.0f);
                        map.Add(go);
                    }
                }
            }

            mapQueue.Enqueue(map);
        }
        yield return this;
    }
}
