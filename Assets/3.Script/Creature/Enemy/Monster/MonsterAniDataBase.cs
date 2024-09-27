using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterType
{
    None,
    Minotaur,
    Head,
    Skelleton,
    Oak
};

public enum EMonsterAni
{
    Idle,
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
    public int[] headAttackClips { get; private set; } 
    public int[] headSkillClips { get; private set; }

    public Dictionary<EMonsterAni, int> monsterAniClips {  get; private set; }
    public Dictionary<EMonsterAni, int> monsterParams {  get; private set; }

    public void init()
    {
        monsterAniClips = new Dictionary<EMonsterAni, int>();
        monsterParams = new Dictionary<EMonsterAni, int>();
        minoAttackClips = new int[5];
        headAttackClips = new int[2];
        headSkillClips = new int[1];

        //Clips---------------------------------------------------------------------------------------
        monsterAniClips.Add(EMonsterAni.Idle, Animator.StringToHash("Base Layer.idle"));
        monsterAniClips.Add(EMonsterAni.LocoBlend, Animator.StringToHash("Base Layer.LocoBlend"));
        monsterAniClips.Add(EMonsterAni.RunBlend, Animator.StringToHash("Base Layer.RunBlend"));
        monsterAniClips.Add(EMonsterAni.RotateBlend, Animator.StringToHash("Base Layer.RotateBlend"));
        monsterAniClips.Add(EMonsterAni.Hit, Animator.StringToHash("Base Layer.Hit"));
        monsterAniClips.Add(EMonsterAni.WalkLeft, Animator.StringToHash("Base Layer.Walk Left"));
        monsterAniClips.Add(EMonsterAni.WalkRight, Animator.StringToHash("Base Layer.Walk Right"));
        //Clips---------------------------------------------------------------------------------------

        //Params----------------------------------------------------------------------------
        monsterParams.Add(EMonsterAni.LocoBlend, Animator.StringToHash("LocoBlend"));
        monsterParams.Add(EMonsterAni.RunBlend, Animator.StringToHash("RunBlend"));
        monsterParams.Add(EMonsterAni.BlendX, Animator.StringToHash("BlendX"));
        monsterParams.Add(EMonsterAni.BlendY, Animator.StringToHash("BlendY"));
        //Params----------------------------------------------------------------------------

        //AttackDB--------------------------------------------------------------------------
        //Mino
        minoAttackClips[0] = Animator.StringToHash("Base Layer.attack1");
        minoAttackClips[1] = Animator.StringToHash("Base Layer.attack2");
        minoAttackClips[2] = Animator.StringToHash("Base Layer.attack3");
        minoAttackClips[3] = Animator.StringToHash("Base Layer.attack4_kick");
        minoAttackClips[4] = Animator.StringToHash("Base Layer.attack5_kick");
        //Head
        headAttackClips[0] = Animator.StringToHash("Base Layer.Attack1");
        headAttackClips[1] = Animator.StringToHash("Base Layer.Attack2");
        //What else?...
        //AttackDB--------------------------------------------------------------------------

        //SkillDB===========================================================================
        //Head
        headSkillClips[0] = Animator.StringToHash("Base Layer.Scream");
        //SkillDB===========================================================================
    }
}
