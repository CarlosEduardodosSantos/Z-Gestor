using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ZapFood.WinForm
{
    public class MsgBoxExt
    {
        #region events
        //event result (on closing or timeout)
        public static event EventHandler<MsgBoxResultEventArgs> msgBoxResultEvent;

        #endregion

        #region constants

        //constant for destroying the messagebox
        const int WM_NCDESTROY = 0x0082;

        #endregion

        #region p/Invoke

        // p/Invoke for destroying the messagebox

        // For Windows Mobile, replace user32.dll with coredll.dll

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region fields
        //current options
        private static MsgBoxExtOptions _currentOptions;

        //Thread for showing messagebox        
        private static Thread msgBoxThread;

        //timeout timer used for automaticly closing msgBox
        private static System.Windows.Forms.Timer timeoutTimer;

        #endregion

        #region methods

        //main public method for showing msgBox
        public static bool Show(MsgBoxExtOptions options)
        {
            //check whether there is a messagebox active
            if (_currentOptions != null)
            {
                //check whether current active Msgbox is model
                if (_currentOptions.model)
                {
                    return false; //do nothing
                }
                //else close the current msgBox
                closeMessageBox(_currentOptions.caption);
            }

            //save options in current options
            _currentOptions = options;

            //if timeout > 0  start timeout timer
            if (options.timeout > 0)
            {
                timeoutTimer = new System.Windows.Forms.Timer();
                timeoutTimer.Tick += new EventHandler(timeoutTimer_Tick);
                timeoutTimer.Interval = options.timeout;
                timeoutTimer.Enabled = true;
            }

            //start messagebox thread
            msgBoxThread = new Thread(new ThreadStart(startMsgBoxThread));
            msgBoxThread.IsBackground = true;
            msgBoxThread.Start();

            return true; //messagebox started

        }

        private static void startMsgBoxThread()
        {
            //standard Messagebox with results 
            DialogResult result = MessageBox.Show(_currentOptions.text, _currentOptions.caption, _currentOptions.buttons, _currentOptions.icon, _currentOptions.defaultButton);

            //dispose the tiemout timer
            disposeTimeoutTimer();

            //fire result event
            onMsgBoxResultEvent(_currentOptions.resultReference, result);

            //dispose current options
            _currentOptions = null;

        }

        private static void timeoutTimer_Tick(object sender, EventArgs e)
        {
            // close current messagebox
            closeMessageBox(_currentOptions.caption);

            //fire result event
            onMsgBoxResultEvent(_currentOptions.resultReference, DialogResult.None);

            //dispose current options

            _currentOptions = null;
        }
        //method dispose timeout timer 
        private static void disposeTimeoutTimer()
        {
            if (timeoutTimer != null)
            {
                timeoutTimer.Dispose();
            }
            timeoutTimer = null;
        }

        //method close messagebox
        public static void closeMessageBox(string title)
        {
            //disopose tiemout timer
            disposeTimeoutTimer();

            //kill window
            KillWindow(null, title);

            //kill dialog thread
            killDialogThread();

        }
        //method killwindow
        private static void KillWindow(string className, string title)
        {
            //find window handle
            IntPtr handle = FindWindow(className, title);

            //send destroy message
            SendMessage(handle, WM_NCDESTROY, (IntPtr)0, (IntPtr)0);
        }

        //method kill dialog thread
        private static void killDialogThread()
        {
            //check wether exists
            if (msgBoxThread != null)
            {
                //abort the thread
                msgBoxThread.Abort();
                msgBoxThread = null;
            }
        }

        //method fire  messagebox result event
        private static void onMsgBoxResultEvent
            (MsgBoxResultReferences resultReference, DialogResult resultButton)
        {
            MsgBoxResultEventArgs e = new MsgBoxResultEventArgs(resultReference, resultButton);
            EventHandler<MsgBoxResultEventArgs> handler = msgBoxResultEvent;
            if (handler != null)
            {
                handler(null, e);
            }
        }
        #endregion


    }


    //event arguments for MsgBoxResultReferences
    public class MsgBoxResultEventArgs : EventArgs
    {
        private MsgBoxResultReferences _resultReference;
        private DialogResult _resultButton;

        public MsgBoxResultEventArgs(MsgBoxResultReferences resultReference, DialogResult resultButton)
        {
            _resultReference = resultReference;
            _resultButton = resultButton;
        }

        public MsgBoxResultReferences resultReference
        { get { return _resultReference; } }
        public DialogResult resultButton
        { get { return _resultButton; } }

    }

    //msgBoxExt options class
    public class MsgBoxExtOptions
    {
        #region fields

        private string _text, _caption;
        private MsgBoxResultReferences _resultReference;
        private MessageBoxButtons _buttons;
        private MessageBoxIcon _icon;
        private MessageBoxDefaultButton _defaultButton;
        private int _timeout;
        private bool _model;

        #endregion

        #region constructors

        public MsgBoxExtOptions(string text, string caption, MsgBoxResultReferences resultReference)
        {
            setOptions(text, caption, resultReference, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, false);

        }
        public MsgBoxExtOptions(string text, string caption, MsgBoxResultReferences resultReference, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            setOptions(text, caption, resultReference, buttons, icon, MessageBoxDefaultButton.Button1, 0, false);

        }

        public MsgBoxExtOptions(string text, string caption, MsgBoxResultReferences resultReference, MessageBoxButtons buttons, MessageBoxIcon icon, int timeout, bool model, MessageBoxDefaultButton defaultButton)
        {

            setOptions(text, caption, resultReference, buttons, icon, defaultButton, timeout, model);

        }
        public MsgBoxExtOptions(string text, string caption, MsgBoxResultReferences resultReference, MessageBoxButtons buttons, MessageBoxIcon icon, int timeout, bool model)
        {
            setOptions(text, caption, resultReference, buttons, icon, MessageBoxDefaultButton.Button1, timeout, model);

        }
        #endregion

        #region methods

        private void setOptions(string text, string caption, MsgBoxResultReferences msgBoxResultReferences, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon, MessageBoxDefaultButton messageBoxDefaultButton, int timeout, bool model)
        {
            _resultReference = msgBoxResultReferences;
            _text = text;

            //this is important for finding the window
            if (caption == String.Empty)
                _caption = "empty_caption";
            else
                _caption = caption;

            _buttons = messageBoxButtons;
            _icon = messageBoxIcon;
            _defaultButton = messageBoxDefaultButton;
            _timeout = timeout;
            _model = model;

        }

        #endregion

        #region properties

        public MsgBoxResultReferences resultReference
        {
            set { _resultReference = value; }
            get { return _resultReference; }
        }
        public MessageBoxIcon icon
        {
            set { _icon = value; }
            get { return _icon; }
        }
        public string text
        {
            set { _text = value; }
            get { return _text; }
        }
        public string caption
        {
            set { _caption = value; }
            get { return _caption; }
        }

        public MessageBoxButtons buttons
        {
            set { _buttons = value; }
            get { return _buttons; }
        }
        public MessageBoxDefaultButton defaultButton
        {
            set { _defaultButton = value; }
            get { return _defaultButton; }
        }
        public int timeout
        {
            set { _timeout = value; }
            get { return _timeout; }
        }
        public bool model
        {
            set { _model = value; }
            get { return _model; }
        }
        #endregion
    }

    #region enums
    //enumerate MsgBoxResultReferences
    public enum MsgBoxResultReferences
    {
        EMPTY = 0, //nothing
        CLOSE_ON_YES = 0x0001,
    }
    #endregion
}