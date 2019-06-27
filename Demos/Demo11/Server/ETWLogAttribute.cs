using MethodBoundaryAspect.Fody.Attributes;

namespace Server {
    public sealed class ETWLogAttribute : OnMethodBoundaryAspect {
        public override void OnEntry(MethodExecutionArgs args) {
            DemoETWProvider.Log.Information($"Init: {args.Method.DeclaringType.FullName}.{args.Method.Name} [{args.Arguments.Length}] params");
            foreach (var item in args.Method.GetParameters()) {
                DemoETWProvider.Log.Information($"{item.Name}: {args.Arguments[item.Position]}");
            }
        }

        public override void OnExit(MethodExecutionArgs args) {
            DemoETWProvider.Log.Information($"Exit: [{args.ReturnValue}]");
        }

        public override void OnException(MethodExecutionArgs args) {
            //ToDo: Add Error level or category to DemoETWProvider
            DemoETWProvider.Log.Information($"OnException: {args.Exception.GetType()}: {args.Exception.Message}");
        }
    }
}
