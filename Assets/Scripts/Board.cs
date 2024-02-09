using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public bool midLevelChange;
    public GameObject GM;
    public GMScript gmScript;
    public GameObject MM;
    public float pieceSpeed;

    //define array of tetromino data to customise in editor
    public TetrominoData[] tetrominoes;
    //need to refrence our tile map
    public Tilemap tilemap {get; private set;}
    public Piece activePiece {get; private set;}
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10,20);

    public bool gamePaused = false;


    // use RectInt as it has a useful built in function we can use. 
    public RectInt Bounds // htis is a c# property. need position (bottom left) & size.
        {
            get{ //offsetting by half size of board to move from 0,0, centre to bottom corner.
                Vector2Int position = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2);
                return new RectInt(position, boardSize);
            }
        }

    private void Awake(){
        gmScript = GM.GetComponent<GMScript>();
        //get tilemap which is on child of this 'board'
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        //need to initialise all of the data
        for (int i = 0; i < this.tetrominoes.Length; i++){
            this.tetrominoes[i].Initialise();
        }
    }


    public void Start ()
    {
        SpawnPiece();
    }


    public void SpawnPiece()
    {
        if (!midLevelChange && !gamePaused){
        //need random tetromino, get one of our structs & assoc data.
        // tile is not set, this is set after.
        int random = Random.Range(0,tetrominoes.Length);
        TetrominoData data = tetrominoes[random];
        // set tile in this dataset, from the level tile set the GM is holding.
        int blockColorInt = Random.Range(0,3);
        data.tile = gmScript.currentlevelData.levelTiles[blockColorInt];
        // THIS IS WHERE SET TILE TYPE/COLOR
        pieceSpeed = gmScript.currentlevelData.levelStepDelay;

        //this instantiates new active piece.
        this.activePiece.Initialise(this,this.spawnPosition, data, blockColorInt, pieceSpeed);

        if (IsValidPosition(activePiece, spawnPosition)){
            Set(activePiece);
        } else{
            gamePaused = true;
            Destroy(activePiece);
            StartCoroutine(GameOverLost()); //this & methods copied from tutorial git
            }
        }
    }

    IEnumerator GameOverLost(){
        tilemap.ClearAllTiles();
        RectInt bounds = this.Bounds;
        Debug.Log("row min:"+bounds.yMin+"row max:"+bounds.yMax);
        int colorInt = 1;
        for (int row = bounds.yMin; row < bounds.yMax; row++){
            Tile endingRowTile = gmScript.currentlevelData.levelTiles[(colorInt%3)];
        for (int col = bounds.xMin ; col < bounds.xMax; col++){
                Vector3Int position = new Vector3Int(col, row, 0);
                //Debug.Log("im setting row"+row+"col: "+col+" Tile colour: "+ colorInt%3);
                tilemap.SetTile(position, endingRowTile); //set tile of same colour per row.
            }
            colorInt++;
            yield return new WaitForSeconds(0.05f);
        }
    }
 
   // public void GameOverLost(){
        //tilemap.ClearAllTiles(); 
   

    public void Set(Piece piece)
    {
        for (int i = 0; i< piece.cells.Length; i++){
            //set tile on tilemap
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //but need to offset based on position of piece.
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }

    }

    //a copy of set but with putting null in instead.
        public void Clear(Piece piece)
    {
        for (int i = 0; i< piece.cells.Length; i++){
            //set tile on tilemap
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //but need to offset based on position of piece.
            this.tilemap.SetTile(tilePosition, null);
        }
    }


    public bool IsValidPosition(Piece piece, Vector3Int checkposition)
    {
        RectInt bounds = this.Bounds;

        //need to test each cell of the piece is valid.
        for (int i = 0; i < piece.cells.Length ;i++){
            Vector3Int tilePosition = piece.cells[i] + checkposition;

            if (!bounds.Contains((Vector2Int)tilePosition)){ //if tile position is not in bounds.
                return false;
            }
            if (this.tilemap.HasTile(tilePosition)){ //already a tile in position.
                return false;
            }
        }
        return true;
    }


    public void ClearLines(){
        if (!gamePaused){
        //loop through every row in tile map, & check if all cols full.
        // if so, is full & we then clear & shift everything down.
        // iterate from bottom row to top, can use our bounds rect
        int completedRowsThisRound = 0;
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        while (row < bounds.yMax)
        {
            //check if row full
            if (IsLineFull(row))
            {
                completedRowsThisRound++; //LEVEL IS LEVEL BEFORE THE LINE CLEAR. TODO ensure this is correct order.
                                            //ITS NOT. ?recursive to count before deletes?
                LineClear(row); //do not iterate the row if we do a line clear.
            } else {
                row++; //ONLY IF CLEAR. as need to retest, as tiles will fall to this row.
            }
        }
        ScoreRound(completedRowsThisRound);
        completedRowsThisRound = 0; //reset counter.
        }
    }


    void ScoreRound(int completedRows){
        switch (completedRows){
            case 4:
                //TETRIS
                gmScript.updateScore(800);
                break;
            case 3:
                gmScript.updateScore(500);
                break;
            case 2:
                gmScript.updateScore(300);
                break;
            case 1: 
                //single row
                gmScript.updateScore(100);
                break;
            default: 
                break;
        }
    }


    public bool IsLineFull(int row){
        //iterates through cols to check if row full
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin ; col < bounds.xMax; col++){
            Vector3Int position = new Vector3Int(col, row, 0);//tile position we are checking
            if (!this.tilemap.HasTile(position)){
                //is not full
                return false;
            }
        }
        //every tile is set
        return true;
    }


    //  TODO THIS ISNT WORKING THE WAY I WANT IT TO.
    // WHY IS IT NOT WAITING.
   public void LineClear(int row){
        RectInt bounds = this.Bounds;
            Debug.Log("doin a line clear");
        //split this into two functions & then IEnumerator for animation/deletion?
        // HOW WILL I GET THESE TO RUN AT THE SAME TIME.
        //going from middle to beginning.

        // for (int col = bounds.xMin ; col < bounds.xMax; col++)
        // {
        //     Debug.Log("forloop in line clear");
        //     Vector3Int position = new Vector3Int(col, row, 0);
        //     this.tilemap.SetTile(position, null); //delete tile.
        //     yield return new WaitForSeconds(0.1f);

        // }

        for (int col = -1 ; col >= bounds.xMin; col--)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null); //delete tile.
           // yield return new WaitForSeconds(0.1f);
        }

        //going from middle to end
        for (int col = 0 ; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null); //delete tile.
           // yield return new WaitForSeconds(0.1f);
        }

        //Update line counters in GM.
        gmScript.levellineCounter++;
        gmScript.gameLineCounter++;

        //has to be here as holds the next piece from dropping too fast (without first changing color)
        if (gmScript.levellineCounter >= gmScript.currentlevelData.goalLines){
            midLevelChange = true;
            gmScript.ChangeLevel();
            midLevelChange = false;
        }

        //allows falling
        // INCORRECT CODE. THE TILES DROP ONTO THIS ROW, REGARDLESS OF THE TILES DISAPPEARING FIRST!
        //TODO!!!!
        // should this be separate function called after the line is cleared?
        //does work in correct order as is.
        //or add a check for hasTile? 
        
        while (row < bounds.yMax)
        {
            //Debug.Log("falling tile while loop for row");
            for (int col = bounds.xMin ; col < bounds.xMax; col++){

                //get position of tile, but get the one above it.
                Vector3Int position = new Vector3Int(col, row+1, 0); //get position of the tile
                //if (!tilemap.HasTile(position)) // if this tile is null...

                TileBase above = this.tilemap.GetTile(position);
                position = new Vector3Int(col, row,0); //back to our current row.
                this.tilemap.SetTile(position, above); //set spot to tile which is above.
                }   
            row++;
        }
    }
   }

