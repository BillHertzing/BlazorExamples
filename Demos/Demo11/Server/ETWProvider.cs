using System.Diagnostics.Tracing;

namespace Server {

    /// <summary>
    /// 
    /// </summary>
    public sealed class DemoETWProvider : EventSource {
        public class Tasks {
            public const EventTask Information = (EventTask)1;
            public const EventTask MethodBoundry = (EventTask)2;
            public const EventTask MethodBoundryFromAspect = (EventTask)3;
        }

        public static DemoETWProvider Log = new DemoETWProvider();

        [Event(1, Message = "{0}", Opcode = EventOpcode.Info, Task = Tasks.Information)]
        public void Information(string message,[System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
            if (IsEnabled()) {
                WriteEvent(1, message, memberName);
            }
        }
        [Event(2, Message = "{0}", Opcode = EventOpcode.Info, Task = Tasks.MethodBoundry)]
        public void MethodBoundry(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
            if (IsEnabled()) {
                WriteEvent(2, message+memberName);
            }
        }
        [Event(3, Message = "{0}", Opcode = EventOpcode.Info, Task = Tasks.MethodBoundryFromAspect)]
        public void MethodBoundryFromAspect(string message) {
            if (IsEnabled()) {
                WriteEvent(3, message);
            }
        }
    }

}