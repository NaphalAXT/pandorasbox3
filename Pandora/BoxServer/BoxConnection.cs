using System;
using System.Windows.Forms;
using TheBox.Forms;

namespace TheBox.BoxServer
{
    /// <summary>
    /// Provides methods for managing interaction with the BoxServer
    /// </summary>
    public class BoxConnection
    {
        /// <summary>
        /// Occurs when the online state of Pandora's Box is changed
        /// </summary>
        public event EventHandler OnlineChanged;

        private BoxRemote m_Remote;
        private ProfileManager m_Profiles;

        /// <summary>
        /// Specifies whether Pandora is connected to the BoxServer
        /// </summary>
        private static bool m_Connected = false;

        /// <summary>
        /// States whether Pandora is connected to the BoxServer
        /// </summary>
        public bool Connected
        {
            get { return m_Connected; }
            private set
            {
                if (m_Connected != value)
                {
                    m_Connected = value;

                    if (OnlineChanged != null)
                    {
                        OnlineChanged(null, new EventArgs());
                    }
                }
            }
        }

        public BoxConnection(ProfileManager profiles)
        {
            m_Profiles = profiles;
        }

        public void RequestConnection()
        {
            if (Pandora.Profile.Server.Enabled)
            {
                if (MessageBox.Show(Pandora.BoxForm as Form,
                    Pandora.Localization.TextProvider["Misc.RequestConnection"],
                    null,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Disconnect();

                    TheBox.Forms.BoxServerForm form = new TheBox.Forms.BoxServerForm(false);
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show(Pandora.Localization.TextProvider["Errors.NoServer"]);
            }
        }

        /// <summary>
        /// Checks a BoxMessage and processes any errors occurred
        /// </summary>
        /// <param name="msg">The BoxMessage returned by the server</param>
        /// <returns>True if the message is OK, false if errors have been found</returns>
        public bool CheckErrors(BoxMessage msg)
        {
            if (msg == null)
                return true; // null message means no error

            if (msg is ErrorMessage)
            {
                // Generic error message
                MessageBox.Show(string.Format(Pandora.Localization.TextProvider["Errors.GenServErr"], (msg as ErrorMessage).Message));
                return false;
            }
            else if (msg is LoginError)
            {
                LoginError logErr = msg as LoginError;

                string err = null;

                switch (logErr.Error)
                {
                    case AuthenticationResult.AccessLevelError:

                        err = Pandora.Localization.TextProvider["Errors.LoginAccess"];
                        break;

                    case AuthenticationResult.OnlineMobileRequired:

                        err = Pandora.Localization.TextProvider["Errors.NotOnline"];
                        break;

                    case AuthenticationResult.UnregisteredUser:

                        err = Pandora.Localization.TextProvider["Errors.LogUnregistered"];
                        break;

                    case AuthenticationResult.WrongCredentials:

                        err = Pandora.Localization.TextProvider["Errors.WrongCredentials"];
                        break;

                    case AuthenticationResult.Success:

                        return true;
                }

                MessageBox.Show(err);
                return false;
            }
            else if (msg is FeatureNotSupported)
            {
                MessageBox.Show(Pandora.Localization.TextProvider["Errors.NotSupported"]);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to connect to the BoxServer
        /// </summary>
        /// <returns>True if succesful</returns>
        public bool Connect()
        {
            return Connect(true);
        }

        /// <summary>
        /// Tries to connect to the BoxServer
        /// </summary>
        /// <param name="ProcessErrors">Specifies whether to process errors and display them to the user</param>
        /// <returns>True if succesful</returns>
        public bool Connect(bool ProcessErrors)
        {
            try
            {
                string ConnectionString = string.Format("tcp://{0}:{1}/BoxRemote", Pandora.Profile.Server.Address, Pandora.Profile.Server.Port);

                m_Remote = Activator.GetObject(typeof(BoxRemote), ConnectionString) as BoxRemote;

                // Perform Login
                BoxMessage msg = new LoginMessage();

                msg.Username = Pandora.Profile.Server.Username;
                msg.Password = Pandora.Profile.Server.Password;
                byte[] data = msg.Compress();
                string outType = null;

                byte[] result = m_Remote.PerformRemoteRequest(msg.GetType().FullName, data, out outType);

                if (result == null)
                {
                    MessageBox.Show(Pandora.Localization.TextProvider["Errors.ServerError"]);
                    Connected = false;
                    return false;
                }

                Type t = Type.GetType(outType);

                BoxMessage outcome = BoxMessage.Decompress(result, t);

                if (ProcessErrors)
                {
                    if (!CheckErrors(outcome))
                    {
                        Connected = false;
                        return false;
                    }
                }

                if (outcome is LoginSuccess)
                {
                    Connected = true;
                    return true;
                }
                else
                {
                    Connected = false;
                    return false;
                }
            }
            catch (Exception err)
            {
                Pandora.Log.WriteError(err, "Connection failed to box server");
                Connected = false;
            }

            return false;
        }

        /// <summary>
        /// Closes the connection with the server
        /// </summary>
        public void Disconnect()
        {
            if (m_Remote != null)
            {
                m_Remote = null;
                Connected = false;
            }
        }

        /// <summary>
        /// Sends a message to the server
        /// </summary>
        /// <param name="msg">The message being sent to the server</param>
        /// <param name="window">Specifies whether to use the connection form</param>
        /// <returns>The outcome of the transaction</returns>
        public BoxMessage ProcessMessage(BoxMessage msg, bool window)
        {
            if (window)
            {
                TheBox.Forms.BoxServerForm form = new TheBox.Forms.BoxServerForm(msg);
                form.ShowDialog();
                return form.Response;
            }
            else
            {
                return ProcessMessage(msg);
            }
        }

        /// <summary>
        /// Sends a message to the server. Processes errors too.
        /// </summary>
        /// <param name="msg">The message to send to the server</param>
        /// <returns>A BoxMessage if there is one</returns>
        public BoxMessage ProcessMessage(BoxMessage msg)
        {
            BoxMessage outcome = null;

            if (!Connected)
                Connect();

            if (!Connected)
                return null;

            byte[] data = msg.Compress();
            string outType = null;

            try
            {
                byte[] result = m_Remote.PerformRemoteRequest(msg.GetType().FullName, data, out outType);

                if (result == null)
                {
                    return null;
                }

                Type t = Type.GetType(outType);
                outcome = BoxMessage.Decompress(result, t);

                if (!CheckErrors(outcome))
                {
                    outcome = null;
                }
            }
            catch (Exception err)
            {
                Pandora.Log.WriteError(err, "Error when processing a BoxMessage of type: {0}", msg.GetType().FullName);
                MessageBox.Show(Pandora.Localization.TextProvider["Errors.ConnectionLost"]);
                Connected = false;
                outcome = null;
            }

            return outcome;
        }

        

        /// <summary>
        /// Sends a message to the BoxServer
        /// </summary>
        /// <param name="message">The message that must be sent</param>
        /// <returns>The message outcome</returns>
        public BoxMessage SendToServer(BoxMessage message)
        {
            if (!Connected)
            {
                // Not connected, request connection
                if (MessageBox.Show(null, Pandora.Localization.TextProvider["Misc.RequestConnection"], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BoxServerForm form = new BoxServerForm(false);
                    form.ShowDialog();
                }

                if (!Connected)
                {
                    return null;
                }
            }

            Pandora.Profile.Server.FillBoxMessage(message);

            BoxServerForm msgForm = new BoxServerForm(message);
            msgForm.ShowDialog();

            TheBox.Common.Utility.BringClientToFront();

            return msgForm.Response;
        }

        
    }
}