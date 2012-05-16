using System;
using EPiServer.Core;
using EPiServer.PlugIn;
using GoogleMapsLocation.Control;

namespace GoogleMapsLocation
{
	[Serializable, PageDefinitionTypePlugIn]
	public class PropertyGoogleMapsLocation : PropertyData
	{
		private GoogleMapsLocation _location;

		public override IPropertyControl CreatePropertyControl()
		{
			return new PropertyGoogleMapsLocationControl();
		}

		public static PropertyGoogleMapsLocation Parse(string value)
		{
			PropertyGoogleMapsLocation property = new PropertyGoogleMapsLocation();
			property.Value = GoogleMapsLocation.Parse(value);
			return property;
		}

		public override PropertyData ParseToObject(string value)
		{
			return Parse(value);
		}

		public override void ParseToSelf(string value)
		{
			Value = Parse(value);
		}

		public override Type PropertyValueType
		{
			get { return typeof(string); }
		}

		protected override void SetDefaultValue()
		{
			ThrowIfReadOnly();
			_location = null;
		}

		public override PropertyDataType Type
		{
			get { return PropertyDataType.String; }
		}

		protected virtual GoogleMapsLocation Location
		{
			get 
			{ 
				return _location; 
			}
			set
			{
				ThrowIfReadOnly();

				if (value == null)
				{
					Clear();
				}
				else if ((_location != value) || IsNull)
				{
					_location = value;
					Modified();
				}
			}
		}

		public override object Value
		{
			get
			{
				return Location;
			}
			set
			{
				SetPropertyValue(value, delegate 
					{
						if (value == null)
							Location = null;
						else if (value is GoogleMapsLocation)
							Location = (GoogleMapsLocation)value;
						else
							Location = GoogleMapsLocation.Parse(value as string);
					}
				);
			}
		}
	}
}
