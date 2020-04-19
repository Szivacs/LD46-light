using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathPoint
{
    public Vector2 position;
    public Vector2 left;
    public Vector2 right;
}

public class Path : MonoBehaviour
{
    //[HideInInspector]
    public List<PathPoint> points = new List<PathPoint>();

    public Vector2 Evaluate(int i, int j, float t)
    {
        Vector2 p0 = Vector2.Lerp(Vector2.Lerp(points[i].position, points[i].right, t), Vector2.Lerp(points[i].right, points[j].left, t), t);
        Vector2 p1 = Vector2.Lerp(Vector2.Lerp(points[i].right, points[j].left, t), Vector2.Lerp(points[j].left, points[j].position, t), t);
        return Vector2.Lerp(p0, p1, t);
    }

    public void Add(Vector2 pos)
    {
        PathPoint p = new PathPoint();
        p.position = pos;
        p.left = pos + Vector2.left;
        p.right = pos + Vector2.right;
        points.Add(p);
    }

    public void Move(int i, Vector2 pos)
    {
        Vector2 offset = pos - points[i].position;
        points[i].position += offset;
        points[i].left += offset;
        points[i].right += offset;

    }
}
