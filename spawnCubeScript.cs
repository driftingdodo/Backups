using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawnCubeScript : MonoBehaviour {


    public Transform prefab;
    public Transform ground;
    public Transform player;

    //spawn settings
    public int spacing = 20;
    public int minElements = 1;
    public int maxElements = 10;

    //other spawn releated variables
    private int trackWidth;
    private int rows = 10;
    private int rowOffset = 0;
    private float firstRowPosition = 50;

    Transform[,] map;

    // Start is called before the first frame update
    void Start()
    {
        //test set up a block
        //Transform temp = Instantiate(prefab, new Vector3(0, 1, firstRowPosition), Quaternion.identity);

        trackWidth = (int)ground.localScale.x;
        
        //create map array to hold the block positions
        map = new Transform[rows, trackWidth];

        
        //generate map
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        //dertermine if we have gine far enough to move the last row forwards
        if (player.position.z > firstRowPosition + 2*spacing)
        {
            int currentRow = rowOffset % rows; //detemines which row in the map array we need to work on
            //move the coordinates of the last row (currentrow) forwards
            MoveRow(currentRow);
            GenerateRow(currentRow);
            rowOffset++;
        }

        //check if we have gone far enough to move the world backwards
        if (player.position.z > 9000) MoveTheWorld();
        
    }
    void GenerateMap() 
    {

        //fill the map
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < trackWidth; j++)
            {
                map[i, j] = Instantiate(prefab, new Vector3(j - trackWidth / 2, 1, firstRowPosition + i * spacing), Quaternion.identity);
            }
            //now that the line is filled with objects, make some holes in it
            GenerateRow(i);
        }
        
        
    }
    void GenerateRow(int r)
    {
        

        //determine the structure of the row, and turn on active objects
        int nrOfObjects = Random.Range(minElements, maxElements);
        for (int j = 0; j < trackWidth; j++)
        {
            if (Random.Range(0, 100) < 30 && nrOfObjects > 0)
            {
                map[r, j].GetComponent<MeshRenderer>().enabled = true;
                map[r, j].GetComponent<BoxCollider>().enabled = true;
                nrOfObjects--;
            }else
            {
                //if they are not turned on, then theyre off
                map[r, j].GetComponent<MeshRenderer>().enabled = false;
                map[r, j].GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
    
    void MoveRow(int r)
    {
        //move all blocks in the row forwards by the number of rows times the spacing between them (all the way to the front)
        for (int j = 0; j < trackWidth; j++)
        {
            map[r, j].position = new Vector3(j - trackWidth / 2, 1, firstRowPosition + rows * spacing);
        }
        //update where the first row is
        firstRowPosition += spacing;
    }

    void MoveTheWorld()
    {
        //move the world backwards
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < trackWidth; j++)
            {
                map[i, j].position -= new Vector3(0,0,player.position.z);
            }
        }
        firstRowPosition -= player.localPosition.z;
        player.position -= new Vector3(0, 0, player.position.z);
    }

}
