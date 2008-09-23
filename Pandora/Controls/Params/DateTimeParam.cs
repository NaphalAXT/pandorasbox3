using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace TheBox.Controls.Params
{
	/// <summary>
	/// Summary description for DateTimeParam.
	/// </summary>
	public class DateTimeParam : System.Windows.Forms.UserControl, IParam
	{
		private System.Windows.Forms.Label labName;
		private System.Windows.Forms.DateTimePicker picker;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DateTimeParam()
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
			this.picker = new System.Windows.Forms.DateTimePicker();
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
			// picker
			// 
			this.picker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.picker.Location = new System.Drawing.Point(0, 16);
			this.picker.Name = "picker";
			this.picker.Size = new System.Drawing.Size(96, 20);
			this.picker.TabIndex = 2;
			// 
			// DateTimeParam
			// 
			this.Controls.Add(this.picker);
			this.Controls.Add(this.labName);
			this.Name = "DateTimeParam";
			this.Size = new System.Drawing.Size(96, 36);
			this.Load += new System.EventHandler(this.DateTimeParam_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void DateTimeParam_Load(object sender, System.EventArgs e)
		{
			picker.Value = DateTime.Now;
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
				return picker.Value.ToString();
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
