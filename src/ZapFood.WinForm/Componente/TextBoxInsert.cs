using System;
using System.Windows.Forms;

namespace ZapFood.WinForm.Componente
{
    public partial class TextBoxInsert : TextBox
    {
        public TextBoxInsert()
        {
            InitializeComponent();
            btnGravar.Click += new EventHandler(cConfirmEvent);
            btnCancelar.Click += new EventHandler(cCancelEvent);
        }



        public event EventHandler<EventArgs> ConfirmEvent;
        void cConfirmEvent(object sender, EventArgs e)
        {
            var completedEvent = ConfirmEvent;
            if (completedEvent != null)
            {
                completedEvent(this, e);
            }
        }

        public event EventHandler<EventArgs> CancelEvent;
        void cCancelEvent(object sender, EventArgs e)
        {
            var cancelEvent = CancelEvent;
            if (cancelEvent != null)
            {
                cancelEvent(this, e);
            }
        }

    }
}
