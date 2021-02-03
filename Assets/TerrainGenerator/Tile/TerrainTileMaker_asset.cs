using UnityEngine;
using BP.Core;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    [CreateAssetMenu(fileName ="terrainTileMaker",menuName ="Terrain Generator/(single) Terrain Tile Maker")]
    public class TerrainTileMaker_asset : ScriptableObject
    {
        [SerializeField] private TerrainVars_asset m_terrainVars = null;

        private Vector2Int tileSize;

        private Transform m_parentFolder;

        //prefab
        private Vector3[] m_vertices;
        private int[] m_triangles;
        private TerrainTile m_prefabTile;
        private GameObject m_prefabGO;
        private GameObject m_prefabMeshGO;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private Mesh m_mesh;

        public TerrainVars_asset TerrainVars() { return m_terrainVars; }
        public void SetParent(Transform parentFolder) { m_parentFolder = parentFolder; }
        public GameObject GetTilePrefab() { return m_prefabGO; }
        public Vector2Int TileSize()
        {
            return tileSize;
        }

        #region coords helpers
        public Vector2Int ConvertWorldPosToTileCoords(Vector3 worldPos)
        {
            var x = Mathf.FloorToInt(worldPos.x / tileSize.x);
            var z = Mathf.FloorToInt(worldPos.z / tileSize.y);

            return new Vector2Int(x, z);
        }
        public Vector3 ConvertTileCoordsToWorldPos(Vector2Int coords)
        {
            return new Vector3(
                m_terrainVars.TerrainCenter().x + (coords.x * tileSize.x),
                m_terrainVars.TerrainCenter().y,
                m_terrainVars.TerrainCenter().z + (coords.y * tileSize.y)
                );
        }
        #endregion

        #region tile instances
        public TerrainTile NewTileFromPrefab(Vector2Int coords)
        {
            var center = m_terrainVars.TerrainCenter();
            var worldPos = new Vector3(
                center.x + (coords.x * tileSize.x),
                center.y,
                center.z + (coords.y * tileSize.y)
                );

            var go = Instantiate(m_prefabGO);
            go.transform.parent = m_parentFolder;
            var tile = go.GetComponent<TerrainTile>();
            tile.SetMeshData(this, m_terrainVars, m_vertices);

            tile.Build(worldPos, coords);
            tile.GetComponent<TileActivation>().Activate();
            
            return tile;
        }

        public Vector3[] SetVertexHeightWithPerlin(Vector3[] vertices, Vector3 tilePos)
        {
            float posX;
            float posZ;
            var noiseOffset = m_terrainVars.GetNoiseOffset();
            var heightScale = m_terrainVars.GetHeightScale();
            var detailScale = m_terrainVars.GetDetailScale();

            for (int v = 0; v < vertices.Length; v++)
            {
                posX = (vertices[v].x + tilePos.x + noiseOffset) / detailScale;
                posZ = (vertices[v].z + tilePos.z + noiseOffset) / detailScale;
                vertices[v].y = Mathf.PerlinNoise(posX, posZ) * heightScale;
            }
            return vertices;
        }
        #endregion

        #region create prefab
        public TerrainTile MakePrefabTileAt(Vector3 pos)
        {
            if (!m_terrainVars) { Debug.Log("no terrain vars asset"); }

            CalculateMeshSizeInWorldUnits();
            m_prefabTile = CreatePrefabGO(pos);
            m_prefabGO = m_prefabTile.gameObject;
            m_prefabGO.isStatic = true;
            CreateMeshGO(m_prefabTile);
            CreateShape();
            UpdateMesh();
            UpdatePrefabWithVerts();
            return m_prefabTile;
        }

        private void CalculateMeshSizeInWorldUnits()
        {
            tileSize = new Vector2Int(
                m_terrainVars.MeshSizeInCells().x * m_terrainVars.CellSize(),
                m_terrainVars.MeshSizeInCells().y * m_terrainVars.CellSize()
                );
        }

        private TerrainTile CreatePrefabGO(Vector3 pos)
        {
            m_prefabGO = new GameObject();
            m_prefabGO.name = "DynamicTilePrefab";
            m_prefabGO.transform.parent = m_parentFolder;
            m_prefabGO.transform.position = pos;
            m_prefabTile = m_prefabGO.AddComponent<TerrainTile>();
            m_prefabGO.AddComponent<TileActivation>();
            return m_prefabTile;
        }

        private void CreateMeshGO(TerrainTile prefabTile)
        {
            m_prefabMeshGO = new GameObject();
            m_prefabMeshGO.name = "DynamicTileMesh";
            m_prefabMeshGO.transform.parent = prefabTile.transform;
            m_prefabMeshGO.transform.position = prefabTile.transform.position;

            meshFilter = m_prefabMeshGO.AddComponent<MeshFilter>();
            meshRenderer = m_prefabMeshGO.AddComponent<MeshRenderer>();
            m_prefabMeshGO.AddComponent<MeshCollider>();
        }

        private void CreateShape()
        {
            m_vertices = new Vector3[(tileSize.x + 1) * (tileSize.y + 1)];

            for (int i = 0, z = 0; z <= tileSize.y; z++)
            {
                for (int x = 0; x <= tileSize.x; x++)
                {
                    m_vertices[i] = new Vector3(x, 0, z);
                    i++;
                }
            }

            m_triangles = new int[tileSize.x * tileSize.y * 6];
            int vert = 0;
            int tris = 0;
            for (int z = 0; z < tileSize.y; z++)
            {
                for (int x = 0; x < tileSize.x; x++)
                {
                    m_triangles[tris + 0] = vert + 0;
                    m_triangles[tris + 1] = vert + tileSize.x + 1;
                    m_triangles[tris + 2] = vert + 1;
                    m_triangles[tris + 3] = vert + 1;
                    m_triangles[tris + 4] = vert + tileSize.x + 1;
                    m_triangles[tris + 5] = vert + tileSize.x + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }
        }

        private void UpdateMesh()
        {
            m_mesh = new Mesh();
            m_mesh.name = "DynamicTileMesh";
            meshFilter.mesh = m_mesh;
            meshRenderer.material = m_terrainVars.DefaultMaterial();

            m_mesh.Clear();

            m_mesh.vertices = m_vertices;
            m_mesh.triangles = m_triangles;
            m_mesh.RecalculateNormals();
        }

        private void UpdatePrefabWithVerts()
        {
            m_prefabTile.SetMeshData(this, m_terrainVars, m_vertices);
        }
        #endregion
    }
}

