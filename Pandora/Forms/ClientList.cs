using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using TheBox.BoxServer;

namespace TheBox.Forms
{
	/// <summary>
	/// Summary description for ClientList.
	/// </summary>
	public class ClientListForm : System.Windows.Forms.Form
	{
		#region InternalComparer

		private class InternalComparer : IComparer
		{
			private int m_Column;
			private bool m_Ascending = true;

			/// <summary>
			/// Gets the column that's used for sorting
			/// </summary>
			public int Column
			{
				get { return m_Column; }
			}

			public bool Ascending
			{
				get { return m_Ascending; }
			}

			public InternalComparer()
			{
				m_Column = 0;
			}

			public InternalComparer( int column )
			{
				m_Column = column;
			}

			public InternalComparer( int column, bool ascending )
			{
				m_Column = column;
				m_Ascending = ascending;
			}

			public int Compare(object x, object y)
			{
				ListViewItem X = x as ListViewItem;
				ListViewItem Y = y as ListViewItem;

				if ( X == null || Y == null )
					return 0;

				ClientEntry item1 = null;
				ClientEntry item2 = null;

				try
				{
					int s1 = int.Parse( X.SubItems[ 5 ].Text );
					int s2 = int.Parse( Y.SubItems[ 5 ].Text );

					item1 = ClientListForm.m_Clients[ s1 ] as ClientEntry;
					item2 = ClientListForm.m_Clients[ s2 ] as ClientEntry;
				}
				catch
				{
				}

				if ( item1 == null || item2 == null )
				{
					return 0;
				}
				
				if ( ! m_Ascending )
				{
					ClientEntry temp = item1;
					item1 = item2;
					item2 = temp;
				}

				switch ( m_Column )
				{
					case 0: // Name

						return string.Compare( item1.Name, item2.Name );

					case 1: // Account

						return string.Compare( item1.Account, item2.Account );

					case 2: // Map

						return item1.Map.CompareTo( item2.Map );

					case 3: // Location

						int cmp = item1.X.CompareTo( item2.Y );

						if ( cmp == 0 )
							return item1.Y.CompareTo( item2.Y );
						else
							return cmp;

					case 4: // Login time

						return DateTime.Compare( item1.LoggedIn, item2.LoggedIn );

					case 5: // Serial

						return item1.Serial.CompareTo( item2.Serial );
				}

				return 0;
			}
		}

		#endregion

		private System.Windows.Forms.Button bRefresh;
		private System.Windows.Forms.Button bGoTo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bProps;
		private System.Windows.Forms.Button bClient;
		private System.Windows.Forms.Button bAccount;
		private System.Windows.Forms.ListView lst;
		private System.Windows.Forms.Button bClose;
		private System.Windows.Forms.ColumnHeader col0;
		private System.Windows.Forms.ColumnHeader col1;
		private System.Windows.Forms.ColumnHeader col2;
		private System.Windows.Forms.ColumnHeader col3;
		private System.Windows.Forms.ColumnHeader col4;
		private System.Windows.Forms.ColumnHeader col5;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ClientListForm()
		{
			InitializeComponent();

			Pandora.Localization.LocalizeControl( this );

			lst.ListViewItemSorter = new InternalComparer();

			col0.Text = Pandora.Localization.TextProvider[ "Common.Name" ];
			col1.Text = Pandora.Localization.TextProvider[ "Common.Account" ];
			col2.Text = Pandora.Localization.TextProvider[ "Common.Map" ];
			col3.Text = Pandora.Localization.TextProvider[ "Common.Location" ];
			col4.Text = Pandora.Localization.TextProvider[ "ClientList.Login" ];
			col5.Text = Pandora.Localization.TextProvider[ "ClientList.Serial" ];
		}

		static ClientListForm()
		{
			m_Clients = new Hashtable();
		}

		/// <summary>
		/// List of the clients connected to the server
		/// </summary>
		private static Hashtable m_Clients;

		/// <summary>
		/// The client currently selected
		/// </summary>
		private ClientEntry m_Client;

