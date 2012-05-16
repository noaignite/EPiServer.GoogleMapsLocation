<%@ Page Language="C#" AutoEventWireup="true" Inherits="GoogleMapsLocation.UI.LocationSelectionPage" %>

<%@ Register TagPrefix="EPiServerUI" Namespace="EPiServer.UI.WebControls" Assembly="EPiServer.UI" %>
<%@ Register TagPrefix="EPiServerScript" Namespace="EPiServer.ClientScript.WebControls" Assembly="EPiServer" %>
<%@ Register TagPrefix="EPiServerScript" Namespace="EPiServer.UI.ClientScript.WebControls" Assembly="EPiServer.UI" %>

<asp:Content ContentPlaceHolderID="HeaderContentRegion" runat="server">
    <base target="_self" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FullRegion" runat="server">

    <div id="map" style="width: 500px; height: 400px"></div>

    <asp:Panel DefaultButton="buttonSearch" style="margin-top: 8px; position: relative;" runat="server">
        <input id="searchTextBox" class="episize240" style="width: 405px; margin: 1px 8px;" type="text" />
        <span style="position: absolute; top: -2px;">
            <EPiServerUI:ToolButton ID="buttonSearch" GeneratesPostBack="False" runat="server" 
				OnClientClick="searchAddress(document.getElementById('searchTextBox').value); return false;" 
                Text="<%$ Resources: EPiServer, button.search %>" ToolTip="<%$ Resources: EPiServer, button.search %>" SkinID="Search" />
        </span>
    </asp:Panel>
   
    <div style="position: absolute; bottom: 25px; width: 485px; text-align: right;">
        <asp:HiddenField ID="hiddenFieldLocation" runat="server" />
        <EPiServerUI:ToolButton GeneratesPostBack="False" runat="server" OnClientClick="onOK();" Text="<%$ Resources: EPiServer, button.select %>" ToolTip="<%$ Resources: EPiServer, button.select %>" SkinID="Check" />
        <EPiServerUI:ToolButton GeneratesPostBack="False" runat="server" OnClientClick="onNothing();" Text="<%$ Resources: EPiServer, button.clear %>" ToolTip="<%$ Resources: EPiServer, button.clear %>" SkinID="Delete" />
        <EPiServerUI:ToolButton GeneratesPostBack="False" runat="server" OnClientClick="onCancel();" Text="<%$ Resources: EPiServer, button.cancel %>" ToolTip="<%$ Resources: EPiServer, button.cancel %>" SkinID="Cancel" />
    </div>
   
    <asp:ScriptManager runat="server" />

    <script type="text/javascript">
        //<![CDATA[

        var map = null;
        var geocoder = null;
        var marker = null;
		var infowindow = null;
		var returnValue = false;

		$(function(){
			initialize();
		});

        function initialize() {
			var center = new google.maps.LatLng(<%= DefaultCenter %>);
			var zoom = <%= DefaultZoomLevel %>;

        	map = new google.maps.Map(document.getElementById("map"), { mapTypeId: google.maps.MapTypeId.ROADMAP });
			geocoder = new google.maps.Geocoder();
			infowindow = new google.maps.InfoWindow();

			if (document.getElementById("<%= hiddenFieldLocation.ClientID %>").value.length > 0) {
				var coordinates = document.getElementById("<%= hiddenFieldLocation.ClientID %>").value;
                coordinates = coordinates.substring(coordinates.indexOf("(") + 1, coordinates.indexOf(")"));
                coordinates = coordinates.split(",");
				
				var address = document.getElementById("<%= hiddenFieldLocation.ClientID %>").value;
                address = address.substring(0, address.indexOf(" ("));
				center = new google.maps.LatLng(coordinates[0], coordinates[1]);

				addMarker(center);
				openInfoWindow(address);

				document.getElementById("searchTextBox").value = address;
                zoom = <%= CurrentLocationZoomLevel %>;
			}

			map.setCenter(center);
			map.setZoom(zoom);

			google.maps.event.addListener(map, 'click', function(event){
				getAddress(event.latLng);
			});
        }

		function openInfoWindow(address) {
			infowindow.setContent(address);
			infowindow.open(map, marker);
		}

		function addMarker(center) {
			marker = new google.maps.Marker({
				position: center,
				map: map,
				draggable: true
			});
			google.maps.event.addListener(marker, 'dragend', function(event){
				getAddress(event.latLng);
			});
		}

		function getAddress(latLng) {
            if (latLng != null) {
                geocoder.geocode({ 'latLng': latLng }, function(results, status){
					if (status == google.maps.GeocoderStatus.OK) {
						if (results[0]) {
							if (!marker)
								addMarker(results[0].geometry.location);
							else 
								marker.setPosition(results[0].geometry.location);
							openInfoWindow(results[0].formatted_address);
							document.getElementById("searchTextBox").value = results[0].formatted_address;
							updateLocationField(results[0].formatted_address);
						}
					} else {
						alert("Geocoder failed due to: " + status);
					}
				});
            }
        }

		function updateLocationField(address) {
            var markerLatitude = marker.getPosition().lat();
            var markerLongitude = marker.getPosition().lng();
            var location = address + " (" + markerLatitude + "," + markerLongitude + ")";
            document.getElementById("<%= hiddenFieldLocation.ClientID %>").value = location;
        }

		function searchAddress(address) {
			if (geocoder) {
				geocoder.geocode({ 'address': address}, function(results, status){
					if (results[0]) {
						if (!marker)
							addMarker(results[0].geometry.location);
						else
							marker.setPosition(results[0].geometry.location);
						map.setCenter(results[0].geometry.location);
						map.setZoom(<%= CurrentLocationZoomLevel %>);
						openInfoWindow(results[0].formatted_address);
						document.getElementById("searchTextBox").value = results[0].formatted_address;
						updateLocationField(results[0].formatted_address);
					} else {
						alert(address = " not found");
					}
				});
			}
		}

        function onOK() {
            var location = document.getElementById("<%= hiddenFieldLocation.ClientID %>").value;
            EPi.GetDialog().dialogArguments.getElementById('<%= Request.QueryString["valueid"] %>').value = location;
            EPi.GetDialog().dialogArguments.getElementById('<%= Request.QueryString["displayid"] %>').value = location;
            EPi.GetDialog().Close(true);
        }

        function onNothing() {
            EPi.GetDialog().dialogArguments.getElementById('<%= Request.QueryString["valueid"] %>').value = '';
            EPi.GetDialog().dialogArguments.getElementById('<%= Request.QueryString["displayid"] %>').value = '';
            EPi.GetDialog().Close(true);
        }

        function onCancel() {
            EPi.GetDialog().Close(false);
        }

        function onLoad() {
            window.focus();
            if (this.activeItem)
                this.activeItem.scrollIntoView(false);
        }
       
        function SetValues(value, info) {
            var doc = EPi.GetDialog().dialogArguments;
            returnValue = true;
        } 
		  
        //]]>
    </script>

</asp:Content>