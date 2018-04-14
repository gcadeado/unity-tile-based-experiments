using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField]
    private MapGlobalReference globalReference;


    [Header("Tilemaps")]

    private Grid grid;

    [SerializeField]
    private Tilemap groundTilemap = null;

    [SerializeField]
    private Tilemap obstaclesTilemap = null;

    [SerializeField]
    private Tilemap voidTilemap = null;

    private void Awake()
    {
        grid = groundTilemap.layoutGrid;

        globalReference.RegisterMap(this);
    }

    public bool IsObstacle(Vector3 worldPos)
    {
        return HasTile(obstaclesTilemap, worldPos);
    }

    public bool IsVoid(Vector3 worldPos)
    {
        return HasTile(voidTilemap, worldPos);
    }

    public bool IsGround(Vector3 worldPos)
    {
        return HasTile(groundTilemap, worldPos);
    }


    /// <summary>
    /// Use raycasting to determine if the tile has a Lever
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public bool HasLever(Vector3 worldPos)
    {

        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector3.forward, 1f);

        foreach (RaycastHit2D hit in hits)
        {
            if (!hit)
                continue;

            if (hit.collider.gameObject.GetComponent<Lever>() != null)
                return true;

        }

        return false;
    }

    public bool CanMoveFromTo(Player player, Vector3 startPos, Vector3 endPos)
    {
        Vector3Int cellStartPos = grid.WorldToCell(startPos);
        Vector3Int cellEndPos = grid.WorldToCell(endPos);

        //Verify if the player can move to the target cell

        //Is it an adjacent cell ?
        if ((cellStartPos - cellStartPos).magnitude > 1f)
        {
            return false;
        }

        //Is it an obstacle ?
        if (IsObstacle(endPos))
        {
            return false;
        }


        // --- INTERACTABLE

        //test for obstacles objets (not belonging to tilemap)
        Interactable interactable = GetInteractable(endPos);

        //not null
        if (interactable)
        {
            if (!interactable.CanMoveTo(player))
            {
                return false;
            }
        }
        return true;
    }


    private Interactable GetInteractable(Vector3 worldPos)
    {
        RaycastHit2D hit;
        hit = Physics2D.Linecast(worldPos, worldPos);

        if (hit.collider == null)
        {
            return null;
        }
        return hit.collider.GetComponent<Interactable>();
    }

    private bool HasTile(Tilemap tilemap, Vector3 worldCellPos)
    {
        //Get Tile =/= null ?
        return tilemap.HasTile(tilemap.WorldToCell(worldCellPos));
    }
}
