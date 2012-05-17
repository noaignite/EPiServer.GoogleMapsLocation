EPiServer.GoogleMapsLocation
==

EPiServer.GoogleMapsLocation is a specialized property for a PageTypeBuilder PageType that allows you to save a Google Maps location. This version was based off the [original source code from EPiCode](https://www.coderesort.com/p/epicode/browser/Nansen.GoogleMapsLocation), and updated for Google Maps API v3. This was developed and tested with EPiServer CMS 6 R2 and PageTypeBuilder 1.3.1.

The GoogleMapsLocation object has properties for the Latitude, Longitude, and Address (the address that was returned from the Google Maps geocoding service).

If you call `ToString()` on the object, the output string will be formatted with all of the object's information, formatted as `Address (Latitude, Longitude)`.

There is also a `Parse(string value)` method that will turn a string, formatted as `Address (Latitude, Longitude)`, into a GoogleMapsLocation object.

Usage
--

1) Set the Type to point to the PropertGoogleMapsLocation class in your PageTypeProperty attribute

	[PageTypeProperty(Type = typeof(PropertyGoogleMapsLocation), EditCaption = "Map Location")]
	public virtual GoogleMapsLocation MapLocation { get; set; }

2) Access the property object as you would any other property

	GoogleMapsLocation mapPoint = CurrentPage.MapLocation;
