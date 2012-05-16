using EPiServer.Web.PropertyControls;

namespace GoogleMapsLocation.Control
{
	public class PropertyGoogleMapsLocationControl : PropertyDataControl
	{
		public override void ApplyEditChanges()
		{
			SetValue(EditControl.Value);
		}

		public override void CreateEditControls()
		{
			EditControl = new InputGoogleMapsLocation();
			EditControl.Value = PropertyData.Value as GoogleMapsLocation;
			ApplyControlAttributes(EditControl);
			Controls.Add(EditControl);
			SetupEditControls();
		}

		private InputGoogleMapsLocation EditControl { get; set; }
	}
}
