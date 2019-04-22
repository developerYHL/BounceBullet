using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BuildingSystem : MonoBehaviour
{
    private Grid grid;

    [SerializeField]
    private Camera playerCamera;

    private bool buildModeOn = false;
    private bool canBuild = false;

    private BlockSystem bSys;

    [SerializeField]
    private LayerMask buildableSurfacesLayer;

    private Vector3 buildPos;

    private GameObject currentTemplateBlock;

    [SerializeField]
    private GameObject blockTemplatePrefab;
    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private Material templateMaterial;

    private int blockSelectCounter = 0;

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
        bSys = GetComponent<BlockSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            buildModeOn = !buildModeOn;

            if (buildModeOn)
            {
                //Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (Input.GetKeyDown("r"))
        {
            blockSelectCounter++;
            if (blockSelectCounter >= bSys.allBlocks.Count) blockSelectCounter = 0;
        }

        if (buildModeOn)
        {
            RaycastHit buildPosHit;

            if (Physics.Raycast(playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out buildPosHit, 10, buildableSurfacesLayer))
            {
                Vector3 point = buildPosHit.point;
                buildPos = transform.position + transform.forward *1.0f; //Mathf.Round(point.x), Mathf.Round(point.y), Mathf.Round(point.z));
                canBuild = true;
            }
            else
            {
                Destroy(currentTemplateBlock.gameObject);
                canBuild = false;
            }
        }

        if (!buildModeOn && currentTemplateBlock != null)
        {
            Destroy(currentTemplateBlock.gameObject);
            canBuild = false;
        }

        if (canBuild && currentTemplateBlock == null)
        {
            currentTemplateBlock = Instantiate(blockTemplatePrefab, buildPos, Quaternion.identity);
            currentTemplateBlock.GetComponent<MeshRenderer>().material = templateMaterial;
        }

        if (canBuild && currentTemplateBlock != null)
        {
            currentTemplateBlock.transform.position = PlaceCubeNear(buildPos);

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBlock();
            }
        }
    }

    private void PlaceBlock()
    {
        GameObject newBlock = Instantiate(blockPrefab, PlaceCubeNear(buildPos), Quaternion.identity);
        Block tempBlock = bSys.allBlocks[blockSelectCounter];
        newBlock.name = tempBlock.blockName + "-Block";
        newBlock.GetComponent<MeshRenderer>().material = tempBlock.blockMaterial;
    }

    private Vector3 PlaceCubeNear(Vector3 clickPoint) {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;

        return finalPosition;
        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = nearPoint;
    }
}
