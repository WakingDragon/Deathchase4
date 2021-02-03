using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    [CreateAssetMenu(fileName ="terrainVars_asset",menuName ="Terrain Generator/(single) Terrain Vars")]
    public class TerrainVars_asset : ScriptableObject
    {
        [Header("tile basic")]
        [SerializeField] private Vector3 m_terrainCenter = new Vector3(0f, 0f, 0f);
        [SerializeField] private int m_cellSize;
        [SerializeField] private Vector2Int m_meshSizeInCells;
        [SerializeField] private Material m_defaultMat;

        [Header("perlin settings")]
        [SerializeField] [Range(0f,1f)] private float noiseOffset = 0f;
        [SerializeField] private float detailScale = 20f;
        [SerializeField] private float heightScale = 10f;

        [Header("fixed size terrain")]
        [SerializeField] private bool m_isFixedSize = true;
        [SerializeField] private Vector2Int m_terrainSize = new Vector2Int(40, 40);

        [Header("refresh settings")]
        [SerializeField] private int m_tileRadiusForCam = 3; //todo - make based on direction
        [SerializeField] private int m_tileRadiusForNPC = 1; //todo - make based on direction
        [SerializeField] private float m_refreshInterval = 0.1f;

        public Vector3 TerrainCenter() { return m_terrainCenter; }
        public void SetTerrainVars(int cellSize, Vector2Int meshSizeInCells,Material defaultMat)
        {
            m_defaultMat = defaultMat;
            SetCellSizeAndMeshSize(cellSize, meshSizeInCells);
        }
        public void SetCellSizeAndMeshSize(int cellSize, Vector2Int meshSizeInCells)
        {
            m_cellSize = cellSize;
            m_meshSizeInCells = meshSizeInCells;
            ClampData();
        }
        public int CellSize()
        {
            ClampData();
            return m_cellSize;
        }
        public Vector2Int MeshSizeInCells()
        {
            ClampData();
            return m_meshSizeInCells;
        }
        public Material DefaultMaterial()
        {
            if(m_defaultMat == null) { Debug.Log("no default terrain material set"); }
            return m_defaultMat; 
        }
        private void ClampData()
        {
            if (m_cellSize < 1) { m_cellSize = 1; }

            if (m_meshSizeInCells.x < 1) { m_meshSizeInCells.x = 1; }
            if (m_meshSizeInCells.y < 1) { m_meshSizeInCells.y = 1; }
        }

        public Vector3 GetRandomPointOnMap()
        {
            var lowerBoundWorldPos = LowerCornerWorldPos();
            var upperBoundWorldPos = UpperCornerWorldPos();
            var x = Random.Range(lowerBoundWorldPos.x, upperBoundWorldPos.x);
            var z = Random.Range(lowerBoundWorldPos.z, upperBoundWorldPos.z);
            return new Vector3(x, 0f, z);
        }

        #region perlin settings
        public float GetNoiseOffset()
        {
            return noiseOffset * 256f * detailScale - 100f;
        }
        public float GetDetailScale()
        {
            return detailScale;
        }
        public float GetHeightScale()
        {
            return heightScale;
        }
        #endregion

        #region fixed size
        public bool IsFixedSize() { return m_isFixedSize; }
        public Vector2Int TerrainSize() { return m_terrainSize; }
        public Vector2Int UpperBoundEdgeInCells()
        {
            return new Vector2Int(
                Mathf.RoundToInt(m_terrainCenter.x) + EdgeDistanceFromCenter().x,
                Mathf.RoundToInt(m_terrainCenter.z) + EdgeDistanceFromCenter().y
                );
        }
        public Vector2Int LowerBoundEdgeInCells()
        {
            return new Vector2Int(
                Mathf.RoundToInt(m_terrainCenter.x) - EdgeDistanceFromCenter().x,
               Mathf.RoundToInt(m_terrainCenter.z) - EdgeDistanceFromCenter().y
                );
        }
        private Vector3 UpperCornerWorldPos()
        {
            var point = UpperBoundEdgeInCells() * m_cellSize * m_meshSizeInCells;
            return new Vector3(
                point.x,
                0f,
                point.y
                );
        }
        private Vector3 LowerCornerWorldPos()
        {
            var point = LowerBoundEdgeInCells() * m_cellSize * m_meshSizeInCells;
            return new Vector3(
                point.x,
                0f,
                point.y
                );
        }
        private Vector2Int EdgeDistanceFromCenter()
        {
            return new Vector2Int(
                Mathf.RoundToInt(m_terrainSize.x * 0.5f),
                Mathf.RoundToInt(m_terrainSize.y * 0.5f)
                );
        }
        #endregion

        #region refresh settings
        public int TileRadiusForCam() { return m_tileRadiusForCam; }
        public int TileRadiusForNPCs() { return m_tileRadiusForNPC; }
        public float RefreshInterval() { return m_refreshInterval; }
        #endregion
    }
}

