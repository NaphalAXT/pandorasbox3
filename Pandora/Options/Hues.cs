using System;

namespace TheBox.Options
{
	[ Serializable ]
	/// <summary>
	/// Summary description for Hues.
	/// </summary>
	public class HuesOptions
	{
		private int m_SelectedIndex = 1;
		private ArtViewer.Art m_PreviewArt = ArtViewer.Art.Items;
		private int m_PreviewIndex = 0;
		private bool m_Scale = true;
		private bool m_RoomView = true;
		private bool m_Animate = false;
		private int m_Darkness = 28;
		private TheBox.Common.RecentIntList m_RecentHues;

		#region Events

		/// <summary>
		/// Occurs when the selected hue has changed
		/// </summary>
		public event EventHandler HueChanged;

		#endregion

		/// <summary>
		/// Gets or sets the index of the hue currently selected in the hue picker
		/// </summary>
		public int SelectedIndex
		{
			get { return m_SelectedIndex; }
			set
			{ 
				m_SelectedIndex = value;

				if ( HueChanged != null )
				{
					HueChanged( this, new EventArgs() );
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of art being previed
		/// </summary>
		public ArtViewer.Art Art
		{
			get { return m_PreviewArt; }
			set { m_PreviewArt = value; }
		}

		/// <summary>
		/// Gets or sets the index of the previewed art
		/// </summary>
		public int PreviewIndex
		{
			get { return m_PreviewIndex; }
			set { m_PreviewIndex = value; }
		}

		/// <summary>
		/// Gets or sets a value stating whether items should be scaled in the preview window
		/// </summary>
		public bool Scale
		{
			get { return m_Scale; }
			set { m_Scale = value; }
		}

		/// <summary>
		/// Gets or sets a value stating whether items should be displayed using the room view
		/// </summary>
		public bool RoomView
		{
			get { return m_RoomView; }
			set { m_RoomView = value; }
		}

		/// <summary>
		/// Gets or sets a value stating whether NPCs should be animated
		/// </summary>
		public bool Animate
		{
			get { return m_Animate; }
			set { m_Animate = value; }
		}

		/// <summary>
		/// Gets or sets the brightness level on the hues chart
		/// </summary>
		public int Darkness
		{
			get { return m_Darkness; }
			set
			{
				m_Darkness = value;
				
				if ( m_Darkness > 31 )
					m_Darkness = 31;

				if ( m_Darkness < 0 )
					m_Darkness = 0;
			}
		}

		/// <summary>
		/// Gets or sets the recently used hues
		/// </summary>
		public TheBox.Common.RecentIntList RecentHues
		{
			get { return m_RecentHues; }
			set { m_RecentHues = value; }
		}

		public HuesOptions()
		{
			m_RecentHues = new TheBox.Common.RecentIntList();
			m_RecentHues.Capacity = 10;
		}
	}
}
