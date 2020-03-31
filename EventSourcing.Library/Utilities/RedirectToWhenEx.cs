using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace EventSourcing.Library.Utilities
{
    public class RedirectToWhenEx
	{
		static readonly MethodInfo InternalPreserveStackTraceMethod =
			typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

		static class Cache
		{
			static Dictionary<Type, IDictionary<Type, MethodInfo>> dictionaries = new Dictionary<Type, IDictionary<Type, MethodInfo>>();

			public static IDictionary<Type, MethodInfo> GetDict(Type instanceType)
			{
				if (!dictionaries.TryGetValue(instanceType, out IDictionary<Type, MethodInfo> value))
				{
					value = instanceType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
							.Where(m => m.Name == "When")
							.Where(m => m.GetParameters().Length == 1)
							.ToDictionary(m => m.GetParameters().First().ParameterType, m => m);
					dictionaries.Add(instanceType, value);
				}
				return value;
			}
		}

		[DebuggerNonUserCode]
		public static void InvokeEventOptional(object instance, Type instanceType, object @event)
		{
			MethodInfo info;
			var type = @event.GetType();
			if (!Cache.GetDict(instanceType).TryGetValue(type, out info))
			{
				// we don't care if state does not consume events
				// they are persisted anyway
				return;
			}
			try
			{
				info.Invoke(instance, new[] { @event });
			}
			catch (TargetInvocationException ex)
			{
				if (null != InternalPreserveStackTraceMethod)
					InternalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
				throw ex.InnerException;
			}
		}
	}

}
