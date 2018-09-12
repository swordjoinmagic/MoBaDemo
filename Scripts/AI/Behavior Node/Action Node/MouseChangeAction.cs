using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MouseChangeAction : Action{
    public override TaskStatus OnUpdate() {
        MouseIconManager.Instace.ChangeMouseIcon(MouseIconManager.MouseState.Spell);
        return TaskStatus.Success;
    }
}

