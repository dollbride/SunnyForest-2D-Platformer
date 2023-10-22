using Platformer.Controllers;
using Unity.VisualScripting;

namespace Platformer.FSM
{
    public abstract class CharacterMachine : StateMachine<CharacterStateID>
    {
        public CharacterController owner;

        public CharacterMachine(CharacterController owner)
        {
            this.owner = owner;
        }
    }
}
