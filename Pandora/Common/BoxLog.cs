using System;
using System.IO;
using System.Runtime.InteropServices;
using log4net;
using log4net.Repository;
using log4net.Appender;
using log4net.Layout;

namespace TheBox.Common
{
	/// <summary>
	/// Provides logging functionality for Pandora's Box
	/// </summary>
	public class BoxLog
	{
        // Issue 6:  	 Improve error management - Tarion
        // Intergration of log4net

		#region Imports

		//Struct to retrive system info
		[StructLayout(LayoutKind.Sequential)]   
		private struct SYSTEM_INFO 
		{
			public  uint  dwOemId;
			public  uint  dwPageSize;
			public  uint  lpMinimumApplicationAddress;
			public	uint  lpMaximumApplicationAddress;
			public  uint  dwActiveProcessorMask;
			public  uint  dwNumberOfProcessors;
			public  uint  dwProcessorType;
			public  uint  dwAllocationGranularity;
			public  uint  dwProcessorLevel;
			public  uint  dwProcessorRevision;		
		}
		//struct to retrive memory status
		[StructLayout(LayoutKind.Sequential)]   
		private struct MEMORYSTATUS
		{
			public  uint dwLength;
			public  uint dwMemoryLoad;
			public  uint dwTotalPhys;
			public  uint dwAvailPhys;
			public  uint dwTotalPageFile;
			public  uint dwAvailPageFile;
			public  uint dwTotalVirtual;
			public  uint dwAvailVirtual;
		}

		// Constants used for processor types
		private const int PROCESSOR_INTEL_386 = 386;
		private const int PROCESSOR_INTEL_486 = 486;
		private const int PROCESSOR_INTEL_PENTIUM = 586;
		private const int PROCESSOR_MIPS_R4000 = 4000;
		private const int PROCESSOR_ALPHA_21064 = 21064;

		//To get system information
		[DllImport("kernel32")]
		static extern void GetSystemInfo(ref SYSTEM_INFO pSI);
		
		//To get Memory status
		[DllImport("kernel32")]
		static extern void GlobalMemoryStatus(ref MEMORYSTATUS buf);

		#endregion

        private string filename;
		private StreamWriter m_Stream;


        #region log4net

        /// <summary>
        /// Should we user log4net? Otherwise the old system is used.
        /// Use this for testing and later remove the old code.
        /// </summary>
        public static readonly bool USE_LOG4NET = true;

        /// <summary>
        /// The log repository
        /// </summary>
        private ILoggerRepository repository;

        /// <summary>
        /// Replacement for the old boxLog
        /// </summary>
        private ILog boxLog;

        #endregion

        public BoxLog( string filename )
		{
            this.filename = filename;
            if (USE_LOG4NET)
            {
                SetupLog4Net();
            }
            else
            {
                string folder = Path.GetDirectoryName(filename);

                // Ensure directory
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                try
                {
                    m_Stream = new StreamWriter(filename, false);
                }
                catch
                {
                    m_Stream = null;
                    return;
                }

                
            }

            LogHeader();
		}

        /// <summary>
        /// Log the systeminformation at the beginning of the logile
        /// </summary>
        private void LogHeader()
        {
            // System info
            SYSTEM_INFO pSI = new SYSTEM_INFO();
            GetSystemInfo(ref pSI);
            string CPUType;
            switch (pSI.dwProcessorType)
            {
                case PROCESSOR_INTEL_386:
                    CPUType = "Intel 386";
                    break;
                case PROCESSOR_INTEL_486:
                    CPUType = "Intel 486";
                    break;
                case PROCESSOR_INTEL_PENTIUM:
                    CPUType = "Intel Pentium";
                    break;
                case PROCESSOR_MIPS_R4000:
                    CPUType = "MIPS R4000";
                    break;
                case PROCESSOR_ALPHA_21064:
                    CPUType = "DEC Alpha 21064";
                    break;
                default:
                    CPUType = "(unknown)";
                    break;
            }

            MEMORYSTATUS memSt = new MEMORYSTATUS();
            GlobalMemoryStatus(ref memSt);

            if (USE_LOG4NET)
            {
                boxLog.Info("Pandora's Box - Log");
                boxLog.InfoFormat("Pandora version {0}", Pandora.Version);
                boxLog.Info("");
                boxLog.Info(DateTime.Now.ToString());
                boxLog.Info("Windows version: " + Environment.OSVersion.Version.ToString());
                boxLog.Info("Processor family: " + CPUType);

                boxLog.Info("Physical memory: " + (memSt.dwTotalPhys / 1024).ToString());
                boxLog.Info("");
            }
            else
            {
                m_Stream.WriteLine("Pandora's Box - Log");
                m_Stream.WriteLine(string.Format("Pandora version {0}", Pandora.Version));
                m_Stream.WriteLine("");
                m_Stream.WriteLine(DateTime.Now.ToString());
                m_Stream.WriteLine("Windows version: " + Environment.OSVersion.Version.ToString());

                
                m_Stream.WriteLine("Processor family: " + CPUType);

                
                m_Stream.WriteLine("Physical memory: " + (memSt.dwTotalPhys / 1024).ToString());
                m_Stream.WriteLine();
            }
        }

