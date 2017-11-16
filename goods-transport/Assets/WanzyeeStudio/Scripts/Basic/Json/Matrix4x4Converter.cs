
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEngine;
using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WanzyeeStudio.Json{

	/// <summary>
	/// <c>Newtonsoft.Json.JsonConverter</c> for <c>UnityEngine.Matrix4x4</c>.
	/// </summary>
	public class Matrix4x4Converter : JsonConverter{
		
		/// <summary>
		/// Return <c>false</c>, since default serializer does well.
		/// </summary>
		/// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
		public override bool CanRead{
			get{ return false; }
		}

		/// <summary>
		/// Determine if the type is <c>UnityEngine.Matrix4x4</c>.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns><c>true</c> if this can convert the specified type; otherwise, <c>false</c>.</returns>
		public override bool CanConvert(Type objectType){
			return typeof(Matrix4x4) == objectType;
		}

		/// <summary>
		/// Not implemented, unnecessary because <c>CanRead</c> is false.
		/// </summary>
		/// <returns>The object value.</returns>
		/// <param name="reader">The <c>Newtonsoft.Json.JsonReader</c> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override object ReadJson(
			JsonReader reader,
			Type objectType,
			object existingValue,
			JsonSerializer serializer
		){
			throw new InvalidOperationException("Unnecessary because CanRead is false.");
		}

		/// <summary>
		/// Write the 4x4 components.
		/// </summary>
		/// <param name="writer">The <c>Newtonsoft.Json.JsonWriter</c> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer){

			var _value = (Matrix4x4)value;
			var _object = new JObject();

			for(int i = 0; i < 4; i++){
				for(int j = 0; j < 4; j++){
					_object[string.Format("m{0}{1}", i, j)] = _value[i, j];
				}
			}

			_object.WriteTo(writer);

		}

	}

}
