using System;
using System.Globalization;
using EPiServer.Core;

namespace GoogleMapsLocation
{
	public class GoogleMapsLocation : IReadOnly<GoogleMapsLocation>, IConvertible
	{
		private bool _isReadOnly;

		public GoogleMapsLocation(string address, double latitude, double longitude)
		{
			Address = address;
			Latitude = latitude;
			Longitude = longitude;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1},{2})", Address, Latitude.ToString(GoogleMapsDoubleFormatProvider), Longitude.ToString(GoogleMapsDoubleFormatProvider));
		}

		public static GoogleMapsLocation Parse(string value)
		{
			if (string.IsNullOrEmpty(value))
				return null;
			
			if (!value.Contains("(") && !value.Contains(")"))
				return null;
			
			if (value.IndexOf(")") < value.IndexOf("("))
				return null;

			GoogleMapsLocation location = null;

			string address = value.Substring(0, value.IndexOf(" ("));
			string coordinates = value.Substring(value.IndexOf("(") + 1, value.IndexOf(")") - value.IndexOf("(") - 1);
			string[] splitCoordinates = coordinates.Split(new[] { ',' });
			
			double latitude;
			double longitude;
			if (double.TryParse(splitCoordinates[0], NumberStyles.Float, GoogleMapsDoubleFormatProvider, out latitude) 
				&& double.TryParse(splitCoordinates[1], NumberStyles.Float, GoogleMapsDoubleFormatProvider, out longitude))
			{
				location = new GoogleMapsLocation(address, latitude, longitude);
			}

			return location;
		}

		public string Address { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public static IFormatProvider GoogleMapsDoubleFormatProvider
		{
			get 
			{
				return CultureInfo.GetCultureInfo("en-US");
			}
		}

		#region IReadOnly<GoogleMapsLocation> Members

		public GoogleMapsLocation CreateWritableClone()
		{
			GoogleMapsLocation clone = (GoogleMapsLocation)MemberwiseClone();
			clone._isReadOnly = false;
			return clone;
		}

		#endregion

		#region IReadOnly Members

		public bool IsReadOnly
		{
			get { return _isReadOnly; }
		}

		public void MakeReadOnly()
		{
			_isReadOnly = true;
		}

		#endregion

		#region IConvertible Members

		public TypeCode GetTypeCode()
		{
			throw new NotImplementedException();
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public byte ToByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public char ToChar(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public double ToDouble(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public short ToInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public int ToInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public long ToInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public float ToSingle(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public string ToString(IFormatProvider provider)
		{
			return ToString();
		}

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public ulong ToUInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
