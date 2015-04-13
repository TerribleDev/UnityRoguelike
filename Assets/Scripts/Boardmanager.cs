using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using Random = UnityEngine.Random;
public class Boardmanager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int Minimum;
        public int Maximum;

        public Count(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }

    }

    public int Columns = 8;
    public int Rows = 8;
    public Count WallCount = new Count(5,9);
    public Count FoodCount = new Count(1,5);
    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Transform _boardHolder;
    private List<Vector3> _gridPositions = new List<Vector3>();

    void InitializeList()
    {
        _gridPositions.Clear();
        for (int x = 1; x < Columns -1 ; x++)
        {
            for (int y = 1; y < Rows - 1; y++)
            {
                _gridPositions.Add(new Vector3(x,y,0f));
            }
        }
    }

    void BoardSetup()
    {
        _boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < Columns + 1; x++)
        {
            for (int y = 0; y < Rows + 1; y++)
            {
                GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];
                if (x == -1 || x == Columns || y == -1 || y == Rows)
                {
                    toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                if (instance == null)
                {
                    throw  new Exception("Unable to produce instance");
                }
                instance.transform.SetParent(_boardHolder);

            }
        }
    }

    Vector3 RandomPosition()
    {
        var randomIndex = Random.Range(0, _gridPositions.Count);
        var randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        var objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            var tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, RandomPosition(), Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum );
        LayoutObjectAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);
        var enemyCount = (int) Mathf.Log(level, 2f);
        LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);
        Instantiate(Exit, new Vector3(Columns - 1, Rows - 1, 0f), Quaternion.identity);
    }
}
