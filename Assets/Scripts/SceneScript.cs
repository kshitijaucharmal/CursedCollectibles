using Mirror;
using UnityEngine;
using TMPro;

public class SceneScript : NetworkBehaviour {
    public TMP_Text canvasStatusText;
    public M_FPSPlayerMovement player;

    [SyncVar(hook = nameof(OnStatusTextChanged))]
    public string statusText;

    void OnStatusTextChanged(string _Old, string _New) {
        //called from sync var hook, to update info on screen for all players
        canvasStatusText.text = statusText;
    }

    public void ButtonSendMessage(string msg) {
        player?.CmdSendPlayerMessage(msg);
    }
}