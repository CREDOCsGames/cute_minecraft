using System.Collections.Generic;

namespace Puzzle
{
    public static class MediatorBinder
    {
        public static void BindEvent(ICore core, IMediatorCore mediator)
        {
            core.SetMediator(mediator);
        }
        public static void UnbindEvent(ICore core, IMediatorCore mediator)
        {
            core.SetMediator(null);
        }
        public static void BindEvent(List<ICore> cores, IMediatorCore mediator)
        {
            foreach (var core in cores)
            {
                core.SetMediator(mediator);
            }
        }
        public static void UnbindEvent(List<ICore> cores, IMediatorCore mediator)
        {
            foreach (var core in cores)
            {
                core.SetMediator(null);
            }
        }
        public static void BindEvent(IInstance instance, IMediatorInstance mediator)
        {
            instance.SetMediator(mediator);
        }
        public static void UnbindEvent(IInstance instance, IMediatorInstance mediator)
        {
            instance.SetMediator(null);
        }
        public static void BindEvent(List<IInstance> instances, IMediatorInstance mediator)
        {
            foreach (var instance in instances)
            {
                instance.SetMediator(mediator);
            }
        }
        public static void UnbindEvent(List<IInstance> instances, IMediatorInstance mediator)
        {
            foreach (var instance in instances)
            {
                UnbindEvent(instance, mediator);
            }
        }
    }

}
