using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace TheBox.Controls.Params
{
	/// <summary>
	/// Summary description for BoolParam.
	/// </summary>
	public class BoolParam : System.Windows.Forms.UserControl, IParam
	{
		private System.Windows.Forms.Label labName;
		private System.Windows.Forms.ComboBox cmb;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BoolParam()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				Pandora.ToolTip.SetToolTip( labName, null );
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmb = new System.Windows.Forms.ComboBox();
			this.labName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmb
			// 
			this.cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmb.Location = new System.Drawing.Point(0, 16);
			this.cmb.Name = "cmb";
			this.cmb.Size = new System.Drawing.Size(96, 21);
			this.cmb.TabIndex = 0;
			// 
			// labName
			// 
			this.labName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labName.Location = new System.Drawing.Point(0, 0);
			this.labName.Name = "labName";
			this.labName.Size = new System.Drawing.Size(96, 16);
			this.labName.TabIndex = 1;
			// 
			// BoolParam
			// 
			this.Controls.Add(this.labName);
			this.Controls.Add(this.cmb);
			this.Name = "BoolParam";
			this.Size = new System.Drawing.Size(96, 36);
			this.Load += new System.EventHandler(this.BoolParam_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// OnLoad localize the true and false values
		/// </summary>
		private void BoolParam_Load(object sender, System.EventArgs e)
		{
			try
			{
				cmb.Items.Add( Pandora.Localization.TextProvider[ "Common.True" ] );
				cmb.Items.Add( Pandora.Localization.TextProvider[ "Common.False" ] );

				cmb.SelectedIndex = 0;
			}
			catch {}
		}

		#region IParam Members

		public string ParamName
		{
			set
			{
				labName.Text = value;

				Pandora.ToolTip.SetToolTip( labName, value );
			}
		}

		public string Value
		{
			get
			{
				if ( cmb.SelectedIndex == 0 )
					return "true";
				else
					return "false";
			}
		}

		public bool IsDefined
		{
			get
			{
				return true;
			}
		}

		#endregion
	}
}
