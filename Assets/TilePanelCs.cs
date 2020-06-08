using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TilePanelCs : MonoBehaviour
{


    public TextMeshProUGUI owner
        , type,state;
    
    public void updateLabels(Tile tile) {
        owner.text = tile.Owner.Name;
        type.text = tile.Type.ToString();
        state.text = "Units in Tile:" + tile.getUnitsInTile().ToString() + " State: " + tile.State.ToString();

    }


}
