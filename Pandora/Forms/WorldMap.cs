using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using TheBox.MapViewer;
using TheBox.Data;

namespace TheBox.Forms
{
	/// <summary>
	/// Summary description for WorldMap.
	/// </summary>
	public class WorldMap : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ToolBar tBar;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.ToolBarButton bBig;
		private System.Windows.Forms.ToolBarButton bMap0;
		private System.Windows.Forms.ToolBarButton bMap1;
		private System.Windows.Forms.ToolBarButton bMap2;
		private System.Windows.Forms.ToolBarButton bMap3;
		private System.Windows.Forms.ToolBarButton bClose;

		private Maps m_Map;
		private bool m_Big;
		private System.Windows.Forms.PictureBox Img;
		private System.Windows.Forms.ToolBarButton bMap4;
		private ToolBarButton[] m_Buttons;

		public WorldMap()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_Big = Pandora.Profile.Travel.WorldMapBig;
			m_Map = Pandora.Map.Map;

			bClose.Text = Pandora.Localization.TextProvider[ "Common.Exit" ];

			tBar.ImageList = new ImageList();
			tBar.ImageList.ImageSize = new Size( 1,1 );

			Pandora.Localization.LocalizeControl( this );

			m_Buttons = new ToolBarButton[]
				{
					bMap0, bMap1, bMap2, bMap3, bMap4
				};

			InitToolBar();
			DoDisplay();
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
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WorldMap));
			this.tBar = new System.Windows.Forms.ToolBar();
			this.bBig = new System.Windows.Forms.ToolBarButton();
			this.bMap0 = new System.Windows.Forms.ToolBarButton();
			this.bMap1 = new System.Windows.Forms.ToolBarButton();
			this.bMap2 = new System.Windows.Forms.ToolBarButton();
			this.bMap3 = new System.Windows.Forms.ToolBarButton();
			this.bClose = new System.Windows.Forms.ToolBarButton();
			this.Img = new System.Windows.Forms.PictureBox();
			this.bMap4 = new System.Windows.Forms.ToolBarButton();
			this.SuspendLayout();
			// 
			// tBar
			// 
			this.tBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					this.bBig,
																					this.bMap0,
																					this.bMap1,
																					this.bMap2,
																					this.bMap3,
																					this.bMap4,
																					this.bClose});
			this.tBar.ButtonSize = new System.Drawing.Size(58, 18);
			this.tBar.DropDownArrows = true;
			this.tBar.Location = new System.Drawing.Point(0, 0);
			this.tBar.Name = "tBar";
			this.tBar.ShowToolTips = true;
			this.tBar.Size = new System.Drawing.Size(736, 29);
			this.tBar.TabIndex = 0;
			this.tBar.Wrappable = false;
			this.tBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tBar_ButtonClick);
			// 
			// bBig
			// 
			this.bBig.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.bBig.Text = "World.Big";
			// 
			// bClose
			// 
			this.bClose.Text = "Common.Exit";
			// 
			// Img
			// 
			this.Img.Location = new System.Drawing.Point(0, 26);
			this.Img.Name = "Img";
			this.Img.Size = new System.Drawing.Size(736, 448);
			this.Img.TabIndex = 1;
			this.Img.TabStop = false;
			this.Img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Img_MouseDown);
			// 
			// WorldMap
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(736, 479);
			this.Controls.Add(this.Img);
			this.Controls.Add(this.tBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "WorldMap";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "World.WorldMap";
			this.Load += new System.EventHandler(this.WorldMap_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Initializes the tool bar hiding the buttons corresponding to disabled maps
		/// </summary>
		private void InitToolBar()
		{
			if ( m_Big )
			{
				bBig.Pushed = true;
				bBig.Text = Pandora.Localization.TextProvider[ "World.Big" ];
			}
			else
			{
				bBig.Pushed = false;
				bBig.Text = Pandora.Localization.TextProvider[ "World.Small" ];
			}

			for ( int i = 0; i < Pandora.Profile.Travel.MapCount; i++ )
			{
				if ( Pandora.Profile.Travel.EnabledMaps[ i ] )
				{
					m_Buttons[ i ].Visible = true;
					m_Buttons[ i ].Text = Pandora.Profile.Travel.MapNames[ i ];
				}
				else
				{
					m_Buttons[ i ].Visible = false;
				}
			}
		}

		/// <summary>
		/// Displays the selected image and resizes the form accordingly
		/// </summary>
		private void DoDisplay()
		{
			Bitmap bmp = Pandora.Profile.Travel.GetMapImage( (int) m_Map, m_Big );

			if ( bmp == null )
			{
				m_Buttons[ (int) m_Map ].Enabled = false;
				Pandora.Log.WriteError( null, string.Format( "Display of enabled map {0} failed.", (int) m_Map ) );

				MessageBox.Show( Pandora.Localization.TextProvider[ "World.NoImage" ] );
			}
			else
			{
				if ( Pandora.Profile.Travel.ShowSpawns )
				{
					AddSpawns( bmp );
				}

				this.Width = bmp.Width + ( SystemInformation.BorderSize.Width * 2 );
				this.Height = bmp.Height + tBar.Height + SystemInformation.BorderSize.Height + SystemInformation.CaptionHeight;

				Img.Width = bmp.Width;
				Img.Height = bmp.Height;

				Img.Image = bmp;
			}
		}

		private void AddSpawns( Bitmap bmp )
		{
			double xscale = (double) bmp.Width / (double) MapSizes.GetSize( (int) m_Map ).Width;
			double yscale = (double) bmp.Height / (double) MapSizes.GetSize( (int) m_Map ).Height;

			Color color = Pandora.Profile.Travel.SpawnColor;

			foreach ( SpawnEntry spawn in SpawnData.SpawnProvider.Spawns )
			{
				if ( spawn.Map == (int) m_Map )
				{
					int x = (int) ( spawn.X * xscale );
					int y = (int) ( spawn.Y * yscale );

					if ( x >= 0 && y >= 0 && x < bmp.Width && y < bmp.Height )
						bmp.SetPixel( x, y, color );
				}
			}
		}

		private void tBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if ( e.Button == bBig )
			{
				m_Big = bBig.Pushed;

				Pandora.Profile.Travel.WorldMapBig = m_Big;

				bBig.Text = m_Big ? Pandora.Localization.TextProvider[ "World.Big" ] : Pandora.Localization.TextProvider[ "World.Small" ];
			}
			else if ( e.Button == bClose )
			{
				Close();
			}
			else if ( e.Button == bMap0 )
			{
				m_Map = Maps.Felucca;
			}
			else if ( e.Button == bMap1 )
			{
				m_Map = Maps.Trammel;
			}
			else if ( e.Button == bMap2 )
			{
				m_Map = Maps.Ilshenar;
			}
			else if ( e.Button == bMap3 )
			{
				m_Map = Maps.Malas;
			}
			else if ( e.Button == bMap4 )
			{
				m_Map = Maps.Tokuno;
			}

			DoDisplay();
		}

		private void Img_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Size mapSize = MapSizes.GetSize( (int) m_Map );

			int x = e.X * mapSize.Width / Img.Width;
			int y = e.Y * mapSize.Height / Img.Height;

			Pandora.Map.Map = m_Map;
			Pandora.Map.Center = new Point( x,y );
		}

		private void WorldMap_Load(object sender, System.EventArgs e)
		{
			Pandora.Profile.Travel.ShowSpawnsChanged +=new EventHandler(Travel_ShowSpawnsChanged);
		}

		private void Travel_ShowSpawnsChanged(object sender, EventArgs e)
		{
			DoDisplay();
		}
	}
}