using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BoxServerSetup
{
	public class Finish : TSWizards.BaseExteriorStep
	{
		private System.Windows.Forms.TextBox tx;
		private System.ComponentModel.IContainer components = null;

		public Finish()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tx = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// Description
			// 
			this.Description.Name = "Description";
			this.Description.Text = "Please restart RunUO in order to apply changes.";
			// 
			// tx
			// 
			this.tx.Location = new System.Drawing.Point(8, 72);
			this.tx.Multiline = true;
			this.tx.Name = "tx";
			this.tx.ReadOnly = true;
			this.tx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tx.Size = new System.Drawing.Size(296, 232);
			this.tx.TabIndex = 1;
			this.tx.Text = "";
			// 
			// Finish
			// 
			this.Controls.Add(this.tx);
			this.IsFinished = true;
			this.Name = "Finish";
			this.NextStep = "FINISHED";
			this.StepDescription = "Please restart RunUO in order to apply changes.";
			this.StepTitle = "Installation complete";
			this.ShowStep += new TSWizards.ShowStepEventHandler(this.Finish_ShowStep);
			this.Controls.SetChildIndex(this.tx, 0);
			this.Controls.SetChildIndex(this.Description, 0);
			this.ResumeLayout(false);

		}
		#endregion

		private void Finish_ShowStep(object sender, TSWizards.ShowStepEventArgs e)
		{
			string[] lines = new string[ Setup.Log.Count ];
			
			for( int i = 0; i < lines.Length; i++ )
			{
				lines[ i ] = (string) Setup.Log[ i ];
			}

			tx.Lines = lines;
		}
	}
}

