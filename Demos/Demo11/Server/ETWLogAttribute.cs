using MethodBoundaryAspect.Fody.Attributes;

namespace Server {
    public sealed class ETWLogAttribute : OnMethodBoundaryAspect {
        // public override void OnEntry(MethodExecutionArgs args, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
        public override void OnEntry(MethodExecutionArgs args) {
            DemoETWProvider.Log.MethodBoundryFromAspect($"<{args.Method.DeclaringType.FullName}.{args.Method.Name}");
            //foreach (var item in args.Method.GetParameters()) {
            //    DemoETWProvider.Log.Information($"{item.Name}: {args.Arguments[item.Position]}");
            //}
        }

        //public override void OnExit(MethodExecutionArgs args, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
        public override void OnExit(MethodExecutionArgs args) {
            // DemoETWProvider.Log.MethodBoundry($">[{args.ReturnValue}]");
            DemoETWProvider.Log.MethodBoundryFromAspect($">{args.Method.DeclaringType.FullName}.{args.Method.Name}");
        }

        public override void OnException(MethodExecutionArgs args) {
            //ToDo: Add Error level or category to DemoETWProvider
            DemoETWProvider.Log.Information($"OnException: {args.Exception.GetType()}: {args.Exception.Message}");
        }
    }
}
