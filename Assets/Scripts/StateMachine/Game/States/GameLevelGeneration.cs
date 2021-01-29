using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NextOne
{
    class GameLevelGeneration : State<GameContext>
    {
        
        Vector3 BoundsCenter = Vector3.zero;
        Vector3 BoundsSize = new Vector3(2000f, 2000f, 2000f);
        LayerMask BuildMask = ~0;
        LayerMask NullMask = ~0;

        // Constructor taking the state machine + the state id
        public GameLevelGeneration(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GameLevelGeneration) { }

        public override void OnEnter()
        {
            // Generate the Map
            LevelGenerator level = this.sm.ctx.levelManager.GetComponent<LevelGenerator>();
            level.Generate();
            // Generate the NavMesh
            BuildEmptyNavMesh();
            UpdateNavmeshData();


            OnPostGeneration();          
            
        }
        
        public override void OnUpdate()
        {
            this.sm.ctx.animator.SetTrigger("GameFadeIn");
            this.sm.SwitchState((int)GameStates.GameFadeIn);
        }


        void BuildEmptyNavMesh()
        {
            this.sm.ctx.navMeshData = NavMeshBuilder.BuildNavMeshData(
                this.sm.ctx.navMeshSurface.GetBuildSettings(),
                GetBuildSources(NullMask),
                new Bounds(BoundsCenter, BoundsSize),
                Vector3.zero,
                Quaternion.identity);
            AddNavMeshData();
        }

        void UpdateNavmeshData()
        {
            IEnumerator op = UpdateNavmeshDataAsync();
        }

        IEnumerator UpdateNavmeshDataAsync()
        {
            AsyncOperation op = NavMeshBuilder.UpdateNavMeshDataAsync(
                this.sm.ctx.navMeshData,
                NavMesh.GetSettingsByID(0),
                GetBuildSources(BuildMask),
                new Bounds(BoundsCenter, BoundsSize));
            yield return op;

            AddNavMeshData();
            Debug.Log("Update finished " + Time.realtimeSinceStartup.ToString());
        }

        // Create NavMeshData
        void AddNavMeshData()
        {
            if (this.sm.ctx.navMeshData != null)
            {
                if (this.sm.ctx.navMeshDataInstance.valid)
                {
                    NavMesh.RemoveNavMeshData(this.sm.ctx.navMeshDataInstance);
                }
                this.sm.ctx.navMeshDataInstance = NavMesh.AddNavMeshData(this.sm.ctx.navMeshData);
            }
        }
        // Get the sources to build the NavMesh
        List<NavMeshBuildSource> GetBuildSources(LayerMask mask)
        {
            List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
            NavMeshBuilder.CollectSources(
                new Bounds(BoundsCenter, BoundsSize),
                mask,
                NavMeshCollectGeometry.PhysicsColliders,
                0,
                new List<NavMeshBuildMarkup>(),
                sources);
            Debug.Log("Sources found: " + sources.Count.ToString());
            return sources;
        }

        private void OnPostGeneration()
        {
            // Post Generation
            GameObject player = GameObject.Find("Player");
            if (player == null) Debug.LogError("No PLayer Gameobject found :(");
            this.sm.ctx.playerController = player.GetComponent<PlayerController>();
            player.transform.parent = null;

            this.sm.ctx.playerController.gameObject.SetActive(true);
            this.sm.ctx.sun.gameObject.SetActive(true);
            this.sm.ctx.globalVolume.sharedProfile = this.sm.ctx.postProcessProfile;
        }
    }
}
