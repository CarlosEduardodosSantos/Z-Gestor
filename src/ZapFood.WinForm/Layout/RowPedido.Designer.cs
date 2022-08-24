namespace ZapFood.WinForm.Layout
{
    partial class RowPedido
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbAgendamento = new System.Windows.Forms.Label();
            this.lbCliente = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbFone = new System.Windows.Forms.Label();
            this.lbEndereco = new System.Windows.Forms.Label();
            this.lbNroPedido = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lbDataHora = new System.Windows.Forms.Label();
            this.lbTipo = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbAgendamento);
            this.panel1.Controls.Add(this.lbCliente);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lbFone);
            this.panel1.Controls.Add(this.lbEndereco);
            this.panel1.Controls.Add(this.lbNroPedido);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbDataHora);
            this.panel1.Controls.Add(this.lbTipo);
            this.panel1.Controls.Add(this.lbStatus);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(765, 86);
            this.panel1.TabIndex = 0;
            // 
            // lbAgendamento
            // 
            this.lbAgendamento.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAgendamento.BackColor = System.Drawing.SystemColors.Info;
            this.lbAgendamento.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbAgendamento.ForeColor = System.Drawing.Color.Red;
            this.lbAgendamento.Location = new System.Drawing.Point(422, 30);
            this.lbAgendamento.Name = "lbAgendamento";
            this.lbAgendamento.Size = new System.Drawing.Size(126, 23);
            this.lbAgendamento.TabIndex = 9;
            this.lbAgendamento.Text = "Agendado";
            this.lbAgendamento.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbCliente
            // 
            this.lbCliente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCliente.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbCliente.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbCliente.Location = new System.Drawing.Point(85, 30);
            this.lbCliente.Name = "lbCliente";
            this.lbCliente.Size = new System.Drawing.Size(331, 23);
            this.lbCliente.TabIndex = 1;
            this.lbCliente.Text = "Cliente";
            this.lbCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox1.BackgroundImage = global::ZapFood.WinForm.Properties.Resources.Captura_de_tela_2021_12_14_105851;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(79, 84);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // lbFone
            // 
            this.lbFone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFone.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbFone.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbFone.Location = new System.Drawing.Point(554, 30);
            this.lbFone.Name = "lbFone";
            this.lbFone.Size = new System.Drawing.Size(206, 23);
            this.lbFone.TabIndex = 3;
            this.lbFone.Text = "Fone?";
            this.lbFone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbEndereco
            // 
            this.lbEndereco.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbEndereco.BackColor = System.Drawing.Color.Linen;
            this.lbEndereco.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbEndereco.Location = new System.Drawing.Point(85, 58);
            this.lbEndereco.Name = "lbEndereco";
            this.lbEndereco.Size = new System.Drawing.Size(463, 23);
            this.lbEndereco.TabIndex = 2;
            this.lbEndereco.Text = "Endereço";
            this.lbEndereco.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbNroPedido
            // 
            this.lbNroPedido.BackColor = System.Drawing.Color.SeaShell;
            this.lbNroPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbNroPedido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbNroPedido.Location = new System.Drawing.Point(85, 2);
            this.lbNroPedido.Name = "lbNroPedido";
            this.lbNroPedido.Size = new System.Drawing.Size(159, 23);
            this.lbNroPedido.TabIndex = 0;
            this.lbNroPedido.Text = "NroPedido";
            this.lbNroPedido.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGreen;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(554, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(206, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Detalhes do Pedido";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // lbDataHora
            // 
            this.lbDataHora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDataHora.BackColor = System.Drawing.Color.Gainsboro;
            this.lbDataHora.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbDataHora.Location = new System.Drawing.Point(250, 2);
            this.lbDataHora.Name = "lbDataHora";
            this.lbDataHora.Size = new System.Drawing.Size(166, 23);
            this.lbDataHora.TabIndex = 4;
            this.lbDataHora.Text = "DataHora";
            this.lbDataHora.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTipo
            // 
            this.lbTipo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTipo.BackColor = System.Drawing.Color.Gainsboro;
            this.lbTipo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbTipo.Location = new System.Drawing.Point(422, 2);
            this.lbTipo.Name = "lbTipo";
            this.lbTipo.Size = new System.Drawing.Size(126, 23);
            this.lbTipo.TabIndex = 5;
            this.lbTipo.Text = "Tipo";
            this.lbTipo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStatus.BackColor = System.Drawing.SystemColors.Info;
            this.lbStatus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbStatus.Location = new System.Drawing.Point(554, 2);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(206, 23);
            this.lbStatus.TabIndex = 8;
            this.lbStatus.Text = "Status";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RowPedido
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "RowPedido";
            this.Size = new System.Drawing.Size(766, 90);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbEndereco;
        private System.Windows.Forms.Label lbCliente;
        private System.Windows.Forms.Label lbNroPedido;
        private System.Windows.Forms.Label lbTipo;
        private System.Windows.Forms.Label lbDataHora;
        private System.Windows.Forms.Label lbFone;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbAgendamento;
    }
}
