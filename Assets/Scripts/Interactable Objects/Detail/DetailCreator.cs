using Game.Systems.CameraControl;
using Game.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.Systems.Interaction.Detail
{
    public class DetailCreator : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private CameraController CamController;

        [Header("References")]
        [SerializeField] private GameObject DetailObjPrefab;
        [SerializeField] private Transform DetailObjGreatParent;

        [Header("Data")]
        [SerializeField] private DetailDatabaseSO Database;
        [SerializeField] private SpriteAtlas SpriteAtlas;

        [Header("Parameters")]
        [SerializeField] private float RestDuration = 3f;
        [SerializeField] private string AnimationSLName = "Canvas UI";

        public event Action OnObjectCreated;
        private GameObject lastDetailObj;
        private int _originalLayerID;

        public void CreateDetailObject(string objID, Vector3 spawnPosition, Vector3 spawnRotation, Vector3 spawnScale, bool needFocus = true)
        {
            GameObject par = new GameObject();
            par.transform.position = spawnPosition;
            lastDetailObj = Instantiate(DetailObjPrefab, par.transform);
            
            lastDetailObj.transform.localEulerAngles = spawnRotation;

            par.transform.localScale = spawnScale;

            par.transform.parent = DetailObjGreatParent;

            DetailObject detail = lastDetailObj.GetComponent<DetailObject>();
            detail.SetDetailData(Database.DetailDB.FirstOrDefault(a => a.objectID == objID));

            // Update Sprite
            lastDetailObj.GetComponent<SpriteRenderer>().sprite = SpriteAtlasHandling.GetSpriteFromAtlas(SpriteAtlas, detail.m_Data.spriteAtlasID);
            
            // Update Collider to new Sprite Physics form
            UpdateCollider();

            // Focus on Creation
            if (needFocus)
            {
                CamController.ForceMove(lastDetailObj.transform);
                lastDetailObj.SetActive(false);

                CamController.OnArrivedToForcedSection += OnFocusNewDO;
            }
        }

        private void OnFocusNewDO()
        {
            StartCoroutine(UnfocusRoutine());
        }

        private IEnumerator UnfocusRoutine()
        {
            CamController.OnArrivedToForcedSection -= OnFocusNewDO;

            lastDetailObj.SetActive(true);

            _originalLayerID = lastDetailObj.GetComponent<SpriteRenderer>().sortingLayerID;

            lastDetailObj.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID(AnimationSLName);

            yield return new WaitForSeconds(RestDuration);

            lastDetailObj.GetComponent<Animator>().enabled = false;

            lastDetailObj.GetComponent<SpriteRenderer>().sortingLayerID = _originalLayerID;

            lastDetailObj = null;

            OnObjectCreated?.Invoke();

            CamController.ResetForced();
        }

        private void UpdateCollider()
        {
            lastDetailObj.GetComponent<PolygonCollider2D>().pathCount = lastDetailObj.GetComponent<SpriteRenderer>().sprite.GetPhysicsShapeCount();

            List<Vector2> path = new List<Vector2>();

            for (int i = 0; i < lastDetailObj.GetComponent<PolygonCollider2D>().pathCount; i++)
            {
                path.Clear();
                lastDetailObj.GetComponent<SpriteRenderer>().sprite.GetPhysicsShape(i, path);
                lastDetailObj.GetComponent<PolygonCollider2D>().SetPath(i, path.ToArray());
            }
        }
    }
}