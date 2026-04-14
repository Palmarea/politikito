using Game.Systems.Interaction.Detail;
using UnityEngine;

namespace Game.Systems.Milestone.Inspect
{
    public class MilestoneInspectSystem : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private MilestoneInspectView MISView;

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
        
        public void RequestMDOCreation(Milestone ms)
        {
            DetailSystem.Instance.RequestDetailObjCreation(ms.detailObjectID, ms.spawnPosition, ms.spawnRotation, ms.spawnScale, false);
        }
    }
}