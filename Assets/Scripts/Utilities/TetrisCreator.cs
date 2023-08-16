using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisCreator : MonoBehaviour
{
    public TetrisCube tetrisCubePrefab;
    public GameObject spawnPointPrefab;

    public List<TetrisCube> tetrisCubes;
    public List<SpawnPoint> xSpawnPoints;
    public Transform ground;
    public Transform spawnPointsParent;
    public Transform tetrisCubesParent;

    public Vector2 tetrisSize;

    public float tetrisCubeScale;

    private void OnEnable()
    {
        EventManager.CubePainted += CubePainted;
        EventManager.SpawnCubeOnColumns += SpawnCubeOnColumns;
    }

    private void CubePainted(TetrisCube obj)
    {
        tetrisCubes.Remove(obj);
    }

    private void OnDisable()
    {
        EventManager.CubePainted -= CubePainted;
        EventManager.SpawnCubeOnColumns -= SpawnCubeOnColumns;
    }

    private void SpawnCubeOnColumns()
    {
        foreach (var spawnPoint in xSpawnPoints)
        {
            if (spawnPoint.columnBoxes.Count<spawnPoint.columnMaxBoxAmount)
            {
                for (int i = 0; i < spawnPoint.columnMaxBoxAmount-spawnPoint.columnBoxes.Count; i++)
                {
                    var cube = Instantiate(tetrisCubePrefab.gameObject, Vector3.zero, quaternion.identity, tetrisCubesParent);
                    cube.transform.localPosition =
                        new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + (i*tetrisCubeScale), 0);
                    cube.transform.localScale = Vector3.one*tetrisCubeScale;
                    if (Random.value>.5f)
                    {
                        var color = Color.red;
                        cube.GetComponent<TetrisCube>().color = color;
                        cube.GetComponent<TetrisCube>().renderer.material.color = color;
                    }
                    else
                    {
                        var color = Color.green;
                        cube.GetComponent<TetrisCube>().color = color;
                        cube.GetComponent<TetrisCube>().renderer.material.color = color;
                    }
                
                    tetrisCubes.Add(cube.GetComponent<TetrisCube>());
                }
            }
        }
    }

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

        tetrisCubeScale = xScale;
        
        var xAmount = tetrisSize.x;
        var yAmount = tetrisSize.y;

        ground.transform.position = new Vector3(ground.transform.position.x, -border.y, ground.transform.position.z);
        for (int i = 0; i < xAmount; i++)
        {
            var spawnPoint = Instantiate(spawnPointPrefab, Vector3.zero, quaternion.identity, spawnPointsParent);
            spawnPoint.name = "Spawn_Point_" + i;
            spawnPoint.transform.SetParent(spawnPointsParent);
            spawnPoint.transform.localPosition = new Vector3((i * bounds.x * 2) - (bounds.x*(xAmount-1)) , border.y, 0);
            xSpawnPoints.Add(spawnPoint.GetComponent<SpawnPoint>());
            spawnPoint.GetComponent<SpawnPoint>().columnMaxBoxAmount = (int)yAmount;
        }

        

        for (int i = 0; i < yAmount; i++)
        {
            for (int j = 0; j < xSpawnPoints.Count; j++)
            {
                var cube = Instantiate(tetrisCubePrefab.gameObject, Vector3.zero, quaternion.identity, tetrisCubesParent);
                cube.transform.localPosition =
                    new Vector3(xSpawnPoints[j].transform.position.x, xSpawnPoints[j].transform.position.y-(i*bounds.y*2), 0);
                cube.transform.localScale = new Vector3(xScale, xScale, xScale);
                if (Random.value>.5f)
                {
                    var color = Color.red;
                    cube.GetComponent<TetrisCube>().color = color;
                    cube.GetComponent<TetrisCube>().renderer.material.color = color;
                }
                else
                {
                    var color = Color.green;
                    cube.GetComponent<TetrisCube>().color = color;
                    cube.GetComponent<TetrisCube>().renderer.material.color = color;
                }
                
                tetrisCubes.Add(cube.GetComponent<TetrisCube>());
            }
            
        }

        
    }

    [Button]
    public void GetColumnBoxes()
    {
        foreach (var point in xSpawnPoints)
        {
            point.GetBoxes();
        }
    }
    private void Start()
    {
        CreateTetris();
    }
}