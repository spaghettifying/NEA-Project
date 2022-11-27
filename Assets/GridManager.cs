using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{

    public static int rows;
    public static int cols;
    public static int minPreyCount;
    public static int minPredatorCount;


    private string[,] grid = new string[rows, cols];
    private object[,] entityGrid = new object[rows, cols];


    private float tileSize = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //rows = first.rows;
        //cols = first.cols;
        Debug.ClearDeveloperConsole();
        Prey p = new Prey(10, 10, 10, 5, 25, 0, 10, "Bob", 3, 2);
        Predator pred = new Predator(10, 10, 10, 5, 25, 0, 10, "Pred", 3, 2);
        GenerateGrid();
        p.getBlockNeighbours(grid);
        createEntity(entityGrid, p, pred, 3, 2);
    }

    private void GenerateGrid()
    {
        //need to make minimum entity count because sometimes none spawn.

        //setting rules for generating non-grass tiles
        int barrierCount = 0;
        int maxBarrier = (rows * cols) / 5;
        int waterCount = 0;
        int maxWater = (rows * cols) / 5;
        int dotCount = 0;

        int maxDot = (rows * cols) / 10;

        //makes sure square
        //cols = 8 * (5 / rows);

        //makes scale nice
        float tileScale = (float) 5 / rows;
        tileSize = tileScale;


        //reference Tiles for ease of access later on
        GameObject referenceTileWater = (GameObject)Instantiate(Resources.Load("Water"));
        GameObject referenceTileGrass = (GameObject)Instantiate(Resources.Load("Grass"));
        GameObject referenceTileBarrier = (GameObject)Instantiate(Resources.Load("Barrier"));
        GameObject referenceTilePredator = (GameObject)Instantiate(Resources.Load("Predator"));
        GameObject referenceTilePrey = (GameObject)Instantiate(Resources.Load("Prey"));

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int tempRan = UnityEngine.Random.Range(1, 6);
                Debug.Log($"Position = ({col}, {row}), tempRan = {tempRan}");

                int anotherRan = UnityEngine.Random.Range(1, 10);

                //create barrier
                if (tempRan == 4)
                {
                    if (barrierCount < maxBarrier)
                    {
                        GameObject tileBarrier = (GameObject)Instantiate(referenceTileBarrier, transform);
                        float posX = col * tileSize;
                        float posY = row * -tileSize;

                        

                        tileBarrier.transform.localScale = new Vector2(tileScale, tileScale);
                        tileBarrier.transform.position = new Vector2(posX, posY);
                        tileBarrier.name = $"Barrier {posX} {posY}";
                        barrierCount += 1;

                        grid[row, col] = "B";
                    }
                    else
                    {
                        GameObject tileGrass = (GameObject)Instantiate(referenceTileGrass, transform);

                        float posX = col * tileSize;
                        float posY = row * -tileSize;

                        tileGrass.transform.localScale = new Vector2(tileScale, tileScale);
                        tileGrass.transform.position = new Vector2(posX, posY);
                        tileGrass.name = $"Grass {posX} {posY}";

                        grid[row, col] = "G";
                    }
                }
                //create water
                else if (tempRan == 5)
                {
                    if (waterCount < maxWater)
                    {
                        GameObject tileWater = (GameObject)Instantiate(referenceTileWater, transform);
                        float posX = col * tileSize;
                        float posY = row * -tileSize;

                        tileWater.transform.localScale = new Vector2(tileScale, tileScale);
                        tileWater.transform.position = new Vector2(posX, posY);
                        tileWater.name = $"Water {posX} {posY}";
                        waterCount += 1;

                        grid[row, col] = "W";
                    }
                    else
                    {
                        GameObject tileGrass = (GameObject)Instantiate(referenceTileGrass, transform);

                        float posX = col * tileSize;
                        float posY = row * -tileSize;

                        tileGrass.transform.localScale = new Vector2(tileScale, tileScale);
                        tileGrass.transform.position = new Vector2(posX, posY);
                        tileGrass.name = $"Grass {posX} {posY}";

                        grid[row, col] = "G";
                    }
                }
                //create entities
                //entities can only generate on grass tiles so grass tile is always created underneath
                else if (tempRan == 1 || tempRan == 2 || tempRan == 3)
                {
                    GameObject tileGrass = (GameObject)Instantiate(referenceTileGrass, transform);

                    float posX = col * tileSize;
                    float posY = row * -tileSize;

                    tileGrass.transform.localScale = new Vector2(tileScale, tileScale);
                    tileGrass.transform.position = new Vector2(posX, posY);
                    tileGrass.name = $"Grass {posX} {posY}";

                    grid[row, col] = "G";
                    if (anotherRan == 3)
                    {
                        
                    }
                }
                else
                {
                    Debug.Log($"tempRan = {tempRan}");
                }

                

            }
        }

        Destroy(referenceTileGrass);
        Destroy(referenceTileWater);
        Destroy(referenceTileBarrier);
        Destroy(referenceTilePredator);
        Destroy(referenceTilePrey);

        //camera
        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);
    }

    private object[,] createEntity(object[,] entityGridC, Prey entityPrey, Predator entityPredator, int posX, int posY)
    {

        entityGridC[posX, posY] = entityPrey;

        Debug.Log($"ENTITY AT {posX} {posY}, name {entityPrey.getName()} type: {entityGridC[posX, posY]}");

        return entityGridC;
    }

    IEnumerator moveDot()
    {
        float tileScale = (float)5 / rows;

        GameObject referenceTileDot = (GameObject)Instantiate(Resources.Load("Dot"));

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int randomMove = UnityEngine.Random.Range(1, 5);

                if (grid[row, col] == "D")
                {
                    if (randomMove == 1)
                    {
                        try
                        {
                            grid[row - 1, col] = "Dot";
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Exception thrown {e}");
                            Debug.Log($"Row = {row}, Col = {col}");
                            Debug.Log("Dot reached edge, skipping move");
                        }
                    }
                    else if (randomMove == 2)
                    {
                        try
                        {
                            grid[row, col - 1] = "Dot";
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Exception thrown {e}");
                            Debug.Log($"Row = {row}, Col = {col}");
                            Debug.Log("Dot reached edge, skipping move");
                        }
                    }
                    else if (randomMove == 3)
                    {
                        try
                        {
                            grid[row + 1, col] = "Dot";
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Exception thrown {e}");
                            Debug.Log($"Row = {row}, Col = {col}");
                            Debug.Log("Dot reached edge, skipping move");
                        }
                    }
                    else if (randomMove == 4)
                    {
                        try
                        {
                            grid[row, col + 1] = "Dot";
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Exception thrown {e}");
                            Debug.Log($"Row = {row}, Col = {col}");
                            Debug.Log("Dot reached edge, skipping move");
                        }
                    }
                    else
                    {
                        Debug.Log($"Error, randomMove was {randomMove}");
                    }

                    GameObject tileDot = (GameObject)Instantiate(referenceTileDot, transform);

                    float posXX = col * tileSize;
                    float posYY = row * -tileSize;

                    tileDot.transform.localScale = new Vector2(tileScale, tileScale);
                    tileDot.transform.position = new Vector2(posXX, posYY);
                    tileDot.name = $"Dot {posXX} {posYY}";
                    grid[row, col] = "D";

                }
            }
        }

        //wait for 2 seconds
        yield return new WaitForSeconds(2);
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(moveDot());
    }
}
