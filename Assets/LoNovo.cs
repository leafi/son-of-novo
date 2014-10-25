using UnityEngine;
using System.Collections.Generic;
using lo_novo;

[ExecuteInEditMode]
public class LoNovo : MonoBehaviour
{
	private LoopbackClient clientComms;

    private static LoNovo lon = null;
    public static LoopbackClient Comms
    {
        get
        {
            if (lon == null)
                lon = GameObject.Find("LoNovoObject").GetComponent<LoNovo>();
            return lon.clientComms;
        }
    }

	// Use this for initialization
	void Start()
	{
        if (!Program.Running)
            clientComms = Program.StartLoopbackAndJoin(new string[] { });
        else
        {
            Debug.Log("Found lo-novo thread. Re-joining session.");
            clientComms = Program.LoopbackClient;
        }
	}

	// Update is called once per frame
	void Update()
	{
        while (true)
        {
            if (clientComms == null)
            {
                // We lost link to lo-novo in editor.
                Debug.Log("XXX: relink");
                clientComms = Program.StartLoopbackAndJoin(new string[] { });
            }

            var s = clientComms.TryRead();

            if (s == null)
                break;

            Debug.Log(s);
        }
	}
}
