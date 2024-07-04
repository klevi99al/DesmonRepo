using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class EnemyMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public Rigidbody2D rb;
    protected Animator anim; 
    protected SpriteRenderer spriteRenderer;
    protected GameObject protagonist; 
    public bool begunDamage = false; 
    public GameObject attackArea;
    protected bool isFlipped = false;
    protected bool takingDamage = false;
    
    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(rb != null ) rb.freezeRotation = true;
        protagonist =  GameObject.Find("/Protagonist");

        GameObject gridGameObject = GameObject.Find("Grid");
        tilemap = gridGameObject.transform.Find("Floor").GetComponent<Tilemap>();  

        CustomStart();
    }

    protected virtual void CustomStart()
    {
        // This is an empty method in the base class
        // Specific enemy types will override this with their own logic
    }
  
    protected virtual void FlipRecursively(Transform currentTransform, Vector3 centralAxis, bool flipX)
    {
       if (currentTransform != null)
        {
            Vector3 currentScale = currentTransform.localScale;
            currentScale.x = Mathf.Abs(currentScale.x) * (flipX ? -1f : 1f);
            currentTransform.localScale = currentScale;
        }
    }

    protected virtual (bool leftEmpty, bool middleEmpty, bool rightEmpty) FloorBelow()
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        Vector3Int leftCell = cellPosition + new Vector3Int(-1,-1,0);
        Vector3Int rightCell = cellPosition + new Vector3Int(1, -1, 0);
        Vector3Int centerCell = cellPosition + new Vector3Int(0, -1, 0);



        bool leftEmpty = tilemap.GetTile(leftCell) != null;
        bool rightEmpty = tilemap.GetTile(rightCell) != null;
        bool middleEmpty = tilemap.GetTile(centerCell) != null;

        return (leftEmpty, middleEmpty, rightEmpty);
    }

    protected virtual bool IsTileEmpty(Vector3 directionVec)
    {
        
        Vector3Int targetCell = tilemap.WorldToCell(transform.position + directionVec * Time.deltaTime);
        return tilemap.HasTile(targetCell); // Returns true if the target cell is filled

    }

    protected virtual (bool leftEmpty, bool rightEmpty) AdjacentTilesEmpty()
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);

        Vector3Int leftCell = cellPosition + Vector3Int.left;
        Vector3Int rightCell = cellPosition + Vector3Int.right;

        bool leftEmpty = tilemap.GetTile(leftCell) == null;
        bool rightEmpty = tilemap.GetTile(rightCell) == null;

        return (leftEmpty, rightEmpty);
    }
    protected virtual bool IsProtagonistWithinTileBox(int tilesX,int tilesY)
    {
        if (protagonist != null)
        {
            Vector3Int protagonistCell = tilemap.WorldToCell(protagonist.transform.position);
            Vector3Int thisCell = tilemap.WorldToCell(transform.position);

            int distanceX = Mathf.Abs(protagonistCell.x - thisCell.x);
            int distanceY = Mathf.Abs(protagonistCell.y - thisCell.y);

            return distanceX <= tilesX && distanceY <= tilesY;
        }

        return false;
    }
    protected virtual bool IsProtagonistWithinXTiles(int tiles)
    {
        if (protagonist != null)
        {
            Vector3Int protagonistCell = tilemap.WorldToCell(protagonist.transform.position);
            Vector3Int thisCell = tilemap.WorldToCell(transform.position);

            int distanceX = Mathf.Abs(protagonistCell.x - thisCell.x);
            int distanceY = Mathf.Abs(protagonistCell.y - thisCell.y);

            return distanceX <= tiles && distanceY <= tiles;
        }
        return false;
    }
    protected virtual void FaceProtagonist()
    {
        // flip the object to face the protagonist
        if (protagonist != null)
        {
            // Get the positions of this object and the protagonist
            float thisX = transform.position.x;
            float protagonistX = protagonist.transform.position.x;

            // protagonist is to the left, game object is facing right 
            if (protagonistX < thisX && !isFlipped)
            {
                isFlipped = !isFlipped;
                Vector3 centralAxis = transform.position + spriteRenderer.bounds.center;
                FlipRecursively(transform, centralAxis, isFlipped); ; // Protagonist is to the left
            }
            // protagonist is to the right, game object is facing left 
            else if (protagonistX > thisX && isFlipped)
            {
                isFlipped = !isFlipped;
                Vector3 centralAxis = transform.position + spriteRenderer.bounds.center;
                FlipRecursively(transform, centralAxis, isFlipped); ; // Protagonist is to the left
            }
        
        }
    }
    public int GetProtagonistDirection()
    {
        if (protagonist != null)
        {
            // Get the positions of this object and the protagonist
            float thisX = transform.position.x;
            float protagonistX = protagonist.transform.position.x;

            // Check the relative position and return -1 for left, 1 for right
            if (protagonistX < thisX)
            {
                return -1; // Protagonist is to the left
            }
            else if (protagonistX > thisX)
            {
                return 1; // Protagonist is to the right
            }
        }

        // Return 0 if the protagonist is not assigned or at the same position
        return 0;
    }
    public void DamageBounce(float bounceX, float bounceY)

    {
        if ((protagonist.transform.position - this.transform.position).x > 0)
        {
            // positive: attacker is to right (bounce left )
            rb.velocity = new Vector2(-bounceX, bounceY);
        }
        else
        {
            // negative: attacker is to left (bounce right )
            rb.velocity = new Vector2(rb.velocity.x + bounceX, rb.velocity.y + bounceY);
        }
    }
    public bool ProtagonistAbove()
    {
        // returns if the protagonist is in the tiles above the enemy.
        // Get the position of the current game object and protagonist
        Vector3 currentObjectPosition = transform.position;
        Vector3 protagonistPosition = protagonist.transform.position;

        // Check if protagonist is directly above the current game object
        if (Mathf.FloorToInt(currentObjectPosition.x) == Mathf.FloorToInt(protagonistPosition.x) &&
            Mathf.FloorToInt(currentObjectPosition.y) + 1 == Mathf.FloorToInt(protagonistPosition.y) &&
            Mathf.FloorToInt(currentObjectPosition.z) == Mathf.FloorToInt(protagonistPosition.z))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CanJump(Vector3Int cellPosition, Vector2Int direction)
    {
        // Check the tile in the specified direction
        Vector3Int targetCell = cellPosition + new Vector3Int(direction.x, direction.y, 0);
        TileBase targetTile = tilemap.GetTile(targetCell);

        // Check if the target tile is filled and the tile above it is not filled
        bool isTargetTileFilled = targetTile != null;
        bool isTileAboveTargetEmpty = tilemap.GetTile(targetCell + new Vector3Int(0, 1, 0)) == null;

        return isTargetTileFilled && isTileAboveTargetEmpty;
    }
    private bool CanFall(Vector3Int cellPosition, Vector2Int direction)
    {
        // Check the tile in the specified direction
        Vector3Int targetCell = cellPosition + new Vector3Int(direction.x, direction.y, 0);

        // Check if the target tile is filled and the tile above it is not filled
        bool isTileAboveTargetEmpty = tilemap.GetTile(targetCell) == null;

        return !isTileAboveTargetEmpty;
    }
    public bool CanJumpRight()
    {
        // Get the position of your game object in world coordinates
        Vector3 worldPosition = transform.position;

        // Convert the world position to tile coordinates
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // Check if you can jump to the right
        return CanJump(cellPosition, new Vector2Int(1, 0));
    }
    public bool CanJumpLeft()
    {
        // Get the position of your game object in world coordinates
        Vector3 worldPosition = transform.position;

        // Convert the world position to tile coordinates
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // Check if you can jump to the left
        return CanJump(cellPosition, new Vector2Int(-1, 0));
    }
    public bool CanFallRight()
    {
        // Get the position of your game object in world coordinates
        Vector3 worldPosition = transform.position;

        // Convert the world position to tile coordinates
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        bool isRightEmpty = tilemap.GetTile(cellPosition + Vector3Int.right) == null;
        bool isDownRightEmpty = tilemap.GetTile(cellPosition + Vector3Int.right + 2 * Vector3Int.down) != null;
        // Check if you can jump to the right
        return isRightEmpty && isDownRightEmpty;

    }
    public bool CanFallLeft()
    {
        // Get the position of your game object in world coordinates
        Vector3 worldPosition = transform.position;

        // Convert the world position to tile coordinates
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        bool isLeftEmpty = tilemap.GetTile(cellPosition + Vector3Int.left) == null;
        bool isDownLeftEmpty = tilemap.GetTile(cellPosition + Vector3Int.left + 2*Vector3Int.down) != null;
        // Check if you can jump to the right
        return isLeftEmpty && isDownLeftEmpty;

    }
    public Vector3 GetRandomHorizontalTile()
    {
        Vector3Int currentCell = tilemap.WorldToCell(transform.position);

        // Calculate the range of reachable horizontal cells
        int minX = currentCell.x - 3;
        int maxX = currentCell.x + 3;

        // Randomly select a cell within the range
        int randomX = Random.Range(minX, maxX + 1);

        // Ensure the selected cell is not obstructed
        Vector3Int targetCell = new Vector3Int(randomX, currentCell.y, currentCell.z);
        while (tilemap.HasTile(targetCell) || !tilemap.HasTile(targetCell + Vector3Int.down) )
        {
            // Try again if the cell is obstructed
            randomX = Random.Range(minX, maxX + 1);
            targetCell = new Vector3Int(randomX, currentCell.y, currentCell.z);
        }

        // Return the center of the selected cell
        return tilemap.GetCellCenterWorld(targetCell);
    }
}
