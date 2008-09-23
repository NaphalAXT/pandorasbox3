using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace TheBox.Controls.Params
{
	/// <summary>
	/// Summary description for NumericParam.
	/// </summary>
	public class NumericParam : System.Windows.Forms.UserControl, IParam
	{
		private static int m_LastValue = 0;

		private System.Windows.Forms.Label labName;
		private System.Windows.Forms.NumericUpDown num;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NumericParam()
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
			this.labName = new System.Windows.Forms.Label();
			this.num = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.num)).BeginInit();
			this.SuspendLayout();
			// 
			// labName
			// 
			this.labName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labName.Location = new System.Drawing.Point(0, 0);
			this.labName.Name = "labName";
			this.labName.Size = new System.Drawing.Size(96, 16);
			this.labName.TabIndex = 0;
			// 
			// num
			// 
			this.num.Location = new System.Drawing.Point(0, 16);
			this.num.Maximum = new System.Decimal(new int[] {
																				50000,
																				0,
																				0,
																				0});
			this.num.Name = "num";
			this.num.Size = new System.Drawing.Size(96, 20);
			this.num.TabIndex = 1;
			this.num.ValueChanged += new System.EventHandler(this.num_ValueChanged);
			// 
			// NumericParam
			// 
			this.Controls.Add(this.num);
			this.Controls.Add(this.labName);
			this.Name = "NumericParam";
			this.Size = new System.Drawing.Size(96, 36);
			this.Load += new System.EventHandler(this.NumericParam_Load);
			((System.ComponentModel.ISupportInitialize)(this.num)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void NumericParam_Load(object sender, System.EventArgs e)
		{
			num.Value = m_LastValue;
		}

		private void num_ValueChanged(object sender, System.EventArgs e)
		{
			m_LastValue = (int) num.Value;

			if ( labName.Text.ToLower() == "hue" )
			{
				if ( m_LastValue >= 0 && m_LastValue <= 3000 )
				{
					TheBox.Pages.Items.ArtHue = m_LastValue;
				}
			}

			if ( labName.Text.ToLower() == "itemid" )
			{
				TheBox.Pages.Items.ArtIndex = m_LastValue;
			}
		}

		#region IParam Members

		public string ParamName
		{
			set
			{
				labName.Text = value;
				Pandora.ToolTip.SetToolTip( labName, value );

				if ( value.ToLower() == "hue" )
				{
					m_LastValue = Pandora.Profile.Hues.SelectedIndex;
					num.Value = Pandora.Profile.Hues.SelectedIndex;
					TheBox.Pages.Items.ArtHue = m_LastValue;
					
					Pandora.Profile.Hues.HueChanged += new EventHandler(Hues_HueChanged);
				}

				if ( value.ToLower() == "itemid" )
				{
					m_LastValue = Pandora.Profile.Deco.ArtIndex;
					num.Value = m_LastValue;
					TheBox.Pages.Items.ArtIndex = m_LastValue;
					
					Pandora.Art.ArtIndexChanged += new EventHandler(Art_ArtIndexChanged);
				}
			}
		}

		public string Value
		{
			get
			{
				return num.Value.ToString();
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

		private void Hues_HueChanged(object sender, EventArgs e)
		{
			if ( Parent == null )
			{
				return;
			}

			if ( ( Parent as ConstructorsViewer ).AllowHueChange )
			{
				m_LastValue = Pandora.Profile.Hues.SelectedIndex;
				num.Value = Pandora.Profile.Hues.SelectedIndex;
			}
		}

		private void Art_ArtIndexChanged(object sender, EventArgs e)
		{
			if ( Pandora.Art.Art == ArtViewer.Art.Items )
			{
				if ( Parent == null )
				{
					return;
				}

				if ( ( Parent as ConstructorsViewer ).AllowItemIDChange )
				{
					num.Value = Pandora.Art.ArtIndex;
					m_LastValue = Pandora.Art.ArtIndex;

					Pandora.Profile.Items.ArtIndex = m_LastValue;
				}
			}
		}
	}
}