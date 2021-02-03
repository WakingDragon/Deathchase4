using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Units.Weapons;
using BP.ObjectPooling;
using BP.Core;
using BP.Worlds.ProceduralTerrainGenerator;

namespace BP.Units
{
    [CreateAssetMenu(fileName ="new_unit",menuName ="Units/new Unit")]
    public class UnitAsset : ScriptableObject
    {
        [SerializeField] private string m_unitName = "Some unit";
        [SerializeField] private bool m_isPlayer = false;
        [SerializeField] private float m_startingHealth = 100f;
        [SerializeField] private float m_altitudeToFly;
        [SerializeField] private WeaponSlotItem[] m_weaponSlots;
        [SerializeField] private bool m_autoFire = true;
        [SerializeField] private Faction m_faction = null;
        [SerializeField] private Vector3 m_colliderPos;
        [SerializeField] private Vector3 m_colliderSize;
        [SerializeField] private FXAsset m_deathFX = null;
        [SerializeField] private float m_speed = 30f;
        [SerializeField] private float m_rotSpeed = 80f;
        [SerializeField] private GameObject m_unitMeshPrefab = null;
        [SerializeField] private Vector3 m_meshLocalPos;

        [Header("baddie things")]
        [SerializeField] private Transform_listSet m_npcList = null;
        [SerializeField] private VoidGameEvent m_allBaddiesDeadEvent = null;
        [SerializeField] private TerrainVars_asset m_terrainVars = null;
        [SerializeField] private GameObject m_targetIndicator = null;

        [Header("dependencies")]
        [SerializeField] private GameObject m_genericUnitPrefab = null;
        [SerializeField] private Vector2GameEvent m_unitHealthChangeEvent = null;
        [SerializeField] private VoidGameEvent m_unitDeathEvent = null;
        [SerializeField] private ObjectPoolAsset m_pool;
        [SerializeField] private VoidGameEvent m_camShakeEvent = null;

        public bool IsPlayer() { return m_isPlayer; }
        public ObjectPoolAsset ObjectPoolAsset() { return m_pool; }
        public Transform_listSet NPCListSet() { return m_npcList; }
        

        #region assembly
        public UnitBuilder SpawnAndAssembleUnitAt(Vector3 spawnPoint, bool gotoIdle, bool gotoPlay)
        {
            var go = Instantiate(m_genericUnitPrefab);
            var builder = go.GetComponent<UnitBuilder>();
            go.transform.position = spawnPoint;
            go.name = m_unitName;
            builder.AssembleUnit(this, gotoIdle, gotoPlay);
            //builder.SetupBuilder(this, true);
            return builder;
        }

        public void SetColliderAndRBOnUnit(Rigidbody rb, BoxCollider box)
        {
            box.center = m_colliderPos;
            box.size = m_colliderSize;

            rb.isKinematic = true;
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }

        public TerrainVars_asset TerrainVars() { return m_terrainVars; }
        public GameObject TargetIndicator() { return m_targetIndicator; }

        public float Speed() { return m_speed; }
        public float RotSpeed() { return m_rotSpeed; }
        public float Altitude() { return m_altitudeToFly; }

        public float StartingHealth() { return m_startingHealth; }
        public Faction Faction() { return m_faction; }
        public VoidGameEvent CamShakeEvent() { return m_camShakeEvent; }
        public Vector2GameEvent UnitHealthChangeEvent() { return m_unitHealthChangeEvent; }
        public VoidGameEvent UnitDeathEvent() { return m_unitDeathEvent; }
        public FXAsset DeathFX() { return m_deathFX; }

        public bool AutoFire() { return m_autoFire; }
        public List<WeaponSlot> SetWeaponSlotsOnUnit(Transform unitTransform)
        {
            List<WeaponSlot> slots = new List<WeaponSlot>();

            foreach (WeaponSlotItem item in m_weaponSlots)
            {
                var go = new GameObject();
                go.transform.parent = unitTransform;
                go.transform.localPosition = item.location;
                go.name = "WeaponSlot";
                var wepSlot = go.AddComponent<WeaponSlot>();
                wepSlot.DefineSlot(item.slotType, item.defaultWeapon);
                slots.Add(wepSlot);
            }

            return slots;
        }

        public GameObject AddMeshToUnit(Transform unitTransform)
        {
            GameObject go = Instantiate(m_unitMeshPrefab, unitTransform);
            go.transform.localPosition = m_meshLocalPos;
            go.name = "UnitMesh";
            return go;
        }
        #endregion

        public void NotifyAllBaddiesDead()
        {
            m_allBaddiesDeadEvent.Raise();
        }
    }
}

