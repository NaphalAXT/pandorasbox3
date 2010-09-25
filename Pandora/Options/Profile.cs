using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

using TheBox.Data;
using TheBox.Common;

namespace TheBox.Options
{
	[ Serializable ]
	/// <summary>
	/// Defines a profile for Pandora's Box
	/// </summary>
	public class Profile
	{
		#region Options

		private GeneralOptions m_General;
		private TravelOptions m_Travel;
		private HuesOptions m_Hues;
		private CommandsOptions m_Commands;
		private MobilesOptions m_Mobiles;
		private PropsOptions m_Props;
		private ServerOptions m_Server;
		private DecoOptions m_Deco;
		private ItemsOptions m_Items;
		private Notes m_Notes;
		private AdminOptions m_Admin;
		private LauncherOptions m_Launcher;
		private ScreenshotOptions m_Screenshots;
		private MulManager m_MulManager;

		/// <summary>
		/// Gets or sets the general options for this profile
		/// </summary>
		public GeneralOptions General
		{
			get { return m_General; }
			set { m_General = value; }
		}

		/// <summary>
		/// Gets or sets travel related options
		/// </summary>
		public TravelOptions Travel
		{
			get { return m_Travel; }
			set { m_Travel = value; }
		}

		/// <summary>
		/// Gets or sets the hues options
		/// </summary>
		public HuesOptions Hues
		{
			get { return m_Hues; }
			set { m_Hues = value; }
		}

		/// <summary>
		/// Gets or sets the commands options
		/// </summary>
		public CommandsOptions Commands
		{
			get { return m_Commands; }
			set { m_Commands = value; }
		}

		/// <summary>
		/// Gets or sets options related to mobiles
		/// </summary>
		public MobilesOptions Mobiles
		{
			get { return m_Mobiles; }
			set { m_Mobiles = value; }
		}

		/// <summary>
		/// Gets or sets options related to properties
		/// </summary>
		public PropsOptions Props
		{
			get { return m_Props; }
			set { m_Props = value; }
		}

		/// <summary>
		/// Gets or sets BoxServer options
		/// </summary>
		public ServerOptions Server
		{
			get { return m_Server; }
			set { m_Server = value; }
		}

		/// <summary>
		/// Gets or sets the deco options
		/// </summary>
		public DecoOptions Deco
		{
			get { return m_Deco; }
			set { m_Deco = value; }
		}

		/// <summary>
		/// Gets or sets the options for the Items tab
		/// </summary>
		public ItemsOptions Items
		{
			get { return m_Items; }
			set { m_Items = value; }
		}

		/// <summary>
		/// Gets or sets the notes collection
		/// </summary>
		public Notes Notes
		{
			get { return m_Notes; }
			set { m_Notes = value; }
		}

		/// <summary>
		/// Gets or sets admin options
		/// </summary>
		public AdminOptions Admin
		{
			get { return m_Admin; }
			set { m_Admin = value; }
		}

		/// <summary>
		/// Gets or sets the launcher options
		/// </summary>
		public LauncherOptions Launcher
		{
			get { return m_Launcher; }
			set { m_Launcher = value; }
		}

		/// <summary>
		/// Gets or sets the launcher options
		/// </summary>
		public ScreenshotOptions Screenshots
		{
			get { return m_Screenshots; }
			set { m_Screenshots = value; }
		}

		/// <summary>
		/// Gets or sets the mul files manager
		/// </summary>
		public MulManager MulManager
		{
			get { return m_MulManager; }
			set { m_MulManager = value; }
		}

		#endregion

		/// <summary>
		/// Creates a new Profile object
		/// </summary>
		public Profile()
		{
			m_Travel = new TravelOptions();
			m_General = new GeneralOptions();
			m_Hues = new HuesOptions();
			m_Commands = new CommandsOptions();
			m_Mobiles = new MobilesOptions();
			m_Props = new PropsOptions();
			m_Server = new ServerOptions();
			m_Deco = new DecoOptions();
			m_Items = new ItemsOptions();
			m_Notes = new Notes();
			m_Admin = new AdminOptions();
			m_Launcher = new LauncherOptions();
			m_Screenshots = new ScreenshotOptions();
			m_MulManager = new MulManager();
		}

		private string m_Name;
		private string m_Language = "English";
		private string m_CustomClient = null;

