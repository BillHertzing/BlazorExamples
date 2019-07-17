using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// For the dynamic Proxy object
using System.Dynamic;
// To get the Logging, LStorage services from the MS DI Container, and to add State service to the DI
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;

namespace GUI.State {

    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddState(this IServiceCollection services) {
            return services.AddSingleton<IState, State>();
        }
    }

    public interface IState {
        IEnumerable<Page> Pages { get; set; }
        dynamic P { get; set; }
    }

    public class State : IState {
        public ILogger<State> Logger { get; set; }
        public Blazored.LocalStorage.ISyncLocalStorageService LStorage { get; set; }

        // The list of pages in an application
        public IEnumerable<Page> Pages { get; set; }

        // Declare a dynamic object to hold the actual data
        public dynamic P { get; set; }

        public State(ILoggerFactory loggerFactory, Blazored.LocalStorage.ISyncLocalStorageService lStorage) {
            Logger=loggerFactory.CreateLogger<State>();
            LStorage=lStorage;
        }
    }
    public interface IStateBuilder {
        IStateBuilder AddPage(Page p);
        State Build();
    }

    // https://docs.microsoft.com/en-us/aspnet/core/blazor/dependency-injection?view=aspnetcore-3.0
    // https://github.com/aspnet/Blazor/issues/424
    // https://stackify.com/net-core-loggerfactory-use-correctly/
    public class StateBuilder : IStateBuilder {
        public ILoggerFactory LoggerFactory { get; set; }
        public Blazored.LocalStorage.ISyncLocalStorageService LStorage { get; set; }
        public StateBuilder(ILoggerFactory loggerFactory, Blazored.LocalStorage.ISyncLocalStorageService lStorage) {
            LoggerFactory=loggerFactory;
            LStorage=lStorage;
            Pages=new List<Page>();
        }

        public List<Page> Pages { get; set; }

        public IStateBuilder AddPage(Page p) {
            this.Pages.Add(p);
            return this;
        }
        public State Build() {
            State newState = new State(LoggerFactory, LStorage) {
                Pages = this.Pages,
                P = new DynamicDictionary(LoggerFactory, LStorage)
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
        IElementBuilder AddDataAttribute(DataAttribute<int> d);
        IElementBuilder AddAsyncMethodReturningTaskAttribute(Func<Task> m);
        Element Build();
    }

    public class ElementBuilder : IElementBuilder {
        public NOID NOID { get; set; }
        public List<VisualAttribute> VisualAttributes { get; set; }
        public List<DataAttribute<int>> DataAttributes { get; set; }
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
        public IElementBuilder AddDataAttribute(DataAttribute<int> d) {
            this.DataAttributes.Add(d);
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
                DataAttributes=this.DataAttributes,
                AsyncMethodReturningTaskAttributes=this.AsyncMethodReturningTaskAttributes
            };
            return newElement;
        }
    }

    public class Element {
        public NOID NOID { get; set; }
        public IEnumerable<VisualAttribute> VisualAttributes { get; set; }
        public IEnumerable<DataAttribute<int>> DataAttributes { get; set; }
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

    public class DataAttribute<T> {
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
    
    // https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject?view=netcore-3.0
    public class DynamicDictionary : DynamicObject {
        // The inner dictionary.
        Dictionary<string, object> dictionary
            = new Dictionary<string, object>();
        public ILogger<DynamicDictionary> Logger { get; set; }
        public Blazored.LocalStorage.ISyncLocalStorageService LStorage { get; set; }

        public DynamicDictionary(ILoggerFactory loggerFactory, ISyncLocalStorageService lStorage) {
            Logger=loggerFactory.CreateLogger<DynamicDictionary>();
            Logger.LogDebug("$<DynamicDictionary .ctor lStorage = {lStorage}");
            LStorage=lStorage;
            Logger.LogDebug("DynamicDictionary .ctor>");
        }

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
            Logger.LogDebug($"<TryGetMember binder.Name = {binder.Name}");

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            //bool success =  dictionary.TryGetValue(binder.Name, out result);
            //if (success)
            //    Logger.LogDebug($"success, result = {result} TryGetMember>");
            //else
            //    Logger.LogDebug($"failure, TryGetMember>");
            result = LStorage.GetItem<string>(binder.Name);
            Logger.LogDebug($"result = {result} TryGetMember>");
            return true;
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(
            SetMemberBinder binder, object value) {
            Logger.LogDebug($"<TrySetMember  binder.Name = {binder.Name}");
            //dictionary[binder.Name]=value;
            LStorage.SetItem(binder.Name, value.ToString());
            // You can always add a value to a dictionary,
            // so this method always returns true.
            Logger.LogDebug("TrySetMember>");
            return true;
        }

        // https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject.trygetindex?view=netframework-4.8
        // Set the property value by index.
        public override bool TrySetIndex(
            SetIndexBinder binder, object[] indexes, object value) {
            int index = (int)indexes[0];
            // browser local storage allows for setting a value and creating a property if necessary atomicly
            LStorage.SetItem("Property"+index, value.ToString());
            return true;
        }

        // Get the property value by index.
        public override bool TryGetIndex(
            GetIndexBinder binder, object[] indexes, out object result) {
            int index = (int)indexes[0];
            result = LStorage.GetItem<object>("Property"+index);
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

    // https://stackoverflow.com/questions/29923280/how-to-override-get-accessor-of-a-dynamic-objects-property
    public class PersistentDynamicDictionary : DynamicObject {
        public ILogger<PersistentDynamicDictionary> Logger { get; set; }
        public Blazored.LocalStorage.ISyncLocalStorageService LStorage { get; set; }

        public Func<string, dynamic, object> PropertyResolver { get; set; }

        private Dictionary<string, object> obj = new Dictionary<string, object>();

        /// <summary>Enables derived types to initialize a new instance of the <see cref="PersistentDynamicDictionary"></see> type.</summary>
        public PersistentDynamicDictionary(ILoggerFactory loggerFactory, Blazored.LocalStorage.ISyncLocalStorageService lStorage) : base() {
            Logger=loggerFactory.CreateLogger<PersistentDynamicDictionary>();
            Logger.LogDebug("< PersistentDynamicDictionary (sync storage).ctor");
            LStorage=lStorage;
            Logger.LogDebug("> PersistentDynamicDictionary (sync storage) .ctor");
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (obj.ContainsKey(binder.Name)) {
                result=obj[binder.Name];
                return true;
            }

            if (PropertyResolver!=null) {
                var actResult = PropertyResolver(binder.Name, this);
                result=actResult;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            Logger.LogDebug("entering TrySetMember");
            Logger.LogDebug($"in TrySetMember binder.Name = {binder.Name}");
            LStorage.SetItem(binder.Name, value.ToString());
            Logger.LogDebug($"in TrySetMember setitem returned");
            // ToDo: Raise OnPropertyChangeNotify event for the state-backed Property
            obj[binder.Name]=value;
            Logger.LogDebug("leaving TrySetMember");
            return true;
        }
    }


}
