using UnityEngine;
using System.Collections;
using lo_novo.Protocol;

public class RoomComponent : MonoBehaviour
{

    private string oldName;
    public string RoomName;

    // Use this for initialization
    void Start()
    {

    }
	
    // Update is called once per frame
    void Update()
    {

        //
        // TODO: make Room creation wizard.
        // the wizard, a custom Room editor's Update button, and a pre-Play event should be
        // the only things that do ?R.
        //
        // This component should also not ExecuteInEditMode.
        //

        if (!LoNovo.Ready)
            return;

        if (oldName != RoomName)
        {
            oldName = RoomName;
            LoNovo.Comms.Send(FromClient.QueryRoomContents(oldName));
        }
    }
}