		/// <summary>
		/// Gets or sets the client currently selected
		/// </summary>
		private ClientEntry Client
		{
			get { return m_Client; }
			set
			{
				m_Client = value;
				EnableButtons();
			}
		}

		/// <summary>
		/// Enables selection sensitive buttons
		/// </summary>
		private void EnableButtons()
		{
			bool enabled = m_Clients.Count > 0 && m_Client != null;

			bGoTo.Enabled = enabled;
			bProps.Enabled = enabled;
			bClient.Enabled = enabled;
			bAccount.Enabled = enabled;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ClientListForm));
			this.lst = new System.Windows.Forms.ListView();
			this.col0 = new System.Windows.Forms.ColumnHeader();
			this.col1 = new System.Windows.Forms.ColumnHeader();
			this.col2 = new System.Windows.Forms.ColumnHeader();
			this.col3 = new System.Windows.Forms.ColumnHeader();
			this.col4 = new System.Windows.Forms.ColumnHeader();
			this.col5 = new System.Windows.Forms.ColumnHeader();
			this.bRefresh = new System.Windows.Forms.Button();
			this.bGoTo = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.bProps = new System.Windows.Forms.Button();
			this.bClient = new System.Windows.Forms.Button();
			this.bAccount = new System.Windows.Forms.Button();
			this.bClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lst
			// 
			this.lst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lst.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																										this.col0,
																										this.col1,
																										this.col2,
																										this.col3,
																										this.col4,
																										this.col5});
			this.lst.FullRowSelect = true;
			this.lst.HideSelection = false;
			this.lst.Location = new System.Drawing.Point(96, 8);
			this.lst.MultiSelect = false;
			this.lst.Name = "lst";
			this.lst.Size = new System.Drawing.Size(424, 216);
			this.lst.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lst.TabIndex = 0;
			this.lst.View = System.Windows.Forms.View.Details;
			this.lst.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lst_ColumnClick);
			this.lst.SelectedIndexChanged += new System.EventHandler(this.lst_SelectedIndexChanged);
			// 
			// bRefresh
			// 
			this.bRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bRefresh.Location = new System.Drawing.Point(8, 8);
			this.bRefresh.Name = "bRefresh";
			this.bRefresh.Size = new System.Drawing.Size(80, 23);
			this.bRefresh.TabIndex = 1;
			this.bRefresh.Text = "Common.Refresh";
			this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
			// 
			// bGoTo
			// 
			this.bGoTo.Enabled = false;
			this.bGoTo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bGoTo.Location = new System.Drawing.Point(8, 56);
			this.bGoTo.Name = "bGoTo";
			this.bGoTo.Size = new System.Drawing.Size(80, 23);
			this.bGoTo.TabIndex = 2;
			this.bGoTo.Text = "ClientList.Go";
			this.bGoTo.Click += new System.EventHandler(this.bGoTo_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "ClientList.Control";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// bProps
			// 
			this.bProps.Enabled = false;
			this.bProps.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bProps.Location = new System.Drawing.Point(8, 88);
			this.bProps.Name = "bProps";
			this.bProps.Size = new System.Drawing.Size(80, 23);
			this.bProps.TabIndex = 4;
			this.bProps.Text = "ClientList.Props";
			this.bProps.Click += new System.EventHandler(this.bProps_Click);
			// 
			// bClient
			// 
			this.bClient.Enabled = false;
			this.bClient.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bClient.Location = new System.Drawing.Point(8, 120);
			this.bClient.Name = "bClient";
			this.bClient.Size = new System.Drawing.Size(80, 23);
			this.bClient.TabIndex = 5;
			this.bClient.Text = "ClientList.Client";
			this.bClient.Click += new System.EventHandler(this.bClient_Click);
			// 
			// bAccount
			// 
			this.bAccount.Enabled = false;
			this.bAccount.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bAccount.Location = new System.Drawing.Point(8, 152);
			this.bAccount.Name = "bAccount";
			this.bAccount.Size = new System.Drawing.Size(80, 23);
			this.bAccount.TabIndex = 6;
			this.bAccount.Text = "ClientList.Acc";
			this.bAccount.Click += new System.EventHandler(this.bAccount_Click);
			// 
			// bClose
			// 
			this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bClose.Location = new System.Drawing.Point(8, 200);
			this.bClose.Name = "bClose";
			this.bClose.Size = new System.Drawing.Size(80, 23);
			this.bClose.TabIndex = 7;
			this.bClose.Text = "Common.Close";
			this.bClose.Click += new System.EventHandler(this.bClose_Click);
			// 
			// ClientListForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 229);
			this.Controls.Add(this.bClose);
			this.Controls.Add(this.bAccount);
			this.Controls.Add(this.bClient);
			this.Controls.Add(this.bProps);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bGoTo);
			this.Controls.Add(this.bRefresh);
			this.Controls.Add(this.lst);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ClientListForm";
			this.Text = "ClientList.Title";
			this.Load += new System.EventHandler(this.ClientListForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Reloads the clients list from the server
		/// </summary>
		private void RefreshList()
		{
			TheBox.BoxServer.ClientListRequest msg = new ClientListRequest();
			ClientListMessage list = Pandora.BoxConnection.SendToServer( msg ) as ClientListMessage;

			if ( list != null )
			{
				m_Clients.Clear();

				foreach( ClientEntry client in list.Clients )
				{
					m_Clients[ client.Serial ] = client;
				}

				RefreshView();
			}
		}

		private void RefreshView()
		{
			lst.BeginUpdate();
			lst.Items.Clear();

			foreach( ClientEntry client in m_Clients.Values )
			{
				string map = null;

				if ( Pandora.Profile.Travel.EnabledMaps[ client.Map ] )
				{
					map = Pandora.Profile.Travel.MapNames[ client.Map ];
				}

				if ( map == null || map.Length == 0 )
				{
					map = "-";
				}

				string location = string.Format( "({0},{1})", client.X, client.Y );

				ListViewItem item = new ListViewItem( new string[] {
					client.Name,
					client.Account,
					map,
					location,
					client.LoggedIn.ToShortTimeString(),
					client.Serial.ToString( "X" ) } );

				lst.Items.Add( item );					
			}

			lst.EndUpdate();

			EnableButtons();
		}

		private void lst_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			if ( ( lst.ListViewItemSorter as InternalComparer ).Column == e.Column )
			{
				lst.ListViewItemSorter = new InternalComparer( e.Column, !( lst.ListViewItemSorter as InternalComparer ).Ascending );
			}
			else
			{
				lst.ListViewItemSorter = new InternalComparer( e.Column );
			}
		}

		private void ClientListForm_Load(object sender, System.EventArgs e)
		{
			RefreshList();
		}

		private void lst_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( lst.SelectedIndices.Count == 1 )
			{
				ClientEntry current = null;

				try
				{
					current = m_Clients[ int.Parse( lst.SelectedItems[ 0 ].SubItems[ 5 ].Text ) ] as ClientEntry;
				}
				catch {}

				m_Client = current;
			}
			else
			{
				m_Client = null;
			}

			EnableButtons();
		}

		private void bRefresh_Click(object sender, System.EventArgs e)
		{
			RefreshList();
		}

		private void bClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void bGoTo_Click(object sender, System.EventArgs e)
		{
			if ( Client != null )
			{
                Pandora.BoxConnection.SendToServer(new ClientListCommand(Client.Serial, "go"));
			}
		}

		private void bProps_Click(object sender, System.EventArgs e)
		{
			if ( Client != null )
			{
                Pandora.BoxConnection.SendToServer(new ClientListCommand(Client.Serial, "props"));
			}
		}

		private void bClient_Click(object sender, System.EventArgs e)
		{
			if ( Client != null )
			{
                Pandora.BoxConnection.SendToServer(new ClientListCommand(Client.Serial, "client"));
			}
		}

		private void bAccount_Click(object sender, System.EventArgs e)
		{
			if ( Client != null )
			{
                Pandora.BoxConnection.SendToServer(new ClientListCommand(Client.Serial, "account"));
			}
		}
	}
}