		/// <summary>
		/// Gets a list of all the existing profiles
		/// </summary>
		public static StringCollection ExistingProfiles
		{
			get
			{
				string profilesFolder = ProfileManager.ProfilesFolder;
				StringCollection list = new StringCollection();

				if (Directory.Exists(profilesFolder))
				{
                    string[] profiles = Directory.GetDirectories(profilesFolder);

                    int index = profilesFolder.Length + 1;

					foreach ( string pro in profiles )
					{
						list.Add( pro.Substring( index, pro.Length - index ) );
					}
				}

				return list;
			}
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the name of the profile
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		[ XmlAttribute ]
		/// <summary>
		/// Gets or sets the language used by this instance of Pandora
		/// </summary>
		public string Language
		{
			get { return m_Language; }
			set { m_Language = value; }
		}

		public string CustomClient
		{
			get { return m_CustomClient; }
			set
			{
				if ( value == null || File.Exists ( value ) )
				{
					m_CustomClient = value;
					TheBox.Common.Utility.CustomClientPath = m_CustomClient;
				}
			}
		}

		/// <summary>
		/// Gets the base folder for this profile
		/// </summary>
		public string BaseFolder
		{
			get
			{
				return Path.Combine(ProfileManager.ProfilesFolder, m_Name);
			}
		}

		private ButtonIndex m_ButtonIndex;

		[ XmlIgnore ]
		/// <summary>
		/// Gets the ButtonIndex provider
		/// </summary>
		public ButtonIndex ButtonIndex
		{
			get
			{
				if ( m_ButtonIndex == null )
				{
					m_ButtonIndex = ButtonIndex.Load();

					if ( m_ButtonIndex == null )
					{
						m_ButtonIndex = new ButtonIndex();
					}
				}
				return m_ButtonIndex;
			}
		}

		#region Methods

		/// <summary>
		/// Resets the Maps folder by deleting it and creating it again
		/// </summary>
		private void ResetMaps()
		{
			string MapsFolder = Path.Combine( BaseFolder, "Maps" );

			if ( Directory.Exists( MapsFolder ) )
			{
				try
				{
					Directory.Delete( MapsFolder );
				}
				catch {}
			}

			if ( !Directory.Exists( MapsFolder ) )
			{
				Directory.CreateDirectory( MapsFolder );
			}
		}

		/// <summary>
		/// Generates the map images used for the world map
		/// </summary>
		/// <param name="bar">A progress bar that can be used to display progress</param>
		public void GenerateMaps( System.Windows.Forms.ProgressBar bar )
		{
			ResetMaps();

			Pandora.Log.WriteEntry( string.Format( "Profile {0} is generating the images for world map", m_Name ) );

			int count = 0;
			foreach ( bool b in m_Travel.EnabledMaps )
				if ( b )
					count++;

			if ( count == 0 )
			{
				Pandora.Log.WriteEntry( "No maps have been enabled. Skipping." );
				return;
			}

			if ( bar != null )
			{
				bar.Minimum = 0;
				bar.Maximum = count * 2;
				bar.Step = 1;
				bar.Value = 0;
			}

			MapViewer.MapViewer.MulFileManager = this.MulManager;

			for ( int i = 0; i < m_Travel.MapCount; i++ )
			{
				if ( m_Travel.EnabledMaps[ i ] )
				{
					// Enabled
					MapViewer.MapViewer.MapScale scales = MapViewer.MapViewer.MapScale.Sixteenth;
					MapViewer.MapViewer.MapScale scaleb = MapViewer.MapViewer.MapScale.Eigth;

					if ( i == 4 )
					{
						scales = MapViewer.MapViewer.MapScale.Fourth;
						scaleb = MapViewer.MapViewer.MapScale.Half;
					}
					else if ( i > 1 )
					{
						scales = MapViewer.MapViewer.MapScale.Eigth;
						scaleb = MapViewer.MapViewer.MapScale.Fourth;
					}

					string FileSmall = string.Format( "map{0}small.jpg", i );
					string FileBig = string.Format( "map{0}big.jpg", i );

					try
					{
						bar.Value++;
						MapViewer.MapViewer.ExtractMap( (MapViewer.Maps) i, scales, Path.Combine( BaseFolder, string.Format( "Maps{0}{1}", Path.DirectorySeparatorChar, FileSmall ) ) );
						bar.Value++;
						MapViewer.MapViewer.ExtractMap( (MapViewer.Maps) i, scaleb, Path.Combine( BaseFolder, string.Format( "Maps{0}{1}", Path.DirectorySeparatorChar, FileBig ) ) );
						Pandora.Log.WriteEntry( string.Format( "Images for map {0} generated correctly", i ) );
					}
					catch ( Exception err )
					{
						Pandora.Log.WriteError( err, string.Format( "Couldn't extract images for map {0}: map disabled in the profile", i ) );
						System.Windows.Forms.MessageBox.Show( string.Format( "An error has occurred and map {0} has been disabled in this profile. Please review the log for further information on the error.", i ) );

						m_Travel.EnabledMaps[ i ] = false;
					}
				}
				else
				{
					// Disabled
					Pandora.Log.WriteEntry( string.Format( "Map {0} is disabled. Skipping.", m_Travel.MapNames[ i ] ) );
				}
			}
		}

		/// <summary>
		/// Saves the profile and all the options
		/// </summary>
		public void Save()
		{
			if ( !Directory.Exists( BaseFolder ) )
				Directory.CreateDirectory( BaseFolder );

			string file = Path.Combine( BaseFolder, "Profile.xml" );

			XmlSerializer serializer = new XmlSerializer( typeof( Profile ) );
			FileStream stream = new FileStream( file, FileMode.Create, FileAccess.Write, FileShare.None );
			serializer.Serialize( stream, this );
			stream.Close();

			Pandora.Log.WriteEntry( string.Format( "Profile {0} save succesfully", m_Name ) );
		}

		/// <summary>
		/// Loads a profile from its folder
		/// </summary>
		/// <param name="name">The profile name</param>
		/// <returns>The profile loaded. Null if the profile was not found</returns>
		public static Profile Load( string name )
		{
            string file = Path.Combine(Path.Combine(ProfileManager.ProfilesFolder, name), "Profile.xml");

			if ( !File.Exists( file ) )
			{
				throw new System.IO.FileNotFoundException( string.Format( "Profile {0} not found.", name ), file );
			}

			Pandora.Log.WriteEntry( string.Format( "Reading profile {0}", name ) );
			FileStream stream = null;
			Profile p = null;

			try
			{
				XmlSerializer serializer = new XmlSerializer( typeof( Profile ) );
				stream = new FileStream( file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
				p = (Profile) serializer.Deserialize( stream );
				stream.Close();

				Pandora.Log.WriteEntry( "Profile read succesfully" );

				// Set static options
				TheBox.Common.Utility.CustomClientPath = p.CustomClient;
			}
			catch ( Exception err )
			{
				Pandora.Log.WriteError( err, string.Format( "Can't read profile {0}", name ) );
			}

			// Close the already closed stream if there is no error... Can be better ;) - Tarion
			if ( stream != null )
				stream.Close();

			if ( p != null )
				p.ValidateMaps();

			return p;
		}

		/// <summary>
		/// Ensures the map arrays are updated for the current version
		/// </summary>
		private void ValidateMaps()
		{
			if ( m_Travel.EnabledMaps.Length == m_Travel.MapCount )
				return;

			string[] names = new string[ m_Travel.MapCount ];
			bool[] enabled = new bool[ m_Travel.MapCount ];

			for ( int i = 0; i < m_Travel.MapNames.Length; i++ )
			{
				names[ i ] = m_Travel.MapNames[ i ];
				enabled[ i ] = m_Travel.EnabledMaps[ i ];
			}

			for ( int i = m_Travel.MapNames.Length; i < m_Travel.MapCount; i++ )
			{
				names[ i ] = m_Travel.DefaultMaps[ i ];
				enabled[ i ] = false;
			}

			m_Travel.MapNames = names;
			m_Travel.EnabledMaps = enabled;

			Pandora.Log.WriteEntry( "Succesfully updated map count" );
		}

		/// <summary>
		/// Creates the datafiles for the default locations
		/// </summary>
		public void CreateMapFiles()
		{
			if ( !this.Travel.CustomMaps )
			{
				string res = "Data.map{0}.xml";
				string dest = Path.Combine( BaseFolder, "Locations" );

				TheBox.Common.Utility.EnsureDirectory( dest );

				dest = Path.Combine( dest, "map{0}.xml" );

				// Import default files
				Assembly asm = Pandora.DataAssembly;

				for ( int i = 0; i < 5; i++ )
				{
					Stream stream = asm.GetManifestResourceStream( string.Format( res, i.ToString() ) );
					FileStream fStream = new FileStream( string.Format( dest, i.ToString() ), FileMode.Create, FileAccess.Write, FileShare.Read );

					byte[] data = new byte[ stream.Length ];

					stream.Read( data, 0, (int) stream.Length );

					fStream.Write( data, 0, data.Length );

					stream.Close();
					fStream.Flush();
					fStream.Close();
				}
			}
		}

		private static string[] m_EmbeddedData = 
			{
				"BoxData.xml", "RandomTiles.xml", "PropsData.xml", "Skills.ini", "HueGroups.xml"
			};

		/// <summary>
		/// Creates the datafiles
		/// </summary>
		public void CreateData()
		{
			foreach( string data in m_EmbeddedData )
			{
				string dest = Path.Combine( BaseFolder, data );
				string resource = string.Format( "Data.{0}", data );

				Utility.ExtractEmbeddedResource( Pandora.DataAssembly, resource, dest );
			}
		}

		public static void DeleteProfile( string profile )
		{
            string folder = Path.Combine(ProfileManager.ProfilesFolder, profile);

			if ( Directory.Exists( folder ) )
			{
				try
				{
					Directory.Delete( folder, true );
				}
				catch ( Exception err )
				{
					Pandora.Log.WriteError( err, "Couldn't delete profile {0}.", profile );
					System.Windows.Forms.MessageBox.Show( Pandora.Localization.TextProvider[ "Errors.DelProfileErr" ] );
				}
			}
		}

		#endregion
	}
}