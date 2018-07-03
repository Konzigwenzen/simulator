/**
 * Copyright (c) 2018 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */


﻿using UnityEngine;
using UnityEngine.UI;

public class SingleRosConnection : MonoBehaviour
{
    public string Address = "localhost";
    public int Port = RosBridgeConnector.DefaultPort;
    public int Version = 1;

    public Text BridgeStatus;

    public RosBridgeConnector Connector { get; private set; }

    public RobotSetup Robot;
    public UserInterfaceSetup UserInterface;

    void Start()
    {
        Connector = new RosBridgeConnector();
        Connector.BridgeStatus = BridgeStatus;

        if (GameObject.Find("RosRobots") == null)
        {
            Robot.Setup(UserInterface, Connector.Bridge);
        }
        else
        {
            Destroy(this);
            Destroy(GameObject.Find("duckiebot"));
            if (true)
            {
                var go = GameObject.Find("XE_Rigged");
                if (go != null)
                {
                    var cols = go.GetComponentsInChildren<Collider>();
                    foreach (var col in cols)
                    {
                        col.enabled = false;
                    }
                    Destroy(go);
                }
            }

            Destroy(GameObject.Find("UserInterface"));
        }

        string overrideAddress = System.Environment.GetEnvironmentVariable("ROS_BRIDGE_HOST");
        if (overrideAddress != null)
        {
            Address = overrideAddress;
        }
    }

    void Update()
    {
        if (Address != Connector.Address || Port != Connector.Port || Version != Connector.Version)
        {
            Connector.Disconnect();
        }

        Connector.Address = Address;
        Connector.Port = Port;
        Connector.Version = Version;

        Connector.Update();
    }
}
