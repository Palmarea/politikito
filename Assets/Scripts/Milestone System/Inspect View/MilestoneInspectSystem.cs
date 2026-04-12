using Game.Systems.Interaction.Detail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Milestone.Inspect
{
    public class MilestoneInspectSystem : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private MilestoneInspectView MISView;

        [Header("References")]
        [SerializeField] private List<Transform> DOAnchorPoints = new();

        #region VIEW CHANGE
        public void InspectView()
        {
            MISView.RequestViewChange();
        }

        public void RestoreView()
        {
            MISView.RequestViewReset();
        }
        #endregion
        
        public void RequestMDOCreation(string objID, int level)
        {
            DetailSystem.Instance.RequestDetailObjCreation(objID, DOAnchorPoints[level - 1].position, false);
        }
    }
}