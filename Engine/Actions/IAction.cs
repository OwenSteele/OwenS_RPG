using System;
using Engine.Models;

namespace Engine.Actions
{
    //all action classes are bound to this interface, i.e. they must have an event<string> and an Execute function
    //not an interface - this is a check, ensures all actions will fit a model.
    public interface IAction
    {
        event EventHandler<string> OnActionPerformed;
        void Execute(LivingEntity actor, LivingEntity target);
    }
}
