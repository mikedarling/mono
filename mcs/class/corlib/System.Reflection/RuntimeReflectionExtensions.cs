//
// RuntimeReflectionExtensions.cs
//
// Authors:
//       Martin Baulig <martin.baulig@xamarin.com>
//       Marek Safar (marek.safar@gmail.com)
//
// Copyright (c) 2013 Xamarin Inc. (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


using System.Collections.Generic;

namespace System.Reflection
{
	public static class RuntimeReflectionExtensions
	{
		const BindingFlags AllMembersBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

		public static MethodInfo GetMethodInfo (this Delegate del)
		{
			if (del == null)
				throw new ArgumentNullException ("del");

			return del.Method;
		}

		public static MethodInfo GetRuntimeBaseDefinition (this MethodInfo method)
		{
			if (method == null)
				throw new ArgumentNullException ("method");

			return method.GetBaseDefinition ();
		}

		public static EventInfo GetRuntimeEvent (this Type type, string name)
		{
			if (type == null)
				throw new ArgumentNullException ("type");

			return type.GetEvent (name);
		}

		public static IEnumerable<EventInfo> GetRuntimeEvents (this Type type)
		{
			if (type == null)
				throw new ArgumentNullException ("type");

			return type.GetEvents (AllMembersBindingFlags);
		}

		public static FieldInfo GetRuntimeField (this Type type, string name)
		{
			if (type == null)
				throw new ArgumentNullException ("type");

			return type.GetField (name);
		}

		public static IEnumerable<FieldInfo> GetRuntimeFields (this Type type)
		{
			if (type == null)
				throw new ArgumentNullException ("type");

			return type.GetFields (AllMembersBindingFlags);
		}

		public static InterfaceMapping GetRuntimeInterfaceMap (this TypeInfo typeInfo, Type interfaceType)
		{
			if (typeInfo == null)
				throw new ArgumentNullException ("typeInfo");

			return typeInfo.GetInterfaceMap (interfaceType);
		}

		public static MethodInfo GetRuntimeMethod (this Type type, string name, Type[] parameters)
		{
			if (type == null)
				throw new ArgumentNullException ("type");

			return type.GetMethod (name, parameters);
		}

		public static IEnumerable<MethodInfo> GetRuntimeMethods (this Type type)
		{
			if (type == null)
				throw new ArgumentNullException ("type");

			return type.GetMethods (AllMembersBindingFlags);
		}

		public static IEnumerable<PropertyInfo> GetRuntimeProperties (this Type type)
		{
			if (type == null)
				throw new ArgumentNullException ("type");

			return type.GetProperties (AllMembersBindingFlags);
		}

		public static PropertyInfo GetRuntimeProperty (this Type type, string name)
		{
			if (type == null)
				throw new ArgumentNullException ("type");

			return type.GetProperty (name);
		}
	}
}

