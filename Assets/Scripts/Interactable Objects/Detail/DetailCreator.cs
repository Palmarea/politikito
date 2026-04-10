using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game.Systems.Interaction.Detail
{
    public class DetailCreator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DetailDatabaseSO Database;
        [SerializeField] private GameObject DetailObjPrefab;

        public void CreateDetailObject(string objID)
        {
            GameObject dobj = Instantiate(DetailObjPrefab);

            // Setear detalles del objeto
            DetailObject detail = dobj.GetComponent<DetailObject>();
            detail.SetDetailData(Database.DetailDB.FirstOrDefault(a => a.objectID == objID));
        }
    }
}