        private void SetupLog4Net()
        {
            // Uncomment the next line to enable log4net internal debugging
            // log4net.Util.LogLog.InternalDebugging = true;

            // This will instruct log4net to look for a configuration file
            // called config.log4net in the root directory of the device
            log4net.Config.XmlConfigurator.Configure(new FileInfo(@"\logger.config"));

            repository = LogManager.CreateRepository("BoxLogRepository");

            // see: http://logging.apache.org/log4net/release/sdk/log4net.Layout.PatternLayout.html
            ILayout layout = new PatternLayout("[%date{HH:mm:ss}] %-5level %message%newline");

            

            FileAppender fileAppender = new FileAppender();
            fileAppender.File = filename;
            fileAppender.Layout = layout;
            fileAppender.AppendToFile = false;
            fileAppender.ActivateOptions();

            ConsoleAppender consoleAppender = new ConsoleAppender();
            consoleAppender.Layout = layout;
            consoleAppender.ActivateOptions();
            
            log4net.Config.BasicConfigurator.Configure(repository, fileAppender);
            log4net.Config.BasicConfigurator.Configure(repository, consoleAppender);

            boxLog = LogManager.GetLogger(repository.Name, "BoxLog");

            repository.ConfigurationChanged += new LoggerRepositoryConfigurationChangedEventHandler(repository_ConfigurationChanged);
        }

        void repository_ConfigurationChanged(object sender, EventArgs e)
        {
            boxLog.Debug("repository_ConfigurationChanged");
        }

        private string CurrentTime
		{
			get
			{
				return string.Format( "[{0}:{1}:{2}]", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second );
			}
		}

		public void WriteEntry( string text )
		{
            if (USE_LOG4NET)
            {
                boxLog.Info(text);
            }
            else
            {
                if (m_Stream != null)
                {
                    m_Stream.WriteLine(string.Format("{0} {1}", CurrentTime, text));
                    m_Stream.Flush();
                }
            }
		}

		public void WriteEntry( string format, params object[] args )
		{
			WriteEntry( string.Format( format, args ) );
		}

		public void WriteError( Exception error, string additionalInfo )
		{
            if (USE_LOG4NET)
            {
                boxLog.Error(String.Format("Additional information: {0}", additionalInfo), error);
            }
            else
            {
                if (m_Stream != null)
                {
                    WriteEntry("**** ERROR ****");

                    if (error != null)
                        m_Stream.WriteLine(error.ToString());

                    if (additionalInfo != null)
                        m_Stream.WriteLine(string.Format("Additional information: {0}", additionalInfo));

                    m_Stream.Flush();
                }
            }
		}

		public void WriteError( Exception error, string format, params object[] args )
		{
			WriteError( error, string.Format( format, args ) );
		}

        /// <summary>
        /// Unsused! Remove it?
        /// - by Tarion
        /// </summary>
		public void Close()
		{
            if (USE_LOG4NET)
            {
                boxLog.Info("Session closed");
                LogManager.Shutdown();
            }
            else
            {

                if (m_Stream != null)
                {
                    WriteEntry("Session closed");
                    m_Stream.Close();
                    m_Stream = null;
                }
            }
        }

    }
}