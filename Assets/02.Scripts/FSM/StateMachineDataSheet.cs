using Platformer.Datum;
using Platformer.FSM.Character;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.FSM
{
    public static class StateMachineDataSheet
    {
        public static IDictionary<CharacterStateID, IState<CharacterStateID>>
            GetPlayerData(CharacterMachine machine)
        {
            return new Dictionary<CharacterStateID, IState<CharacterStateID>>()
            {
                { CharacterStateID.Idle, new Idle(machine) },
                { CharacterStateID.Move, new Move(machine) },
                { CharacterStateID.Fall, new Fall(machine, 0.8f) },
                { CharacterStateID.Jump, new Jump(machine, 3.2f) },
                { CharacterStateID.DoubleJump, new DoubleJump(machine, 3.2f) },
                { CharacterStateID.DownJump, new DownJump(machine) },
                { CharacterStateID.Crouch, new Crouch(machine, new Vector2(0.0f, 0.06f), new Vector2(0.12f, 0.15f)) },
                { CharacterStateID.Land, new Land(machine) },
                { CharacterStateID.LadderUp, new LadderUp(machine) },
                { CharacterStateID.LadderDown, new LadderDown(machine) },
                { CharacterStateID.WallSlide, new WallSlide(machine) },
                { CharacterStateID.Dash, new Dash(machine, 0.8f) },
                { CharacterStateID.Slide, new Slide(machine, 0.8f, new Vector2(0.0f, 0.06f), new Vector2(0.12f, 0.15f)) },
                { CharacterStateID.Hurt, new Hurt(machine) },
                { CharacterStateID.Die, new Die(machine) },
            { CharacterStateID.Attack, new Attack(machine, 0.5f,
                    new SkillCastSetting[]
                    {
                        SkillCastSettingAssets.instance["PlayerAttack1"],
                        SkillCastSettingAssets.instance["PlayerAttack2"],
                    }) },
            };
        }

        public static IDictionary<CharacterStateID, IState<CharacterStateID>> GetSlugData(CharacterMachine machine)
        {
            return new Dictionary<CharacterStateID, IState<CharacterStateID>>()
            {
                { CharacterStateID.Idle, new Idle(machine) },
                { CharacterStateID.Move, new Move(machine) },
                { CharacterStateID.Fall, new Fall(machine, 0.8f) },
                { CharacterStateID.Jump, new Jump(machine, 1.5f) },
                { CharacterStateID.Land, new Land(machine) },
                { CharacterStateID.Hurt, new Hurt(machine) },
                { CharacterStateID.Die, new Die(machine) },
            };
        }

    }
}