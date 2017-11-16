
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEngine;
using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WanzyeeStudio{
	/// <summary>
	/// Integrate custom <c>Newtonsoft.Json.JsonConverter</c> to use 
	/// <a href="http://www.newtonsoft.com/json" target="_blank">Json.NET</a> in Unity.
	/// </summary>
	/// 
	/// <remarks>
	/// To use Json.NET, please set Unity "PlayerSettings/Api Compatibility Lavel" to .NET 2.0.
	/// Then download from its website and import the .NET 2.0 dll.
	/// Json.NET doesn't support serializing some types originally, e.g., <c>UnityEngine.Vector3</c>.
	/// This has the <c>defaultSettings</c> includes necessary custom converters by default for Unity using it.
	/// And assign to <c>Newtonsoft.Json.JsonConvert.DefaultSettings</c> when initializing if the original <c>null</c>.
	/// </remarks>
	/// 
	/// <example>
	/// Now we can use Json.NET just like before:
	/// </example>
	/// 
	/// <code>
	/// Debug.Log(JsonConvert.SerializeObject(Vector3.up));
	/// var vec = JsonConvert.DeserializeObject("{'x':1.0,'y':0.0}", typeof(Vector2));
	/// </code>
	/// 
	/// <example>
	/// User can directly modify <c>defaultSettings</c> for customization, and override it:
	/// </example>
	/// 
	/// <code>
	/// JsonConvert.DefaultSettings = () => new JsonSerializerSettings(){
	/// 	Converters = JsonNetUtility.defaultSettings.Converters,
	/// 	DefaultValueHandling = DefaultValueHandling.Populate
	/// };
	/// </code>
	/// 
	public static class JsonNetUtility{

		/// <summary>
		/// Initialize when start up, set <c>Newtonsoft.Json.JsonConvert.DefaultSettings</c> if not yet.
		/// </summary>
		#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
		#else
		[RuntimeInitializeOnLoadMethod]
		#endif
		private static void Initialize(){
			if(null == JsonConvert.DefaultSettings) JsonConvert.DefaultSettings = () => defaultSettings;
		}

		/// <summary>
		/// The default <c>Newtonsoft.Json.JsonSerializerSettings</c>.
		/// </summary>
		/// 
		/// <remarks>
		/// All its properties stay default, but the <c>Converters</c> includes below:
		/// 	1. All custom <c>Newtonsoft.Json.JsonConverter</c> with constructor needs no params.
		/// 	2. All <c>Newtonsoft.Json.JsonConverter</c> from <c>WanzyeeStudio.Json</c>.
		/// 	3. <c>Newtonsoft.Json.Converters.StringEnumConverter</c>.
		/// 	4. <c>Newtonsoft.Json.Converters.VersionConverter</c>.
		/// </remarks>
		/// 
		public static JsonSerializerSettings defaultSettings = new JsonSerializerSettings(){
			
			Converters = AppDomain.CurrentDomain.GetAssemblies(

				).SelectMany((assembly) => assembly.GetTypes()
				).Where((type) => typeof(JsonConverter).IsAssignableFrom(type)
				
				).Where((type) => (!type.IsAbstract && !type.IsGenericTypeDefinition)
				).Where((type) => null != type.GetConstructor(new Type[0])
				
				).Where((type) => !(null != type.Namespace && type.Namespace.StartsWith("Newtonsoft.Json"))
				).OrderBy((type) => null != type.Namespace && type.Namespace.StartsWith("WanzyeeStudio")
				
				).Union(new []{typeof(StringEnumConverter), typeof(VersionConverter)}
				).Select((type) => Activator.CreateInstance(type) as JsonConverter

			).ToList()

		};

	}

}
