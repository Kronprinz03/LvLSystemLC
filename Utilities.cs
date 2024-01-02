using GameNetcodeStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LvLSystemLC;

public class Utilities
{
    public static void TeleportPlayer(int playerObj, Vector3 teleportPos)
    {
        PlayerControllerB playerControllerB = StartOfRound.Instance.allPlayerScripts[playerObj];
        if ((bool)UnityEngine.Object.FindObjectOfType<AudioReverbPresets>())
        {
            UnityEngine.Object.FindObjectOfType<AudioReverbPresets>().audioPresets[2].ChangeAudioReverbForPlayer(playerControllerB);
        }
        playerControllerB.isInElevator = false;
        playerControllerB.isInHangarShipRoom = false;
        playerControllerB.isInsideFactory = true;
        playerControllerB.averageVelocity = 0f;
        playerControllerB.velocityLastFrame = Vector3.zero;
        
        StartOfRound.Instance.allPlayerScripts[playerObj].TeleportPlayer(teleportPos);
        StartOfRound.Instance.allPlayerScripts[playerObj].beamOutParticle.Play();
        if (playerControllerB == GameNetworkManager.Instance.localPlayerController)
        {
            HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
        }
    }
}