using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;
using System;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    public class TerrainGenerator : MonoBehaviour
    {
        [Header("activation")]
        [SerializeField] private bool createOnStart = false;

        [Header("assets and settings")]
        [SerializeField] private TerrainLevelAsset m_levelAsset = null;

        [Header("dependencies")]
        [SerializeField] private TerrainTileMaker_asset m_terrainMaker = null;
        [SerializeField] private Transform_listSet m_cameraList = null;
        [SerializeField] private Transform_listSet m_npcsList = null;
        [SerializeField] private Vector2Int_GO_DictSet m_tileGOs_dictSet = null;
        private List<Vector2Int> m_activeCells = new List<Vector2Int>();
        private List<Vector2Int> m_previousCells = new List<Vector2Int>();
        private List<Vector2Int> m_cellsToDestroy = new List<Vector2Int>();
        private List<Vector2Int> m_cellsToActivate = new List<Vector2Int>();

        private bool m_refreshMap = false;
        private float m_refreshTimer;


        private void Awake()
        {
            if (!m_terrainMaker) { Debug.Log("no terrainMaker"); }
            if (!m_cameraList) { Debug.Log("no camera list"); }
            if (!m_npcsList) { Debug.Log("no npc list"); }
            if (!m_tileGOs_dictSet) { Debug.Log("no tile dictionary"); }
            if (!m_levelAsset) { Debug.Log("no level asset"); }
        }

        private void Start()
        {
            if(createOnStart)
            {
                Setup();
            }
        }

        public void Activate()
        {
            Setup();
        }

        private void LateUpdate()
        {
            if (m_refreshMap && m_refreshTimer <= 0f)
            {
                UpdateActiveCellsList();
                ActivateNewCells();
                DeactivateOldCells();
                m_refreshTimer = m_terrainMaker.TerrainVars().RefreshInterval();
            }
            m_refreshTimer -= Time.deltaTime;
        }

        public void SetNewTerrainLevelAsset(TerrainLevelAsset levelAsset)
        {
            m_levelAsset = levelAsset;
        }

        private void Setup()
        {
            m_levelAsset.PoolTerrainObjects();
            m_terrainMaker.SetParent(transform);
            m_terrainMaker.MakePrefabTileAt(Vector3.zero).gameObject.SetActive(false);
            m_refreshMap = true;
            m_refreshTimer = m_terrainMaker.TerrainVars().RefreshInterval();
        }

        public void DestroyTerrain()
        {
            //deactivate all tiles
            foreach (Vector2Int cell in m_activeCells)
            {
                var tileToDestroy = m_tileGOs_dictSet.GetValue(cell);
                var activator = tileToDestroy.GetComponent<TileActivation>();
                activator.Deactivate();
            }
            m_activeCells.Clear();
            m_previousCells.Clear();

            //destroy all item pools in level asset
            m_levelAsset.DestroyTerrainObjectPools();

            //destroy all tile gos
            m_tileGOs_dictSet.Initialize();
            Transform[] tilesToDestroy = GetComponentsInChildren<Transform>();
            for(int i = tilesToDestroy.Length -1; i >= 0; i--)
            {
                if(tilesToDestroy[i] != transform)
                {
                    Destroy(tilesToDestroy[i].gameObject);
                }
            }
        }

        #region tiling
        private void UpdateActiveCellsList()
        {
            if (m_cameraList.Count() > 0 || m_npcsList.Count() > 0)
            {
                InitialiseCellListsForRefresh();

                var hasEdges = m_terrainMaker.TerrainVars().IsFixedSize();
                var upperBound = m_terrainMaker.TerrainVars().UpperBoundEdgeInCells();
                var lowerBound = m_terrainMaker.TerrainVars().LowerBoundEdgeInCells();

                //cameras
                var cameras = m_cameraList.GetList();
                var tileRadiusForCam = m_terrainMaker.TerrainVars().TileRadiusForCam();

                //add all proposed cells to active cells list                 
                foreach (Transform camera in cameras)
                {
                    Vector2Int tileCoords = m_terrainMaker.ConvertWorldPosToTileCoords(camera.position);

                    for (int x = -tileRadiusForCam; x < tileRadiusForCam + 1; x++)
                    {
                        for (int z = -tileRadiusForCam; z < tileRadiusForCam + 1; z++)
                        {
                            var activeTile = new Vector2Int(tileCoords.x + x, tileCoords.y + z);
                            if(!hasEdges)
                            {
                                m_activeCells.Add(activeTile);
                            }
                            else if(IsWithinEdges(activeTile, upperBound, lowerBound))
                            {
                                m_activeCells.Add(activeTile);
                            }
                        }
                    }
                }

                //npcs
                var npcs = m_npcsList.GetList();
                var tileRadiusForNPC = m_terrainMaker.TerrainVars().TileRadiusForNPCs();

                foreach (Transform npc in npcs)
                {
                    Vector2Int tileCoords = m_terrainMaker.ConvertWorldPosToTileCoords(npc.position);

                    for (int x = -tileRadiusForNPC; x < tileRadiusForNPC + 1; x++)
                    {
                        for (int z = -tileRadiusForNPC; z < tileRadiusForNPC + 1; z++)
                        {
                            var activeTile = new Vector2Int(tileCoords.x + x, tileCoords.y + z);
                            m_activeCells.Add(activeTile);
                        }
                    }
                }

                //any cells that were not previously active added to activate list
                foreach (Vector2Int cell in m_activeCells)
                {
                    if(!m_previousCells.Contains(cell))
                    {
                        m_cellsToActivate.Add(cell);
                    }
                }

                //any previous cells no longer needed added to destroy list
                foreach(Vector2Int cell in m_previousCells)
                {
                    if(!m_activeCells.Contains(cell))
                    {
                        m_cellsToDestroy.Add(cell);
                    }
                }
            }
        }

        private void InitialiseCellListsForRefresh()
        {
            m_cellsToActivate.Clear();
            m_cellsToDestroy.Clear();
            m_previousCells.Clear();

            foreach (Vector2Int cell in m_activeCells)
            {
                m_previousCells.Add(cell);
            }

            m_activeCells.Clear();
        }

        private bool IsWithinEdges(Vector2Int coords, Vector2Int upperBound, Vector2Int lowerBound)
        {
            if(coords.x > upperBound.x || coords.x < lowerBound.x || coords.y > upperBound.y || coords.y < lowerBound.y)
            {
                return false;
            }
            return true;
        }

        private void ActivateNewCells()
        {
            if(m_cellsToActivate.Count < 1) { return; }

            foreach (Vector2Int cellCoords in m_cellsToActivate)
            {
                //if its in the dictionary then activate it
                if(m_tileGOs_dictSet.Contains(cellCoords))
                {
                    var tile = m_tileGOs_dictSet.GetValue(cellCoords);
                    tile.SetActive(true);
                    tile.GetComponent<TileActivation>().Activate();
                }
                else
                {
                    //else create it
                    var newTile = m_terrainMaker.NewTileFromPrefab(cellCoords);
                    newTile.Populate(m_levelAsset);
                    m_tileGOs_dictSet.AddToSetOrUpdate(cellCoords, newTile.gameObject);
                }
            }
        }

        private void DeactivateOldCells()
        {
            if (m_cellsToDestroy.Count < 1) { return; }

            foreach (Vector2Int cell in m_cellsToDestroy)
            {
                var tileToDestroy = m_tileGOs_dictSet.GetValue(cell);
                var activator = tileToDestroy.GetComponent<TileActivation>();
                activator.Deactivate();
            }
        }
        #endregion
    }
}

