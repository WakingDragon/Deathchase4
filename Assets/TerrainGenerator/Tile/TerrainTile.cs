using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    public class TerrainTile : MonoBehaviour
    {
        private TerrainVars_asset m_terrainVars;
        private TerrainTileMaker_asset m_terrainMaker;
        private TerrainLevelAsset m_levelAsset;
        private Vector3[] m_vertices;
        private Mesh m_mesh;
        private Vector2Int m_tileCoords;
        private Vector3 m_worldPos;
        private TileActivation activator;
        private MeshCollider m_meshCollider;

        #region terrain
        public void SetMeshData(TerrainTileMaker_asset terrainMaker, TerrainVars_asset terrainVars, Vector3[] vertices)
        {
            if (!m_mesh) { m_mesh = GetComponentInChildren<MeshFilter>().mesh; }
            m_vertices = vertices;
            m_terrainVars = terrainVars;
            m_terrainMaker = terrainMaker;
        }

        public void Build(Vector3 worldPos, Vector2Int tileCoords)
        {
            m_worldPos = worldPos;
            transform.position = m_worldPos;
            gameObject.name = "Tile(" + tileCoords.x + "," + tileCoords.y + ")";
            m_tileCoords = tileCoords;
            gameObject.SetActive(true);
            activator = GetComponent<TileActivation>();
            SetVertexHeightWithPerlin();
            RedoMeshCollider();
        }

        public void SetVertexHeightWithPerlin()
        {
            if(!m_mesh) { m_mesh = GetComponentInChildren<MeshFilter>().mesh; }

            m_vertices = m_terrainMaker.SetVertexHeightWithPerlin(m_vertices, transform.position);

            m_mesh.vertices = m_vertices;
            m_mesh.RecalculateBounds();
            m_mesh.RecalculateNormals();
        }

        private void RedoMeshCollider()
        {
            //if(m_meshCollider) { DestroyImmediate(m_meshCollider); }
            if (!m_meshCollider) { m_meshCollider = gameObject.AddComponent<MeshCollider>(); }
            m_meshCollider.sharedMesh = m_mesh;
        }
        #endregion

        #region content
        public void Populate(TerrainLevelAsset levelAsset)
        {
            activator.Populate(m_worldPos, m_vertices, levelAsset);
        }
        #endregion
    }
}

