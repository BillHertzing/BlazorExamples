using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// For the dynamic Proxy object
using System.Dynamic;
// To get the Logging services from the MS DI Container
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GUI.State {

    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddState(this IServiceCollection services) {
            return services.AddSingleton<IState, State>();
        }
    }

    /*
    public static class StateExtensions {
        public static State Merge(State aState, State anotherState) {
            List<Page> newPages = aState.Pages as List<Page>;
            newPages.AddRange(anotherState.Pages);
            StateBuilder newStateBuilder = new StateBuilder().AddPages(newPages);
            return newStateBuilder.Build();
        }

    }
    */

    public interface IState {
        IEnumerable<Page> Pages { get; set; }
        dynamic P { get; set; }
    }

    public class State : IState {
        // public ILogger<State> Logger { get; set; }

        // The list of pages in an application
        public IEnumerable<Page> Pages { get; set; }

        // Declare a dynamic object to hold the actual data
        public dynamic P { get; set; }

        public State()  {

        }

        /* 
        // ToDo move to later demo
        private void BuildDynamicDictionary(IEnumerable<Page> pages, out dynamic p) {
            foreach (var _p in pages) {
                // Record p
                foreach (var _e in elements) {
                    // Record e
                    foreach (var _a in attributes) {
                        // Record a
                        foreach (var _kvp in kvp) {
                            // Record attribute name and attribute value
                        }
                    }
                }
                foreach (var _c in collections) {
                    // Record c
                    foreach (var _e in elements) {
                        // Record e
                        foreach (var _a in attributes) {
                            // Record a
                            foreach (var _kvp in kvp) {
                                // Record attribute name and attribute value
                            }
                        }
                    }
                }
            }
            DynamicDictionary P = new DynamicDictionary();
            IEnumerable<Page> _pT;
            IEnumerable<Element> _eT;
            IEnumerable<Collection> _eT;
            IEnumerable<Attribute> _aT;
            IEnumerable<KeyValuePair<string,string>> _kvpT;
            IEnumerable<KeyValuePair<string, Func<Task>>>>_ftrT;
            foreach (var _p in pages) {
                // Record p
                _pT.TryAdd(_p);
                foreach (var _e in elements) {
                    // Record e
                    _eT.TryAdd(_p, _e);
                    foreach (var _a in attributes) {
                        // Record a
                        _aC.TryAdd(_p, _e, _a);
                        foreach (var _kvp in kvp) {
                            // Record attribute name and attribute value
                            _kvpC.TryAdd(_p, _e, _a, _kvp.name, _kvp.Value);
                        }
                        foreach (var _frt in frt) {
                            // Record attribute name and attribute value
                            _frtC.TryAdd(_p, _e, _a, _frt.name, _frt.Value);
                        }
                    }
                }
                foreach (var _c in collections) {
                    // Record c
                    _cC.TryAdd(_p, _c);
                    foreach (var _e in elements) {
                        // Record e
                        _eT.TryAdd(_p, _c, _e);
                        foreach (var _a in attributes) {
                            // Record a
                            _aC.TryAdd(_p, _c, _e, _a);
                            foreach (var _kvp in kvp) {
                                // Record attribute name and attribute value
                                _kvpC.TryAdd(_p, _c, _e, _a, _kvp.name, _kvp.Value);
                            }
                            foreach (var _frt in frt) {
                                // Record attribute name and attribute value
                                _frtC.TryAdd(_p, _c, _e, _a, _frt.name, _frt.Value);
                            }
                        }
                    }
                }
            }
            // return the dynamic object back to the caller
            p=P;
        }
    */
    }
    public interface IStateBuilder {
        IStateBuilder AddPage(Page p);
        State Build();
    }

    public class StateBuilder : IStateBuilder {
        public StateBuilder() {
            Pages=new List<Page>();
        }

        //public ILogger<StateBuilder> Logger { get; set; }

        public List<Page> Pages { get; set; }

        public IStateBuilder AddPage(Page p) {
            //Logger.LogDebug($"in AddPage p = {p}");
            this.Pages.Add(p);
            return this;
        }
        public State Build() {
            State newState = new State() {
                Pages=this.Pages,
                P=new DynamicDictionary()
            };
            //ToDo: BuildDynamicDictionary(newState.Pages, out newState.P);
            return newState;
        }
    }

    public interface IPageBuilder {
        IPageBuilder AddPAID(string paid);
        IPageBuilder AddElement(Element element);
        Page Build();
    }

    public class PageBuilder : IPageBuilder {
        public string PAID { get; set; }
        public List<Element> Elements { get; set; }
        public PageBuilder() {
            Elements=new List<Element>();
        }
        public IPageBuilder AddPAID(string paid) {
            this.PAID=paid;
            return this;
        }

        public IPageBuilder AddElement(Element element) {
            this.Elements.Add(element);
            return this;
        }

        public Page Build() {
            Page newPage = new Page() {
                PAID=this.PAID,
                Elements=this.Elements
            };
            return newPage;
        }

    }

    public class Page {
        public string PAID { get; set; }
        public IEnumerable<Element> Elements { get; set; }
        public Page() { }
    }


    public interface IElementBuilder {
        IElementBuilder AddNOID(NOID n);
        IElementBuilder AddVisualAttribute(VisualAttribute v);
        IElementBuilder AddAsyncMethodReturningTaskAttribute(Func<Task> m);
        Element Build();
    }
    public class ElementBuilder : IElementBuilder {
        public NOID NOID { get; set; }
        public List<VisualAttribute> VisualAttributes { get; set; }
        public List<Func<Task>> AsyncMethodReturningTaskAttributes { get; set; }
        public ElementBuilder() {
            VisualAttributes=new List<VisualAttribute>();
            AsyncMethodReturningTaskAttributes=new List<Func<Task>>();
        }
        public IElementBuilder AddNOID(NOID n) {
            this.NOID=n;
            return this;
        }

        public IElementBuilder AddVisualAttribute(VisualAttribute v) {
            this.VisualAttributes.Add(v);
            return this;
        }
        public IElementBuilder AddAsyncMethodReturningTaskAttribute(Func<Task> m) {
            this.AsyncMethodReturningTaskAttributes.Add(m);
            return this;
        }
        public Element Build() {
            Element newElement = new Element() {
                NOID=this.NOID,
                VisualAttributes=this.VisualAttributes,
                AsyncMethodReturningTaskAttributes=this.AsyncMethodReturningTaskAttributes
            };
            return newElement;
        }
    }

    public class Element {
        public NOID NOID { get; set; }
        public IEnumerable<VisualAttribute> VisualAttributes { get; set; }
        public IEnumerable<Func<Task>> AsyncMethodReturningTaskAttributes { get; set; }
        public Element() { }
    }

    public class VisualAttribute : Attribute {
        public VisualAttribute() : base() { }
    }

    public class Attribute {
        public KeyValuePair<string, string> KVP { get; set; }

        public Attribute() { }

        public override string ToString() {
            return KVP.ToString();
        }

    }

    public class DataAttrbute<T> {
        public T Value { get; set; }
    }

    public class NOID {
        public KeyValuePair<string, string> Name { get; set; }
        public KeyValuePair<string, string> ID { get; set; }
        public NOID() { }

        public NOID(string NVal, string IVal) {
            Name=new KeyValuePair<string, string>("name", NVal);
            ID=new KeyValuePair<string, string>("id", IVal);
        }

        public override string ToString() {
            if (!(string.IsNullOrWhiteSpace(Name!.Value))&&!(string.IsNullOrWhiteSpace(ID!.Value))) {
                return $"{Name.ToString()} {ID.ToString()}";
            } else if (string.IsNullOrWhiteSpace(Name!.Value)) {
                return ID.ToString();
            } else {
                return Name.ToString();
            }
        }
    }
    public static class KVPExtensions {
        static string ToString(KeyValuePair<string, string> KVP) {
            //ToDo: investigate faster alternatives if needed, also
            // ToDo: support for other kinds of whitespace characters
            return $"{(KVP.Key.Contains(" ") ? ""+KVP.Key+"" : KVP.Key)}={(KVP.Value.Contains(" ") ? ""+KVP.Value+"" : KVP.Value)}";
        }
    }

    // https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject?view=netcore-3.0
    public class DynamicDictionary : DynamicObject {
        // The inner dictionary.
        Dictionary<string, object> dictionary
            = new Dictionary<string, object>();

        // This property returns the number of elements
        // in the inner dictionary.
        public int Count {
            get {
                return dictionary.Count;
            }
        }

        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(
            GetMemberBinder binder, out object result) {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return dictionary.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(
            SetMemberBinder binder, object value) {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            dictionary[binder.Name.ToLower()]=value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }
    }

    // Create an enumeration for the states that a trigger can be in
    public enum TriggerStates {
        //ToDo: Add [LocalizedDescription("Ignore", typeof(Resource))]
        //[Description("Ignore")]
        Ignore,
        //[Description("Active")]
        Active,
        //[Description("Enqueue")]
        Enqueue
    }

    // Create an enumeration for the kinds of triggers
    public enum StateTriggerKinds {
        //ToDo: Add an attribute for "<timer>" value
        //ToDo: Add [LocalizedDescription("Expired", typeof(Resource))]
        //[Description("Expired")]
        Expired,
        //ToDo: Add an attribute for "<input class=>" value
        //ToDo: Add [LocalizedDescription("OnClick", typeof(Resource))]
        //ToDo: Add an attribute for "<input class=>" value
        //[Description("OnClick")]
        OnClick
    }

    // A class for identifying StateTransitionTrigger handlers
    public class StateTransitionTriggerHandler {
        public string ElementName;
        public string ElementType;
        public StateTriggerKinds TriggerKind;
        public TriggerStates TriggerState;
        public Func<Task> MethodToUse;

        public StateTransitionTriggerHandler() {
        }

        public StateTransitionTriggerHandler(string elementName, string elementType, StateTriggerKinds triggerKind, TriggerStates triggerState, Func<Task> methodToUse) {
            ElementName=elementName;
            ElementType=elementType;
            TriggerKind=triggerKind;
            TriggerState=triggerState;
            MethodToUse=methodToUse;
        }
    }

}
