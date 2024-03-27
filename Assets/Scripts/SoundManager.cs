using UnityEngine;

public class SoundManager : Loader <SoundManager>
{
    [SerializeField]
    AudioClip arrow;
    [SerializeField]
    AudioClip death;
    [SerializeField]
    AudioClip fireball;
    [SerializeField]
    AudioClip gameover;
    [SerializeField]
    AudioClip hit;
    [SerializeField]
    AudioClip level;
    [SerializeField]
    AudioClip newgame;
    [SerializeField]
    AudioClip rock;
    [SerializeField]
    AudioClip towerBuild;

    public AudioClip Arrow
    {
        get
        {
            return arrow;
        }
    }

    public AudioClip Death
    {
        get
        {
            return death;
        }
    }

    public AudioClip Fireball
    {
        get
        {
            return fireball;
        }
    }

    public AudioClip Gameover
    {
        get
        {
            return gameover;
        }
    }

    public AudioClip Hit
    {
        get
        {
            return hit;
        }
    }

    public AudioClip Level
    {
        get
        {
            return level;
        }
    }

    public AudioClip Newgame
    {
        get
        {
            return newgame;
        }
    }

    public AudioClip Rock
    {
        get
        {
            return rock;
        }
    }

    public AudioClip TowerBuild
    {
        get
        {
            return towerBuild;
        }
    }
}
