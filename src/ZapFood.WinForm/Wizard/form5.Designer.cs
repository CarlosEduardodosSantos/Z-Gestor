namespace ZapFood.WinForm.Wizard
{
    partial class form5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.chkImprimeGestor = new System.Windows.Forms.CheckBox();
            this.chkUsaDesApp = new System.Windows.Forms.CheckBox();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.chkNotificacao = new System.Windows.Forms.CheckBox();
            this.chkIniciaWindows = new System.Windows.Forms.CheckBox();
            this.chkMoniDelivey = new System.Windows.Forms.CheckBox();
            this.chkMoniMesa = new System.Windows.Forms.CheckBox();
            this.chkMoniRetira = new System.Windows.Forms.CheckBox();
            this.chkMoniToten = new System.Windows.Forms.CheckBox();
            this.chkAutoConfirmar = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::ZapFood.WinForm.Properties.Resources.barra;
            this.panel2.Controls.Add(this.label17);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(560, 42);
            this.panel2.TabIndex = 180;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.DimGray;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label17.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(0, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(560, 42);
            this.label17.TabIndex = 4;
            this.label17.Text = "Configurações adicionais";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkImprimeGestor
            // 
            this.chkImprimeGestor.AutoSize = true;
            this.chkImprimeGestor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.helpProvider1.SetHelpNavigator(this.chkImprimeGestor, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.chkImprimeGestor, "Com esse opão ativa, você deixara de imprimir pelo gerenciamento de impressão e p" +
        "assará a imprimir pelo CUPOM.FRX que deverá estar na raiz do aplicativo");
            this.chkImprimeGestor.Location = new System.Drawing.Point(12, 81);
            this.chkImprimeGestor.Name = "chkImprimeGestor";
            this.helpProvider1.SetShowHelp(this.chkImprimeGestor, true);
            this.chkImprimeGestor.Size = new System.Drawing.Size(160, 18);
            this.chkImprimeGestor.TabIndex = 194;
            this.chkImprimeGestor.Text = "Usar a impressão Gestor.";
            this.chkImprimeGestor.UseVisualStyleBackColor = true;
            // 
            // chkUsaDesApp
            // 
            this.chkUsaDesApp.AutoSize = true;
            this.chkUsaDesApp.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkUsaDesApp.Location = new System.Drawing.Point(12, 57);
            this.chkUsaDesApp.Name = "chkUsaDesApp";
            this.chkUsaDesApp.Size = new System.Drawing.Size(143, 18);
            this.chkUsaDesApp.TabIndex = 193;
            this.chkUsaDesApp.Text = "Usar a descrição App.";
            this.chkUsaDesApp.UseVisualStyleBackColor = true;
            // 
            // chkNotificacao
            // 
            this.chkNotificacao.AutoSize = true;
            this.chkNotificacao.Font = new System.Drawing.Font("Tahoma", 9F);
            this.helpProvider1.SetHelpNavigator(this.chkNotificacao, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.chkNotificacao, "Com esse opão ativa, você deixara de imprimir pelo gerenciamento de impressão e p" +
        "assará a imprimir pelo CUPOM.FRX que deverá estar na raiz do aplicativo");
            this.chkNotificacao.Location = new System.Drawing.Point(12, 104);
            this.chkNotificacao.Name = "chkNotificacao";
            this.helpProvider1.SetShowHelp(this.chkNotificacao, true);
            this.chkNotificacao.Size = new System.Drawing.Size(259, 18);
            this.chkNotificacao.TabIndex = 195;
            this.chkNotificacao.Text = "Notificação sonora ao chegar novo pedido.";
            this.chkNotificacao.UseVisualStyleBackColor = true;
            // 
            // chkIniciaWindows
            // 
            this.chkIniciaWindows.AutoSize = true;
            this.chkIniciaWindows.Font = new System.Drawing.Font("Tahoma", 9F);
            this.helpProvider1.SetHelpNavigator(this.chkIniciaWindows, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.chkIniciaWindows, "Iniciar gestor ao iniciar o windows");
            this.chkIniciaWindows.Location = new System.Drawing.Point(12, 127);
            this.chkIniciaWindows.Name = "chkIniciaWindows";
            this.helpProvider1.SetShowHelp(this.chkIniciaWindows, true);
            this.chkIniciaWindows.Size = new System.Drawing.Size(239, 18);
            this.chkIniciaWindows.TabIndex = 196;
            this.chkIniciaWindows.Text = "Iniciar o Z-Food junto com o Windows.";
            this.chkIniciaWindows.UseVisualStyleBackColor = true;
            this.chkIniciaWindows.CheckedChanged += new System.EventHandler(this.chkIniciaWindows_CheckedChanged);
            // 
            // chkMoniDelivey
            // 
            this.chkMoniDelivey.AutoSize = true;
            this.helpProvider1.SetHelpNavigator(this.chkMoniDelivey, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.chkMoniDelivey, "Iniciar gestor ao iniciar o windows");
            this.chkMoniDelivey.Location = new System.Drawing.Point(18, 30);
            this.chkMoniDelivey.Name = "chkMoniDelivey";
            this.helpProvider1.SetShowHelp(this.chkMoniDelivey, true);
            this.chkMoniDelivey.Size = new System.Drawing.Size(69, 18);
            this.chkMoniDelivey.TabIndex = 197;
            this.chkMoniDelivey.Tag = "0";
            this.chkMoniDelivey.Text = "Delivey";
            this.chkMoniDelivey.UseVisualStyleBackColor = true;
            this.chkMoniDelivey.CheckedChanged += new System.EventHandler(this.chkMoniDelivey_CheckedChanged);
            // 
            // chkMoniMesa
            // 
            this.chkMoniMesa.AutoSize = true;
            this.helpProvider1.SetHelpNavigator(this.chkMoniMesa, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.chkMoniMesa, "Iniciar gestor ao iniciar o windows");
            this.chkMoniMesa.Location = new System.Drawing.Point(214, 30);
            this.chkMoniMesa.Name = "chkMoniMesa";
            this.helpProvider1.SetShowHelp(this.chkMoniMesa, true);
            this.chkMoniMesa.Size = new System.Drawing.Size(57, 18);
            this.chkMoniMesa.TabIndex = 198;
            this.chkMoniMesa.Tag = "2";
            this.chkMoniMesa.Text = "Mesa";
            this.chkMoniMesa.UseVisualStyleBackColor = true;
            this.chkMoniMesa.CheckedChanged += new System.EventHandler(this.chkMoniDelivey_CheckedChanged);
            // 
            // chkMoniRetira
            // 
            this.chkMoniRetira.AutoSize = true;
            this.helpProvider1.SetHelpNavigator(this.chkMoniRetira, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.chkMoniRetira, "Iniciar gestor ao iniciar o windows");
            this.chkMoniRetira.Location = new System.Drawing.Point(115, 30);
            this.chkMoniRetira.Name = "chkMoniRetira";
            this.helpProvider1.SetShowHelp(this.chkMoniRetira, true);
            this.chkMoniRetira.Size = new System.Drawing.Size(63, 18);
            this.chkMoniRetira.TabIndex = 199;
            this.chkMoniRetira.Tag = "1";
            this.chkMoniRetira.Text = "Retira";
            this.chkMoniRetira.UseVisualStyleBackColor = true;
            this.chkMoniRetira.CheckedChanged += new System.EventHandler(this.chkMoniDelivey_CheckedChanged);
            // 
            // chkMoniToten
            // 
            this.chkMoniToten.AutoSize = true;
            this.helpProvider1.SetHelpNavigator(this.chkMoniToten, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.chkMoniToten, "Iniciar gestor ao iniciar o windows");
            this.chkMoniToten.Location = new System.Drawing.Point(307, 30);
            this.chkMoniToten.Name = "chkMoniToten";
            this.helpProvider1.SetShowHelp(this.chkMoniToten, true);
            this.chkMoniToten.Size = new System.Drawing.Size(65, 18);
            this.chkMoniToten.TabIndex = 200;
            this.chkMoniToten.Tag = "3";
            this.chkMoniToten.Text = "Totem";
            this.chkMoniToten.UseVisualStyleBackColor = true;
            this.chkMoniToten.CheckedChanged += new System.EventHandler(this.chkMoniDelivey_CheckedChanged);
            // 
            // chkAutoConfirmar
            // 
            this.chkAutoConfirmar.AutoSize = true;
            this.chkAutoConfirmar.Font = new System.Drawing.Font("Tahoma", 9F);
            this.helpProvider1.SetHelpNavigator(this.chkAutoConfirmar, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.chkAutoConfirmar, "Ao marcar essa opção o sistema era auto confirmar os pedidos novos");
            this.chkAutoConfirmar.Location = new System.Drawing.Point(12, 150);
            this.chkAutoConfirmar.Name = "chkAutoConfirmar";
            this.helpProvider1.SetShowHelp(this.chkAutoConfirmar, true);
            this.chkAutoConfirmar.Size = new System.Drawing.Size(263, 18);
            this.chkAutoConfirmar.TabIndex = 198;
            this.chkAutoConfirmar.Text = "Confirmar novos pedidos automaticamente.";
            this.chkAutoConfirmar.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkMoniToten);
            this.groupBox1.Controls.Add(this.chkMoniRetira);
            this.groupBox1.Controls.Add(this.chkMoniMesa);
            this.groupBox1.Controls.Add(this.chkMoniDelivey);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(85, 196);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 69);
            this.groupBox1.TabIndex = 197;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipos de monitoramento Z-Food";
            // 
            // form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(560, 333);
            this.Controls.Add(this.chkAutoConfirmar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkIniciaWindows);
            this.Controls.Add(this.chkNotificacao);
            this.Controls.Add(this.chkImprimeGestor);
            this.Controls.Add(this.chkUsaDesApp);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "form5";
            this.Text = "form5";
            this.Load += new System.EventHandler(this.form5_Load);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkImprimeGestor;
        private System.Windows.Forms.CheckBox chkUsaDesApp;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.CheckBox chkNotificacao;
        private System.Windows.Forms.CheckBox chkIniciaWindows;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkMoniToten;
        private System.Windows.Forms.CheckBox chkMoniRetira;
        private System.Windows.Forms.CheckBox chkMoniMesa;
        private System.Windows.Forms.CheckBox chkMoniDelivey;
        private System.Windows.Forms.CheckBox chkAutoConfirmar;
    }
}