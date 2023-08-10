using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class TetrisCreator : MonoBehaviour
{
    public TetrisCube tetrisCubePrefab;

    public List<TetrisCube> tetrisCubes;
    public List<Transform> xSpawnPoints;
    public Transform ground;
    public Transform spawnPointsParent;
    public Transform tetrisCubesParent;

    public Vector2 tetrisSize;
    [Button]
    public void CreateTetris()
    {
        foreach (var cube in tetrisCubes)
        {
            Destroy(cube);
        }

        tetrisCubes.Clear();
        
        
        
        var border = EventManager.GetBorders();
        border -= new Vector3(1, 1, 0);
        var xScale = (border.x * 2) / tetrisSize.x;
        var yScale = (border.y * 2) / tetrisSize.y;
        if (yScale<xScale)
        {
            xScale = yScale;
        }
        else
        {
            yScale = xScale;
        }

        var bounds = new Vector2(xScale / 2,yScale/2);

        Debug.Log(bounds);
        
        var xAmount = tetrisSize.x;
        var yAmount = tetrisSize.y;

        ground.transform.position = new Vector3(ground.transform.position.x, -border.y, ground.transform.position.z);
        for (int i = 0; i < xAmount; i++)
        {
            var spawnPoint = new GameObject();
            spawnPoint.name = "Spawn_Point_" + i;
            spawnPoint.transform.SetParent(spawnPointsParent);
            spawnPoint.transform.localPosition = new Vector3((i * bounds.x * 2) - (bounds.x*(xAmount-1)) , border.y, 0);
            xSpawnPoints.Add(spawnPoint.transform);
        }

        

        for (int i = 0; i < yAmount; i++)
        {
            for (int j = 0; j < xSpawnPoints.Count; j++)
            {
                var cube = Instantiate(tetrisCubePrefab.gameObject, Vector3.zero, quaternion.identity, tetrisCubesParent);
                cube.transform.localPosition =
                    new Vector3(xSpawnPoints[j].position.x, xSpawnPoints[j].position.y+(i*bounds.y*2), 0);
                cube.transform.localScale = new Vector3(xScale, xScale, 1);
                tetrisCubes.Add(cube.GetComponent<TetrisCube>());
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}