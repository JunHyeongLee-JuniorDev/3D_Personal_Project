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
    LocoBlend,
    RunBlend,
    RotateBlend,
    BlendX,
    BlendY,
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
        monsterAniClips.Add(EMonsterAni.RunBlend, Animator.StringToHash("Base Layer.Run Blend"));
        monsterAniClips.Add(EMonsterAni.RotateBlend, Animator.StringToHash("Base Layer.Rotate Blend"));
        monsterAniClips.Add(EMonsterAni.Hit, Animator.StringToHash("Base Layer.Hit"));
        monsterAniClips.Add(EMonsterAni.WalkLeft, Animator.StringToHash("Base Layer.Walk Left"));
        monsterAniClips.Add(EMonsterAni.WalkRight, Animator.StringToHash("Base Layer.Walk Right"));

        //Params
        monsterParams.Add(EMonsterAni.LocoBlend, Animator.StringToHash("LocoBlend"));
        monsterParams.Add(EMonsterAni.RunBlend, Animator.StringToHash("RunBlend"));
        monsterParams.Add(EMonsterAni.BlendX, Animator.StringToHash("BlendX"));
        monsterParams.Add(EMonsterAni.BlendY, Animator.StringToHash("BlendY"));

        //AttackDB
        minoAttackClips[0] = Animator.StringToHash("Base Layer.attack1");
        minoAttackClips[1] = Animator.StringToHash("Base Layer.attack2");
        minoAttackClips[2] = Animator.StringToHash("Base Layer.attack3");
        minoAttackClips[3] = Animator.StringToHash("Base Layer.attack4_kick");
        minoAttackClips[4] = Animator.StringToHash("Base Layer.attack5_kick");
    }
}
