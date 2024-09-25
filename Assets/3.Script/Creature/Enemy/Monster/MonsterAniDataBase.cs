using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterType
{
    None,
    Minotaur,
    Wizard,
    Skelleton,
    Oak
};

public enum EMonsterAni
{
    Idle,
    LocoBlend,
    Run,
    Hit,
    WalkLeft,
    WalkRight
};

public class MonsterAniDataBase
{
    public int[] minoAttackClips {  get; private set; }

    public Dictionary<EMonsterAni, int> monsterAniClips {  get; private set; }
    public Dictionary<EMonsterAni, int> monsterParams {  get; private set; }

    public void init()
    {
        monsterAniClips = new Dictionary<EMonsterAni, int>();
        monsterParams = new Dictionary<EMonsterAni, int>();
        minoAttackClips = new int[5];

        //Clips
        monsterAniClips.Add(EMonsterAni.LocoBlend, Animator.StringToHash("Base Layer.LocoBlend"));
        monsterAniClips.Add(EMonsterAni.Idle, Animator.StringToHash("Base Layer.idle"));
        monsterAniClips.Add(EMonsterAni.Run, Animator.StringToHash("Base Layer.Run"));
        monsterAniClips.Add(EMonsterAni.Hit, Animator.StringToHash("Base Layer.Hit"));
        monsterAniClips.Add(EMonsterAni.WalkLeft, Animator.StringToHash("Base Layer.Walk Left"));
        monsterAniClips.Add(EMonsterAni.WalkRight, Animator.StringToHash("Base Layer.Walk Right"));

        //Params
        monsterParams.Add(EMonsterAni.LocoBlend, Animator.StringToHash("LocoBlend"));

        //AttackDB
        minoAttackClips[0] = Animator.StringToHash("Base Layer.attack1");
        minoAttackClips[1] = Animator.StringToHash("Base Layer.attack2");
        minoAttackClips[2] = Animator.StringToHash("Base Layer.attack3");
        minoAttackClips[3] = Animator.StringToHash("Base Layer.attack4_kick");
        minoAttackClips[4] = Animator.StringToHash("Base Layer.attack5_kick");
    }
}
