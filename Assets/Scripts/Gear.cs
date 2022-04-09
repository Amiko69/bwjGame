using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gear : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Vector2 center = new Vector2 (1000, 1000);
    
    int distance = 500;
    int angle = 0;

    public List <Vector2> vertices = new List<Vector2>();
    public List <ushort> triangles = new List<ushort>();
    List <Vector2> verticesAux = new List <Vector2>();

    public Gear RandomizeGear()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var texture2D = new Texture2D(2000, 2000);
        spriteRenderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 100);

        vertices.Add(center);
        for (int i = 0 ; i < 12; i++)
        {
            GenerateNextPoint();
            if (i != 0)
            {
                GenerateNextTriangle();
            }
        }
        GenerateLastTriangle();

        ChangeSprite();
        EditCollider();

        return this;
    }

    void GenerateNextPoint()
    {
        float x;
        float y;
        // randomize distance
        // randomize angle

        x = center.x + (distance * Mathf.Cos(angle * Mathf.PI / 180));
        y = center.y + (distance * Mathf.Sin(angle * Mathf.PI / 180));
        angle += 30;

        Debug.Log(x + ", " + y);
        vertices.Add(new Vector2(x, y));
    }

    void GenerateNextTriangle()
    {
        triangles.Add(0);
        triangles.Add((ushort)(vertices.Count - 1));
        triangles.Add((ushort)(vertices.Count - 2));
    }
    
    void GenerateLastTriangle()
    {
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add((ushort)(vertices.Count - 1));
    }

    void ChangeSprite()
    {
        spriteRenderer.sprite.OverrideGeometry(vertices.ToArray(), triangles.ToArray());
    }

    void EditCollider()
    {
        vertices.RemoveAt(0);
        foreach (Vector2 vertice in vertices)
        {
            verticesAux.Add(vertice / 100);
        }

        GetComponent<PolygonCollider2D>().points = verticesAux.ToArray();
    }
}
