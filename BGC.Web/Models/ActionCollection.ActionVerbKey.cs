using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeShield;
using BGC.Utilities;

namespace BGC.Web.Models
{
	public partial class ActionCollection
    {
        private struct ActionVerbKey
        {
            private static IEnumerable<HttpVerbs> BreakDownFlags(HttpVerbs verbs)
            {
                List<HttpVerbs> result = new List<HttpVerbs>();
                if (verbs.HasFlag(HttpVerbs.Get))     result.Add(HttpVerbs.Get);
                if (verbs.HasFlag(HttpVerbs.Post))    result.Add(HttpVerbs.Post);
                if (verbs.HasFlag(HttpVerbs.Put))     result.Add(HttpVerbs.Put);
                if (verbs.HasFlag(HttpVerbs.Delete))  result.Add(HttpVerbs.Delete);
                if (verbs.HasFlag(HttpVerbs.Patch))   result.Add(HttpVerbs.Patch);
                if (verbs.HasFlag(HttpVerbs.Options)) result.Add(HttpVerbs.Options);
                if (verbs.HasFlag(HttpVerbs.Head))    result.Add(HttpVerbs.Head);

                return result;
            }

            private readonly string _actionName;
            private readonly HttpVerbs _verbs;

            public string ActionName => _actionName;

            public HttpVerbs Verbs => _verbs;

            public ActionVerbKey(string actionName, HttpVerbs verbs) :
                this()
            {
                Shield.IsNotNullOrEmpty(actionName).ThrowOnError();

                _actionName = actionName;
                _verbs = verbs;
            }

            public override int GetHashCode() => _actionName.GetHashCode() ^ _verbs.GetHashCode();

            public override bool Equals(object obj)
            {
                if (obj is ActionVerbKey)
                {
                    ActionVerbKey other = (ActionVerbKey)obj;
                    return other._actionName.CompareTo(_actionName) == 0 && other._verbs == _verbs;
                }
                else
                {
                    return false;
                }
            }

            public override string ToString() => $"{_actionName} [{BreakDownFlags(_verbs).ToStringAggregate(",")}]";
        }
    }
}