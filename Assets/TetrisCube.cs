using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisCube : MonoBehaviour
{
    public Collider collider;

    public Renderer renderer;
    public Color color;
    public Vector3 bounds;

    public List<TetrisCube> neighbours;

    [Button]
    public void SetColor()
    {
        color = renderer.material.color;

    }
    [Button]
    public void RandomColor()
    {
        color = Random.ColorHSV();
            renderer.material.color = color;
    }
    [Button]
    public void SetBound()
    {
        bounds = collider.bounds.size;
    }


    [Button]
    public void GetNeighbours(List<TetrisCube> sameColorCubes)
    {
        if (!sameColorCubes.Contains(this))
        {
            sameColorCubes.Add(this);
            neighbours.Clear();
            List<Collider> hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity).ToList();
            int i = 0;
            while (i < hitColliders.Count)
            {
                if (hitColliders[i].GetComponent<TetrisCube>() && hitColliders[i].GetComponent<TetrisCube>()!=this)
                {
                    neighbours.Add(hitColliders[i].GetComponent<TetrisCube>());
                }
                i++;
            }

            foreach (var neighbour in neighbours)
            {
                if (neighbour.color==color)
                {
                    sameColorCubes.Add(neighbour);
                }
            }

            foreach (var cube in sameColorCubes)
            {
                cube.GetNeighbours(sameColorCubes);
            }
        }
        
    }
    
    
    
}
