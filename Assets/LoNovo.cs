using UnityEngine;
using System.Collections.Generic;
using lo_novo;

public class LoNovo : MonoBehaviour
{
	private LoopbackClient clientComms;

	// Use this for initialization
	void Start()
	{
		clientComms = Program.StartLoopbackAndJoin (new string[] { });
	}

	// Update is called once per frame
	void Update()
	{
        while (true)
        {
            var s = clientComms.TryRead();

            if (s == null)
                break;

            Debug.Log(s);
        }
	}
}
