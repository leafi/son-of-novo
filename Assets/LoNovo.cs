using UnityEngine;
using System;
using System.Collections.Generic;
using lo_novo;
using lo_novo.Protocol;

//[ExecuteInEditMode]
public class LoNovo : MonoBehaviour
{
	private StupidLoopbackClient clientComms;

    private static LoNovo lon = null;
    public static StupidLoopbackClient Comms
    {
        get
        {
            if (lon == null)
                lon = GameObject.Find("LoNovoObject").GetComponent<LoNovo>();
            return lon.clientComms;
        }
    }

    private static bool ready = false;
    public static bool Ready { get { return ready; } }

	// Use this for initialization
	void Start()
	{
        tryConnect();
	}

    private void tryConnect()
    {
        try
        {
            clientComms = new StupidLoopbackClient();
            ready = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            ready = false;
        }
    }

	// Update is called once per frame
	void Update()
	{
        if (!ready)
            return;

        var s = clientComms.TryRead();

        if (s == null)
            return;

        Debug.Log(s);

	}
}
