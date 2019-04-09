﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.TeamPlug.Patterns;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

}
