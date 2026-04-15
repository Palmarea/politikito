using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Interaction.Detail
{
    [CreateAssetMenu(fileName = "DetailDB", menuName = "Game/Detail Database")]
    public class DetailDatabaseSO : ScriptableObject
    {
        public List<DetailObjData> DetailDB = new List<DetailObjData>();
    }
}