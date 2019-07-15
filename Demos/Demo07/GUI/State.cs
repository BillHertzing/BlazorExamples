using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.State {

    public interface IElementBuilder {
        IElementBuilder AddNOID(NOID n);
        IElementBuilder AddVisualAttribute(VisualAttribute v);
        IElementBuilder AddAsyncMethodReturningTaskAttribute(Func<Task> m);
        Element Build();
    }

    public class ElementBuilder : IElementBuilder {
        public NOID NOID;
        public List<VisualAttribute> VisualAttributes;
        public List<Func<Task>> AsyncMethodReturningTaskAttributes;
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
            Element newElement = new Element();
            return newElement;
        }
    }

    public interface IStateBuilder {
        IStateBuilder AddElement(Element e);
        State Build();
    }

    public class StateBuilder : IStateBuilder {
        public List<Element> Elements;
        public StateBuilder() {
            Elements=new List<Element>();
        }
        public IStateBuilder AddElement(Element e) {
            this.Elements.Add(e);
            return this;
        }
        public State Build() {
            State newState = new State(Elements);
            return newState;
        }
    }
    public class State {
        public IEnumerable<Element> Elements;
        public InitializationValues IV;
        public State(IEnumerable<Element> elements) {
            Elements=elements;
        }
    }

    public class Element {
        public NOID NOID;
        public IEnumerable<VisualAttribute> VisualAttributes;
        public IEnumerable<Func<Task>> AsyncMethodReturningTaskAttributes;

        public Element() {
        }
    }
    public class VisualAttribute : Attribute {
        public VisualAttribute() : base() {
        }
    }
    public class Attribute {
        public KeyValuePair<string, string> KVP { get; set; }

        public Attribute() {
        }

        public override string ToString() {
            return KVP.ToString();
        }

    }

    public class NOID {
        public KeyValuePair<string, string> Name;
        public KeyValuePair<string, string> ID;

        public NOID() {
        }

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

    public class InitializationValues {
        IEnumerable<Element> ElementsToBeInitialized;
        public InitializationValues() {
        }
    }
    public static class KVPExtensions {
        static string ToString(KeyValuePair<string, string> KVP) {
            //ToDo: investigate faster alternatives if needed, also
            // ToDo: support for other kinds of whitespace characters
            return $"{(KVP.Key.Contains(" ") ? ""+KVP.Key+"" : KVP.Key)}={(KVP.Value.Contains(" ") ? ""+KVP.Value+"" : KVP.Value)}";
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
