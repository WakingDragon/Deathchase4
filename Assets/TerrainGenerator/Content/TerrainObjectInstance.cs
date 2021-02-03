using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    public class TerrainObjectInstance
    {
        private Vector3 m_position;
        private Vector3 m_rotation;
        private float m_scale;
        private TerrainObjectAsset m_asset;
        private GameObject m_go;

        public Vector3 Pos() { return m_position; }
        public void SetPos(Vector3 position) { m_position = position; }
        public Vector3 Rot() { return m_rotation; }
        public void SetRot(Vector3 rotation) { m_rotation = rotation; }
        public float Scale() { return m_scale; }
        public void SetScale(float scale) { m_scale = scale; }
        public TerrainObjectAsset Asset() { return m_asset; }
        public void SetAsset(TerrainObjectAsset asset) { m_asset = asset; }
        public GameObject GO() { return m_go; }
        public void SetGO(GameObject go) { m_go = go; }
    }
}

