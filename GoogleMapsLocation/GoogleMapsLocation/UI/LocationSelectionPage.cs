using System;
using System.Configuration;
using System.Web.UI.WebControls;
using EPiServer.UI;

namespace GoogleMapsLocation.UI
{
	public class LocationSelectionPage : SystemPageBase
	{
		protected HiddenField hiddenFieldLocation;

		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			MasterPageFile = EPiServer.Configuration.Settings.Instance.UIUrl + "MasterPages/EPiServerUI.master";
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			AddGoogleMapsJavascriptReference();
			hiddenFieldLocation.Value = Request.QueryString["value"];
		}

		private void AddGoogleMapsJavascriptReference()
		{
			string googleMapsScript = "http://maps.googleapis.com/maps/api/js?sensor=false";
			System.Web.UI.ScriptManager.RegisterClientScriptInclude(this, typeof(LocationSelectionPage), "googleMapsScript", googleMapsScript);
		}

		protected string DefaultCenter 
		{
			get
			{
				string defaultCenter = ConfigurationManager.AppSettings["GoogleMapsLocationDefaultCenter"];
				if (string.IsNullOrEmpty(defaultCenter))
					defaultCenter = "0,0";
				return defaultCenter;
			} 
		}

		protected string DefaultZoomLevel 
		{ 
			get
			{
				string defaultZoomLevel = ConfigurationManager.AppSettings["GoogleMapsLocationDefaultZoomLevel"];
				if (string.IsNullOrEmpty(defaultZoomLevel))
					defaultZoomLevel = "1";
				return defaultZoomLevel;
			} 
		}

		protected string CurrentLocationZoomLevel
		{
			get
			{
				string currentLocationZoomLevel = ConfigurationManager.AppSettings["GoogleMapsLocationCurrentLocationZoomLevel"];
				if (string.IsNullOrEmpty(currentLocationZoomLevel))
					currentLocationZoomLevel = "13";
				return currentLocationZoomLevel;
			}
		}
	}
}
