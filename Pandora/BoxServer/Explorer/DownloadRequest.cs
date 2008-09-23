using System;
using System.IO;

namespace TheBox.BoxServer
{
	[ Serializable ]
	/// <summary>
	/// Requests to download a file from the server
	/// </summary>
	public class DownloadRequest : ExplorerMessage
	{
		private string m_Filename;

		/// <summary>
		/// Gets or sets the filename to be retrieved, relative to the RunUO path
		/// </summary>
		public string Filename
		{
			get { return m_Filename; }
			set { m_Filename = value; }
		}
		
		public DownloadRequest()
		{
		}
	}
}