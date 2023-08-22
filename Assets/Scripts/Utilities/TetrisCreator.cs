using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisCreator : MonoBehaviour
{
    public TetrisCube tetrisCubePrefab;
    public GameObject columnPointPrefab;
    public GameObject rowPointPrefab;

    public List<TetrisCube> tetrisCubes;
    public List<Column> columnPoints;
    public List<Transform> rowPositions;

    public Transform ground;
    public Transform spawnPointsParent;
    public Transform tetrisCubesParent;
    public Transform rowParent;

    public Vector2 tetrisSize;

    public float tetrisCubeScale;

    public List<Color> colors;

    private void OnValidate()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            colors[i] = new Color(float.Parse(colors[i].r.ToString("F1")), float.Parse(colors[i].g.ToString("F1")),
                float.Parse(colors[i].b.ToString("F1")));
        }
    }

    private void OnEnable()
    {
        EventManager.CheckForRows += CheckForRows;
        EventManager.CubePainted += CubePainted;
        EventManager.SpawnCubeOnColumns += SpawnCubeOnColumns;
    }

    private void CheckForRows()
    {
        
        foreach (var row in rowPositions)
        {
            row.GetComponent<Row>().CheckForRow();
        }

        foreach (var sp in columnPoints)
        {
            sp.GetBoxes();
        }
        
    }

    private void CubePainted(TetrisCube obj)
    {
        tetrisCubes.Remove(obj);
    }

    private void OnDisable()
    {
        EventManager.CheckForRows -= CheckForRows;
        EventManager.CubePainted -= CubePainted;
        EventManager.SpawnCubeOnColumns -= SpawnCubeOnColumns;
    }

    [Button]
    private void SpawnCubeOnColumns()
    {
        var col = columnPoints.OrderBy(x => x.columnBoxes.Count).ToList();
        col.First().SpawnCube(tetrisCubePrefab.gameObject,tetrisCubesParent,tetrisCubeScale,colors[Random.Range(0, colors.Count)],tetrisCubes);
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
        
        CreateRowAndColumnTransforms((int)xAmount,(int)yAmount,border,bounds);
        

        for (int i = 0; i < yAmount; i++)
        {
            for (int j = 0; j < columnPoints.Count; j++)
            {
                var cube = Instantiate(tetrisCubePrefab.gameObject, Vector3.zero, quaternion.identity, tetrisCubesParent);
                cube.transform.localPosition =
                    new Vector3(columnPoints[j].transform.position.x, columnPoints[j].transform.position.y-(i*bounds.y*2), 0);
                cube.transform.localScale = new Vector3(xScale, xScale, xScale);
                var color = colors[Random.Range(0, colors.Count)];
                cube.GetComponent<TetrisCube>().color = color;
                cube.GetComponent<TetrisCube>().renderer.material.color = color;
                
                tetrisCubes.Add(cube.GetComponent<TetrisCube>());
            }
            
        }

        
    }

    public void CreateRowAndColumnTransforms(int xAmount, int yAmount, Vector3 border, Vector2 bounds)
    {
        for (int i = 0; i < yAmount; i++)
        {
            var row = Instantiate(rowPointPrefab, Vector3.zero, quaternion.identity, rowParent);
            row.transform.SetParent(rowParent);
            row.transform.position = new Vector3(ground.transform.position.x-10, (-border.y + tetrisCubeScale/2)+ i * tetrisCubeScale, ground.transform.position.z);
            rowPositions.Add(row.transform);
            row.GetComponent<Row>().columnMaxBoxAmount=(int)xAmount;
        }
        for (int i = 0; i < xAmount; i++)
        {
            var column = Instantiate(columnPointPrefab, Vector3.zero, quaternion.identity, spawnPointsParent);
            column.name = "Column_Point_" + i;
            column.transform.SetParent(spawnPointsParent);
            column.transform.localPosition = new Vector3((i * bounds.x * 2) - (bounds.x*(xAmount-1)) , border.y, 0);
            columnPoints.Add(column.GetComponent<Column>());
            column.GetComponent<Column>().columnMaxBoxAmount = (int)yAmount;
        }
    }

    private float timer;
    private float spawnTime = .3f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer>spawnTime)
        {
            //SpawnCubeOnColumns();
            timer = 0;
            spawnTime = Random.Range(.3f, .6f);

        }
    }

    [Button]
    public void GetColumnBoxes()
    {
        foreach (var point in columnPoints)
        {
            point.GetBoxes();
        }
    }
    private void Start()
    {
        CreateTetris();
    }
}