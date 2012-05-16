using System;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Globalization;
using EPiServer.Web.WebControls;

namespace GoogleMapsLocation.Control
{
	public class InputGoogleMapsLocation : InputBase
	{
		private const string ControlPath = "~/SpecializedProperties/GoogleMapsLocationSelection.aspx";

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (DisplayControl != null)
			{
				DisplayControl.Enabled = false;
				if (Value != null)
					DisplayControl.Text = Value.ToString();
				else
					DisplayControl.Text = String.Empty;
			}
		}

		protected override void CreateChildControls()
		{
			Page.ClientScript.RegisterClientScriptBlock(GetType(), "ShowLocationSelectionDialog", CreateShowDialogScript());

			if (ValueControl == null)
				CreateValueControl();

			DisplayControl = new TextBox();
			DisplayControl.ID = "Display";
			DisplayControl.Enabled = false;

			CopyWebAttributes(DisplayControl);
			Controls.Add(DisplayControl);

			HtmlInputButton openDialogButton = new HtmlInputButton();
			if (!Enabled)
				openDialogButton.Attributes.Add("disabled", "disabled");

			openDialogButton.Value = "...";
			openDialogButton.Attributes.Add("class", "epismallbutton");
			openDialogButton.Attributes.Add("onclick", string.Format(
				"ShowLocationSelectionDialog('{0}', document.getElementById('{1}').value, '{2}', '{3}', '{4}', '{5}', '{6}');",
				UriSupport.ResolveUrlBySettings(ControlPath),
				ValueControl.ClientID,
				DisplayControl.ClientID,
				ValueControl.ClientID,
				ContentLanguage.PreferredCulture.Name,
				DialogWidth,
				DialogHeight
			));

			Controls.Add(openDialogButton);
		}

		private void CreateValueControl()
		{
			ValueControl = new HtmlInputHidden();
			ValueControl.ID = "Value";
			Controls.Add(ValueControl);
		}

		private string CreateShowDialogScript()
		{
			StringBuilder dialogMethodStringBuilder = new StringBuilder();
			dialogMethodStringBuilder.AppendLine("<script type='text/javascript'>");
			dialogMethodStringBuilder.AppendLine("//<![CDATA[");
			dialogMethodStringBuilder.AppendLine("ShowLocationSelectionDialog = function(url, value, displayid, valueid, language, dialogWidth, dialogHeight, callbackMethod, callbackArguments)");
			dialogMethodStringBuilder.AppendLine("{");
			dialogMethodStringBuilder.AppendLine("  var completeUrl = url + '?value=' + value + '&displayid=' + displayid + '&valueid=' + valueid + '&epslanguage=' + language;");
			dialogMethodStringBuilder.AppendLine("  if (!callbackMethod)");
			dialogMethodStringBuilder.AppendLine("  {");
			dialogMethodStringBuilder.AppendLine("    callbackMethod = function(returnValue)");
			dialogMethodStringBuilder.AppendLine("    {");
			dialogMethodStringBuilder.AppendLine("      if (returnValue)");
			dialogMethodStringBuilder.AppendLine("      {");
			dialogMethodStringBuilder.AppendLine("         EPi.PageLeaveCheck.SetPageChanged(true);");
			dialogMethodStringBuilder.AppendLine("      }");
			dialogMethodStringBuilder.AppendLine("    }");
			dialogMethodStringBuilder.AppendLine("  }");
			dialogMethodStringBuilder.AppendLine("  if (!callbackArguments)");
			dialogMethodStringBuilder.AppendLine("  {");
			dialogMethodStringBuilder.AppendLine("    callbackArguments = null;");
			dialogMethodStringBuilder.AppendLine("  }");
			dialogMethodStringBuilder.AppendLine("  var dialogArguments = window.document;");
			dialogMethodStringBuilder.AppendLine("  var features = {width:dialogWidth, height:dialogHeight, scrollbars:'no', resizable:'no'};");
			dialogMethodStringBuilder.AppendLine("  return EPi.CreateDialog(completeUrl, callbackMethod, callbackArguments, dialogArguments, features);");
			dialogMethodStringBuilder.AppendLine("}");
			dialogMethodStringBuilder.AppendLine("//]]>");
			dialogMethodStringBuilder.AppendLine("</script>");
			return dialogMethodStringBuilder.ToString();
		}

		public GoogleMapsLocation Value 
		{
			get
			{
				if (ValueControl == null)
					return null;

				if (string.IsNullOrEmpty(ValueControl.Value))
					return null;

				return GoogleMapsLocation.Parse(ValueControl.Value);
			}
			set
			{
				if (ValueControl == null)
					CreateValueControl();

				if (value == null)
					ValueControl.Value = String.Empty;
				else
					ValueControl.Value = value.ToString();
			}
		}

		public TextBox DisplayControl { get; set; }

		public HtmlInputHidden ValueControl { get; set; }

		private int DialogWidth 
		{ 
			get { return 500; } 
		}

		private int DialogHeight
		{
			get { return 500; }
		}
	}
}
