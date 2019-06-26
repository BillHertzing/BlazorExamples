﻿using System.Diagnostics.Tracing;

namespace Server {

    public sealed class DemoETWProvider : EventSource {
        public class Tasks {
            public const EventTask Information = (EventTask)1;
        }

        public static DemoETWProvider Log = new DemoETWProvider();

        [Event(1, Message = "{0}", Opcode = EventOpcode.Info, Task = Tasks.Information)]
        public void Information(string message) {
            if (IsEnabled()) {
                WriteEvent(1, message);
            }
        }
    }

}
