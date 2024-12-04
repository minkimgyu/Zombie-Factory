using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundControllable
{
    public enum SoundName
    {
        PistolFire,
        RifleFire,
        DMRFire,
        SniperFire,

        ShotgunFire,
        ShotgunDotFire,
        ShotgunExplosionFire,

        MagIn,
        MagOut,

        PullHandle,
        PushHandle,

        PushAmmo,
        EquipShotgun,
        EquipKnife,

        EquipMagazineGun,

        ZoomIn,
        ZoomOut,

        Reload,
        Die,
        Click,

        KnifeStap,
        GetItem,

        Lobby,
        InGame,
    }

    void PlayBGM(SoundName name);
    void PlayBGM(SoundName name, float volume);


    void PlaySFX(SoundName name);
    void PlaySFX(SoundName name, float volume);
    void PlaySFX(SoundName name, Vector3 pos, float volume);

    void StopBGM();
    void StopAllSound();
}