
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
	/// <c>Newtonsoft.Json.JsonConverter</c> for <c>UnityEngine.Quaternion</c>.
	/// </summary>
	public class QuaternionConverter : JsonConverter{
		
		/// <summary>
		/// Return <c>false</c>, since default serializer does well.
		/// </summary>
		/// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
		public override bool CanRead{
			get{ return false; }
		}

		/// <summary>
		/// Determine if the type is <c>UnityEngine.Quaternion</c>.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns><c>true</c> if this can convert the specified type; otherwise, <c>false</c>.</returns>
		public override bool CanConvert(Type objectType){
			return typeof(Quaternion) == objectType;
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
		/// Write <c>x</c>, <c>y</c>, <c>z</c> and <c>w</c>.
		/// </summary>
		/// <param name="writer">The <c>Newtonsoft.Json.JsonWriter</c> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer){

			var _value = (Quaternion)value;
			var _object = new JObject();

			_object["x"] = _value.x;
			_object["y"] = _value.y;
			_object["z"] = _value.z;
			_object["w"] = _value.w;

			_object.WriteTo(writer);

		}

	}

}
