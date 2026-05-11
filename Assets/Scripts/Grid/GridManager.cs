using UnityEngine;
public class GridManager : MonoBehaviour
{
    [Header("Grid Size")]
    public int gridWidth = 4;
    public int gridHeight = 4;
    public float cellSize = 2f;
    [Header("Obstacles")]
    public Vector2Int[] obstaclePositions = new Vector2Int[]{ new Vector2Int(1,1), new Vector2Int(2,0), new Vector2Int(3,2), new Vector2Int(0,3) };
    [Header("Kenney Prefabs - drag from Resources/GridAssets")]
    public GameObject floorPrefab;
    public GameObject obstaclePrefab;
    public GameObject borderStraightPrefab;
    public GameObject borderCornerPrefab;
    public Texture2D colormapTexture;
    private bool[,] _blocked;
    public static GridManager Instance { get; private set; }
    private Material _mat;
    private const float FS = 1.8f;
    private const float OS = 1.4f;
    private const float BS = 1.8f;
    void Awake(){ Instance=this; _blocked=new bool[gridWidth,gridHeight]; BuildMat(); }
    void BuildMat(){
        Shader sh = Shader.Find("Universal Render Pipeline/Lit");
        if(sh==null) sh = Shader.Find("Standard");
        if(sh==null) sh = Shader.Find("Diffuse");
        _mat=new Material(sh); _mat.name="KenneyMat";
        Texture2D tx = colormapTexture;
        if(tx==null) tx = Resources.Load<Texture2D>("GridAssets/colormap");
        if(tx!=null) _mat.mainTexture=tx;
    }
    void Start(){
        if(floorPrefab==null) floorPrefab=Resources.Load<GameObject>("GridAssets/floor");
        if(obstaclePrefab==null) obstaclePrefab=Resources.Load<GameObject>("GridAssets/column");
        if(borderStraightPrefab==null) borderStraightPrefab=Resources.Load<GameObject>("GridAssets/border-straight");
        if(borderCornerPrefab==null) borderCornerPrefab=Resources.Load<GameObject>("GridAssets/border-corner");
        if(floorPrefab==null){Debug.LogError("floor prefab missing");return;}
        BuildGrid(); BuildObstacles();
        if(borderStraightPrefab!=null && borderCornerPrefab!=null) BuildBorders();
    }
    void BuildGrid(){
        var p=new GameObject("_Tiles"); p.transform.parent=transform;
        for(int x=0;x<gridWidth;x++) for(int z=0;z<gridHeight;z++){
            var t=Instantiate(floorPrefab,GridToWorld(new Vector2Int(x,z)),Quaternion.identity,p.transform);
            t.name="Tile_"+x+"_"+z;
            t.transform.localScale=Vector3.one*FS;
            ApplyMat(t);
            BoxCollider bc = t.GetComponent<BoxCollider>();
            if(bc==null) bc = t.AddComponent<BoxCollider>();
            bc.size=new Vector3(cellSize*0.9f,0.1f,cellSize*0.9f);
        }
    }
    void BuildObstacles(){
        if(obstaclePrefab==null) return;
        var p=new GameObject("_Obstacles"); p.transform.parent=transform;
        foreach(var c in obstaclePositions){
            if(!IsInBounds(c)) continue;
            _blocked[c.x,c.y]=true;
            var o=Instantiate(obstaclePrefab,GridToWorld(c),Quaternion.identity,p.transform);
            o.name="Obs_"+c.x+"_"+c.y;
            o.transform.localScale=Vector3.one*OS;
            ApplyMat(o);
            BoxCollider bc = o.GetComponent<BoxCollider>();
            if(bc==null) bc = o.AddComponent<BoxCollider>();
            bc.size=new Vector3(cellSize*0.8f,2f,cellSize*0.8f);
        }
    }
    void BuildBorders(){
        var p=new GameObject("_Borders"); p.transform.parent=transform;
        float h=(gridWidth-1)*cellSize/2f, off=cellSize*0.6f;
        for(int i=0;i<gridWidth;i++){
            float t=i*cellSize-h;
            Sb(borderStraightPrefab,new Vector3(t,0,h+off),0,p);
            Sb(borderStraightPrefab,new Vector3(t,0,-h-off),180,p);
            Sb(borderStraightPrefab,new Vector3(h+off,0,t),90,p);
            Sb(borderStraightPrefab,new Vector3(-h-off,0,t),270,p);
        }
        float co=h+off;
        Sb(borderCornerPrefab,new Vector3(co,0,co),0,p);
        Sb(borderCornerPrefab,new Vector3(-co,0,co),90,p);
        Sb(borderCornerPrefab,new Vector3(-co,0,-co),180,p);
        Sb(borderCornerPrefab,new Vector3(co,0,-co),270,p);
    }
    void Sb(GameObject pf,Vector3 pos,float ry,GameObject par){
        if(pf==null) return;
        var b=Instantiate(pf,pos,Quaternion.Euler(0,ry,0),par.transform);
        b.transform.localScale=Vector3.one*BS;
        ApplyMat(b);
    }
    void ApplyMat(GameObject obj){
        foreach(var r in obj.GetComponentsInChildren<Renderer>(true))
            r.material=_mat;
    }
    public Vector3 GridToWorld(Vector2Int c){
        return new Vector3(c.x*cellSize-(gridWidth-1)*cellSize/2f, 0, c.y*cellSize-(gridHeight-1)*cellSize/2f);
    }
    public bool IsInBounds(Vector2Int c){ return c.x>=0&&c.x<gridWidth&&c.y>=0&&c.y<gridHeight; }
    public bool IsBlocked(Vector2Int c){ if(!IsInBounds(c))return true; return _blocked[c.x,c.y]; }
}
