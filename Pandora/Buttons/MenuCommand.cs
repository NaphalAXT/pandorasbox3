using System;

namespace TheBox.Buttons
{
	[ Serializable ]
	/// <summary>
	/// Describes the command sent to UO by a menu item
	/// </summary>
	public class MenuCommand : ICloneable, IButtonFunction
	{
		private string m_Caption;
		private string m_Command;
		private bool m_UsePrefix;

		[ System.Xml.Serialization.XmlAttribute ]
		/// <summary>
		/// Gets or sets the caption displayed on the menu item
		/// </summary>
		public string Caption
		{
			get { return m_Caption; }
			set { m_Caption = value; }
		}

		[ System.Xml.Serialization.XmlAttribute ]
		/// <summary>
		/// Gets or sets the command sent to UO
		/// </summary>
		public string Command
		{
			get { return m_Command; }
			set { m_Command = value; }
		}

		[ System.Xml.Serialization.XmlAttribute ]
		/// <summary>
		/// Gets or sets a value stating whether the command required the command prefix
		/// </summary>
		public bool UsePrefix
		{
			get { return m_UsePrefix; }
			set { m_UsePrefix = value; }
		}

		/// <summary>
		/// Creates a new MenuCommand object
		/// </summary>
		public MenuCommand()
		{
		}

		/// <summary>
		/// Gets an empty MenuCommand
		/// </summary>
		public static MenuCommand Empty
		{
			get
			{
				MenuCommand mc = new MenuCommand();

				mc.m_UsePrefix = false;
				mc.m_Command = "";
				mc.m_Caption = "";

				return mc;
			}
		}

		[ System.Xml.Serialization.XmlIgnore ]
		/// <summary>
		/// Gets a BoxMenuItem corresponding to this command
		/// </summary>
		public BoxMenuItem MenuItem
		{
			get
			{
				return new BoxMenuItem( this );
			}
		}

		[ System.Xml.Serialization.XmlIgnore ]
		/// <summary>
		/// Gets the full command for this menu command
		/// </summary>
		public string FullCommand
		{
			get
			{
				return string.Format( "{0}{1}", m_UsePrefix ? Pandora.Profile.General.CommandPrefix : string.Empty, m_Command );
			}
		}

		#region ICloneable Members

		/// <summary>
		/// Clones the current object
		/// </summary>
		/// <returns>A MenuCommand object initialized to the same values as the original one</returns>
		public object Clone()
		{
			MenuCommand mc = new MenuCommand();

			if ( m_Caption != null )
				mc.m_Caption = string.Copy( m_Caption );
			if ( m_Command != null )
				mc.m_Command = string.Copy( m_Command );
			mc.m_UsePrefix = m_UsePrefix;

			return mc;
		}

		#endregion

		#region IButtonFunction Members

		public string Name
		{
			get
			{
				return "Buttons.Single";
			}
		}

		public bool AllowsSecondButton
		{
			get
			{
				return true;
			}
		}

		public bool RequiresSecondButton
		{
			get
			{
				return false;
			}
		}

		public void DoAction(BoxButton button, System.Drawing.Point clickPoint, System.Windows.Forms.MouseButtons mouseButton)
		{
			OnSendCommand( new SendCommandEventArgs( m_Command, m_UsePrefix ) );
		}

		public event TheBox.SendCommandEventHandler SendCommand;

		protected virtual void OnSendCommand( SendCommandEventArgs e )
		{
			if ( SendCommand != null )
			{
				SendCommand( this, e );
			}
		}

		public event System.EventHandler SendLastCommand;

		protected virtual void OnSendLastCommand( EventArgs e )
		{
			if ( SendLastCommand != null )
			{
				SendLastCommand( this, e );
			}
		}

		public event TheBox.CommandChangedEventHandler CommandChanged;

		protected virtual void OnCommandChanged( CommandChangedEventArgs e )
		{
			if ( CommandChanged != null )
			{
				CommandChanged( this, e );
			}
		}

		#endregion
	}
}
