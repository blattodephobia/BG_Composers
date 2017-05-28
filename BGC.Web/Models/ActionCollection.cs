using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace BGC.Web.Models
{
    public partial class ActionCollection : IEnumerable<MethodInfo>
    {
        private static readonly IReadOnlyDictionary<string, HttpVerbs> StringToHttpVerb = new Dictionary<string, HttpVerbs>()
        {
            { "GET",     HttpVerbs.Get     },
            { "POST",    HttpVerbs.Post    },
            { "PATCH",   HttpVerbs.Patch   },
            { "OPTIONS", HttpVerbs.Options },
            { "PUT",     HttpVerbs.Put     },
            { "HEAD",    HttpVerbs.Head    },
            { "DELETE",  HttpVerbs.Delete  },
        };

        private static readonly IReadOnlyDictionary<Type, HttpVerbs> AttributeToHttpVerb = new Dictionary<Type, HttpVerbs>()
        {
            { typeof(HttpPostAttribute),    HttpVerbs.Post    },
            { typeof(HttpGetAttribute),     HttpVerbs.Get     },
            { typeof(HttpPatchAttribute),   HttpVerbs.Patch   },
            { typeof(HttpDeleteAttribute),  HttpVerbs.Delete  },
            { typeof(HttpOptionsAttribute), HttpVerbs.Options },
            { typeof(HttpPutAttribute),     HttpVerbs.Put     },
            { typeof(HttpHeadAttribute),    HttpVerbs.Head    },
        };

        private static IEnumerable<ActionVerbKey> GetAllVerbs(string actionName)
        {
            return new List<ActionVerbKey>()
            {
                new ActionVerbKey(actionName, HttpVerbs.Get),
                new ActionVerbKey(actionName, HttpVerbs.Put),
                new ActionVerbKey(actionName, HttpVerbs.Post),
                new ActionVerbKey(actionName, HttpVerbs.Head),
                new ActionVerbKey(actionName, HttpVerbs.Patch),
                new ActionVerbKey(actionName, HttpVerbs.Delete),
                new ActionVerbKey(actionName, HttpVerbs.Options),
            };
        }

        /// <summary>
        /// Creates a new <see cref="ActionCollection"/> from the public methods returning an <see cref="ActionResult"/> of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ActionCollection FromType(Type type)
        {
            IEnumerable<MethodInfo> actionMethods = from method in type.GetMethods()
                                                    let returnType = method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                                                            ? method.ReturnType.GetGenericArguments().First()
                                                            : method.ReturnType
                                                    where typeof(ActionResult).IsAssignableFrom(returnType) &&
                                                          method.GetCustomAttribute<NonActionAttribute>() == null
                                                    select method;
            ActionCollection result = new ActionCollection(actionMethods);
            return result;
        }
        
        private Dictionary<ActionVerbKey, MethodInfo> _storage = new Dictionary<ActionVerbKey, MethodInfo>();

        public ActionCollection(params MethodInfo[] actionMethods) :
            this(actionMethodsCollection: actionMethods)
        {
        }

        public ActionCollection(IEnumerable<MethodInfo> actionMethodsCollection)
        {
            Dictionary<ActionVerbKey, MethodInfo> actionsWithoutAttributes = new Dictionary<ActionVerbKey, MethodInfo>();
            foreach (MethodInfo method in actionMethodsCollection)
            {
                string methodName = method.Name;
                string actionName = method.GetCustomAttribute<ActionNameAttribute>()?.Name ?? method.Name;
                IEnumerable<ActionVerbKey> keys = method.GetCustomAttributes<ActionMethodSelectorAttribute>().SelectMany(attr =>
                {
                    List<ActionVerbKey> methodKeys = new List<ActionVerbKey>();
                    AcceptVerbsAttribute verbs = attr as AcceptVerbsAttribute;
                    if (verbs != null)
                    {
                        methodKeys.AddRange(verbs.Verbs.Select(s => new ActionVerbKey(actionName, StringToHttpVerb[s])));
                    }
                    else
                    {
                        methodKeys.Add(new ActionVerbKey(actionName, AttributeToHttpVerb[attr.GetType()]));
                    }

                    return methodKeys;
                });

                if (!keys.Any())
                {
                    foreach (ActionVerbKey queuedKey in GetAllVerbs(actionName))
                    {
                        actionsWithoutAttributes.Add(queuedKey, method);
                    }
                }

                foreach (ActionVerbKey key in keys)
                {
                    _storage.Add(key, method);
                }
            }

            foreach (ActionVerbKey queuedKey in actionsWithoutAttributes.Keys)
            {
                if (!_storage.ContainsKey(queuedKey))
                {
                    _storage.Add(queuedKey, actionsWithoutAttributes[queuedKey]);
                }
            }
        }

        public MethodInfo this[string actionName, HttpVerbs verbs = HttpVerbs.Get]
        {
            get
            {
                var key = new ActionVerbKey(actionName, verbs);
                return _storage.ContainsKey(key) ? _storage[key] : null;
            }
        }

        public IEnumerator<MethodInfo> GetEnumerator() => _storage.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}