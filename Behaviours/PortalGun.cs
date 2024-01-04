using System.Collections.Generic;
using GameNetcodeStuff;
using LethalLib.Modules;
using LvLSystemLC.Behaviours;
using Unity.Netcode;
using UnityEngine;


namespace LvLSystemLC.Behaviours;

public class PortelGun : SaveableObject
{
    public Light laserPointer;
    public Transform lightSource;

    public AudioSource mainAudio;

    public AudioClip[] activateClips;
    public AudioClip[] noAmmoSounds;

    public Transform aimDirection;

    public int maxAmmo = 4;

    private NetworkVariable<int> currentAmmo = new NetworkVariable<int>(4, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<bool> isLaserOn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public GameObject missilePrefab;

    //public float LobForce = 100f;

    private float timeSinceLastShot;

    private PlayerControllerB previousPlayerHeldBy;

    private Material[] ammoLampMaterials;

    public Animator Animator;

    public ParticleSystem particleSystem;

    public LineRenderer laserLine;

    private Transform laserRoot;
    public override void SaveObjectData()
    {
        SaveData.SaveObjectData<int>("rocketLauncherAmmoData", currentAmmo.Value, uniqueId);
    }

    public override void LoadObjectData()
    {
        if (IsHost)
        {
            currentAmmo.Value = SaveData.LoadObjectData<int>("rocketLauncherAmmoData", uniqueId);
        }
    }
    
    public override void Awake()
    {
        base.Awake();

        // get materials from mesh renderer
        var renderer = GetComponentInChildren<MeshRenderer>();
        List<Material> materials = new List<Material>();
        for (int i = 1; i < renderer.materials.Length; i++)
        {
            materials.Add(renderer.materials[i]);
        }

        ammoLampMaterials = materials.ToArray();
    }
}
